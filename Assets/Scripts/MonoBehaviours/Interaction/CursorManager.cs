using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager instance = null;
    CursorMode cursorMode = CursorMode.Auto;
    Vector2 hotSpot = Vector2.zero;
    public Texture2D defaultCursorTexture;
    public Texture2D interactableCursorTexture;

    public enum cursorTypes { defaultCursor, interactableCursor };

    void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else instance = this;

        SetCursorTexture(cursorTypes.defaultCursor);
    }

    public void SetCursorTexture(cursorTypes cursorType) {
        Cursor.SetCursor(GetCursorTextureFromEnum(cursorType), hotSpot, cursorMode);
    }

    Texture2D GetCursorTextureFromEnum(cursorTypes cursorType) {
        switch ( cursorType ) {
            case cursorTypes.defaultCursor:
                return defaultCursorTexture;
            case cursorTypes.interactableCursor:
                return interactableCursorTexture;
            default:
                return defaultCursorTexture;
        }
    }
}
