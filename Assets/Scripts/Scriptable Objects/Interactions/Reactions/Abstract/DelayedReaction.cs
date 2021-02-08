using System.Collections;
using UnityEngine;

public abstract class DelayedReaction : Reaction {
    public float delay;

    protected WaitForSeconds wait;

    public new void Init() {
        wait = new WaitForSeconds(delay);
        Exec();
    }

    public new void React(MonoBehaviour monoBehaviour) {
        monoBehaviour.StartCoroutine(ReactCoroutine()); // https://docs.unity3d.com/Manual/Coroutines.html, necessary for function execution over the span of >1 frames
    }
    protected IEnumerator ReactCoroutine() {
        yield return wait;
        InstantReaction();
    }
}