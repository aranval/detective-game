using UnityEditor;

[CustomEditor (typeof (ZoomReaction))]
public class ZoomReactionEditor : ReactionEditor 
{   
    protected override string GetFoldoutLabel ()
    {
        return "Zoom Reaction";
    }
}