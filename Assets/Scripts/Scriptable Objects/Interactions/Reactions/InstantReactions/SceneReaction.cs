public class SceneReaction : Reaction {
    public string sceneName;
    //public UnityEngine.Transform startingPointInLoadedScene;

    private SceneController sceneController;

    protected override void Exec() => sceneController = FindObjectOfType<SceneController>();
    protected override void InstantReaction() => sceneController.FadeAndLoadScene(this);
}