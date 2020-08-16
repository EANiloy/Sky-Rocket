using SpaceShip.Spawning;
using SpaceShip.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

namespace SpaceShip.Core
{

    public class GameManager : MonoBehaviour
    {
        [SerializeField] private bool _isGameOver = false;
        [SerializeField] public bool _isCoOp = false;
        [SerializeField] public bool _isPlayerOneDead = false;
        [SerializeField] public bool _isPlayerTwoDead = false;

        private bool _gameOverSequenceStarted = false;

        private void Update() {
            if(Input.GetKeyDown(KeyCode.R) && _isGameOver)
            {
                SceneManager.LoadScene(0);
            }
            #if UNITY_ANDROID
            if(Input.touchCount > 0 && _isGameOver)
            {
                SceneManager.LoadScene(0);
            }
            #endif
            if(_isCoOp == true)
            {
                if (_isPlayerOneDead && _isPlayerTwoDead && _isGameOver == false)
                {
                    GameOver();
                }
            }
            else
            {
                if(_isPlayerOneDead && _isGameOver == false)
                {
                    GameOver();
                }
            }
        }

        public void UpdatePlayers(bool isFirstPlayer)
        {
            if(isFirstPlayer == true)
            {
                _isPlayerOneDead = true;
            }
            else
            {
                _isPlayerTwoDead = true;
            }
        }

        public void GameOver()
        {
            SpawnManager spawnmanager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
            if(spawnmanager == null)
            {
                Debug.Log("Spawn manager in gamemanager is null");
            }
            UIManager uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
            if(uIManager == null)
            {
                Debug.Log("UI Manager in gamemanager is null");
            }
            _isGameOver = true;
            spawnmanager.StopSpawning();
            uIManager.GameOverSequence();
        }
    }

}