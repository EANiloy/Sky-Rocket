using System;
using System.Collections;
using SpaceShip.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SpaceShip.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Sprite[] _firstPlayerLivesSprite;
        [SerializeField] private Sprite[] _secondPlayerLivesSprite;
        [SerializeField] private Image _firstPlayerLivesImage;
        [SerializeField] private Image _secondPlayerLivesImage;
        [SerializeField] private Image _gameOverImg;
        [SerializeField] private Text _gameOverText;
        [SerializeField] private Text _reloadMessage;
        [SerializeField] private Text _movementInstructions;
        [SerializeField] private Text _fireInstructions;
        [SerializeField] private bool isCoOp = false;
        [SerializeField] private Text _firstPlayerScore;
        [SerializeField] private Text _secondPlayerScore;
        [SerializeField] private GameObject _pauseMenu;
        [SerializeField] private Text _highScore;
        [SerializeField] private Text _playerInfo;

        private int _score=0;
        private GameManager _gameManager;
        private Animator _pauseAnimator;
        private bool _isPaused = false;
        private int _coOpBestScore;
        private int _singlePlayerBestScore;

        void Start()
        {
            if(isCoOp == true)
            {
                _coOpBestScore = PlayerPrefs.GetInt("CoOpHighScore", 0);
                _highScore.text = _coOpBestScore.ToString();
            }
            else
            {
                _singlePlayerBestScore = PlayerPrefs.GetInt("SinglePlayerHighScore", 0);
                _highScore.text = _singlePlayerBestScore.ToString();
            }
            
            _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            if(_gameManager == null)
            {
                Debug.LogError("Game Manager is null");
            }
            _firstPlayerScore.text = "Score: " + _score;
            if(isCoOp == true)
            {
                _firstPlayerScore.text = "1st Player: " + _score;
                _secondPlayerScore.text = "2nd Player: " + _score;
            }
            _gameOverImg.gameObject.SetActive(false);
            _gameOverText.gameObject.SetActive(false);
            _reloadMessage.gameObject.SetActive(false);
            StartCoroutine(DisableInstruction());
            
        }

        private void Update() {

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P) && _isPaused == false)
            {
                _pauseMenu.gameObject.SetActive(true);
                _pauseAnimator = GameObject.FindWithTag("PauseMenu").GetComponent<Animator>();
                _pauseAnimator.SetBool("isPaused", true);
                _isPaused = true;
                Time.timeScale = 0.0f;
            }
            else if((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)) && _isPaused == true)
            {
                _isPaused = false;
                ResumeGame();
            }
        }

        public void ResumeGame()
        {
            _isPaused = false;
            StartCoroutine(Resume());
        }

        IEnumerator Resume()
        {
            
            _pauseAnimator.SetBool("isPaused", false);
            Time.timeScale = 1f;
            yield return new WaitForSeconds(0.625f);
            _pauseMenu.gameObject.SetActive(false);
            
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void GotoMainMenu()
        {
            SceneManager.LoadScene(0);
        }

        IEnumerator DisableInstruction()
        {
            yield return new WaitForSeconds(5f);
            _movementInstructions.gameObject.SetActive(false);
            _fireInstructions.gameObject.SetActive(false);
        }
        public void UpdateScore(int _playerScore,bool isFirstPlayer)
        {
            if(isCoOp == false)
            {
                _firstPlayerScore.text = "Score: " + _playerScore;
                if(_singlePlayerBestScore <= _playerScore)
                {
                    _singlePlayerBestScore = _playerScore;
                    _highScore.text = _playerScore.ToString();
                }
            }
            else
            {
                if(isFirstPlayer == true)
                {
                    _firstPlayerScore.text = "1st Player: " + _playerScore;
                    if (_coOpBestScore <= _playerScore)
                    {
                        _coOpBestScore = _playerScore;
                        _highScore.text = _playerScore.ToString();
                        _playerInfo.text = "By Player One";
                    }
                    
                }
                else
                {
                    _secondPlayerScore.text = "2nd Player: " + _playerScore;
                    if (_coOpBestScore <= _playerScore)
                    {
                        _coOpBestScore = _playerScore;
                        _highScore.text = _playerScore.ToString();
                        _playerInfo.text = "By Player Two";
                    }
                }
                
            }
        }
        public void UpdateLives(Player player,bool isFirstPlayer)
        {
            if(isFirstPlayer == true)
            {
                if (player.PlayerLives >= 0)
                {
                    _firstPlayerLivesImage.sprite = _firstPlayerLivesSprite[player.PlayerLives];
                }
                if (player.PlayerLives <= 0)
                {
                    Destroy(player.gameObject);
                }
            }
            else
            {
                if (player.PlayerLives >= 0)
                {
                    _secondPlayerLivesImage.sprite = _secondPlayerLivesSprite[player.PlayerLives];
                }
                if (player.PlayerLives <= 0)
                {
                    Destroy(player.gameObject);
                }
            }
            
        }

        public void GameOverSequence()
        {
            if(isCoOp == false)
            {
                PlayerPrefs.SetInt("SinglePlayerHighScore", _singlePlayerBestScore);
            }
            else
            {
                PlayerPrefs.SetInt("CoOpHighScore", _coOpBestScore);
            }
            _gameOverText.gameObject.SetActive(true);
            _gameOverImg.gameObject.SetActive(true);
            _reloadMessage.gameObject.SetActive(true);
            if(isCoOp == true)
            {
                _firstPlayerScore.gameObject.SetActive(false);
                _secondPlayerScore.gameObject.SetActive(false);
                _firstPlayerLivesImage.gameObject.SetActive(false);
                _secondPlayerLivesImage.gameObject.SetActive(false);
            }
            StartCoroutine(FlickerMessage());
            StartCoroutine(FlickerText());
        }

        IEnumerator FlickerMessage()
        {
            while(true)
            {
                _reloadMessage.gameObject.SetActive(false);
                yield return new WaitForSeconds(2f);
                _reloadMessage.gameObject.SetActive(true);
                yield return new WaitForSeconds(1f);
            }
        }

        IEnumerator FlickerText()
        {
            while(true)
            {
                _gameOverText.gameObject.SetActive(false);
                yield return new WaitForSeconds(0.5f);
                _gameOverText.gameObject.SetActive(true);
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
