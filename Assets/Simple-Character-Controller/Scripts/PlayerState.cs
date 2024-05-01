using UnityEngine;

namespace PixelWizards.Gameplay.Controllers
{
    /// <summary>
    /// Holds all of the player's dynamic state, passed around to the various systems to do their thing
    /// </summary>
    [System.Serializable]
    public class PlayerState
    {
        public float walkSpeed;                     // player configuable
        public float runSpeed;                      // player configuable
        public float horizontal;                    // raw input
        public float vertical;                      // raw input
        public float aiming = 0f;                   // 0 == normal movement, 1 == combat movement
        public float magicNumber;                   // we use this for various Time.deltaTime variations
        public Transform mainCam;                   // share the camera we're all using
        public Vector3 movement = Vector3.zero;     // movement vector (including the move speed)
        public bool isGrounded = false;             // we on the ground?
        public bool isRunning = false;              // we be runnin
        public bool doRotate = false;               // if we are standing still, we don't automatically rotate, we wait until we get past a threshold
        public Vector3 rotateAmount = Vector3.zero; // if we are standing and need to rotate, by how much?
    }
}