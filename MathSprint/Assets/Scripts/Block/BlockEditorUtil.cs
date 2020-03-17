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

        private bool _lengthValueChanged = false;
        private bool _endingValueChanged = false;

        void OnEnable()
        {
            _horizontalLengthProperty = serializedObject.FindProperty("_horizontalLength");
            _blockAtomPrefabProperty = serializedObject.FindProperty("_blockAtomPrefab");
            _leftEndingEnabledProperty = serializedObject.FindProperty("_leftEndingEnabled");
            _rightEndingEnabledProperty = serializedObject.FindProperty("_rightEndingEnabled");
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
                GameObject atomInstance = Instantiate(atomPrefab, block.transform);
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


            if (childCount == 0)
            {
                return;
            }
            else if (childCount == 1)
            {
                Transform childTransform = block.transform.GetChild(0);
                GameObject childGo = childTransform.gameObject;
                BlockAtom atom = childGo.GetComponent<BlockAtom>();
                if (leftEndingEnabled && rightEndingEnabled)
                {
                    atom.SetBothEnding();
                }
                else if (leftEndingEnabled)
                {
                    atom.SetLeftEnding();
                }
                else if (rightEndingEnabled)
                {
                    atom.SetRightEnding();
                }
                else
                {
                    atom.SetNoEnding();
                }
            }
            else
            {
                Transform childLeftTransform = block.transform.GetChild(0);
                GameObject childLeftGo = childLeftTransform.gameObject;
                BlockAtom atomLeft = childLeftGo.GetComponent<BlockAtom>();
                Transform childRightTransform = block.transform.GetChild(childCount - 1);
                GameObject childRightGo = childRightTransform.gameObject;
                BlockAtom atomRight = childRightGo.GetComponent<BlockAtom>();

                if (leftEndingEnabled)
                {
                    atomLeft.SetLeftEnding();
                }
                else
                {
                    atomLeft.SetNoEnding();
                }

                if (rightEndingEnabled)
                {
                    atomRight.SetRightEnding();
                }
                else
                {
                    atomRight.SetNoEnding();
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
