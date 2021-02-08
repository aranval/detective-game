using UnityEngine;
public class ConditionReaction : Reaction {
    public Condition condition;
    public bool satisfied;

    protected override void InstantReaction() => condition.satisfied = satisfied;
}