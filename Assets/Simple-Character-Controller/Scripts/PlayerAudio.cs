using UnityEngine;

namespace PixelWizards.Gameplay.Controllers
{
    /// <summary>
    /// Handles animation audio events and other player audio
    ///
    /// TODO: flesh this out more, currently just a stub
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class PlayerAudio : MonoBehaviour
    {
        private AudioSource audioSource;

        private void Start()
        {
            audioSource = gameObject.GetComponent<AudioSource>();
        }
        
        /// <summary>
        /// Callbacks from animation event
        /// </summary>
        public void OnFootStep()
        {
            
        }

        /// <summary>
        /// Callbacks from animation event
        /// </summary>
        public void OnLand()
        {
            
        }
    }
}