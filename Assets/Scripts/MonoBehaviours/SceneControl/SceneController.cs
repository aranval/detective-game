using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Zarzadzanie scenami + fade
 * https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.html
 */
public class SceneController : MonoBehaviour {
    public CanvasGroup fadeCanvasGroup;
    public string startingSceneName = "Test";
    public bool doNotLoadScene = false;
    public bool doNotSetActiveScene = false;
    private float fadeDuration = 1f;

    private IEnumerator Start() {
        fadeCanvasGroup.alpha = 1f;
        // load scene
        if (!doNotLoadScene) yield return StartCoroutine(LoadScene(startingSceneName));
        // set active scene
        if (!doNotSetActiveScene) SceneManager.SetActiveScene(SceneManager.GetSceneAt(1));
        // fade in
        StartCoroutine(Fade (0f));
    }

    public void FadeAndLoadScene(SceneReaction sceneReaction) {
       StartCoroutine(SwitchScenes(sceneReaction.sceneName));
    }

    public void FadeAndLoadScene(string sceneName) {
       StartCoroutine(SwitchScenes(sceneName));
    }

    private IEnumerator SwitchScenes(string sceneName) {
        // wait for screen to fade to black
        yield return StartCoroutine(Fade (1f));
        // load the new scene
        yield return SceneManager.LoadSceneAsync(sceneName: sceneName, LoadSceneMode.Additive);
        // unload old scene
        yield return SceneManager.UnloadSceneAsync(scene: SceneManager.GetActiveScene());
        // set active scene
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        // move the player to a starting position in the scene
        //FindObjectOfType<PlayerMovement>().transform.position = playerStartingPosition;
        // fade from black
        yield return StartCoroutine(Fade (0f));
    }

    private IEnumerator LoadScene(string sceneName) {
        yield return SceneManager.LoadSceneAsync(sceneName: sceneName, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
    }

    private IEnumerator Fade(float finalAlpha) {
        fadeCanvasGroup.blocksRaycasts = true; //wylacza input
        float fadeSpeed = Math.Abs(fadeCanvasGroup.alpha - finalAlpha) / fadeDuration;
        
        while(!(fadeCanvasGroup.alpha == finalAlpha)) {
            fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, finalAlpha, fadeSpeed * Time.deltaTime);
            yield return null; //czeka klatkę
        }
        fadeCanvasGroup.blocksRaycasts = false;
    }
    
    public void QuitToDesktop() {
        StartCoroutine(FadeAndQuitToDesktop());
    }

    private IEnumerator FadeAndQuitToDesktop() {
        // wait for screen to fade to black
        yield return StartCoroutine(Fade (1f));
        // quit to desktop
        Application.Quit();
        Debug.Log("Application Quit.");
    }
}