using UnityEngine;

public class Interactable : MonoBehaviour {
    public Transform interactionLocation;
    public ConditionCollection[] conditionCollections = new ConditionCollection[0];
    public ReactionCollection defaultReactionCollection;
    public GameObject targetGameObject;
    private Shader defaultShader;
    private Shader outlineShader;
    /**
     * Interact():
     * W momencie gdy player dotrze do interactionLocation wywołuje sie Interact() 
     * który sprawdza wszystkie kolekcje warunków. VerifyAndExecute() wywołuje reakcje
     * dla pierwszego spełnionego ConditionCollection. Jeśli żaden nie jest spełniony
     * wywołana zostaje defaultowa reakcja (może być pusta)
     */

    public void Start() {
        defaultShader = targetGameObject.GetComponent<Renderer>().material.shader;
        outlineShader = Shader.Find("Outlined/Regular");
    }
    public void Interact() {
        for (int i = 0; i < conditionCollections.Length; i++) {
            if (conditionCollections[i].VerifyAndExecute()) {
                return;
            }
        }
        defaultReactionCollection.React();
    }

    void OnMouseOver() {
        if (targetGameObject.GetComponent<Renderer>().material.shader != outlineShader) {
            targetGameObject.GetComponent<Renderer>().material.shader = outlineShader;
        }
        if (CursorManager.instance != null) CursorManager.instance.SetCursorTexture(CursorManager.cursorTypes.interactableCursor);
        // Debug.Log("Outline ON " + outlineShader.name);
    }

    void OnMouseExit() {
        if (targetGameObject.GetComponent<Renderer>().material.shader != defaultShader) {
            targetGameObject.GetComponent<Renderer>().material.shader = defaultShader;
        }
        if (CursorManager.instance != null) CursorManager.instance.SetCursorTexture(CursorManager.cursorTypes.defaultCursor);
        // Debug.Log("Outline OFF " + defaultShader.name + "  currently ON: " + targetGameObject.GetComponent<Renderer>().material.shader);
    }

}