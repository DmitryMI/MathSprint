using Assets.Scripts.Behaviour;
using UnityEngine;

namespace Assets.Scripts.CameraViewers
{
    /// <summary>
    /// Camera's component to enable player tracking
    /// </summary>
    public class TrackingCamera : MonoBehaviour, IUpdateable
    {
        [SerializeField]
#pragma warning disable 649
        private Transform _viewedObject;
#pragma warning restore 649

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
