using UnityEngine;

namespace Assets.Scripts.Block
{
    public class BlockAtom : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private Sprite _standardSprite;
        [SerializeField] private Sprite _leftEndingSprite;
        [SerializeField] private Sprite _rightEndingSprite;
        [SerializeField] private Sprite _bothEndingSprite;
#pragma warning restore 649

        private SpriteRenderer _spriteRenderer;

        public void SetLeftEnding()
        {
            if (_spriteRenderer == null)
            {
                _spriteRenderer = GetComponent<SpriteRenderer>();
            }

            _spriteRenderer.sprite = _leftEndingSprite;
        }

        public void SetRightEnding()
        {
            if (_spriteRenderer == null)
            {
                _spriteRenderer = GetComponent<SpriteRenderer>();
            }
            _spriteRenderer.sprite = _rightEndingSprite;
        }

        public void SetBothEnding()
        {
            if (_spriteRenderer == null)
            {
                _spriteRenderer = GetComponent<SpriteRenderer>();
            }
            _spriteRenderer.sprite = _bothEndingSprite;
        }

        public void SetNoEnding()
        {
            if (_spriteRenderer == null)
            {
                _spriteRenderer = GetComponent<SpriteRenderer>();
            }
            _spriteRenderer.sprite = _standardSprite;
        }

        // Start is called before the first frame update
        void Start()
        {
            if (_spriteRenderer == null)
            {
                _spriteRenderer = GetComponent<SpriteRenderer>();
            }
        }

    }
}
