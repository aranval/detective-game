using UnityEngine;

public class AnimationReaction : DelayedReaction {
    public Animator animator;
    public string trigger;
    private int triggerHash;

    protected override void Exec() => triggerHash = Animator.StringToHash(trigger);
    protected override void InstantReaction() => animator.SetTrigger(triggerHash);
}