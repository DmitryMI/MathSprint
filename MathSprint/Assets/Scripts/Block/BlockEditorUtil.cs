using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Block
{

#if UNITY_EDITOR
    /// <summary>
    /// Unity editor script used to create surface automatically
    /// </summary>
    [CustomEditor(typeof(Block))]
    public class BlockEditorUtil : Editor
    {
        private SerializedProperty _horizontalLengthProperty;
        private SerializedProperty _blockAtomPrefabProperty;
        private SerializedProperty _leftEndingEnabledProperty;
        private SerializedProperty _rightEndingEnabledProperty;
        private SerializedProperty _topEndingEnabledProperty;

        private bool _lengthValueChanged = false;
        private bool _endingValueChanged = false;

        private void OnEnable()
        {
            _horizontalLengthProperty = serializedObject.FindProperty("_horizontalLength");
            _blockAtomPrefabProperty = serializedObject.FindProperty("_blockAtomPrefab");
            _leftEndingEnabledProperty = serializedObject.FindProperty("_leftEndingEnabled");
            _rightEndingEnabledProperty = serializedObject.FindProperty("_rightEndingEnabled");
            _topEndingEnabledProperty = serializedObject.FindProperty("_topEndingEnabled");
        }

        private static void ClearChildren(Transform parent)
        {
            int childrenCount = parent.childCount;
            List<GameObject> toDestroy = new List<GameObject>(childrenCount);
            for (int i = 0; i < childrenCount; i++)
            {
                Transform child = parent.GetChild(i);
                GameObject go = child.gameObject;
                toDestroy.Add(go);
            }

            for (int i = 0; i < childrenCount; i++)
            {
                if (toDestroy[i] != null)
                {
                    toDestroy[i].transform.parent = null;
                    DestroyImmediate(toDestroy[i]);
                }
            }
        }

        private Block GetBlock()
        {
            Object instance = serializedObject.targetObject;
            if (instance == null)
            {
                return null;
            }

            Block block = (Block)instance;
            return block;
        }

        private static void ConstructAtoms(Block block, GameObject atomPrefab, int length)
        {
            if (block == null)
                return;

            ClearChildren(block.transform);

            if (length <= 0)
                return;
            

            for (int i = 0; i < length; i++)
            {
                //GameObject atomInstance = Instantiate(atomPrefab, block.transform);
                GameObject atomInstance = (GameObject) PrefabUtility.InstantiatePrefab(atomPrefab);
                atomInstance.transform.parent = block.transform;
                atomInstance.transform.localPosition = Vector3.right * i;
                atomInstance.name = $"BlockAtom#{i}";
            }

            BoxCollider2D collider2D = (BoxCollider2D)block.GetComponent<Collider2D>();
            Vector2 colliderSize = new Vector2(length, 1);
            Vector2 offset = new Vector2(length / 2.0f - 0.5f, 0);
            collider2D.size = colliderSize;
            collider2D.offset = offset;
        }

        private static void MakeEndings(Block block, bool leftEndingEnabled, bool rightEndingEnabled, bool topEndingEnabled)
        {
            if (block == null)
                return;

            int childCount = block.transform.childCount;

            if (childCount == 0)
            {
                return;
            }
            else if (childCount == 1)
            {
                Transform childTransform = block.transform.GetChild(0);
                GameObject childGo = childTransform.gameObject;
                BlockAtom atom = childGo.GetComponent<BlockAtom>();
                atom.SetEnding(topEndingEnabled, leftEndingEnabled, rightEndingEnabled);
            }
            else
            {
                Transform childLeftTransform = block.transform.GetChild(0);
                GameObject childLeftGo = childLeftTransform.gameObject;
                BlockAtom atomLeft = childLeftGo.GetComponent<BlockAtom>();
                Transform childRightTransform = block.transform.GetChild(childCount - 1);
                GameObject childRightGo = childRightTransform.gameObject;
                BlockAtom atomRight = childRightGo.GetComponent<BlockAtom>();

                atomLeft.SetEnding(topEndingEnabled, leftEndingEnabled, false);
                atomRight.SetEnding(topEndingEnabled, false, rightEndingEnabled);

                for (int i = 1; i < childCount - 1; i++)
                {
                    Transform transform = block.transform.GetChild(i);
                    GameObject go = transform.gameObject;
                    BlockAtom atom = go.GetComponent<BlockAtom>();

                    atom.SetEnding(topEndingEnabled, false, false);
                }
            }
        }

        private void ProcessInspectorUi()
        {
            int lengthUserValue = EditorGUILayout.IntField("Horizontal length", _horizontalLengthProperty.intValue);
            if (lengthUserValue > 0)
            {
                if (lengthUserValue != _horizontalLengthProperty.intValue)
                {
                    _lengthValueChanged = true;
                }
                _horizontalLengthProperty.intValue = lengthUserValue;
            }

            bool leftEndingEnabled = EditorGUILayout.Toggle("Enable left ending", _leftEndingEnabledProperty.boolValue);
            if (leftEndingEnabled != _leftEndingEnabledProperty.boolValue)
            {
                _endingValueChanged = true;
            }
            _leftEndingEnabledProperty.boolValue = leftEndingEnabled;

            bool rightEndingEnabled = EditorGUILayout.Toggle("Enable right ending", _rightEndingEnabledProperty.boolValue);
            if (rightEndingEnabled != _rightEndingEnabledProperty.boolValue)
            {
                _endingValueChanged = true;
            }
            _rightEndingEnabledProperty.boolValue = rightEndingEnabled;

            bool topEndingEnabled = EditorGUILayout.Toggle("Enable top ending", _topEndingEnabledProperty.boolValue);
            if (topEndingEnabled != _topEndingEnabledProperty.boolValue)
            {
                _endingValueChanged = true;
            }
            _topEndingEnabledProperty.boolValue = topEndingEnabled;

            EditorGUILayout.PropertyField(_blockAtomPrefabProperty, new GUIContent("Block atom prefab"));
        }

        /// <summary>
        /// Invoked by Unity engine
        /// </summary>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            ProcessInspectorUi();

            Block block = (Block)serializedObject.targetObject;
            int length = _horizontalLengthProperty.intValue;
            bool leftEndingEnabled = _leftEndingEnabledProperty.boolValue;
            bool rightEndingEnabled = _rightEndingEnabledProperty.boolValue;
            bool topEndingEnabled = _topEndingEnabledProperty.boolValue;
            GameObject atomPrefab = (GameObject)_blockAtomPrefabProperty.objectReferenceValue;

            if (_lengthValueChanged)
            {
                ConstructAtoms(block, atomPrefab, length);
                _lengthValueChanged = false;
                MakeEndings(block, leftEndingEnabled, rightEndingEnabled, topEndingEnabled);
                _endingValueChanged = false;
            }

            if (_endingValueChanged)
            {
                MakeEndings(block, leftEndingEnabled, rightEndingEnabled, topEndingEnabled);
                _endingValueChanged = false;
            }

            serializedObject.ApplyModifiedProperties();
        }

        [MenuItem("BlockEditor/Update blocks")]
        private static void DoSomething()
        {
            Debug.Log("Updating blocks");

            Block[] blocks = GameObject.FindObjectsOfType<Block>();

            foreach (var block in blocks)
            {
                int length = block.HorizontalLength;
                bool leftEndingEnabled = block.LeftEndingEnabled;
                bool rightEndingEnabled = block.RightEndingEnabled;
                bool topEndingEnabled = block.TopEndingEnabled;
                GameObject atomPrefab = block.BlockAtomPrefab;

                ConstructAtoms(block, atomPrefab, length);
                MakeEndings(block, leftEndingEnabled, rightEndingEnabled, topEndingEnabled);
            }
        }
    }
#endif
}
