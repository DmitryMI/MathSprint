using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Behaviour
{
    /// <summary>
    /// Manages behaviour updating
    /// </summary>
    public class BehaviourManager : MonoBehaviour, IList<IUpdateable>
    {
        #region Singleton
        private static BehaviourManager _instance = null;
        /// <summary>
        /// Active instance (singleton)
        /// </summary>
        public static BehaviourManager Instance => _instance;
        #endregion

        private List<IUpdateable> _updateables = new List<IUpdateable>();

        private void Start()
        {
            _instance = this;
        }
        private void Update()
        {
            foreach (var updateable in _updateables)
            {
                updateable?.OnUpdate();
            }
        }

        /// <summary>
        /// Returns enumerator for list of registered IUpdateable
        /// </summary>
        /// <returns>Created enumerator</returns>
        public IEnumerator<IUpdateable> GetEnumerator()
        {
            return _updateables.GetEnumerator();
        }

        /// <summary>
        /// Returns enumerator for list of registered IUpdateable
        /// </summary>
        /// <returns>Created enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _updateables).GetEnumerator();
        }

        /// <summary>
        /// Registers new updateable object
        /// </summary>
        /// <param name="item">Updateable object</param>
        public void Add(IUpdateable item)
        {
            _updateables.Add(item);
        }

        /// <summary>
        /// Clears list of registered IUpdateable
        /// </summary>
        public void Clear()
        {
            _updateables.Clear();
        }

        /// <summary>
        /// Checks if updateable is registered
        /// </summary>
        /// <param name="item"></param>
        /// <returns>true if IUpdateable is registered</returns>
        public bool Contains(IUpdateable item)
        {
            return _updateables.Contains(item);
        }

        /// <summary>
        /// Copies list of updateable objects to an array
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(IUpdateable[] array, int arrayIndex)
        {
            _updateables.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Unregisters IUpdateable object
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(IUpdateable item)
        {
            return _updateables.Remove(item);
        }

        /// <summary>
        /// Count of registered updateable objects
        /// </summary>
        public int Count => _updateables.Count;

        /// <summary>
        /// Is it possible to register IUpdateable
        /// </summary>
        public bool IsReadOnly => ((ICollection<IUpdateable>) _updateables).IsReadOnly;

        /// <summary>
        /// Returns index of updateable instance
        /// </summary>
        /// <param name="item"></param>
        /// <returns>-1 if object is not registered; index if is registered</returns>
        public int IndexOf(IUpdateable item)
        {
            return _updateables.IndexOf(item);
        }

        /// <summary>
        /// Inserts updateable object to certain execution position
        /// </summary>
        /// <param name="index">Execution position</param>
        /// <param name="item">Updateable object</param>
        public void Insert(int index, IUpdateable item)
        {
            _updateables.Insert(index, item);
        }

        /// <summary>
        /// Removes object on certain execution position
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            _updateables.RemoveAt(index);
        }

        /// <summary>
        /// Gets or sets execution position
        /// </summary>
        /// <param name="index">Index of position</param>
        /// <returns>Object on certain position</returns>
        public IUpdateable this[int index]
        {
            get => _updateables[index];
            set => _updateables[index] = value;
        }
    }
}
