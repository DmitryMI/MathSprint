using Assets.Scripts.Game;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UserInterface.InGame
{
    public class HealthDrawer : MonoBehaviour
    {


#pragma warning disable 649
        [SerializeField]
        private GameObject _heartPrefab;

        [SerializeField] private HorizontalLayoutGroup _layoutGroup;
#pragma warning restore 649
        
        void Start()
        {
            GameManager.Instance.OnLifeCountChanged += OnLifeCountChanged;
            int lifesCount = GameManager.Instance.LifeCount;
            OnLifeCountChanged(lifesCount);
        }

        void OnLifeCountChanged(int lifeCount)
        {
            int children = _layoutGroup.transform.childCount;
            int delta = lifeCount - children;

            if (delta > 0)
            {
                for (int i = 0; i < delta; i++)
                {
                    GameObject heart = Instantiate(_heartPrefab, _layoutGroup.transform);
                    heart.name = "Heart";
                }
            }
            else
            {
                for (int i = 0; i < children; i++)
                {
                    GameObject heart = _layoutGroup.transform.GetChild(0).gameObject; // 0 is correct!
                    Destroy(heart);
                }
            }
        }
    }
}
