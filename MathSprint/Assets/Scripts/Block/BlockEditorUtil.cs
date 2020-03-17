using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Block
{
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

        void OnEnable()
        {
            _horizontalLengthProperty = serializedObject.FindProperty("_horizontalLength");
            _blockAtomPrefabProperty = serializedObject.FindProperty("_blockAtomPrefab");
            _leftEndingEnabledProperty = serializedObject.FindProperty("_leftEndingEnabled");
            _rightEndingEnabledProperty = serializedObject.FindProperty("_rightEndingEnabled");
            _topEndingEnabledProperty = serializedObject.FindProperty("_topEndingEnabled");
        }

        private void ClearChildren(Transform parent)
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

        private void ConstructAtoms()
        {
            Block block = GetBlock();
            if(block == null)
                return;

            ClearChildren(block.transform);

            int length = _horizontalLengthProperty.intValue;

            if (length <= 0)
            {
                return;
            }

            Object atomReference = _blockAtomPrefabProperty.objectReferenceValue;

            if (atomReference == null)
            {
                return;
            }

            GameObject atomPrefab = (GameObject)_blockAtomPrefabProperty.objectReferenceValue;

            for (int i = 0; i < length; i++)
            {
                //GameObject atomInstance = Instantiate(atomPrefab, block.transform);
                GameObject atomInstance = (GameObject) PrefabUtility.InstantiatePrefab(atomReference);
                atomInstance.transform.parent = block.transform;
                atomInstance.transform.localPosition = Vector3.right * i;
                atomInstance.name = $"BlockAtom#{i}";
            }
        }

        private void MakeEndings()
        {
            Block block = GetBlock();
            if (block == null)
                return;

            int childCount = block.transform.childCount;

            bool leftEndingEnabled = _leftEndingEnabledProperty.boolValue;
            bool rightEndingEnabled = _rightEndingEnabledProperty.boolValue;
            bool topEndingEnabled = _topEndingEnabledProperty.boolValue;


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

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            ProcessInspectorUi();

            if (_lengthValueChanged)
            {
                ConstructAtoms();
                _lengthValueChanged = false;
                MakeEndings();
                _endingValueChanged = false;
            }

            if (_endingValueChanged)
            {
                MakeEndings();
                _endingValueChanged = false;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
