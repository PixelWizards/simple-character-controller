using UnityEngine;

namespace PixelWizards.Gameplay.Controllers
{
    /// <summary>
    /// Handles animation audio events and other player audio
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class PlayerAudio : MonoBehaviour
    {
        private AudioSource audioSource;

        private void Start()
        {
            audioSource = gameObject.GetComponent<AudioSource>();
        }
        
        public void OnFootStep()
        {
            
        }

        public void OnLand()
        {
            
        }
    }
}