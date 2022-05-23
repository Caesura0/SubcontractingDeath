
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


    public class DialogueNode : ScriptableObject
    {
        [SerializeField] bool isPlayerSpeaking = false;

        [TextArea]
        [SerializeField] private string text;
        [SerializeField] List<string> children = new List<string>();

        static float baseHeight = 220f;
        static float baseWidth = 250f;

        [SerializeField] Rect rect = new Rect(0, 0, 200, 200);
        //note, this should be an array if you want multiple actions, or a randomizer? 
        [SerializeField] string onEnterAction;
        [SerializeField] string onExitAction;
        [SerializeField] Condition condition;
        Vector2 scrollPosition;



        public string GetNodeText()
        {
            return text;
        }

        public Rect GetNodeRect()
        {
            return rect;
        }

        public Vector2 GetScrollPosition()
        {
            return scrollPosition;
        }

        public List<string> GetChildren()
        {
            return children;
        }
        public bool IsPlayerSpeaking()
        {
            return isPlayerSpeaking;
        }

        public string GetOnEnterAction()
        {
            return onEnterAction;
        }
        public string GetOnExitAction()
        {
            return onExitAction;
        }





#if UNITY_EDITOR

        public bool SetIsPlayerSpeaking()
        {
            isPlayerSpeaking = !isPlayerSpeaking;
            return isPlayerSpeaking;
        }
        public void SetPosition(Vector2 newPosition)
        {
            Undo.RecordObject(this, "Move Dialogue Node");
            rect.position = newPosition;
            EditorUtility.SetDirty(this);
        }

        public void AdjustNodeSize(float childHeight)
        {

            rect.height = baseHeight + (childHeight * children.Count);
            rect.width = baseWidth;
        }

        public void SetScrollPosition(Vector2 newPosition)
        {
            Undo.RecordObject(this, "Move Dialogue Node");
            scrollPosition = newPosition;
            EditorUtility.SetDirty(this);
        }

        public void SetText(string newText)
        {
            if (newText != text)
            {
                Undo.RecordObject(this, "Update Dialogue Text");
                text = newText;
                EditorUtility.SetDirty(this);
            }
        }

        public void SetEnterAction(string newText)
        {
            if (newText != text)
            {
                Undo.RecordObject(this, "Update Dialogue Enter Action");
                onEnterAction = newText;
                EditorUtility.SetDirty(this);
            }
        }

        public void SetExitAction(string newText)
        {
            if (newText != text)
            {
                Undo.RecordObject(this, "Update Dialogue Exit Action");
                onExitAction = newText;
                EditorUtility.SetDirty(this);
            }
        }
        public void SetIsPlayerSpeaking(bool flag)
        {
            Undo.RecordObject(this, "Change Dialogue Speaker");
            isPlayerSpeaking = flag;
            EditorUtility.SetDirty(this);
        }

        public void AddChild(string childID)
        {
            Undo.RecordObject(this, "Add Dialogue Link");
            children.Add(childID);
            EditorUtility.SetDirty(this);
        }

        public void RemoveChild(string childID)
        {
            Undo.RecordObject(this, "Remove Dialogue Link");
            children.Remove(childID);
            EditorUtility.SetDirty(this);
        }

    public bool CheckCondition(IEnumerable<IPredicateEvaluator> evaluators)
    {
        return condition.Check(evaluators);
    }


#endif

}
