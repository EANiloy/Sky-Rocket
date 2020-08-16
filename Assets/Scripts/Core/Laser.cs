using UnityEngine;

namespace SpaceShip.Core
{
    public class Laser : MonoBehaviour
    {
        [SerializeField] private float _laserspeed = 22f;
        [SerializeField] private bool _isEnemyLaser = false;

        void Update()
        {
            if(_isEnemyLaser == true)
            {
                MoveDown();
            }
            else
            {
                LaserMovementUp();
            }
        }
        
        void LaserMovementUp()
        {
            transform.Translate(Vector3.up * _laserspeed * Time.deltaTime);
            if(transform.position.y>=21f)
            {
                if(transform.parent != null)
                {
                    Destroy(transform.parent.gameObject);
                }
                Destroy(this.gameObject);
            }
        }
        void MoveDown()
        {
            transform.Translate(Vector3.down * _laserspeed * Time.deltaTime);
            if (transform.position.y <= -21f)
            {
                if (transform.parent != null)
                {
                    Destroy(transform.parent.gameObject);
                }
                Destroy(this.gameObject);
            }
        }
        public void InstansitateEnemy()
        {
            _isEnemyLaser = true;
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if(other.tag == "Enemy" && _isEnemyLaser == true) return;
            if(other.tag == "Player" && _isEnemyLaser == true)
            {
                Player player = other.GetComponent<Player>();
                if(player != null)
                {
                    player.Damage();
                }
                Destroy(this.gameObject);            
            }
            
        }

    }
    
}
