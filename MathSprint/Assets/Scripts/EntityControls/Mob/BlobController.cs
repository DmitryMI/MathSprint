using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Behaviour;
using Assets.Scripts.Entities;
using UnityEngine;

namespace Assets.Scripts.EntityControls.Mob
{
    public class BlobController : MonoBehaviour, IUpdateable
    {
        [SerializeField]
        private float _reactionDistance;

        private IPointControllable _mob;
        private float _reactionDistance2;

        void Start()
        {
            BehaviourManager.Instance.Add(this);
            _mob = GetComponent<IPointControllable>();

            _reactionDistance2 = _reactionDistance * _reactionDistance;
        }

        public void OnUpdate()
        {
            if (Player.Instance == null)
            {
                return;
            }

            Vector3 playerPosition = Player.Instance.transform.position;

            Vector3 mobPos = _mob.CurrentPosition;
            Vector3 delta = playerPosition - mobPos;

            if (delta.sqrMagnitude < _reactionDistance2)
            {
                _mob.OnControlInput(playerPosition);
            }
        }
    }
}
