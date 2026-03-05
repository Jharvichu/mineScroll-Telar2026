using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PanelBlackText : MonoBehaviour
{
    public Image[] paragraphs;
    public Image blackPanel;

    public float fadeDuration = 2f;
    public float visibleTime = 2f;

    void Start()
    {
        
        foreach (Image img in paragraphs)
        {
            Color c = img.color;
            c.a = 0;
            img.color = c;
        }

        StartCoroutine(PlayCinematic());
    }

    IEnumerator PlayCinematic()
    {

        foreach (Image paragraph in paragraphs)
        {
            
            SetAlpha(paragraph, 1);

            
            yield return StartCoroutine(FadeBlack(1, 0));

            yield return new WaitForSeconds(visibleTime);

            
            yield return StartCoroutine(FadeBlack(0, 1));

           
            SetAlpha(paragraph, 0);
        }
    }

    IEnumerator FadeBlack(float from, float to)
    {
        float time = 0;
        Color color = blackPanel.color;

        while (time < fadeDuration)
        {
            color.a = Mathf.Lerp(from, to, time / fadeDuration);
            blackPanel.color = color;
            time += Time.deltaTime;
            yield return null;
        }

        color.a = to;
        blackPanel.color = color;
    }

    void SetAlpha(Image img, float value)
    {
        Color c = img.color;
        c.a = value;
        img.color = c;
    }
}

