using UnityEditor;
using UnityEngine;

/**
 * NestedEditors:
 * Klasa pozwalajaca na zagniezdzanie edytorow wewnatrz siebie
 * TEditor - typ edytora; TTarget - typ objektu
 * np. NestedEditors<ConditionCollectionEditor, ConditionCollection> w InteractableEditor
 */
public abstract class NestedEditors<TEditor, TTarget> : Editor
where TEditor : Editor
where TTarget : Object {
    protected TEditor[] nestedEditors;
    protected void PrepareNestedEditors(TTarget[] subEditorTargets) {
        if (nestedEditors != null && nestedEditors.Length == subEditorTargets.Length)
            return;

        ClearEditors();

        // Create an array of the subEditor type that is the right length for the targets.
        nestedEditors = new TEditor[subEditorTargets.Length];

        // Populate the array and setup each Editor.
        for (int i = 0; i < nestedEditors.Length; i++) {
            nestedEditors[i] = CreateEditor(subEditorTargets[i]) as TEditor;
            NestedEditorInit(nestedEditors[i]);
        }
    }

    protected void ClearEditors() {
        if (nestedEditors == null)
            return;

        for (int i = 0; i < nestedEditors.Length; i++) {
            DestroyImmediate(nestedEditors[i]);
        }
        nestedEditors = null;
    }

    protected abstract void NestedEditorInit(TEditor editor);
}