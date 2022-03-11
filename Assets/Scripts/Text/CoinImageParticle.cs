using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinImageParticle : ParticleTextElement
{
    public override void Activate(string text, Vector2 point)
    {
        textComponent.enabled = true;
        textComponent.text = text;
        rectTransform.position = point;
        StartCoroutine(Moving());
    }

    protected override IEnumerator Moving()
    {
        float alpha = 1f;
        while (alpha >= 0)
        {
            textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, alpha);
            alpha -= 0.01f;
            yield return null;
        }
        textComponent.enabled = false;
    }
}
