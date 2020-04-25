using Assets.Scripts.Game;
using UnityEngine;

namespace Assets
{
    public class Gate : MonoBehaviour
    {
        [SerializeField] private int _nextSceneIndex;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        void OnCollisionEnter2D(Collision2D collision2D)
        {
            if (collision2D.gameObject.tag == "Player")
            {
                GameManager.Instance.RequestNextLevel(_nextSceneIndex);
            }
        }
    }
}
