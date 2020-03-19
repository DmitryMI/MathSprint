using System;
using Assets.Scripts.Behaviour;
using Assets.Scripts.EntityControls;
using Assets.Scripts.EntityControls.Mob;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class SpikeBall : MonoBehaviour, ICollisionMob
    {
        [SerializeField]
        private float _speed;

        private Rigidbody2D _rigidbody2D;

        private Action<Collision2D> _collisionHandler;
        private Animator _animator;

        public event Action<Collision2D> CollisionEnterEvent;

        void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        public void OnControlInput(float horizontal, float vertical, float jump)
        {
            _rigidbody2D.velocity = new Vector3(horizontal, vertical) * _speed;

            _animator.SetFloat("RotationSpeed", horizontal);
        }

        void OnCollisionEnter2D(Collision2D collision2D)
        {
            CollisionEnterEvent?.Invoke(collision2D);
        }

        public Collider2D Collider2D => GetComponent<Collider2D>();
    }
}
