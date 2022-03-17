using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : MonoBehaviour
{
    [SerializeField] private float _slideSpeed = 2000f;
    [SerializeField] private GameObject _exitButton;
    private RectTransform rectTransform;
    private Vector2 startPosition;
    bool inGame;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.position;
    }
    public void Sliding(float y)
    {
        if (inGame)
            Disappear();
        StartCoroutine(SlidingCoroutine(y));
    }

    public IEnumerator SlidingCoroutine(float y)
    {
        Vector2 targetVector = new Vector2(rectTransform.position.x, y);
        while (Mathf.Abs(rectTransform.position.y) != Mathf.Abs(y))
        {
            rectTransform.position = Vector2.MoveTowards(transform.position, targetVector, Time.deltaTime * _slideSpeed);
            yield return null;
        }
        rectTransform.position = targetVector;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void Appear()
    {
        gameObject.SetActive(true);
        rectTransform.position = new Vector2(0, 250);
        ShopPanel.Instance.gameObject.SetActive(false);
        inGame = true;
        _exitButton.SetActive(true);
        ShopPanel.Instance.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Disappear()
    {
        rectTransform.position = startPosition;
        ShopPanel.Instance.gameObject.SetActive(true);
        Time.timeScale = 1f;
        _exitButton.SetActive(false);
        gameObject.SetActive(false);
        ShopPanel.Instance.gameObject.SetActive(false);
    }
}