using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : MonoBehaviour
{
    private RectTransform rectTransform;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void Sliding(float y)
    {
        StartCoroutine(SlidingCoroutine(y));
    }

    public IEnumerator SlidingCoroutine(float y)
    {
        Vector2 targetVector = new Vector2(rectTransform.position.x, y);
        while (Mathf.Abs(rectTransform.position.y) != Mathf.Abs(y))
        {
            rectTransform.position = Vector2.MoveTowards(transform.position, targetVector, Time.deltaTime * 2000f);
            yield return null;
        }
        rectTransform.position = targetVector;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}