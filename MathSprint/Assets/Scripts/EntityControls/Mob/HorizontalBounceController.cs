using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Behaviour;
using UnityEngine;

namespace Assets.Scripts.EntityControls.Mob
{
    public class HorizontalBounceController : MonoBehaviour, IUpdateable
    {
        [SerializeField]
        private int _direction;

        private ICollisionMob _mob;
        private Collider2D _collider;

        void Start()
        {
            BehaviourManager.Instance.Add(this);
            _mob = GetComponent<ICollisionMob>();
            _mob.CollisionEnterEvent += OnMobCollision;
            _collider = _mob.Collider2D;
        }

        private void OnMobCollision(Collision2D collision)
        {
            float collisionX = collision.collider.transform.position.x;
            float colliderX = _collider.transform.position.x;
            if (collisionX < colliderX && _direction < 0)
            {
                _direction = 1;
            }

            if (collisionX > colliderX && _direction > 0)
            {
                _direction = -1;
            }
        }

        public void OnUpdate()
        {
            float horizontal;
            if (_direction < 0)
            {
                horizontal = -1;
            }
            else if (_direction > 0)
            {
                horizontal = 1;
            }
            else
            {
                horizontal = 0;
            }
            _mob.OnControlInput(horizontal, 0, 0);
        }

        void OnDestroy()
        {
            BehaviourManager.Instance.Remove(this);
        }
    }
}
