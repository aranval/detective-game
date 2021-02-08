using UnityEngine;

/**
 * ConditionCollection: 
 * Weryfikuje warunki i wywouje kolekcje reakcji.
 */
public class ConditionCollection : ScriptableObject {
    public string description;
    public Condition[] requiredConditions = new Condition[0]; //zeby null sie nie sypał
    public ReactionCollection reactionCollection;

    public bool VerifyAndExecute() {
        for (int i = 0; i < requiredConditions.Length; i++) {
            if (!AllConditions.VerifyCondition(requiredConditions[i]))
                return false;
        }

        if (reactionCollection) {
            reactionCollection.React();
        } else {
            Debug.Log("No ReactionCollection set.");
        }
        return true;
    }
}