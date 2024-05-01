using System;
using UnityEngine;

namespace PixelWizards.Gameplay.Controllers
{
    public class PlayerMovement : MonoBehaviour
    {
        /*
         *  user configurable
         */
        [SerializeField]
        private float walkSpeed = 3.0f;
        
        [SerializeField]
        private float runSpeed = 5.0f;
        
        [SerializeField]
        private float gravity = 9.8f;   // set to positive number we subtract it from the player's position
        
        /*
         * Internals
         */
        
        /// <summary>
        /// magic number we use to multiply with Time.deltaTime for proper update cadence for lerping etc
        /// </summary>
        [SerializeField]
        private float magicNumber = 15;
        
        /// <summary>
        /// the master copy of our player state, we pass this around to the other components
        /// </summary>
        private PlayerState state = new();
        
        private CharacterController cc;
        
        /*
         * Movement
         */
        private float desiredSpeed = 0f;
        private float currentSpeed;

        private Vector3 forward;
        private Vector3 right;
        private float desiredAim = 0f;
  
        /// <summary>
        /// The master event, all other systems listen to this and react accordingly
        /// </summary>
        public static event Action<PlayerState> onUpdatePlayerState;

        private void Start()
        {
            cc = GetComponent<CharacterController>();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            state = new PlayerState
            {
                walkSpeed = walkSpeed,
                runSpeed = runSpeed,
                magicNumber = magicNumber,
                mainCam = Camera.main.transform,
            };
        }
        
        private void Update()
        {
            HandleInput();
            Movement();
            DoRotation();
            
            // let the other systems know what's going on
            onUpdatePlayerState?.Invoke(state);
        }

        /// <summary>
        /// process player input
        /// </summary>
        private void HandleInput()
        {
            state.horizontal = Input.GetAxis("Horizontal");
            state.vertical = Input.GetAxis("Vertical");
            state.isRunning = Input.GetButton("Run");
            desiredAim = Mathf.Lerp(desiredAim, Input.GetButton("Aim") ? 1.0f : 0f, Time.deltaTime * magicNumber);
            state.aiming = desiredAim;
        }

        /// <summary>
        /// move the character
        /// </summary>
        private void Movement()
        {
            state.isGrounded = cc.isGrounded;
            if (state.isGrounded)
            {
                // camera-relative movement
                forward = state.mainCam.TransformDirection(Vector3.forward);
                forward.y = 0;
                forward = forward.normalized;
                right = new Vector3(forward.z, 0, -forward.x);

                state.movement = (state.horizontal * right + state.vertical * forward).normalized;
                desiredSpeed = (state.isRunning) ? state.runSpeed : state.walkSpeed;
                currentSpeed = cc.velocity.magnitude;
                state.movement *= Mathf.Lerp(currentSpeed, desiredSpeed, Time.deltaTime * magicNumber);    
            }
            
            var gravityForce = gravity * (Time.deltaTime * magicNumber);
            state.movement.y -= gravityForce;
            cc.Move(state.movement * Time.deltaTime);
        }
        
        private void DoRotation()
        {
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

    }
}