using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextManager : MonoBehaviour {
    private string message;
    private float delay;
    private float delayBetweenLetters = 0.05f;
    private float delayBeforeFadeOut = 2f;
    private TextMeshProUGUI textBox;
    private Image textBoxBg; 

    private void Awake() {
        textBox = GetComponent<TextMeshProUGUI>();
        textBoxBg = GetComponentInParent<Image>();
        if (textBox == null || textBoxBg == null) { throw new NullReferenceException("Some components have not been found on the " + name); }

        // hide the text box
        SetTextBoxPanelVisibility(false);
    }

    public void DisplayMessage(string _message, float _delay) {
        message = _message;
        delay = _delay;

        StopAllCoroutines();
        // hide the text box
        SetTextBoxPanelVisibility(false);
        StartCoroutine(TypeTextCoroutine());
        //throw new NotImplementedException("We should actually print the text now. Text: " + message + " / delay: " + delay);
    }

    protected IEnumerator TypeTextCoroutine() {
        // clear text
        textBox.text = "";
        // wait a while before typing the text
        yield return new WaitForSeconds(delay);
        // show the text box
        SetTextBoxPanelVisibility(true);
        // type the message in with delay between letters
        for (int i = 0; i < message.Length; i++)
        {
            textBox.text += message[i];
            yield return new WaitForSeconds(delayBetweenLetters);
        }
        // wait a while before hiding the text box
        yield return new WaitForSeconds(delayBeforeFadeOut);
        // hide the text box
        SetTextBoxPanelVisibility(false);
    }

    public void SetTextBoxPanelVisibility(bool flag = true) {
        textBoxBg.enabled = flag;
        textBox.enabled = flag;
    }
}