using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.EntityControls.User;

namespace Assets.Scripts.EntityControls
{
    /// <summary>
    /// Creates instance of User's input manager
    /// </summary>
    public static class InputManagerBuilder
    {
        private static IInputManager _instance;
        /// <summary>
        /// Get instance
        /// </summary>
        /// <returns>User's input</returns>
        public static IInputManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new PlayerInputManager();
            }
            return _instance;
        }
    }
}
