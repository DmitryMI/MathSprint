using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Entities;
using UnityEngine;

namespace Assets.Scripts.Game
{
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

        public float MinY => _minY;

        void Start()
        {
            _instance = this;

            if (_playerInstance == null)
            {
                _playerInstance = GameObject.FindObjectOfType<Player>();
            }
        }

        public event Action<int> OnLifeCountChanged;
        public event Action<bool> OnGamePause;

        public int LifeCount => _lifeCount;

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

        public void RequestNextLevel()
        {
            // TODO Load next scene
        }

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
