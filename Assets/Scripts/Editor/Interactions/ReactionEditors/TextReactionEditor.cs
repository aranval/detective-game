using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TextReaction))]
public class TextReactionEditor : ReactionEditor {
    new TextReaction target;
    private SerializedProperty messageProperty;
    // private SerializedProperty textColorProperty;
    private const string textReactionPropMessageName = "message";
    //"This text will be printed out\n in the dialogue window as a text reaction.";
    // private const string textReactionPropTextColorName = "textColor";

    protected override string GetFoldoutLabel() {
        return "Text Reaction";
    }

    protected override void Init() {
        messageProperty = serializedObject.FindProperty(textReactionPropMessageName);
    }

    protected override void DrawReaction() {
        DrawDefaultInspector ();
        messageProperty.stringValue = EditorGUILayout.TextArea(messageProperty.stringValue, GUILayout.ExpandHeight(true));
    }

}