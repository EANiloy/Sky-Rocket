using System.Collections;
using SpaceShip.UI;
using UnityEngine;

namespace SpaceShip.Core
{

    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float _enemySpeed = 8f;
        [SerializeField] private int _enemyLives = 3;
        [SerializeField] private AudioClip _explosionSound;
        [SerializeField] private GameObject _enemyLaser;
        [SerializeField] private bool _isEnemyDead = false;

        private Player _player;
        private Animator _anim;
        private EnemyHealthManager _enemyHealthManager;
        private AudioSource _audioSource;
        private float _firerate = 3f;
        private float _canFire = 0f;

        private void Start() {
            _player = GameObject.FindWithTag("Player").GetComponent<Player>();
            if(_player == null)
            {
                Debug.LogError("Player is null");
            }
            _anim = GetComponent<Animator>();
            if (_anim == null)
            {
                Debug.LogError("Animator is null");
            }
            _enemyHealthManager = GetComponentInChildren<Canvas>().GetComponent<EnemyHealthManager>();
            if(_enemyHealthManager == null)
            {
                Debug.LogError("Enemy Health Manager is null");
            }
            _audioSource = GetComponent<AudioSource>();
            if(_audioSource == null)
            {
                Debug.LogError("Audio Source on Enemy is null");
            }
            else
            {
                _audioSource.clip = _explosionSound;
            }
            StartCoroutine(IncreaseEnemySpeed());
        }
        void Update()
        {
            EnemyMovement();
            FireLaser();
        }
        void FireLaser()
        {
            if (Time.time > _canFire && _isEnemyDead == false)
            {
                _firerate = Random.Range(3f, 7f);
                _canFire = Time.time + _firerate;
                Vector3 laserPosition = new Vector3(transform.position.x, transform.position.y - 2f, 0f);
                GameObject enemyLaser = Instantiate(_enemyLaser,laserPosition, Quaternion.identity);
                Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
                for (int i = 0; i < lasers.Length;i++)
                {
                    lasers[i].InstansitateEnemy();
                }
            }
        }
        void EnemyMovement()
        {
            transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
            if (transform.position.y <= -22f)
            {
                float randomX = Random.Range(-28f, 28f);
                transform.position = new Vector3(randomX, 22f, 0f);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                Player player = other.GetComponent<Player>();
                if(player != null)
                {
                    player.Damage();
                    Damage("player",player._isPlayerOne);
                }
                else
                {
                    return;
                }
            }
            else if (other.tag != "EnemyLaser" && other.tag == "FirstPlayerLaser")
            {
                Destroy(other.gameObject);
                Damage("FirstPlayerLaser",true);
            }
            else if (other.tag != "EnemyLaser" && other.tag == "SecondPlayerLaser")
            {
                Destroy(other.gameObject);
                Damage("SecondPlayerLaser", false);
            }
        }
        void Damage(string player,bool isFirstPlayer)
        {
            _enemyLives--;
            _enemyHealthManager.UpdateLives(_enemyLives);
            if(_enemyLives<1)
            {
                if(player == "FirstPlayerLaser" && isFirstPlayer == true)
                {
                    _player.ScoreUp(true);
                }
                else if(player == "SecondPlayerLaser" && isFirstPlayer == false)
                {
                    _player.ScoreUp(false);
                }
                else if(player =="player" && isFirstPlayer == true)
                {
                    _player.ScoreUp(true);
                }
                else if(player == "player" && isFirstPlayer == false)
                {
                    _player.ScoreUp(false);
                }
                _anim.SetTrigger("OnEnemyDeath");
                _isEnemyDead = true;
                _enemySpeed = 0;
                GetComponent<BoxCollider2D>().enabled = false;
                Destroy(_enemyHealthManager.gameObject);
                _audioSource.Play();
                Destroy(this.gameObject,2.633f);
            }
        }
        IEnumerator IncreaseEnemySpeed()
        {
            while ( _enemySpeed <18f)
            {
                if (_enemySpeed > 5f && _enemySpeed<=13f)
                {
                    _enemySpeed += 0.2f;
                }
                else if(_enemySpeed > 13f && _enemySpeed <= 14f)
                {
                    _enemySpeed += 0.35f;
                }
                else if (_enemySpeed > 14f && _enemySpeed <= 16f)
                {
                    _enemySpeed += 0.45f;
                }
                else if (_enemySpeed > 16f)
                {
                    _enemySpeed += 0.55f;
                }
                yield return new WaitForSeconds(3f);
            }
        }
    }
}
