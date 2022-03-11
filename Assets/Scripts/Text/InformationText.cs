using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InformationText : MonoBehaviour
{
    private RectTransform rect;
    private Vector2 startPosition;
    private TextMeshProUGUI messageText;
    private bool isPlaying = false;

    public static InformationText Instance;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        messageText = GetComponent<TextMeshProUGUI>();
        Instance = this;
        gameObject.SetActive(false);
    }
    public void Activate()
    {
        messageText.text = "Недостаточно монет...";
        gameObject.SetActive(true);
        rect.position = Input.mousePosition;
        startPosition = rect.position;
        messageText.color = new Color(messageText.color.r, messageText.color.g, messageText.color.b, 1);
        if (isPlaying)
        {
            gameObject.SetActive(false);
            gameObject.SetActive(true);
        }
        StartCoroutine(VanishAnimation());
    }

    IEnumerator VanishAnimation()
    {
        isPlaying = true;
        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            rect.position = Vector2.Lerp(startPosition, new Vector2(startPosition.x, startPosition.y + 100), t);
            messageText.color = new Color(messageText.color.r, messageText.color.g, messageText.color.b, 1 - t);
            yield return null;
        }
        gameObject.SetActive(false);
        isPlaying = false;
    }
}
