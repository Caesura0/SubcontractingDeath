using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;


    public class DialogueEditor : EditorWindow
    {

        Dialogue selectedDialogue = null;
        [NonSerialized]
        GUIStyle nodeStyle = null;
        [NonSerialized]
        GUIStyle playerNodeStyle = null;
        [NonSerialized]
        GUIStyle selectedNodeStyle = null;
        [NonSerialized]
        GUIStyle selectedPlayerNodeStyle = null;
        [NonSerialized]
        DialogueNode draggingNode = null;
        [NonSerialized]
        Vector2 draggingOffset;
        [NonSerialized]
        DialogueNode creatingNode = null;
        [NonSerialized]
        DialogueNode deletingNode = null;
        [NonSerialized]
        DialogueNode linkingParentNode = null;
        Vector2 scrollPosition;
        [NonSerialized]
        bool draggingCanvas = false;
        [NonSerialized]
        Vector2 draggingCanvasOffset;

        //
        [NonSerialized] private bool showContextMenu;
        [NonSerialized] private Vector2 contextMenuPosition;


        //only used if i figure out how to getstyle() to work with the colour changing functionality
        DialogueNode selectedNode;

        const float canvasSize = 4000f;
        const float backgroundSize = 50;



        [MenuItem("Window/DialogueEditor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
        }

        [OnOpenAssetAttribute(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            Dialogue dialogue = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;
            if (dialogue != null)
            {
                ShowEditorWindow();
                return true;
            }
            return false;
        }




        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChanged;

            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            nodeStyle.padding = new RectOffset(20, 20, 20, 20);
            nodeStyle.border = new RectOffset(12, 12, 12, 12);



            playerNodeStyle = new GUIStyle();
            playerNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
            playerNodeStyle.padding = new RectOffset(20, 20, 20, 20);
            playerNodeStyle.border = new RectOffset(12, 12, 12, 12);

            selectedNodeStyle = new GUIStyle();
            selectedNodeStyle.normal.background = EditorGUIUtility.Load("node0 on") as Texture2D;
            selectedNodeStyle.padding = new RectOffset(20, 20, 20, 20);
            selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);

            selectedPlayerNodeStyle = new GUIStyle();
            selectedPlayerNodeStyle.normal.background = EditorGUIUtility.Load("node1 on") as Texture2D;
            selectedPlayerNodeStyle.padding = new RectOffset(20, 20, 20, 20);
            selectedPlayerNodeStyle.border = new RectOffset(12, 12, 12, 12);

            Texture2D something = EditorGUIUtility.Load("node0 on") as Texture2D;
        }


        private void OnDisable()
        {
            Selection.selectionChanged -= OnSelectionChanged;
        }
        private void OnSelectionChanged()
        {
            Dialogue newDialogue = Selection.activeObject as Dialogue;
            if (newDialogue != null)
            {
                selectedDialogue = newDialogue;
                Repaint();
            }

        }

        private void OnGUI()
        {
            if (selectedDialogue == null)
            {
                EditorGUILayout.LabelField("No Dialogue selected");
            }
            else
            {
                ProcessEvents();

                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

                Rect canvas = GUILayoutUtility.GetRect(canvasSize, canvasSize);
                Texture2D backgroundText = Resources.Load("background") as Texture2D;
                Rect textCoords = new Rect(0, 0, canvasSize / backgroundSize, canvasSize / backgroundSize);
                GUI.DrawTextureWithTexCoords(canvas, backgroundText, textCoords);


                foreach (var node in selectedDialogue.GetAllNodes())
                {
                    DrawConnections(node);
                }
                foreach (var node in selectedDialogue.GetAllNodes())
                {
                    DrawNode(node);
                }

                EditorGUILayout.EndScrollView();

                if (creatingNode != null)
                {
                    Undo.RecordObject(selectedDialogue, "Added Dialogue Node");
                    selectedDialogue.CreateNode(creatingNode);
                    creatingNode = null;
                }
                if (deletingNode != null)
                {

                    selectedDialogue.DeleteNode(deletingNode);
                    deletingNode = null;
                }
                if (showContextMenu)
                    ShowContextMenu();
            }

        }


        private void ProcessEvents()
        {
            if (Event.current.type == EventType.MouseDown)
            {
                if (Event.current.button == 0 || Event.current.button == 1)
                {
                    selectedNode = GetNodeAtPoint(Event.current.mousePosition + scrollPosition);
                    Selection.activeObject = selectedNode;
                    GUI.changed = true;
                }
            }

            if (Event.current.type == EventType.MouseDown && Event.current.button == 1 && draggingNode == null)
            {
                draggingNode = GetNodeAtPoint(Event.current.mousePosition + scrollPosition);

                if (draggingNode != null)
                {
                    draggingOffset = draggingNode.GetNodeRect().position - Event.current.mousePosition;
                    Selection.activeObject = draggingNode;
                }
                else
                {
                    draggingCanvas = true;
                    draggingCanvasOffset = Event.current.mousePosition + scrollPosition;
                    Selection.activeObject = selectedDialogue;
                }
            }
            else if (Event.current.type == EventType.MouseDrag && draggingNode != null)
            {

                draggingNode.SetPosition(Event.current.mousePosition + draggingOffset);
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseDrag && draggingCanvas)
            {
                scrollPosition = draggingCanvasOffset - Event.current.mousePosition;
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseUp && draggingNode != null)
            {
                draggingNode = null;

            }
            else if (Event.current.type == EventType.MouseUp && draggingCanvas)
            {
                draggingCanvas = false;
            }
            //
            if (Event.current.type == EventType.MouseDown && Event.current.button == 1 && draggingNode == null)
            {
                showContextMenu = true;
                contextMenuPosition = Event.current.mousePosition;
            }
            else
            {
                showContextMenu = false;
            }
            //
        }




        private void DrawNode(DialogueNode node)
        {
            //TODO should i have a class to store all these styles? maybe in a serialized field? hmm
            GUIStyle textStyle = new GUIStyle(EditorStyles.textArea);


            GUILayout.BeginArea(node.GetNodeRect(), GetStyle(node));


            EditorGUILayout.LabelField("Dialogue:");
            node.SetScrollPosition(EditorGUILayout.BeginScrollView(node.GetScrollPosition()));
            node.SetText(EditorGUILayout.TextArea(node.GetNodeText(), textStyle));
            EditorGUILayout.EndScrollView();
            EditorGUILayout.LabelField("Enter Action");
            node.SetEnterAction(EditorGUILayout.TextField(node.GetOnEnterAction()));
            EditorGUILayout.LabelField("Exit Action");
            node.SetExitAction(EditorGUILayout.TextField(node.GetOnExitAction()));
            node.SetIsPlayerSpeaking(EditorGUILayout.Toggle("PlayerSpeaking?", node.IsPlayerSpeaking()));
            GUILayout.BeginHorizontal();


            if (GUILayout.Button("Delete"))
            {
                deletingNode = node;
            }

            DrawLinkButtons(node);

            if (GUILayout.Button("Add"))
            {
                creatingNode = node;
            }

            GUILayout.EndHorizontal();


            foreach (DialogueNode childNode in selectedDialogue.GetAllChildren(node))
            {
                EditorGUILayout.LabelField(childNode.GetNodeText());

                Rect lastRect = GUILayoutUtility.GetLastRect();

            }
            //ignore the magic number!! couldn't figure out how to calculate it from previous
            node.AdjustNodeSize(20);

            GUILayout.EndArea();
        }




        GUIStyle GetStyle(DialogueNode node)
        {
            if (node == selectedNode && !node.IsPlayerSpeaking())
            {

                return selectedNodeStyle;

            }
            else if (node == selectedNode && node.IsPlayerSpeaking())
            {
                return selectedPlayerNodeStyle;

            }
            else if (node != selectedNode && node.IsPlayerSpeaking())
            {
                return playerNodeStyle;
            }
            else
                return nodeStyle;
        }

        //
        private void ShowContextMenu()
        {
            GenericMenu contextMenu = new GenericMenu();
            contextMenu.AddItem(new GUIContent("Add New Node"), false, () => selectedDialogue.CreateNode(null, contextMenuPosition));
            contextMenu.ShowAsContext();
        }
        //

        private void DrawLinkButtons(DialogueNode node)
        {
            if (linkingParentNode == null)
            {
                if (GUILayout.Button("Link"))
                {
                    linkingParentNode = node;
                }
            }
            else if (linkingParentNode == node)
            {
                if (GUILayout.Button("Cancel"))
                {
                    linkingParentNode = null;
                }
            }

            else if (selectedDialogue.IsRootNode(node))
            {
                GUILayout.Button("NA");
            }
            else if (linkingParentNode.GetChildren().Contains(node.name))
            {
                if (GUILayout.Button("UnLink"))
                {
                    linkingParentNode.RemoveChild(node.name);
                    linkingParentNode = null;
                }
            }
            else
            {
                if (GUILayout.Button("Child"))
                {

                    linkingParentNode.AddChild(node.name);
                    linkingParentNode = null;
                }
            }
        }

        private void DrawConnections(DialogueNode node)
        {
            Vector3 startPosition = new Vector2(node.GetNodeRect().xMax, node.GetNodeRect().center.y);
            foreach (DialogueNode childNode in selectedDialogue.GetAllChildren(node))
            {

                Vector3 endPosition = new Vector2(childNode.GetNodeRect().xMin, childNode.GetNodeRect().center.y);
                Vector3 controlPointOffset = endPosition - startPosition;
                controlPointOffset.y = 0;
                controlPointOffset.x *= 0.8f;
                Handles.DrawBezier(
                    startPosition, endPosition,
                    startPosition + controlPointOffset,
                    endPosition - controlPointOffset,
                    Color.white, null, 4f);
            }
        }
        private DialogueNode GetNodeAtPoint(Vector2 point)
        {
            DialogueNode foundNode = null;
            foreach (DialogueNode node in selectedDialogue.GetAllNodes())
            {
                if (node.GetNodeRect().Contains(point))
                {
                    foundNode = node;
                }
            }
            return foundNode;
        }

    }

