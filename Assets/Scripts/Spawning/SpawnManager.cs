using System.Collections;
using UnityEngine;

namespace SpaceShip.Spawning
{

    public class SpawnManager : MonoBehaviour
    {
        [SerializeField] private Transform _enemy = null;
        [SerializeField] private float _waitingTime = 5f;
        [SerializeField] private GameObject _enemyContainer;
        [SerializeField] private GameObject _powerUpContainer;
        [SerializeField] private GameObject[] _powerUps = null;

        private bool _stopSpawning = false;

        public void StartSpawning()
        {
            StartCoroutine(SpawnEnemy());
            StartCoroutine(ReduceWaitTime());
            StartCoroutine(SpawnPowerUp());
        }

        IEnumerator SpawnEnemy()
        {
            yield return new WaitForSeconds(3f);
            while(!_stopSpawning)
            {
                float enemyX = Random.Range(-28f, 28f);
                Vector3 enemyPosition = new Vector3(enemyX, 22f, 0f);
                Transform newEnemy = Instantiate(_enemy, enemyPosition, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
                yield return new WaitForSeconds(_waitingTime);
            }
        }
        IEnumerator SpawnPowerUp()
        {
                while (true)
                {
                    if(Time.time <10f) yield return null;
                else
                {
                    float powerUpX = Random.Range(-28f, 28f);
                    Vector3 powerUpPosition = new Vector3(powerUpX, 22f, 0f);
                    int randomPowerup = Random.Range(0, 3);
                    Transform newTriplePowerUp = Instantiate(_powerUps[randomPowerup].transform, powerUpPosition, Quaternion.identity);
                    newTriplePowerUp.transform.parent = _powerUpContainer.transform;
                    yield return new WaitForSeconds(Random.Range(15f, 20f));
                }
            }
        }
        IEnumerator ReduceWaitTime()
        {
            while(_stopSpawning == false)
            {
                if(_waitingTime>=1f)
                {
                    _waitingTime -= 0.1f;
                }
                else
                {
                    _waitingTime -= 0.05f;

                }
                if(_waitingTime <=0.1)
                {
                    _waitingTime = 0.05f;
                }
                yield return new WaitForSeconds(5f);
            }
        }
        public void StopSpawning()
        {
            _stopSpawning = true;
        }
    }

}