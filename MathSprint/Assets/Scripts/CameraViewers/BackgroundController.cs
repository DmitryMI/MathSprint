using System.Collections.Generic;
using Assets.Scripts.Behaviour;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.CameraViewers
{
    public class BackgroundController : MonoBehaviour, IUpdateable
    {
        #region Singleton

        private static BackgroundController _instance;
        public static BackgroundController Instanc => _instance;

        #endregion

#pragma warning disable 649
        [SerializeField]
        private Sprite _backgroundTileSprite;

        [SerializeField] private GameObject _backgroundTilePrefab;

        [SerializeField]
        private float _relativeSpeed;
#pragma warning restore 649

        [SerializeField] private Bounds _playableZoneBounds;


        private Camera _mainCamera;

        private Vector2 _cameraPreviousPosition;

        private const float OffsetZ = 10.0f;

        // Start is called before the first frame update
        public void Start()
        {
            _instance = this;

            BehaviourManager.Instance.Add(this);
            _mainCamera = Camera.main;
            _cameraPreviousPosition = _mainCamera.transform.position;

            Vector3 reposition = (Vector3) _cameraPreviousPosition;
            reposition.z = OffsetZ;
            transform.position = reposition;
        }

        public void OnUpdate()
        {
            Vector2 cameraPosition = _mainCamera.transform.position;
            Vector2 delta = cameraPosition - _cameraPreviousPosition;
            _cameraPreviousPosition = cameraPosition;

            Vector2 deltaScaled = delta * _relativeSpeed;

            transform.position += (Vector3)deltaScaled;
        }

        public void ConstructBackground()
        {
            float pixelPerUnit = _backgroundTileSprite.pixelsPerUnit;
            float spriteWidth = _backgroundTileSprite.texture.width / pixelPerUnit;
            float spriteHeight = _backgroundTileSprite.texture.height / pixelPerUnit;

            List<Transform> children = new List<Transform>(transform.childCount);
            for (int i = 0; i < transform.childCount; i++)
            {
                children.Add(transform.GetChild(i));
            }

            for (int i = 0; i < children.Count; i++)
            {
                DestroyImmediate(children[i].gameObject);
            }

            Vector3 boundsSize = _playableZoneBounds.size;

            int tileWidth = (int)Mathf.Ceil(boundsSize.x * _relativeSpeed);
            int tileHeight = (int)Mathf.Ceil(boundsSize.y * _relativeSpeed);

            for (int x = -1; x < tileWidth; x++)
            {
                for (int y = -1; y < tileHeight; y++)
                {
                    float tileX = x * spriteWidth + _playableZoneBounds.center.x;
                    float tileY = y * spriteHeight + _playableZoneBounds.center.y;
                    Vector3 tilePosition = new Vector3(tileX, tileY, OffsetZ);
                    GameObject tileGameObject = (GameObject)PrefabUtility.InstantiatePrefab(_backgroundTilePrefab);
                    tileGameObject.transform.position = tilePosition;
                    SpriteRenderer spriteRenderer = tileGameObject.GetComponent<SpriteRenderer>();
                    spriteRenderer.sprite = _backgroundTileSprite;
                    tileGameObject.transform.parent = transform;
                }
            }

            Vector3 cameraPosition = Camera.main.transform.position;
            cameraPosition.z = OffsetZ;
            transform.position = cameraPosition;
        }

        [MenuItem("Background/Construct background")]
        public static void ConstructBackgroundMenu()
        {
            BackgroundController controller = GameObject.FindObjectOfType<BackgroundController>();
            _instance = controller;

            _instance.ConstructBackground();
        }
    }
}
