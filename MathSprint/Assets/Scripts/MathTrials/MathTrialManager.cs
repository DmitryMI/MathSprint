using Assets.Scripts.Entities;
using Assets.Scripts.Game;
using Assets.Scripts.MathTrials.Exercises;
using Assets.Scripts.UserInterface.InGame;
using UnityEngine;

namespace Assets.Scripts.MathTrials
{
    public class MathTrialManager : MonoBehaviour
    {
        #region Singleton

        private static MathTrialManager _instance;
        public static MathTrialManager Instance => _instance;

        #endregion

#pragma warning disable 649
        [SerializeField] private ExerciseDrawer _exerciseDrawerInstance;
#pragma warning restore 649

        private Mob _pendingMob;

        void Start()
        {
            _instance = this;

            if (_exerciseDrawerInstance == null)
            {
                Debug.LogError("ExerciseDrawerInstance not assigned");
            }
        }

        public void RequestMathTrial(Mob sourceMob)
        {
            Debug.Log("MathTrial requested");
            IExercise exercise = sourceMob.Exercise;

            _pendingMob = sourceMob;

            GameManager.Instance.RequestGamePause(true);
            ShowExercise(exercise);
        }

        private void ShowExercise(IExercise exercise)
        {
            _exerciseDrawerInstance?.ShowExercise(OnExerciseAnswer, exercise);
        }

        public void OnExerciseAnswer(bool correct)
        {
            Debug.Log("Exercise answered");
            _exerciseDrawerInstance.Hide();
            GameManager.Instance.RequestGamePause(false);
            _pendingMob.MathTrialComplete(correct);
            _pendingMob = null;
        }
    }
}
