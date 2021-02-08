using UnityEditor;
using UnityEngine;

/**
 * Well... fuck. https://docs.unity3d.com/ScriptReference/SerializedProperty.html
 * Przeglądałem funkcje w docsach aktualnej wersji unity i nie znalazłem nic z czego dało by się skorzystać.
 */
public static class SerializedPropertyExtensions {
    public static void AddToObjectArray<T>(this SerializedProperty arrayProperty, T elementToAdd)
    where T : Object {
        if (!arrayProperty.isArray)
            throw new UnityException("SerializedProperty " + arrayProperty.name + " is not an array.");

        arrayProperty.serializedObject.Update();
        arrayProperty.InsertArrayElementAtIndex(arrayProperty.arraySize);
        arrayProperty.GetArrayElementAtIndex(arrayProperty.arraySize - 1).objectReferenceValue = elementToAdd;
        arrayProperty.serializedObject.ApplyModifiedProperties();
    }

    /**
     * Kompletnie nie rozumiem, czemu wg tego tutorialowego projektu trzeba sprawdzać index < 0,
     * czemu trzeba nullować wartość referencji przed usunięciem elementu, czemu funkcja
     * DeleteArrayElementAtIndex jest wykorzystana do nullowania referencji. Trzeba podpytać kogoś
     * z solidnym work expem w Unity. Zostawiam jak znalazłem, ale wypadałoby się temu jeszcze później
     * przyjrzeć.
     * TODO: Przyjrzeć się.
     */
    public static void RemoveFromObjectArrayAt(this SerializedProperty arrayProperty, int index) {
        if (index < 0)
            throw new UnityException("SerializedProperty " + arrayProperty.name + " cannot have negative elements removed.");
        if (!arrayProperty.isArray)
            throw new UnityException("SerializedProperty " + arrayProperty.name + " is not an array.");
        if (index > arrayProperty.arraySize - 1)
            throw new UnityException("SerializedProperty " + arrayProperty.name + " has only " + arrayProperty.arraySize + " elements so element " + index + " cannot be removed.");

        arrayProperty.serializedObject.Update();

        // If there is a non-null element at the index, null it.
        if (arrayProperty.GetArrayElementAtIndex(index).objectReferenceValue)
            arrayProperty.DeleteArrayElementAtIndex(index);

        // Delete the null element from the array at the index.
        arrayProperty.DeleteArrayElementAtIndex(index);

        // Push all the information on the serializedObject back to the target.
        arrayProperty.serializedObject.ApplyModifiedProperties();
    }

    // Use this to remove an object from an object array represented by a SerializedProperty.
    public static void RemoveFromObjectArray<T>(this SerializedProperty arrayProperty, T elementToRemove)
    where T : Object {
        // If either the serializedProperty doesn't represent an array or the element is null, throw an exception.
        if (!arrayProperty.isArray)
            throw new UnityException("SerializedProperty " + arrayProperty.name + " is not an array.");

        if (!elementToRemove)
            throw new UnityException("Removing a null element is not supported using this method.");

        // Pull all the information from the target of the serializedObject.
        arrayProperty.serializedObject.Update();

        // Go through all the elements in the serializedProperty's array...
        for (int i = 0; i < arrayProperty.arraySize; i++) {
            SerializedProperty elementProperty = arrayProperty.GetArrayElementAtIndex(i);

            // ... until the element matches the parameter...
            if (elementProperty.objectReferenceValue == elementToRemove) {
                // ... then remove it.
                arrayProperty.RemoveFromObjectArrayAt(i);
                return;
            }
        }

        throw new UnityException("Element " + elementToRemove.name + "was not found in property " + arrayProperty.name);
    }
}