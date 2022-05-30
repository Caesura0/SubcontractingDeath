using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue", order = 0)]
    public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField]

        List<DialogueNode> nodes = new List<DialogueNode>();
        //when you create a list/dictionary etc, set it to a "new" empty variable(class? method?) to avoid null
        Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();

        Vector3 newOffset = new Vector2(100, 100);


        private void Awake()
        {
            OnValidate();

        }


        private void OnValidate()
        {
            nodeLookup.Clear();
            foreach (DialogueNode node in GetAllNodes())
            {
                nodeLookup[node.name] = node;
            }

        }



        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }

        public DialogueNode GetRootNode()
        {
            return nodes[0];
        }

        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
        {

            foreach (string childID in parentNode.GetChildren())
            {
                if (nodeLookup.ContainsKey(childID))
                {
                    yield return (nodeLookup[childID]);
                }

            }
        }
        public IEnumerable<DialogueNode> GetPlayerChildren(DialogueNode currentNode)
        {
            foreach (DialogueNode node in GetAllChildren(currentNode))
            {
                if (node.IsPlayerSpeaking())
                {
                    yield return node;
                }

            }
        }

        public IEnumerable<DialogueNode> GetAIChildren(DialogueNode currentNode)
        {
            foreach (DialogueNode node in GetAllChildren(currentNode))
            {
                if (!node.IsPlayerSpeaking())
                {
                    yield return node;
                }

            }
        }
#if UNITY_EDITOR
        public void CreateNode(DialogueNode parent)
        {
            DialogueNode newNode = MakeNode(parent);
            Undo.RegisterCreatedObjectUndo(newNode, "Created Dialogue Node");
            Undo.RecordObject(this, "Add Child Dialogue");
            AddNode(newNode);
        }

        public void CreateNode(DialogueNode parent, Vector2 position)
        {
            var newNode = MakeNode(parent);

            newNode.SetPosition(position);

            Undo.RegisterCreatedObjectUndo(newNode, "Create Dialogue Node");
            Undo.RecordObject(this, "Add Dialogue Node");

            AddNode(newNode);
        }

        public void DeleteNode(DialogueNode nodeToDelete)
        {
            Undo.RecordObject(this, "Deleted Dialogue Node");
            nodes.Remove(nodeToDelete);
            OnValidate();
            RemoveOrphanedChildren(nodeToDelete);
            Undo.DestroyObjectImmediate(nodeToDelete);
        }

        private void RemoveOrphanedChildren(DialogueNode nodeToDelete)
        {
            foreach (DialogueNode node in GetAllNodes())
            {
                node.RemoveChild(nodeToDelete.name);
            }
        }


        private DialogueNode MakeNode(DialogueNode parent)
        {
            DialogueNode newNode = CreateInstance<DialogueNode>();
            newNode.name = Guid.NewGuid().ToString();

            if (parent != null)
            {
                Vector3 parentOffsetPosition = new Vector2(parent.GetNodeRect().xMax, parent.GetNodeRect().center.y);
                newNode.SetPosition(parentOffsetPosition);
                parent.AddChild(newNode.name);
                newNode.SetIsPlayerSpeaking(!parent.IsPlayerSpeaking());
            }

            return newNode;
        }
        private void AddNode(DialogueNode newNode)
        {
            nodes.Add(newNode);
            OnValidate();
        }
#endif
        public bool IsRootNode(DialogueNode checkNode)
        {
            foreach (DialogueNode node in GetAllNodes())
            {
                if (node.GetChildren().Contains(checkNode.name))
                {
                    return false;
                }
            }
            return true;
        }

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (nodes.Count == 0)
            {
                //should I change this to "set" the root node instead of figuring it out dynamically?
                DialogueNode newNode = MakeNode(null);
                AddNode(newNode);
            }

            if (AssetDatabase.GetAssetPath(this) != "")
            {
                foreach (DialogueNode node in GetAllNodes())
                {
                    if (AssetDatabase.GetAssetPath(node) == "")
                    {
                        AssetDatabase.AddObjectToAsset(node, this);
                    }
                }
            }
#endif
        }

        public void OnAfterDeserialize()
        {
        }
    }


