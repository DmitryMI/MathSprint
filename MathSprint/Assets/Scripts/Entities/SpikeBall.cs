using System;
using Assets.Scripts.Behaviour;
using Assets.Scripts.EntityControls;
using Assets.Scripts.EntityControls.Mob;
using Assets.Scripts.Game;
using Assets.Scripts.MathTrials;
using Assets.Scripts.MathTrials.Exercises;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class SpikeBall : Mob, ICollisionMob
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

            GameManager.Instance.OnGamePause += OnGamePause;
        }

        public void OnControlInput(float horizontal, float vertical, float jump)
        {
            if(_rigidbody2D != null)
                _rigidbody2D.velocity = new Vector3(horizontal, vertical) * _speed;

            if(_animator != null)
                _animator.SetFloat("RotationSpeed", horizontal);
        }

        void OnCollisionEnter2D(Collision2D collision2D)
        {
            if (collision2D.gameObject.tag.Equals("Player"))
            {
                MathTrialManager.Instance.RequestMathTrial(this);
            }
            else
            {
                CollisionEnterEvent?.Invoke(collision2D);
            }
        }

        public Collider2D Collider2D => GetComponent<Collider2D>();
        public override IExercise Exercise { get; }

        private void OnGamePause(bool pause)
        {
            if (_rigidbody2D == null)
            {
                return;
            }
            if (pause)
            {
                _rigidbody2D.constraints |= RigidbodyConstraints2D.FreezeAll;
            }
            else
            {
                _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }

        void OnDestroy()
        {
            
        }
    }
}
