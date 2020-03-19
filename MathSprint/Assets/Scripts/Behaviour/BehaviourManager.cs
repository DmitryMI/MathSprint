using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Behaviour
{
    public class BehaviourManager : MonoBehaviour, IList<IUpdateable>
    {
        #region Singleton
        private static BehaviourManager _instance = null;
        public static BehaviourManager Instance => _instance;
        #endregion

        private List<IUpdateable> _updateables = new List<IUpdateable>();

        void Start()
        {
            _instance = this;
        }

        // Update is called once per frame
        void Update()
        {
            foreach (var updateable in _updateables)
            {
                updateable?.OnUpdate();
            }
        }


        public IEnumerator<IUpdateable> GetEnumerator()
        {
            return _updateables.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _updateables).GetEnumerator();
        }

        public void Add(IUpdateable item)
        {
            _updateables.Add(item);
        }

        public void Clear()
        {
            _updateables.Clear();
        }

        public bool Contains(IUpdateable item)
        {
            return _updateables.Contains(item);
        }

        public void CopyTo(IUpdateable[] array, int arrayIndex)
        {
            _updateables.CopyTo(array, arrayIndex);
        }

        public bool Remove(IUpdateable item)
        {
            return _updateables.Remove(item);
        }

        public int Count => _updateables.Count;

        public bool IsReadOnly => ((ICollection<IUpdateable>) _updateables).IsReadOnly;

        public int IndexOf(IUpdateable item)
        {
            return _updateables.IndexOf(item);
        }

        public void Insert(int index, IUpdateable item)
        {
            _updateables.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _updateables.RemoveAt(index);
        }

        public IUpdateable this[int index]
        {
            get => _updateables[index];
            set => _updateables[index] = value;
        }
    }
}
