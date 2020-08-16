using SpaceShip.Core;
using UnityEngine;

namespace SpaceShip.PowerUps
{

    public class PowerUp : MonoBehaviour
    {
        [SerializeField] private float _powerSpeed = 10f;
        [SerializeField] private int _powerUpID = 0;
        [SerializeField] private AudioClip _powerUpSound;
        void Update()
        {
            MovePowerUp();
        }
        void MovePowerUp()
        {
            transform.Translate(Vector3.down * _powerSpeed * Time.deltaTime);
            if (transform.position.y <= -22f)
            {
                Destroy(this.gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.tag != "Player") return;
            AudioSource.PlayClipAtPoint(_powerUpSound, transform.position);
            Player player = other.GetComponent<Player>();
            switch(_powerUpID)
            {
                case 0:
                    {
                        if(player._isPlayerOne == true)
                        {
                            player.SetActiveTriple(true);
                        }
                        else
                        {
                            player.SetActiveTriple(false);
                        }
                        break;
                    }
                case 1:
                    {
                        if(player._isPlayerOne == true)
                        {
                            player.BoostSpeed(true);
                        }
                        else
                        {
                            player.BoostSpeed(false);
                        }
                        break;
                    }
                case 2:
                    {
                        if(player._isPlayerOne == true)
                        {
                            player.ActivateShield(true);
                        }
                        else
                        {
                            player.ActivateShield(false);
                        }
                        break;
                    }
                default:
                    {
                        Debug.LogError("Error happened at powerup");
                        break;
                    }
            }

            Destroy(this.gameObject);
        }
    }
}