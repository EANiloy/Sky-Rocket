using SpaceShip.Spawning;
using UnityEngine;

namespace SpaceShip.Core
{

    public class Asteroid : MonoBehaviour
    {
        [SerializeField] private float _rotateSpeed = 20.0f;
        [SerializeField] private GameObject _explosion;

        private SpawnManager _spawnManager;


        private void Start() {
            _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        }

        private void Update() {
            AsteroidRotation();
        }

        void AsteroidRotation()
        {
            transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if(other.tag == "FirstPlayerLaser" || other.tag =="SecondPlayerLaser")
            {
                Instantiate(_explosion, transform.position, Quaternion.identity);
                this.transform.GetComponent<Collider2D>().enabled = false;
                Destroy(other.gameObject);
                _spawnManager.StartSpawning();
                Destroy(this.gameObject,0.25f);
            }
        }
    }
}
