using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Behaviour;
using Assets.Scripts.Entities;
using UnityEngine;

namespace Assets.Scripts.EntityControls.Mob
{
    /// <summary>
    /// Default blob AI controller
    /// </summary>
    public class BlobController : MonoBehaviour, IUpdateable
    {
        [SerializeField]
#pragma warning disable 649
        private float _reactionDistance;
#pragma warning restore 649

        private IPointControllable _mob;
        private float _reactionDistance2;

        [SerializeField] private float _activationTimeout;

        private bool _active;

        void Start()
        {
            BehaviourManager.Instance.Add(this);
            _mob = GetComponent<IPointControllable>();

            _reactionDistance2 = _reactionDistance * _reactionDistance;

            StartCoroutine(ActivationTimer());
        }

        public void OnUpdate()
        {
            if (!_active)
            {
                return;
            }
            if (Player.Instance == null)
            {
                return;
            }

            Vector3 playerPosition = Player.Instance.transform.position;

            Vector3 mobPos = _mob.CurrentPosition;
            Vector3 delta = playerPosition - mobPos;

            if (delta.magnitude < _reactionDistance)
            {
                _mob.OnControlInput(playerPosition);
            }
        }

        IEnumerator ActivationTimer()
        {
            yield return new WaitForSeconds(_activationTimeout);
            _active = true;
        }

        void OnDestroy()
        {
            BehaviourManager.Instance.Remove(this);
        }
    }
}
