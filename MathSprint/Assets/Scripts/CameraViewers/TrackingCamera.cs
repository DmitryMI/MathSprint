using Assets.Scripts.Behaviour;
using UnityEngine;

namespace Assets.Scripts.CameraViewers
{
    public class TrackingCamera : MonoBehaviour, IUpdateable
    {
        [SerializeField]
        private Transform _viewedObject;

        [SerializeField]
        private float _cameraZ = 10.0f;

        void Start()
        { 
            BehaviourManager.Instance.Add(this);
        }


        public void OnUpdate()
        {
            Vector3 position = _viewedObject.position;
            position.z = _cameraZ;
            transform.position = position;
        }
    }
}
