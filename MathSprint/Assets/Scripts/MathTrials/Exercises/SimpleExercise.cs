using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.MathTrials.Exercises
{
    public class SimpleExercise : IExercise
    {
        private readonly string _answer;
        public string Name { get;  }
        public string Text { get;  }
        public Sprite Sprite { get;  }

        public SimpleExercise(string name, string text, Sprite sprite, string answer)
        {
            Name = name;
            Text = text;
            Sprite = sprite;
            _answer = answer;
        }

        public bool ValidateAnswer(string answer)
        {
            return answer.Equals(_answer);
        }
    }
}
