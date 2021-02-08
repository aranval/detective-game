using UnityEngine;

/**
 * Daje opcje resetowania ScripltableObjects w Inspectorze
 * https://docs.unity3d.com/ScriptReference/ScriptableObject.html
 * Potrzebne do resetowania AllConditions przy starcie gry
 */
public abstract class ResettableScriptableObject : ScriptableObject {
    public abstract void Reset();
}