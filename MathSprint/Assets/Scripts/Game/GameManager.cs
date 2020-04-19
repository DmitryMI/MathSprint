using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Entities;
using UnityEngine;

namespace Assets.Scripts.Game
{
    /// <summary>
    /// Manages game logic
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        #region Singleton

        private static GameManager _instance;
        public static GameManager Instance => _instance;

        #endregion

        [SerializeField]
        private Player _playerInstance;
        [SerializeField]
        private int _lifeCount;

        [SerializeField] private float _minY;

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
        /// Request net game level
        /// </summary>
        public void RequestNextLevel()
        {
            // TODO Load next scene
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
            // TODO GameOver splash
            Debug.Log("Game over!");
            RequestGamePause(true);
            Application.Quit();
        }
    }
}
