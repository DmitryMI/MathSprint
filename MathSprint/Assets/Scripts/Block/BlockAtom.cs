using UnityEngine;

namespace Assets.Scripts.Block
{
    /// <summary>
    /// BlockAtoms are used to create surface inside Block's collider
    /// </summary>
    public class BlockAtom : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private Sprite _leftEndingSprite;
        [SerializeField] private Sprite _rightEndingSprite;
        [SerializeField] private Sprite _bothEndingSprite;

        [SerializeField] private Sprite _innerEndingSprite;

        [SerializeField] private Sprite _topLeftEndingSprite;
        [SerializeField] private Sprite _topRightEndingSprite;
        [SerializeField] private Sprite _topBothEndingSprite;
        [SerializeField] private Sprite _topOnlyEndingSprite;
#pragma warning restore 649

        private SpriteRenderer _spriteRenderer;

        /// <summary>
        /// Modifies sprite renderer according to ending settings
        /// </summary>
        /// <param name="top">Is top ending enabled</param>
        /// <param name="left">Is left ending enabled</param>
        /// <param name="right">Is right ending enabled</param>
        public void SetEnding(bool top, bool left, bool right)
        {
            LoadRenderer();
            if (top && left && right)
            {
                _spriteRenderer.sprite = _topBothEndingSprite;
            }
            else if (top && left)
            {
                _spriteRenderer.sprite = _topLeftEndingSprite;
            }
            else if (top && right)
            {
                _spriteRenderer.sprite = _topRightEndingSprite;
            }
            else if (left && right)
            {
                _spriteRenderer.sprite = _bothEndingSprite;
            }
            else if (top)
            {
                _spriteRenderer.sprite = _topOnlyEndingSprite;
            }
            else if (left)
            {
                _spriteRenderer.sprite = _leftEndingSprite;
            }
            else if (right)
            {
                _spriteRenderer.sprite = _rightEndingSprite;
            }
            else
            {
                _spriteRenderer.sprite = _innerEndingSprite;
            }
        }

        private void LoadRenderer()
        {
            if (_spriteRenderer == null)
            {
                _spriteRenderer = GetComponent<SpriteRenderer>();
            }
        }

        // Start is called before the first frame update
        private void Start()
        {
            if (_spriteRenderer == null)
            {
                _spriteRenderer = GetComponent<SpriteRenderer>();
            }
        }

    }
}
