using UnityEngine;

namespace Assets.Scripts.Block
{
    /// <summary>
    /// Represents surface block
    /// </summary>
    public class Block : MonoBehaviour
    {

#pragma warning disable 649
        [SerializeField]
        private int _horizontalLength;
        [SerializeField] private GameObject _blockAtomPrefab;
        [SerializeField] private bool _leftEndingEnabled;
        [SerializeField] private bool _rightEndingEnabled;
        [SerializeField] private bool _topEndingEnabled;
#pragma warning restore 649

        /// <summary>
        /// Atom prefab. Will be cloned to match Block size
        /// </summary>
        public GameObject BlockAtomPrefab => _blockAtomPrefab;

        /// <summary>
        /// Is left ending enabled
        /// </summary>
        public bool LeftEndingEnabled => _leftEndingEnabled;

        /// <summary>
        /// Is right ending enabled
        /// </summary>
        public bool RightEndingEnabled => _rightEndingEnabled;

        /// <summary>
        /// Is top ending enabled
        /// </summary>
        public bool TopEndingEnabled => _topEndingEnabled;

        /// <summary>
        /// Length of atom sequence
        /// </summary>
        public int HorizontalLength => _horizontalLength;
    }
}
