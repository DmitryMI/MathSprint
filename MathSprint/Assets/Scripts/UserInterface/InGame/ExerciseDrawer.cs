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

        private IExercise _exercise;
        private Action<bool> _onAnswerCallback;

        void Start()
        {
            if (_imageDrawer == null)
            {
                Debug.LogError("ImageDrawer not assigned");
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

        public void SetCallback(Action<bool> onAnswer)
        {
            _onAnswerCallback = onAnswer;
        }

        public void ShowExercise(Action<bool> onAnswer, IExercise exercise)
        {
            SetCallback(onAnswer);
            ShowExercise(exercise);
        }

        public void ShowExercise(IExercise exercise)
        {
            if (exercise != null)
            {
                _exercise = exercise;
                _textDrawer.text = exercise.Text;
                _imageDrawer.sprite = exercise.Sprite;
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
