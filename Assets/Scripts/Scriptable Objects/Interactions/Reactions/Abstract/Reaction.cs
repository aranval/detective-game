using UnityEngine;

public abstract class Reaction : ScriptableObject {
    public void Init() {
        Exec();
    }

    protected virtual void Exec() { }

    public void React(MonoBehaviour monoBehaviour) => InstantReaction();
    protected abstract void InstantReaction();
}