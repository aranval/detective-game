using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ConditionCollection))]
public class ConditionCollectionEditor : NestedEditors<ConditionEditor, Condition> {
    public SerializedProperty collectionsProperty;
    private ConditionCollection conditionCollection;
    private SerializedProperty descriptionProperty;
    private SerializedProperty conditionsProperty;
    private SerializedProperty reactionCollectionProperty;

    private const float conditionButtonWidth = 40f;
    private const float collectionButtonWidth = 125f;
    private const string conditionCollectionPropDescriptionName = "description";
    private const string conditionCollectionPropRequiredConditionsName = "requiredConditions";
    private const string conditionCollectionPropReactionCollectionName = "reactionCollection";

    private void OnEnable() {
        conditionCollection = (ConditionCollection) target;

        if (target == null) {
            DestroyImmediate(this);
            return;
        }

        descriptionProperty = serializedObject.FindProperty(conditionCollectionPropDescriptionName);
        conditionsProperty = serializedObject.FindProperty(conditionCollectionPropRequiredConditionsName);
        reactionCollectionProperty = serializedObject.FindProperty(conditionCollectionPropReactionCollectionName);

        PrepareNestedEditors(conditionCollection.requiredConditions);
    }

    private void OnDisable() {
        ClearEditors();
    }

    protected override void NestedEditorInit(ConditionEditor editor) {
        editor.editorType = ConditionEditor.EditorType.ConditionCollection;
        editor.conditionsProperty = conditionsProperty;
    }

    /** 
     * TODO: Dodać funkcjonalność/guzik Duplicate Collection
     */
    public override void OnInspectorGUI() {
        serializedObject.Update();

        PrepareNestedEditors(conditionCollection.requiredConditions);

        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;

        EditorGUILayout.BeginHorizontal();
        descriptionProperty.isExpanded = EditorGUILayout.Foldout(descriptionProperty.isExpanded, descriptionProperty.stringValue);
        if (GUILayout.Button("Remove Collection", GUILayout.Width(collectionButtonWidth))) {
            collectionsProperty.RemoveFromObjectArray(conditionCollection);
        }
        EditorGUILayout.EndHorizontal();

        if (descriptionProperty.isExpanded) {
            ExpandedGUI();
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
    }

    /**
     * TODO: Zmodyfikować float space tak żeby widok nie robil sie za szeroki
     */
    private void ExpandedGUI() {
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(descriptionProperty);
        EditorGUILayout.Space();

        //przeliczenie przestrzeni dla Condition, Satisfied, Add/Remove
        float space = (0.9f * (EditorGUIUtility.currentViewWidth)) / 3f;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Condition", GUILayout.Width(space));
        EditorGUILayout.LabelField("Satisfied?", GUILayout.Width(space));
        EditorGUILayout.LabelField("Add/Remove", GUILayout.Width(space));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginVertical(GUI.skin.box);
        for (int i = 0; i < nestedEditors.Length; i++) {
            nestedEditors[i].OnInspectorGUI();
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Add", GUILayout.Width(conditionButtonWidth))) {
            Condition newCondition = ConditionEditor.CreateCondition();
            conditionsProperty.AddToObjectArray(newCondition);
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(reactionCollectionProperty);
    }

    public static ConditionCollection CreateConditionCollection() {
        ConditionCollection newConditionCollection = CreateInstance<ConditionCollection>();
        newConditionCollection.description = "Nowa kolekcja warunkow";
        newConditionCollection.requiredConditions = new Condition[1];
        newConditionCollection.requiredConditions[0] = ConditionEditor.CreateCondition();
        return newConditionCollection;
    }
}