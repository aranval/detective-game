using UnityEngine;
using System;

public class ReactionCollection : MonoBehaviour {
    public Reaction[] reactions = new Reaction[0];

    private void Start() {
        for (int i = 0; i < reactions.Length; i++) {
            reactions[i].Init();
        }
    }

    public void React() {
        Debug.Log(reactions.Length);
        for (int i = 0; i < reactions.Length; i++) {
            Debug.Log("Reaction type " + reactions[i].GetType() + " was triggered.");
            reactions[i].React(this);
        }
    }
}
