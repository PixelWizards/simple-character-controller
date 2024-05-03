using System;
using UnityEngine;

namespace PixelWizards.Gameplay.Controllers
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        /*
         *  user configurable
         */
        [Header("User Configurable\n\nMovement Variables")]
        [SerializeField]
        private float walkSpeed = 3.0f;     // how fast we walk
        
        [SerializeField]
        private float runSpeed = 5.0f;      // how fast we run

        [SerializeField]
        private float jumpHeight = 1.2f;      // how high we jump
        
        [SerializeField]
        private float gravity = 9.8f;       // set to positive number we subtract it from the player's position

        [SerializeField]
        private LayerMask groundLayers;     // what layers should we collide with to detect if we're on the ground. NOT used for collsion detection

        [SerializeField]
        private float jumpTimeout = 0.5f;  // how often can we jump again?

        [SerializeField]
        private float fallTimeout = 0.15f;    // how long we fall before displaying the fall animation
        /*
         * Internals
         */
        /// <summary>
        /// magic number we use to multiply with Time.deltaTime for proper update cadence for lerping etc
        /// </summary>
        [Header("Magic number == how 'snappy' the movement / rotation is")]
        [SerializeField]
        private float magicNumber = 50;
        
        /// <summary>
        /// the master copy of our player state, we pass this around to the other components
        /// </summary>
        private PlayerState state = new();
        
        /// <summary>
        /// required components
        /// </summary>
        private CharacterController cc;
        
        /*
         * Movement
         */
        private float desiredSpeed = 0f;            // how fast do we WANT to move
        private float currentSpeed;                 // how fast are we moving
        private float verticalVelocity = 0f;        // so we can check if we're falling
        private Vector3 forward;
        private Vector3 right;
        
        /// <summary>
        /// ground detection
        /// </summary>
        private float groundOffset = 0.25f;
        private Vector3 groundCheckPosition;
        
        private float desiredAim = 0f;

        /// <summary>
        /// jump / fall timers
        /// </summary>
        private float jumpDelta;
        private float fallDelta;
        private float terminalVelocity = 53.0f;
        
        /// <summary>
        /// The master event, all other systems listen to this and react accordingly
        /// </summary>
        public static event Action<PlayerState> onUpdatePlayerState;

        private void Start()
        {
            cc = GetComponent<CharacterController>();

            jumpDelta = jumpTimeout;
            fallDelta = fallTimeout;

            // setup our initial player state
            state = new PlayerState
            {
                walkSpeed = walkSpeed,
                runSpeed = runSpeed,
                magicNumber = magicNumber,
                mainCam = Camera.main.transform,
            };

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            // are we paused?
            if (!state.isEnabled)
                return;
            
            HandleInput();          // player input
            CheckGrounded();        // we on the ground?            
            JumpGravity();          // falling or jumping?
            Movement();             // move the character
            DoRotation();           // rotate the character
            
            // let the other systems know what's going on
            onUpdatePlayerState?.Invoke(state);
        }

        /// <summary>
        /// process player input
        /// </summary>
        private void HandleInput()
        {
            state.input.horizontal = Input.GetAxis("Horizontal");
            state.input.vertical = Input.GetAxis("Vertical");
            state.input.run = Input.GetButton("Run");
            state.input.jump = Input.GetButton("Jump");
            desiredAim = Mathf.Lerp(desiredAim, Input.GetButton("Aim") ? 1.0f : 0f, Time.deltaTime * magicNumber);
            state.aiming = desiredAim;
        }
        
        /// <summary>
        /// We can't trust the character controller's isgrounded, so let's do our own physics check
        /// </summary>
        private void CheckGrounded()
        {
            // set sphere position, with offset
            groundCheckPosition = new Vector3(transform.position.x, transform.position.y - groundOffset, transform.position.z);
            state.isGrounded = Physics.CheckSphere(groundCheckPosition, groundOffset, groundLayers, QueryTriggerInteraction.Ignore);
        }
        
        private void JumpGravity()
        {
            // reset our vars
            state.doJump = false;
            state.inFreeFall = false;
            
            if (state.isGrounded)
            {
                fallDelta = fallTimeout;
                
                if (verticalVelocity < 0f)
                {
                    verticalVelocity = -2f;
                }

                // we trying to jump? we allowed to?
                if (state.input.jump && jumpDelta <= 0f)
                {
                    // do the jump
                    verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * -gravity);
                    state.doJump = true;
                }

                if (jumpDelta >= 0f)
                {
                    jumpDelta -= Time.deltaTime;
                }
            }
            else
            {
                jumpDelta = jumpTimeout;

                if (fallDelta >= 0f)
                {
                    fallDelta -= Time.deltaTime;
                }
                else
                {
                    state.inFreeFall = true;
                }
            }
            
            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (verticalVelocity < terminalVelocity)
            {
                verticalVelocity -= gravity * Time.deltaTime;
            }
        }

        /// <summary>
        /// move the character
        /// </summary>
        private void Movement()
        {
            // allow the player to control their movement as long as they're grounded
            if (state.isGrounded)
            {
                // camera-relative movement
                forward = state.mainCam.TransformDirection(Vector3.forward);
                forward.y = 0;
                forward = forward.normalized;
                right = new Vector3(forward.z, 0, -forward.x);

                state.movement = (state.input.horizontal * right + state.input.vertical * forward).normalized;
                state.isRunning = state.input.run;
                desiredSpeed = (state.isRunning) ? state.runSpeed : state.walkSpeed;
                currentSpeed = cc.velocity.magnitude;
                state.movement *= Mathf.Lerp(currentSpeed, desiredSpeed, Time.deltaTime * magicNumber);
                state.movement.y = verticalVelocity;
            }
            
            var gravityForce = gravity * (Time.deltaTime * magicNumber);
            state.movement.y -= gravityForce;
            cc.Move(state.movement * Time.deltaTime);
        }
        
        private void DoRotation()
        {
            // TODO: make this a proper state machine instead? would allow us to expand this functionality easier
            // just normal movement
            if (state.aiming < 0.5)
            {
                state.movement.y = 0f;
                
                // if we're stopped, we don't rotate at all
                if (state.movement.magnitude > 0.1)
                {
                    var targetRotation = Quaternion.LookRotation(state.movement);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation,
                        Time.deltaTime * state.magicNumber * 15);
                }
            }
            // aiming (combat) movement
            else
            {
                var cameraRotation = state.mainCam.forward;
                // no forward rotation
                cameraRotation.y = 0f;
                
                // amount we need to rotate
                state.rotateAmount = Vector3.Lerp(transform.forward, cameraRotation, Time.deltaTime * state.magicNumber);
                
                    // just rotate
                transform.forward = state.rotateAmount;
                
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(groundCheckPosition, groundOffset);
        }
    }
}