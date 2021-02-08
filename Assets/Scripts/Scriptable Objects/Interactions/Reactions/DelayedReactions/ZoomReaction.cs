/**
 * TODO: Complete reaction and create corresponding editor
 */
public class ZoomReaction : DelayedReaction
{
    public UnityEngine.GameObject zoomedObject;
    public float cameraZoom = 5f;
    private ObjectZoomPanel objectZoomPanel;
    private bool useExistingObject = false;

    // get the ObjectZoomPanel overlord and master
    protected override void Exec() => objectZoomPanel = FindObjectOfType<ObjectZoomPanel>();
    
    // todo: actually execute the zoom mechanics
    protected override void InstantReaction() {
        if (objectZoomPanel != null) objectZoomPanel.ZoomOnObject(zoomedObject, cameraZoom, useExistingObject);
        else UnityEngine.Debug.Log("Zoomed on " + zoomedObject);
    }
}