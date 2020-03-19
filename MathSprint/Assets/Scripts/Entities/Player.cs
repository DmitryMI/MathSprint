using System;
using Assets.Scripts.Behaviour;
using Assets.Scripts.EntityControls;
using UnityEngine;


namespace Assets.Scripts.Entities
{
    public class Player : MonoBehaviour, IUpdateable, IControllable
    {
        [SerializeField]
        private float _acceleration;
        [SerializeField] private float _airCoefficient;
        [SerializeField] private float _jumpForce;

        private Rigidbody2D _rigidbody2D;
        private Collider2D _collider2D;

        [SerializeField]
        private bool _isGrounded;

        private Transform _colliderBottomMarker;

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
        }

        private void Start()
        {
            RegisterUpdateable();
            RegisterControllable();
            InitComponents();
        }


        public void OnUpdate()
        {
            
        }

        public void OnPlayerInput(float horizontal, float vertical, float jump)
        {
            if (_rigidbody2D == null)
            {
                return;
            }

            float horizontalProjection = _rigidbody2D.velocity.x;
            float velocity2 = horizontalProjection * horizontalProjection;
            int direction = Math.Sign(horizontalProjection);

            if (Math.Abs(horizontal) < 0.001f)
            {
                float velocityStopperValue = Math.Abs(horizontalProjection * 10);
                Vector2 stoppingForce = -direction * Vector2.right * velocityStopperValue * _airCoefficient / _rigidbody2D.mass;
                _rigidbody2D.AddForce(stoppingForce);
            }
            else
            {
                
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
            }
        }
    }
}
