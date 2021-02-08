using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ReactionCollection))]
public class ReactionCollectionEditor : NestedEditors<ReactionEditor, Reaction> {
    private ReactionCollection reactionCollection;
    private SerializedProperty reactionsProperty;

    private Type[] reactionTypes;
    private string[] reactionTypeNames;
    private int selectedIndex;

    private const float dropAreaHeight = 50f;
    private const float controlSpacing = 5f;
    private const string reactionsPropName = "reactions";
    private readonly float verticalSpacing = EditorGUIUtility.standardVerticalSpacing;

    private void OnEnable() {
        reactionCollection = (ReactionCollection) target;

        reactionsProperty = serializedObject.FindProperty(reactionsPropName);
        PrepareNestedEditors(reactionCollection.reactions);
        FillReactionNamesArray();
    }

    private void OnDisable() {
        ClearEditors();
    }

    protected override void NestedEditorInit(ReactionEditor editor) {
        editor.reactionsProperty = reactionsProperty;
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();

        PrepareNestedEditors(reactionCollection.reactions);
        for (int i = 0; i < nestedEditors.Length; i++) {
            nestedEditors[i].OnInspectorGUI();
        }

        if (reactionCollection.reactions.Length > 0) {
            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }

        //podzial przestrzeni na polowy, zostal po tym jak chcialem zaimplementowac drag n dropa
        Rect fullWidthRect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(dropAreaHeight + verticalSpacing));
        Rect leftAreaRect = fullWidthRect;
        leftAreaRect.y += verticalSpacing * 0.5f;
        leftAreaRect.width *= 0.5f;
        leftAreaRect.width -= controlSpacing * 0.5f;
        leftAreaRect.height = dropAreaHeight;
        Rect rightAreaRect = leftAreaRect;
        rightAreaRect.x += rightAreaRect.width + controlSpacing;
        TypeSelectionGUI(leftAreaRect);

        serializedObject.ApplyModifiedProperties();
    }

    private void TypeSelectionGUI(Rect containingRect) {
        //podzial na gorna i dolna polowe, gora - selector, dol - button. wszystko robilbym w tym stylu, gdyby nie fakt, ze to ostatnia notacja ktorej sie nauczylem :d
        Rect topHalf = containingRect;
        topHalf.height *= 0.5f;
        Rect bottomHalf = topHalf;
        bottomHalf.y += bottomHalf.height;

        selectedIndex = EditorGUI.Popup(topHalf, selectedIndex, reactionTypeNames);

        if (GUI.Button(bottomHalf, "Add Selected Reaction")) {
            Type reactionType = reactionTypes[selectedIndex];
            Reaction newReaction = ReactionEditor.CreateReaction(reactionType);
            reactionsProperty.AddToObjectArray(newReaction);
        }
    }
    private void FillReactionNamesArray() {
        Type reactionType = typeof(Reaction);

        Type[] allTypes = reactionType.Assembly.GetTypes();
        List<Type> reactionSubTypeList = new List<Type>();

        //fill tablicy z lista reakcji
        for (int i = 0; i < allTypes.Length; i++) {
            if (allTypes[i].IsSubclassOf(reactionType) && !allTypes[i].IsAbstract) {
                reactionSubTypeList.Add(allTypes[i]);
            }
        }
        reactionTypes = reactionSubTypeList.ToArray();
        List<string> reactionTypeNameList = new List<string>();

        for (int i = 0; i < reactionTypes.Length; i++) {
            reactionTypeNameList.Add(reactionTypes[i].Name);
        }
        reactionTypeNames = reactionTypeNameList.ToArray();
    }
}