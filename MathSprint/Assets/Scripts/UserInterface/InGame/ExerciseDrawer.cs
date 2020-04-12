using System;
using Assets.Scripts.Entities;
using Assets.Scripts.MathTrials.Exercises;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UserInterface.InGame
{
    public class ExerciseDrawer : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private Image _imageDrawer;
        [SerializeField] private Text _textDrawer;
        [SerializeField] private Button _button;
        [SerializeField] private InputField _inputField;
#pragma warning restore 649

        private Vector2 _imageDrawerInitialSize;

        private IExercise _exercise;
        private Action<bool> _onAnswerCallback;

        private bool _initialized = false;

        void DoInitialization()
        {
            if (_imageDrawer == null)
            {
                Debug.LogError("ImageDrawer not assigned");
            }
            else
            {
                RectTransform imageDrawerRectTransform = _imageDrawer.GetComponent<RectTransform>();
                Vector2 imageDrawerSize = imageDrawerRectTransform.rect.size;
                _imageDrawerInitialSize = imageDrawerSize;
            }
            if (_textDrawer == null)
            {
                Debug.LogError("TextDrawer not assigned");
            }
            if (_button == null)
            {
                Debug.LogError("Button not assigned");
            }
            else
            {
                _button.onClick.AddListener(OnButtonClick);
            }
            if (_inputField == null)
            {
                Debug.LogError("InputField not assigned");
            }

            
        }

        void Start()
        {
            if (!_initialized)
            {
                DoInitialization();
                _initialized = true;
            }
        }

        public void SetCallback(Action<bool> onAnswer)
        {
            _onAnswerCallback = onAnswer;
        }

        public void ShowExercise(Action<bool> onAnswer, IExercise exercise)
        {
            SetCallback(onAnswer);
            ShowExercise(exercise);
        }

        private void SetImageSprite(Sprite sprite)
        {
            GameObject drawerGameObject = _imageDrawer.gameObject;
            RectTransform rectTransform = drawerGameObject.GetComponent<RectTransform>();
            _imageDrawer.sprite = sprite;

            if (sprite == null)
            {
                Debug.LogError("Sprite is null!");
                return;
            }

            Vector2 textureSize = sprite.textureRect.size;

            if (textureSize.y <= textureSize.x)
            {
                float widthRatio = _imageDrawerInitialSize.x / textureSize.x;
                float nHeight = textureSize.y * widthRatio;
                Vector2 drawerSizeNew = new Vector2(_imageDrawerInitialSize.x, nHeight);

                float deltaX = (drawerSizeNew.x - _imageDrawerInitialSize.x);
                float deltaY = (drawerSizeNew.y - _imageDrawerInitialSize.y);

                Vector2 sizeDelta = new Vector2(deltaX, deltaY);

                rectTransform.sizeDelta = sizeDelta;
            }
            else if (textureSize.y > textureSize.x)
            {
                float heightRatio = _imageDrawerInitialSize.y / textureSize.y;
                float nWidth = textureSize.x * heightRatio;
                Vector2 drawerSizeNew = new Vector2(nWidth, _imageDrawerInitialSize.y);

                float deltaX = (drawerSizeNew.x - _imageDrawerInitialSize.x);
                float deltaY = (drawerSizeNew.y - _imageDrawerInitialSize.y);

                Vector2 sizeDelta = new Vector2(deltaX, deltaY);

                rectTransform.sizeDelta = sizeDelta;
            }
        }

        public void ShowExercise(IExercise exercise)
        {
            if (!_initialized)
            {
                DoInitialization();
                _initialized = true;
            }

            if (exercise != null)
            {
                _exercise = exercise;
                _textDrawer.text = exercise.Text;
                SetImageSprite(exercise.Sprite);
            }

            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void OnButtonClick()
        {
            string answer = _inputField.text;
            if (_exercise != null)
            {
                bool correct = _exercise.ValidateAnswer(answer);
                _onAnswerCallback?.Invoke(correct);
            }
            else
            {
                _onAnswerCallback?.Invoke(true);
            }
        }
    }
}
