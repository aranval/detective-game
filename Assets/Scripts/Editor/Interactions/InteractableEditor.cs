using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Interactable))]
public class InteractableEditor : NestedEditors<ConditionCollectionEditor, ConditionCollection> {
    private Interactable interactable;
    private SerializedProperty interactionLocationProperty;
    private SerializedProperty collectionsProperty;
    private SerializedProperty defaultReactionCollectionProperty;
    private SerializedProperty targetGameObjectProperty;
    private const float collectionButtonWidth = 125f;
    private const string interactablePropInteractionLocationName = "interactionLocation";
    private const string interactablePropConditionCollectionsName = "conditionCollections";
    private const string interactablePropDefaultReactionCollectionName = "defaultReactionCollection";
    private const string interactablePropTargetGameObjectName = "targetGameObject";


    private void OnEnable() {
        interactable = (Interactable) target;

        collectionsProperty = serializedObject.FindProperty(interactablePropConditionCollectionsName);
        interactionLocationProperty = serializedObject.FindProperty(interactablePropInteractionLocationName);
        defaultReactionCollectionProperty = serializedObject.FindProperty(interactablePropDefaultReactionCollectionName);
        targetGameObjectProperty = serializedObject.FindProperty(interactablePropTargetGameObjectName);

        PrepareNestedEditors(interactable.conditionCollections);
    }

    private void OnDisable() {
        ClearEditors();
    }

    protected override void NestedEditorInit(ConditionCollectionEditor editor) {
        editor.collectionsProperty = collectionsProperty;
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();

        PrepareNestedEditors(interactable.conditionCollections);

        EditorGUILayout.PropertyField(interactionLocationProperty);

        for (int i = 0; i < nestedEditors.Length; i++) {
            nestedEditors[i].OnInspectorGUI();
            EditorGUILayout.Space();
        }

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Add Collection", GUILayout.Width(collectionButtonWidth))) {
            ConditionCollection newCollection = ConditionCollectionEditor.CreateConditionCollection();
            collectionsProperty.AddToObjectArray(newCollection);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(defaultReactionCollectionProperty);

        EditorGUILayout.PropertyField(targetGameObjectProperty);

        serializedObject.ApplyModifiedProperties();
    }
}