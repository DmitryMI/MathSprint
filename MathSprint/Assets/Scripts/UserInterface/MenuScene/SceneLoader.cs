using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UserInterface.MenuScene
{
    public class SceneLoader : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        public void SceneLoad(int index)
        {
            SceneManager.LoadScene(index);
        }
    }
}
