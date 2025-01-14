/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// © 2017 TheFlyingKeyboard and released under MIT License
// theflyingkeyboard.net
//Shows string and then hides it with time step
public class FlashingText : MonoBehaviour
{
    [SerializeField] public Text textToUse;
    [SerializeField] private bool useThisText = false;
    [SerializeField] private bool useThisTextText = false;
    [SerializeField] private string flashingString;
    [SerializeField] private float textPause = 0.5f;
    private void Start()
    {
        if (useThisText)
        {
            textToUse = GetComponent<Text>();
        }
        if (useThisTextText)
        {
			flashingString = textToUse.text;
        }
        textToUse.text = "";
        StartCoroutine(TypeText(textToUse, flashingString, textPause));
    }
    private IEnumerator TypeText(Text text, string stringToUse, float timePause)
    {
        bool show = true;
        while (true)
        {
            if (show)
            {
                textToUse.text = stringToUse;
            }
            else
            {
                textToUse.text = "";
            }
            show = !show;
            yield return 0;
            yield return new WaitForSeconds(timePause);
        }
    }
    public void WriteText(Text newText = null, string newTextToShow = null, float newTextPause = -1.0f)
    {
        if (newText != null && newTextToShow != null && newTextPause > 0.0f)
        {
            StartCoroutine(TypeText(newText, newTextToShow, newTextPause));
            return;
        }
        if (newText != null && newTextToShow != null)
        {
            StartCoroutine(TypeText(newText, newTextToShow, textPause));
            return;
        }
        if (newText != null && newTextPause > 0.0f)
        {
            StartCoroutine(TypeText(newText, flashingString, newTextPause));
            return;
        }
        if (newTextToShow != null && newTextPause > 0.0f)
        {
            StartCoroutine(TypeText(textToUse, newTextToShow, newTextPause));
            return;
        }
        if (newTextToShow != null)
        {
            StartCoroutine(TypeText(textToUse, newTextToShow, textPause));
            return;
        }
        if (newTextPause > 0.0f)
        {
            StartCoroutine(TypeText(textToUse, flashingString, textPause));
            return;
        }
    }
}
*/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FlashingText : MonoBehaviour
{

	Text flashingText;

	void Start()
	{
		//get the Text component
		flashingText = GetComponent<Text>();
		//Call coroutine BlinkText on Start
		StartCoroutine(BlinkText());
	}

	//function to blink the text
	public IEnumerator BlinkText()
	{
		//blink it forever. You can set a terminating condition depending upon your requirement
		while (true)
		{
			//set the Text's text to blank
			flashingText.text = "";
			//display blank text for 0.5 seconds
			yield return new WaitForSeconds(.5f);
			//display “I AM FLASHING TEXT” for the next 0.5 seconds
			flashingText.text = "I AM FLASHING TEXT!";
			yield return new WaitForSeconds(.5f);
		}
	}

}