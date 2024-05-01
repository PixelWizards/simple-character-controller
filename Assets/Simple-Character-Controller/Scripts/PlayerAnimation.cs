using UnityEngine;

namespace PixelWizards.Gameplay.Controllers
{
    /// <summary>
    /// Animates the player character based on current state
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimation : MonoBehaviour
    {
        private Animator anim;
        private int speedHash;
        private int strafeHash;
        private int groundedHash;
        private int aimHash;
        private int freeFallHash;
        private int jumpHash;

        private float hSpeed, vSpeed;

        private bool inFreeFall = false;
        private float freeFallTimer;
        private float freeFallThreshold = 0.25f;

        private void Start()
        {
            anim = GetComponent<Animator>();
            PlayerMovement.onUpdatePlayerState += OnUpdatePlayerState;

            // cache the animator parameter lookups
            speedHash = Animator.StringToHash("Speed");
            strafeHash = Animator.StringToHash("Strafe");
            groundedHash = Animator.StringToHash("Grounded");
            aimHash = Animator.StringToHash("Aiming");
            freeFallHash = Animator.StringToHash("FreeFall");
            jumpHash = Animator.StringToHash("Jump");
        }

        private void OnDisable()
        {
            PlayerMovement.onUpdatePlayerState -= OnUpdatePlayerState;
        }

        /// <summary>
        /// Event handler, update player animation
        /// </summary>
        /// <param name="state"></param>
        private void OnUpdatePlayerState(PlayerState state)
        {
            // reset a few things
            anim.SetBool(jumpHash, false);
            anim.SetBool(freeFallHash, false);

            // these will override the move stuff below
            anim.SetBool(jumpHash, state.doJump);
            anim.SetBool(freeFallHash, state.inFreeFall);
            
            /*if (!state.isGrounded)
            {
                // add fall state etc
            }
            else*/
            // normal movement
            if (state.aiming < 0.5)
            {
                anim.SetFloat(speedHash, state.movement.magnitude);
                anim.SetFloat(strafeHash, 0);
            }
            // aiming (combat) animation
            else
            {
                // how fast are we moving
                hSpeed = (state.isRunning)
                    ? state.input.horizontal * state.runSpeed
                    : state.input.horizontal * state.walkSpeed;
                
                vSpeed = (state.isRunning)
                    ? state.input.vertical * state.runSpeed
                    : state.input.vertical * state.walkSpeed;

           
                anim.SetFloat(speedHash, vSpeed);
                anim.SetFloat(strafeHash, hSpeed);  
            }
            

            anim.SetBool(groundedHash, state.isGrounded);
            anim.SetFloat(aimHash, state.aiming);
        }
    }
}