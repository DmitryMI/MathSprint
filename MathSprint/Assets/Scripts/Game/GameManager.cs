using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Behaviour;
using Assets.Scripts.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Game
{
    /// <summary>
    /// Manages game logic
    /// </summary>
    public class GameManager : MonoBehaviour, IUpdateable
    {
        #region Singleton

        private static GameManager _instance;
        public static GameManager Instance => _instance;

        #endregion

        [SerializeField]
        private Player _playerInstance;
        [SerializeField]
        private int _lifeCount;

        [SerializeField] private GameObject _gameOverMessage;

        [SerializeField] private float _minY;

        [SerializeField] private GameObject _menuMessage;

        /// <summary>
        /// Minimal Y coordinate of the map
        /// </summary>
        public float MinY => _minY;

        void Start()
        {
            _instance = this;

            if (_playerInstance == null)
            {
                _playerInstance = GameObject.FindObjectOfType<Player>();
            }

            BehaviourManager.Instance.Add(this);
        }

        /// <summary>
        /// Event registry for Life count change
        /// </summary>
        public event Action<int> OnLifeCountChanged;
        /// <summary>
        /// Event registry for game pause
        /// </summary>
        public event Action<bool> OnGamePause;

        /// <summary>
        /// Player's lives
        /// </summary>
        public int LifeCount => _lifeCount;

        /// <summary>
        /// Request damage dealing to current player
        /// </summary>
        /// <param name="damage"></param>
        public void RequestDamage(int damage)
        {
            _lifeCount -= damage;

            if (_lifeCount < 0)
            {
                _lifeCount = 0;
            }

            OnLifeCountChanged?.Invoke(_lifeCount);

            if (_lifeCount == 0)
            {
                OnLifeZero(); 
            }
        }

        /// <summary>
        /// Requests next level
        /// </summary>
        /// <param name="index">Next level index</param>
        public void RequestNextLevel(int index)
        {
            SceneManager.LoadScene(index);
        }

        /// <summary>
        /// Closes application
        /// </summary>
        public void QuitGame()
        {
            Application.Quit(0);
        }

        /// <summary>
        /// Request game pause
        /// </summary>
        /// <param name="pause">true if pause, false if resume</param>
        public void RequestGamePause(bool pause)
        {
            OnGamePause?.Invoke(pause);
            if (pause)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
            // TODO Game pause
        }

        private void OnLifeZero()
        {
            Debug.Log("Game over!");
            //RequestGamePause(true);
            
            _gameOverMessage.SetActive(true);
        }

        public void OnUpdate()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                if (_menuMessage.activeSelf)
                {
                    _menuMessage.SetActive(false);
                    RequestGamePause(false);
                }
                else
                {
                    _menuMessage.SetActive(true);
                    RequestGamePause(true);
                }
            }
        }

        public void ContinueButtonClick()
        {
            RequestGamePause(false);
            _menuMessage.SetActive(false);
        }
    }
}
