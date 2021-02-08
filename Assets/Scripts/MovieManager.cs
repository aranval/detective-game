using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MovieManager : MonoBehaviour
{
    public VideoPlayer vp;
    public string sceneToLoadAfter = "Scene_Corridor_Mix";
    public bool quitGameAfterPlaythrough = false;

    // Start is called before the first frame update
    private void Start() {
        StartCoroutine("PlayTheClip");
    }

    private void Update() {
        // 'space' for pause / play
        if (Input.GetButtonDown("Jump")) {
            if (vp.isPlaying) {
                vp.Pause();
            }            
            else {
                vp.Play();
            }
        }
        // 'escape' for skip
        if (Input.GetButtonDown("Cancel")) {
            vp.Pause(); 
            EndReached(vp);
        }
    }

    private IEnumerator PlayTheClip() {
        yield return new WaitForSeconds(1f);
        vp.Play();
    }

    private void EndReached(VideoPlayer vp) {
        if (quitGameAfterPlaythrough) QuitGame();
        StartCoroutine("LoadScene");
    }

    private IEnumerator LoadScene() {
        yield return new WaitForSeconds(1f);
        FindObjectOfType<SceneController>().FadeAndLoadScene(sceneToLoadAfter);
    }

    private IEnumerator QuitGame() {
        yield return new WaitForSeconds(1f);
        FindObjectOfType<SceneController>().QuitToDesktop();
    }
}
