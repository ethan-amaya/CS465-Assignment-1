using System.Collections;
using UnityEngine;
using TMPro;

public class GestureUI : MonoBehaviour
{
    public TextMeshProUGUI gestureText;
    private Coroutine clearCoroutine;

    public void ShowMessage(string message)
    {
        gestureText.text = message;

        if (clearCoroutine != null)
            StopCoroutine(clearCoroutine);
        clearCoroutine = StartCoroutine(ClearAfterDelay(2f));
    }

    public void ClearMessage()
    {
        // Only clear if no coroutine is running
    }

    IEnumerator ClearAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gestureText.text = "";
    }
}
