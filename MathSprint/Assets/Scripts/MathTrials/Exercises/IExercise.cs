using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.MathTrials.Exercises
{
    public interface IExercise
    {
        string Name { get; }
        string Text { get; }
        Sprite Sprite { get; }
        bool ValidateAnswer(string answer);
    }
}