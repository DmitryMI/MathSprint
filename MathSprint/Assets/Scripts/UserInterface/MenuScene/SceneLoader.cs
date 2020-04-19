using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UserInterface.MenuScene
{
    /// <summary>
    /// Handles level loading
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        /// <summary>
        /// Loads scene with specified index
        /// </summary>
        /// <param name="index"></param>
        public void SceneLoad(int index)
        {
            SceneManager.LoadScene(index);
        }
    }
}
