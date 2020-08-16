using UnityEngine;

namespace SpaceShip.Core
{
    public class Explosion : MonoBehaviour {
        [SerializeField] private AudioClip _explosionSound;

        private AudioSource _audioSource;
        private void Start() {
            _audioSource = GetComponent<AudioSource>();
            if(_audioSource == null)
            {
                Debug.LogError("Audio Source on Explosion is null");
            }
            _audioSource.clip = _explosionSound;
            _audioSource.Play();
            Destroy(this.gameObject, 3f);
        }
    }
}

