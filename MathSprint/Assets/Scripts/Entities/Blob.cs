using Assets.Scripts.Behaviour;
using Assets.Scripts.EntityControls.Mob;
using Assets.Scripts.Game;
using Assets.Scripts.MathTrials;
using Assets.Scripts.MathTrials.Exercises;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class Blob : Mob, IPointControllable, IUpdateable
    {
        [SerializeField]
#pragma warning disable 649
        private float _jumpCooldownSeconds;

        [SerializeField] private float _forwardForce;

        [SerializeField]
        private float _jumpingForce;

        [SerializeField] private bool _spriteFlipped;
#pragma warning restore 649

        private Vector2 _orderTargetPoint;
        private float _jumpCooldownCounter;
        private Rigidbody2D _rigidbody2d;
        private SpriteRenderer _spriteRenderer;
        private Animator _animator;

        public Vector2 CurrentPosition => transform.position;

        void Start()
        {
            BehaviourManager.Instance.Add(this);
            _rigidbody2d = GetComponent<Rigidbody2D>();

            Transform body = transform.Find("Body");
            _spriteRenderer = body.gameObject.GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();

            GameManager.Instance.OnGamePause += OnGamePause;
        }

        public void OnControlInput(Vector2 targetPoint)
        {
            _orderTargetPoint = targetPoint;
        }

        private void JumpTowardsPoint(Vector2 targetPoint)
        {
            float deltaX = targetPoint.x - transform.position.x;
            if (deltaX > 0)
            {
                AnimateDirection(1);
            }
            else
            {
                AnimateDirection(-1);
            }
            AnimateJumping(true);

            transform.position += Vector3.up * 0.01f;

            Vector2 forwardForce = new Vector2(deltaX * _forwardForce, 0);
            _rigidbody2d.AddForce(forwardForce, ForceMode2D.Force);

            Vector2 jumpingForce = new Vector2(0, _jumpingForce);
            _rigidbody2d.AddForce(jumpingForce, ForceMode2D.Impulse);

            
        }

        private void ApplyStoppingForce()
        {
            _rigidbody2d.velocity = Vector2.zero;
        }

        private void OnGamePause(bool pause)
        {
            if(_rigidbody2d == null)
                return;
            if (pause)
            {
                _rigidbody2d.constraints |= RigidbodyConstraints2D.FreezeAll;
            }
            else
            {
                _rigidbody2d.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag.Equals("Player"))
            {
                MathTrialManager.Instance.RequestMathTrial(this);
            }
            else
            {
                AnimateJumping(false);
                ApplyStoppingForce();
            }
        }

        private void AnimateDirection(int direction)
        {
            bool xFlip = direction >= 0;

            if (_spriteFlipped)
            {
                xFlip = !xFlip;
            }

            _spriteRenderer.flipX = xFlip;
        }

        private void AnimateJumping(bool jumping)
        {
            _animator.SetBool("Jumping", jumping);

        }

        public void OnUpdate()
        {
            if (_jumpCooldownCounter > 0)
            {
                _jumpCooldownCounter -= Time.deltaTime;
                return;
            }

            _jumpCooldownCounter = 0;

            float deltaX = _orderTargetPoint.x - transform.position.x;
            if (Mathf.Abs(deltaX) > 0.01f)
            {
                JumpTowardsPoint(_orderTargetPoint);
            }

            _jumpCooldownCounter = _jumpCooldownSeconds;
        }

        public override IExercise Exercise { get; }


        void OnDestroy()
        {
            BehaviourManager.Instance.Remove(this);
        }
    }
}
