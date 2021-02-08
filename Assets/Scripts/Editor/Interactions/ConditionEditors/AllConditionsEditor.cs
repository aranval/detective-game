using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AllConditions))]
public class AllConditionsEditor : Editor {

    // z jakiegos powodu ponizsze sie wypierdala
    // public static string[] AllConditionDescriptions {
    //     get {
    //         if (AllConditionDescriptions == null) {
    //             CreateConditionsArray();
    //         }
    //         return AllConditionDescriptions;
    //     }
    //     private set => allConditionDescriptions = value;
    // }

    public static string[] AllConditionDescriptions {
        get {
            if (allConditionDescriptions == null) {
                CreateConditionsDescriptionsArray();
            }
            return allConditionDescriptions;
        }
        private set { allConditionDescriptions = value; }
    }

    private static string[] allConditionDescriptions;
    private ConditionEditor[] conditionEditors;
    private AllConditions allConditions;
    private string newConditionDescription = "Nowy Warunek";
    private const string creationPath = "Assets/Resources/AllConditions.asset";
    private const float buttonWidth = 40f;

    private void OnEnable() {
        allConditions = (AllConditions) target; //zawsze w OnEnable trzeba cacheowac target 

        if (allConditions.conditions == null) allConditions.conditions = new Condition[0];
        if (conditionEditors == null) {
            CreateEditors();
        }
    }

    private void OnDisable() {
        for (int i = 0; i < conditionEditors.Length; i++) {
            DestroyImmediate(conditionEditors[i]); //mimo ze w docsach zalecaja uzywanie Destroy to wlasnie DestroyImmediate powinno sie stosowac przy pisaniu Edytorow
        }
        conditionEditors = null;
    }

    private static void CreateConditionsDescriptionsArray() {
        AllConditionDescriptions = new string[GetConditionsAmount()];

        for (int i = 0; i < AllConditionDescriptions.Length; i++) {
            AllConditionDescriptions[i] = GetConditionAt(i).description;
        }
    }

    public override void OnInspectorGUI() {
        //refreshuje edytory jesli dlugosci sie roznia
        if (conditionEditors.Length != GetConditionsAmount()) {
            for (int i = 0; i < conditionEditors.Length; i++) {
                DestroyImmediate(conditionEditors[i]);
            }
            CreateEditors();
        }
        for (int i = 0; i < conditionEditors.Length; i++) {
            conditionEditors[i].OnInspectorGUI();
        }
        if (GUILayout.Button("Reset", GUILayout.Width(80), GUILayout.Height(40))) {
            Debug.Log("Stan przed resetem");
            for (int i = 0; i < GetConditionsAmount(); i++) {
                Debug.Log(GetConditionAt(i).description + ": " + GetConditionAt(i).satisfied);
            }
            allConditions.Reset();
            Debug.Log("Stan po resecie");
            for (int i = 0; i < GetConditionsAmount(); i++) {
                Debug.Log(GetConditionAt(i).description + ": " + GetConditionAt(i).satisfied);
            }
        }
        //przestrzeń wertykalna miedzy spisem conditions a dodawaniem nowego condition
        if (GetConditionsAmount() > 0) {
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }

        EditorGUILayout.BeginHorizontal();
        newConditionDescription = EditorGUILayout.TextField(GUIContent.none, newConditionDescription);
        if (GUILayout.Button("Add", GUILayout.Width(buttonWidth))) {
            AddCondition(newConditionDescription);
            newConditionDescription = "Nowy Warunek";
        }
        EditorGUILayout.EndHorizontal();
    }

    private void CreateEditors() {
        conditionEditors = new ConditionEditor[allConditions.conditions.Length];

        for (int i = 0; i < conditionEditors.Length; i++) {
            conditionEditors[i] = CreateEditor(GetConditionAt(i)) as ConditionEditor;
            conditionEditors[i].editorType = ConditionEditor.EditorType.AllConditionAsset;
        }
    }

    [MenuItem("Assets/Create/AllConditions")]
    private static void CreateAllConditionsAsset() {
        if (checkInstance()) return;

        AllConditions instance = CreateInstance<AllConditions>();
        AssetDatabase.CreateAsset(instance, creationPath);
        AllConditions.Instance = instance;
        instance.conditions = new Condition[0];
    }

    private void AddCondition(string description) {
        if (!checkInstance()) return;

        Condition newCondition = ConditionEditor.CreateCondition(description);
        newCondition.name = description;

        Undo.RecordObject(newCondition, "Created new Condition"); //zapisuje operacje wykonywane na instancji zeby dzialalo undo (?)         
        AssetDatabase.AddObjectToAsset(newCondition, AllConditions.Instance); //podpiecie pod asset
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(newCondition)); //trzeba wykonac import zeby asset byl 
        ArrayUtility.Add(ref AllConditions.Instance.conditions, newCondition);
        EditorUtility.SetDirty(AllConditions.Instance);
        CreateConditionsDescriptionsArray();
    }

    public static void RemoveCondition(Condition condition) {
        if (!checkInstance()) return;

        Undo.RecordObject(AllConditions.Instance, "Removing condition"); //zapisuje operacje wykonywane na instancji zeby dzialalo undo (?) 
        ArrayUtility.Remove(ref AllConditions.Instance.conditions, condition); //usuwa condition z tablicy Condition[]
        DestroyImmediate(condition, true); // niszczy condition... 
        AssetDatabase.SaveAssets(); //...i zapisuje zmiane w assetach
        EditorUtility.SetDirty(AllConditions.Instance); //oznaczenie jako dirty jest potrzebne, zeby edytor zapisywal zmainy przy savie na projekcie
        CreateConditionsDescriptionsArray(); //odtworzenie tablicy stringow bez kasowanego elementu
    }

    private static bool checkInstance() {
        if (!AllConditions.Instance) {
            Debug.LogError("AllConditions has not been created yet.");
            return false;
        }
        return true;
    }

    public static int GetConditionIndex(Condition condition) {

        for (int i = 0; i < GetConditionsAmount(); i++) {
            if (GetConditionAt(i).hash == condition.hash)
                return i;
        }

        return -1;
    }

    public static Condition GetConditionAt(int index) {
        Condition[] allConditions = AllConditions.Instance.conditions;

        if (allConditions == null || allConditions[0] == null)
            return null;
        if (index >= allConditions.Length)
            return allConditions[0];

        return allConditions[index];
    }

    public static int GetConditionsAmount() {
        if (AllConditions.Instance.conditions == null) return 0;
        return AllConditions.Instance.conditions.Length;
    }
}