using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Behaviour;
using Assets.Scripts.EntityControls;
using Assets.Scripts.Game;
using UnityEngine;


namespace Assets.Scripts.Entities
{
    public class Player : MonoBehaviour, IUpdateable, IControllable
    {
        #region Singleton
        private static Player _instance;
        public static Player Instance => _instance;
        #endregion

#pragma warning disable 649
        [SerializeField]
        private float _acceleration;

        [SerializeField] private float _airCoefficient;
        [SerializeField] private float _jumpForce;
        [SerializeField]
        private bool _isGrounded;
#pragma warning restore 649

        private Rigidbody2D _rigidbody2D;
        private Collider2D _collider2D;
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;

        private Transform _colliderBottomMarker;

        [SerializeField]
        private bool _invulnerabilityAnimation;

        private void RegisterUpdateable()
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

        private void UnregisterUpdateable()
        {
            BehaviourManager manager = BehaviourManager.Instance;
            if (manager != null)
            {
                manager.Remove(this);
            }
            else
            {
                Debug.LogError("Behaviour Manager not present!");
            }
        }

        private void RegisterControllable()
        {
            IInputManager inputManager = InputManagerBuilder.GetInstance();
            inputManager.PlayerControllable = this;
        }

        private void InitComponents()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _collider2D = GetComponent<Collider2D>();

            _colliderBottomMarker = transform.Find("ColliderBottom");
            _animator = GetComponent<Animator>();

            GameObject body = transform.Find("Body").gameObject;
            _spriteRenderer = body.GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _instance = this;
            RegisterUpdateable();
            RegisterControllable();
            InitComponents();
        }

        private void Animate(int direction, bool running, bool jumping)
        {
            if (direction == -1)
            {
                _spriteRenderer.flipX = true;
            }
            else
            {
                _spriteRenderer.flipX = false;
            }

            _animator.SetBool("Jumping", jumping);
            _animator.SetInteger("Running",running ? 1 : 0);
        }

        private void AnimateDirection(float direction)
        {
            if (direction < 0)
            {
                _spriteRenderer.flipX = true;
            }
            else
            {
                _spriteRenderer.flipX = false;
            }
        }

        private void AnimateRunning(bool running)
        {
            _animator.SetInteger("Running", running ? 1 : 0);
        }

        private void AnimateJumping(bool jumping)
        {
            _animator.SetBool("Jumping", jumping);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            AnimateJumping(false);
        }

        public void AnimateBlinking(bool blinking)
        {
            _invulnerabilityAnimation = blinking;
            if(blinking)
            { 
                StartCoroutine(BlinkCoroutine());
            }
        }

        private void SetTransparency(float alpha)
        {
            Color color = _spriteRenderer.color;
            color.a = alpha;
            _spriteRenderer.color = color;
        }

        private IEnumerator BlinkCoroutine()
        {
            bool state = false;
            while (_invulnerabilityAnimation)
            {
                yield return new WaitForSeconds(0.1f);
                if (state)
                {
                    SetTransparency(0.2f);
                }
                else
                {
                    SetTransparency(1);
                }

                state = !state;
            }
            SetTransparency(1);
        }


        public void OnUpdate()
        {
            if (transform.position.y < GameManager.Instance.MinY)
            {
                GameManager.Instance.RequestDamage(Int32.MaxValue);
            }
            //_animator.SetBool("Blinking", _invulnerabilityAnimation);
        }

        public void OnControlInput(float horizontal, float vertical, float jump)
        {
            if (_rigidbody2D == null)
            {
                return;
            }

            float horizontalProjection = _rigidbody2D.velocity.x;
            float velocity2 = horizontalProjection * horizontalProjection;
            int direction = Math.Sign(horizontalProjection);

            AnimateDirection(horizontal);

            if (Math.Abs(horizontal) < 0.001f)
            {
                AnimateRunning(false);
                float velocityStopperValue = Math.Abs(horizontalProjection * 10);
                Vector2 stoppingForce = -direction * Vector2.right * velocityStopperValue * _airCoefficient / _rigidbody2D.mass;
                _rigidbody2D.AddForce(stoppingForce);
            }
            else
            {
                AnimateRunning(true);
                float movementForce = horizontal * _acceleration - direction * 
                                      _airCoefficient * velocity2 / _rigidbody2D.mass;
                _rigidbody2D.AddForce(Vector2.right * movementForce);
            }

            Vector3 corner = new Vector3(0.01f, 0.01f);

            _isGrounded = Physics2D.OverlapArea(_colliderBottomMarker.position - corner,
                _colliderBottomMarker.position + corner, LayerMask.GetMask("Ground"));

            if (_isGrounded && jump > 0)
            {
                Vector2 jumpVector = Vector2.up * _jumpForce;
                Vector3 position = gameObject.transform.position;
                position.y += 0.03f;
                gameObject.transform.position = position;
                _rigidbody2D.AddForce(jumpVector, ForceMode2D.Impulse);
                AnimateJumping(true);
            }
        }
    }
}
