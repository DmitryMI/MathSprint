using UnityEngine;

namespace Assets.Scripts.Block
{
    public class Block : MonoBehaviour
    {
        [SerializeField]
        private int _horizontalLength;

        [SerializeField] private GameObject _blockAtomPrefab;

        [SerializeField] private bool _leftEndingEnabled;
        [SerializeField] private bool _rightEndingEnabled;
        [SerializeField] private bool _topEndingEnabled;

        public GameObject BlockAtomPrefab => _blockAtomPrefab;

        public bool LeftEndingEnabled => _leftEndingEnabled;

        public bool RightEndingEnabled => _rightEndingEnabled;

        public bool TopEndingEnabled => _topEndingEnabled;

        public int HorizontalLength => _horizontalLength;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
