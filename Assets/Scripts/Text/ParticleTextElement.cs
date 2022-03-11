using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ParticleTextElement : MonoBehaviour
{
    protected TextMeshProUGUI textComponent;
    protected Image imageComponent;
    protected RectTransform rectTransform;

    private void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        imageComponent = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
    }
    public virtual void Activate(string text, Vector2 point) { }
    protected virtual IEnumerator Moving() { yield return null; }
}
