using System;
using Assets.Scripts.Behaviour;
using Assets.Scripts.EntityControls;
using Assets.Scripts.EntityControls.Mob;
using Assets.Scripts.Game;
using Assets.Scripts.MathTrials;
using Assets.Scripts.MathTrials.Exercises;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    /// <summary>
    /// One of mob types. Dangerous killing machine
    /// </summary>
    public class SpikeBall : Mob, ICollisionMob, IUpdateable
    {
        [SerializeField]
#pragma warning disable 649
        private float _speed;
#pragma warning restore 649

        private Rigidbody2D _rigidbody2D;

        private Action<Collision2D> _collisionHandler;
        private Animator _animator;

        private IExercise[] _exercises;

        private IExercise _currentExercise;

        public event Action<Collision2D> CollisionEnterEvent;

        void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();

            GameManager.Instance.OnGamePause += OnGamePause;
            BehaviourManager.Instance.Add(this);

            _exercises = MathTrialManager.Instance.GetExercisesByName("SpikeBall");
            _currentExercise = ArrayUtils.GetRandomItem(_exercises);

            Debug.Log($"{gameObject.name}: setting exercise to {_currentExercise.Name}");
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
        public override IExercise Exercise => _currentExercise;

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
            BehaviourManager.Instance.Remove(this);
        }

        public void OnUpdate()
        {
            if (transform.position.y < GameManager.Instance.MinY)
            {
                Destroy(gameObject);
            }
        }
    }
}
