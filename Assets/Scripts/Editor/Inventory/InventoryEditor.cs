using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Inventory))] //custom editor definition, req to work in inspector
public class InventoryEditor : Editor {

    private SerializedProperty itemImagesProp;
    private SerializedProperty itemsProp;
    private bool[] showItemSlots = new bool[Inventory.itemSlotsNumber];

    private const string inventoryPropItemImgName = "itemImages"; // WTF is that??? fieldnames
    private const string inventoryPropItemName = "items"; // WTF is that??? fieldnames

    /**
     * OnEnable() cache
     */
    private void OnEnable() {
        itemImagesProp = serializedObject.FindProperty(inventoryPropItemImgName);
        itemsProp = serializedObject.FindProperty(inventoryPropItemName);
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();

        for (int i = 0; i < Inventory.itemSlotsNumber; i++) { //GUI dla itemslotów
            ItemSlotGUI(i);
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void ItemSlotGUI(int i) {
        EditorGUILayout.BeginVertical(GUI.skin.box); //GUI wyswietla sie wertykalnie, GUI.skin.box definicja skina na box - TODO: zweryfikowac jak sie zachowuje bez skina
        EditorGUI.indentLevel++;

        showItemSlots[i] = EditorGUILayout.Foldout(showItemSlots[i], "Item slot " + i);
        if (showItemSlots[i]) {
            EditorGUILayout.PropertyField(itemImagesProp.GetArrayElementAtIndex(i));
            EditorGUILayout.PropertyField(itemsProp.GetArrayElementAtIndex(i));
        }

        EditorGUI.indentLevel--; 
        EditorGUILayout.EndVertical();
    }
}