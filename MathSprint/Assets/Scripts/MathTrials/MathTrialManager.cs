using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Entities;
using Assets.Scripts.Game;
using Assets.Scripts.MathTrials.Exercises;
using Assets.Scripts.UserInterface.InGame;
using UnityEngine;

namespace Assets.Scripts.MathTrials
{
    /// <summary>
    /// Manages math trials
    /// </summary>
    public class MathTrialManager : MonoBehaviour
    {
        #region Singleton

        private static MathTrialManager _instance;
        public static MathTrialManager Instance => _instance;

        #endregion

#pragma warning disable 649
        [SerializeField] private ExerciseDrawer _exerciseDrawerInstance;
        [SerializeField] private float _invulnerabilityDuration;
#pragma warning restore 649

        private IExercise[] _availibleExercises;

        private Mob _pendingMob;

        private bool _temporaryInvulnerable;

        void Start()
        {
            _instance = this;

            if (_exerciseDrawerInstance == null)
            {
                Debug.LogError("ExerciseDrawerInstance not assigned");
            }

            _availibleExercises = ExercisesLoader.LoadFromResources();

            foreach (var exercise in _availibleExercises)
            {
                Debug.Log("Exercise loaded: " + exercise.Name);
            }
        }

        /// <summary>
        /// Return exercise array that start with specified mask
        /// </summary>
        /// <param name="nameStart">Start-word mask</param>
        /// <returns>Array of exercises</returns>
        public IExercise[] GetExercisesByName(string nameStart)
        {
            List<IExercise> exercises = new List<IExercise>();

            foreach (var exercise in _availibleExercises)
            {
                if (exercise.Name.StartsWith(nameStart))
                {
                    exercises.Add(exercise);
                }
            }

            return exercises.ToArray();
        }

        /// <summary>
        /// Request staring trial for the player
        /// </summary>
        /// <param name="sourceMob">Requesting mob</param>
        public void RequestMathTrial(Mob sourceMob)
        {
            Debug.Log("MathTrial requested");
            if (_temporaryInvulnerable)
            {
                return;
            }
            SetInvulnerable(true);

            IExercise exercise = sourceMob.Exercise;
            _pendingMob = sourceMob;
            GameManager.Instance.RequestGamePause(true);
            ShowExercise(exercise);
        }

        private void ShowExercise(IExercise exercise)
        {
            _exerciseDrawerInstance?.ShowExercise(OnExerciseAnswer, exercise);
        }

        private IEnumerator DisableInvulnerabilitySubroutine()
        {
            yield return new WaitForSeconds(_invulnerabilityDuration);
            SetInvulnerable(false);
        }

        private void SetInvulnerable(bool invulnerable)
        {
            _temporaryInvulnerable = invulnerable;
            Player.Instance.AnimateBlinking(invulnerable);
        }

        private void StartInvulnerabilityCountdown()
        {
            StartCoroutine(DisableInvulnerabilitySubroutine());
        }

        /// <summary>
        /// Invoked by UI. Reports user's trial result
        /// </summary>
        /// <param name="correct">Is user's answer correct</param>
        public void OnExerciseAnswer(bool correct)
        {
            Debug.Log($"Exercise answered. Is correct: {correct}");
            _exerciseDrawerInstance.Hide();
            GameManager.Instance.RequestGamePause(false);
            _pendingMob.MathTrialComplete(correct);
            _pendingMob = null;
            StartInvulnerabilityCountdown();
        }
    }
}
