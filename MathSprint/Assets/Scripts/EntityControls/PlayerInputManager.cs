using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Behaviour;
using UnityEngine;

namespace Assets.Scripts.EntityControls
{
    public class PlayerInputManager : IUpdateable, IInputManager
    {
        private IControllable _playerControllable;

        public IControllable PlayerControllable
        {
            get => _playerControllable;
            set => _playerControllable = value;
        }

        void RegisterUpdateable()
        {
            BehaviourManager manager = BehaviourManager.Instance;
            if (manager != null)
            {
                manager.Add(this);
            }
            else
            {
                Debug.LogError("Behaviour Manager not present!");
            }
        }

        public PlayerInputManager()
        {
            RegisterUpdateable();
        }

        public void OnUpdate()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            float jump = Input.GetAxis("Jump");

            _playerControllable?.OnPlayerInput(horizontal, vertical, jump);
        }
    }
}
