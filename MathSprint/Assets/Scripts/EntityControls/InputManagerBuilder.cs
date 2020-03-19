using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.EntityControls
{
    public static class InputManagerBuilder
    {
        private static IInputManager _instance;
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
