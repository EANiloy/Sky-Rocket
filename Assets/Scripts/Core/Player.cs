using System.Collections;
using SpaceShip.Spawning;
using SpaceShip.UI;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace SpaceShip.Core 
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float _firstPlayerSpeed = 20f;
        [SerializeField] private float _secondPlayerSpeed = 20f;
        [SerializeField] private Transform _laser = null;
        [SerializeField] private float _fireRate = 0.2f;
        [SerializeField] private int _playerLives = 3;
        [SerializeField] private Transform _TripleShotLaser = null;
        [SerializeField] private bool _isFirstTripleShotActive = false;
        [SerializeField] private bool _isSecondTripleShotActive = false;
        [SerializeField] private bool _isFirstShieldActive = false;
        [SerializeField] private bool _isSecondShieldActive = false;
        [SerializeField] private int _firstPlayerScore = 0;
        [SerializeField] private int _secondPlayerScore = 0;
        [SerializeField] private GameObject _leftFire,_rightFire;
        [SerializeField] private AudioClip _laserShotSound;
        [SerializeField] private AudioClip _explosionSound;
        [SerializeField] public bool _isPlayerOne = true;

        private SpawnManager _spawnManager;
        private float _canFire = 0f;
        private UIManager _uiManager;
        private AudioSource _audioSource;
        private GameManager _gamemanager;

        public int PlayerLives { get => _playerLives;}

        void Start()
        {
            _gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
            if(_gamemanager._isCoOp == false)
            {
                transform.position = new Vector3(0f, 0f, 0f);
            }
            _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
            _audioSource = GetComponent<AudioSource>();
            transform.GetChild(0).transform.gameObject.SetActive(false);
            if(_uiManager == null)
            {
                Debug.LogError("UI Manager not found");
            }
            _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
            if(_spawnManager == null)
            {
                Debug.LogError("Spawn Manager Not Initiated");
            }
            if(_audioSource == null)
            {
                Debug.LogError("Audio source of player is null");
            }
            _leftFire.SetActive(false);
            _rightFire.SetActive(false);
        }

        void Update()
        {
            if(_isPlayerOne == true)
            {
                PlayerOneCalculateMovement();
                FireLaser();
            }
            else
            {
                PlayerTwoCalculateMovement();
                FireLaser();
            }
            
        }

        void PlayerOneCalculateMovement()
        {
#if UNITY_ANDROID
            float horizontalInput = CrossPlatformInputManager.GetAxis("Horizontal");
            float verticalInput = CrossPlatformInputManager.GetAxis("Vertical");

            Vector3 directions = new Vector3(horizontalInput, verticalInput, 0f);

            transform.Translate(directions * _firstPlayerSpeed * Time.deltaTime);

            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -13f, 6.5f), 0f);
#endif

            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector3.up * _secondPlayerSpeed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector3.right * _secondPlayerSpeed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector3.down * _secondPlayerSpeed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector3.left * _secondPlayerSpeed * Time.deltaTime);
            }

            if(transform.position.y >=8f)
            {
                transform.position = new Vector3(transform.position.x, 8f, 0f);
            }
            if (transform.position.y <= -14f)
            {
                transform.position = new Vector3(transform.position.x, -14f, 0f);
            }            


            //Wrap the player on both sides
            if (transform.position.x >= 31f)
            {
                transform.position = new Vector3(-31f, transform.position.y, 0f);
            }
            else if (transform.position.x <= -31f)
            {
                transform.position = new Vector3(30f, transform.position.y, 0f);
            }
        }

        void PlayerTwoCalculateMovement()
        {
            if(Input.GetKey(KeyCode.UpArrow))
            {
                transform.Translate(Vector3.up * _secondPlayerSpeed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Translate(Vector3.right * _secondPlayerSpeed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.Translate(Vector3.down * _secondPlayerSpeed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Translate(Vector3.left * _secondPlayerSpeed * Time.deltaTime);
            }
            //Wrap the player on both sides
            if (transform.position.x >= 31f)
            {
                transform.position = new Vector3(-31f, transform.position.y, 0f);
            }
            if (transform.position.x <= -31f)
            {
                transform.position = new Vector3(30f, transform.position.y, 0f);
            }
            if (transform.position.y >= 8f)
            {
                transform.position = new Vector3(transform.position.x, 8f, 0f);
            }
            if (transform.position.y <= -14f)
            {
                transform.position = new Vector3(transform.position.x, -14f, 0f);
            }
        }

        void FireLaser()
        {
            Vector3 laserPosition = new Vector3(transform.position.x, transform.position.y + 0.8f, 0f);
            Vector3 tripleLaserPosition = new Vector3(transform.position.x - 0.7f, transform.position.y, 0f);

            #if UNITY_ANDROID
            if (CrossPlatformInputManager.GetButtonDown("Fire") && _isPlayerOne = true) && Time.time > _canFire)
            {
                _canFire = Time.time + _fireRate;
                if (_isTripleShotActive)
                {
                    Instantiate(_TripleShotLaser, tripleLaserPosition, Quaternion.identity);
                }
                else
                {
                    Instantiate(_laser, laserPosition, Quaternion.identity);
                }
                _audioSource.clip = _laserShotSound;
                _audioSource.Play();
            }
            #endif
            if ((Input.GetKeyDown(KeyCode.Space) && _isPlayerOne == true) && Time.time > _canFire)
            {
                _canFire = Time.time + _fireRate;
                if (_isFirstTripleShotActive)
                {
                    Instantiate(_TripleShotLaser, tripleLaserPosition, Quaternion.identity);
                }
                else
                {
                    Instantiate(_laser, laserPosition, Quaternion.identity);
                }
                _audioSource.clip = _laserShotSound;
                _audioSource.Play();
            }
            else if ((Input.GetKeyDown(KeyCode.RightShift) && _isPlayerOne == false) && Time.time > _canFire)
            {
                _canFire = Time.time + _fireRate;
                if (_isSecondTripleShotActive)
                {
                    Instantiate(_TripleShotLaser, tripleLaserPosition, Quaternion.identity);
                }
                else
                {
                    Instantiate(_laser, laserPosition, Quaternion.identity);
                }
                _audioSource.clip = _laserShotSound;
                _audioSource.Play();
            }
            {
                
            }
            
        }
        public void Damage()
        {
            _audioSource.clip = _explosionSound;
            _audioSource.Play();
            if (this._isFirstShieldActive)
            {
                this._isFirstShieldActive = false;
                transform.GetChild(0).transform.gameObject.SetActive(false);
                return;
            }
            else if (this._isSecondShieldActive)
            {
                this._isSecondShieldActive = false;
                transform.GetChild(0).transform.gameObject.SetActive(false);
                return;
            }
            this._playerLives -= 1;
            if(_isPlayerOne == true)
            {
                _uiManager.UpdateLives(this,true);
            }
            else
            {
                _uiManager.UpdateLives(this,false);
            }
            if(PlayerLives == 2)
            {
                this._leftFire.SetActive(true);
            }
            else if(PlayerLives == 1)
            {
                this._rightFire.SetActive(true);
            }
            if(PlayerLives <=0)
            {
                if(_isPlayerOne == true)
                {
                    _gamemanager.UpdatePlayers(true);
                }
                else
                {
                    _gamemanager.UpdatePlayers(false);
                }
                Destroy(this.gameObject,3f);
            }
        }

        public void SetActiveTriple(bool _isFirstPlayer)
        {
            if(_isFirstPlayer == true)
            {
                this._isFirstTripleShotActive = true;
                StartCoroutine(TripleShotPowerDownRoutine(true));
            }
            else{
                this._isSecondTripleShotActive = true;
                StartCoroutine(TripleShotPowerDownRoutine(false));
            }
        }

        public  void BoostSpeed(bool _isFirstPlayer)
        {
            if(_isFirstPlayer == true)
            {
                this._firstPlayerSpeed += 5f;
                StartCoroutine(SpeedBoostPowerDownRoutine(true));
            }
            else
            {
                this._secondPlayerSpeed += 5f;
                StartCoroutine(SpeedBoostPowerDownRoutine(false));
            }
        }

        IEnumerator SpeedBoostPowerDownRoutine(bool isFirstPlayer)
        {
           if(isFirstPlayer == true)
           {
                yield return new WaitForSeconds(5f);
                this._firstPlayerSpeed -= 5f;
           }
           else
           {
                yield return new WaitForSeconds(5f);
                this._secondPlayerSpeed -= 5f;
           }
        }

        IEnumerator TripleShotPowerDownRoutine(bool isFirstPlayer)
        {
            if(isFirstPlayer == true)
            {
                yield return new WaitForSeconds(5f);
                this._isFirstTripleShotActive = false;
            }
            else
            {
                yield return new WaitForSeconds(5f);
                this._isSecondTripleShotActive = false;
            }
        }

        public void ActivateShield(bool isFirstPlayer)
        {
            if(isFirstPlayer == true)
            {
                this._isFirstShieldActive = true;
                this.transform.GetChild(0).transform.gameObject.SetActive(true);
            }
            else
            {
                this._isSecondShieldActive = true;
                this.transform.GetChild(0).transform.gameObject.SetActive(true);
            }
        }
        public void ScoreUp(bool isFirstPlayer)
        {
            if(isFirstPlayer == true)
            {
                int _randomScore = (int)Random.Range(10, 50);
                this._firstPlayerScore += _randomScore;
                _uiManager.UpdateScore(_firstPlayerScore,true);
            }
            else
            {
                int _randomScore = (int)Random.Range(10, 50);
                this._secondPlayerScore += _randomScore;
                _uiManager.UpdateScore(_secondPlayerScore,false);
            }
        }
    }
}