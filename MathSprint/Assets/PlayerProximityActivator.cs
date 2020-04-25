using Assets.Scripts.Behaviour;
using Assets.Scripts.Entities;
using UnityEngine;

namespace Assets
{
    public class PlayerProximityActivator : MonoBehaviour, IUpdateable
    {
        private Mob _controlledMob;
        [SerializeField]
        private bool _activated = false;

        [SerializeField] private RigidbodyConstraints2D _activatedConstraints;
        [SerializeField] private RigidbodyConstraints2D _inactiveConstraints;
        [SerializeField]
        private bool _useX;
        [SerializeField]
        private bool _userY;

        [SerializeField] private bool _makeInvisible;

        [SerializeField] private Vector2 _minDistance;

        private Rigidbody2D _rigidbody2D;
        // Start is called before the first frame update
        void Start()
        {
            _controlledMob = GetComponent<Mob>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _rigidbody2D.constraints = _inactiveConstraints;
            if (_makeInvisible)
            {
                //_controlledMob.gameObject.SetActive(false);
                _controlledMob.gameObject.layer = LayerMask.NameToLayer("Invisible");
            }

            BehaviourManager.Instance.Add(this);
        }

        // Update is called once per frame
        public void OnUpdate()
        {
            if (_activated)
            {
                return;
            }

            Player player = Player.Instance;
            if (player == null)
            {
                return;
            }
            Vector2 delta = _controlledMob.transform.position - player.transform.position;

            if (_useX && !_userY)
            {
                _activated = delta.x < _minDistance.x;
            }
            else if (_userY && !_useX)
            {
                _activated = delta.y < _minDistance.y;
            }
            else if (_useX && _userY)
            {
                _activated = delta.y < _minDistance.y && delta.x < _minDistance.x;
            }
            else
            {
                // Will never activate
            }

            if (_activated)
            {
                _rigidbody2D.constraints = _activatedConstraints;
                _rigidbody2D.AddForce(Vector2.down * 0.01f);
                if (_makeInvisible)
                {
                    _controlledMob.gameObject.layer = LayerMask.NameToLayer("Default");
                }
            }
        }

        void OnDestroy()
        {
            BehaviourManager.Instance.Remove(this);
        }
    }
}
