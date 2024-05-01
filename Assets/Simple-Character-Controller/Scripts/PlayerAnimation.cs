using UnityEngine;

namespace PixelWizards.Gameplay.Controllers
{
    /// <summary>
    /// Animates the player character based on current state
    /// </summary>
    public class PlayerAnimation : MonoBehaviour
    {
        private Animator anim;
        private int speedHash;
        private int strafeHash;
        private int groundedHash;
        private int aimHash;
        private int freeFallHash;

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
        }

        /// <summary>
        /// Event handler, update player animation
        /// </summary>
        /// <param name="state"></param>
        private void OnUpdatePlayerState(PlayerState state)
        {
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
                    ? state.horizontal * state.runSpeed
                    : state.horizontal * state.walkSpeed;
                
                vSpeed = (state.isRunning)
                    ? state.vertical * state.runSpeed
                    : state.vertical * state.walkSpeed;

           
                anim.SetFloat(speedHash, vSpeed);
                anim.SetFloat(strafeHash, hSpeed);  
            }

            if (!state.isGrounded)
            {
                freeFallTimer += Time.deltaTime;
                if (freeFallTimer > freeFallThreshold)
                {
                    Debug.Log("falling!");
                    inFreeFall = true;
                }
            }
            else
            {
                inFreeFall = false;
            }

            anim.SetBool(freeFallHash, inFreeFall);
            anim.SetBool(groundedHash, state.isGrounded);
            anim.SetFloat(aimHash, state.aiming);
        }
    }
}