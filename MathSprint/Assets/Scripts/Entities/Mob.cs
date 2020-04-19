using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Game;
using Assets.Scripts.MathTrials.Exercises;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    /// <summary>
    /// Base class for all NPC enemies in MathSprint
    /// </summary>
    public abstract class Mob : MonoBehaviour
    {
        [SerializeField]
#pragma warning disable 649
        private int _damage;
#pragma warning restore 649

        protected int Damage => _damage;

        /// <summary>
        /// Mob's associated exercise
        /// </summary>
        public abstract IExercise Exercise { get; }

        /// <summary>
        /// Default callback for MathTrial answer input
        /// </summary>
        /// <param name="answerCorrect">was user's answer correct</param>
        public virtual void MathTrialComplete(bool answerCorrect)
        {
            if (!answerCorrect)
            {
                GameManager.Instance.RequestDamage(_damage);
            }
            
            Destroy(gameObject);
        }

        

        
    }
}
