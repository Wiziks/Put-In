using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SnapScrolling : MonoBehaviour
{
    private int panCount;
    [Range(0, 500)][SerializeField] private int panOffset;
    [SerializeField] private GameObject panPrefab;
    [SerializeField][Range(0f, 20f)] private float snapSpeed;
    [SerializeField][Range(1f, 5f)] private float scaleOffset;
    [SerializeField][Range(1f, 20f)] private float scaleSpeed;
    [SerializeField] private ScrollRect scrollRect;
    private GameObject[] instPans;
    private Vector2[] pansPos;
    private RectTransform rectTransform;
    private int selectID;
    private bool isScrolling;
    private Vector2 contVec;
    private Vector2[] pansScale;
    [SerializeField] private Sprite[] _sprite;
    [SerializeField] private string[] _nameText;
    [SerializeField] private int[] _cost;
    [SerializeField] private int[] _strength;
    [SerializeField] private int[] _price;

    void Start()
    {
        panCount = _sprite.Length;

        rectTransform = GetComponent<RectTransform>();
        instPans = new GameObject[panCount];
        pansPos = new Vector2[panCount];
        pansScale = new Vector2[panCount];

        for (int i = 0; i < panCount; i++)
        {
            instPans[i] = Instantiate(panPrefab, transform, false);
            instPans[i].transform.GetChild(0).GetComponent<Image>().sprite = _sprite[i];
            instPans[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = _nameText[i];
            instPans[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = $"Стоимость - {_cost[i]}";
            instPans[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = $"Прочность - {_strength[i]}";
            instPans[i].transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = $"Цена: {_price[i]}";
            if (i == 0) continue;
            instPans[i].transform.localPosition = new Vector2(instPans[i - 1].transform.localPosition.x +
            panPrefab.GetComponent<RectTransform>().sizeDelta.x + panOffset, instPans[i].transform.localPosition.y);
            pansPos[i] = -instPans[i].transform.localPosition;
        }
    }

    void FixedUpdate()
    {
        if ((rectTransform.anchoredPosition.x >= pansPos[0].x || rectTransform.anchoredPosition.x <= pansPos[pansPos.Length - 1].x) && !isScrolling)
            scrollRect.inertia = false;
        float nearestPos = float.MaxValue;
        for (int i = 0; i < panCount; i++)
        {
            float distance = Mathf.Abs(rectTransform.anchoredPosition.x - pansPos[i].x);
            if (distance < nearestPos)
            {
                nearestPos = distance;
                selectID = i;
            }
            float scale = Mathf.Clamp(1 / (distance / panOffset) * scaleOffset, 0.5f, 1f);
            pansScale[i].x = Mathf.SmoothStep(instPans[i].transform.localScale.x, scale, scaleSpeed * Time.fixedDeltaTime);
            pansScale[i].y = Mathf.SmoothStep(instPans[i].transform.localScale.y, scale, scaleSpeed * Time.fixedDeltaTime);
            instPans[i].transform.localScale = pansScale[i];
        }
        float scrollVelocity = Mathf.Abs(scrollRect.velocity.x);
        if (scrollVelocity < 400 && !isScrolling) scrollRect.inertia = false;

        if (isScrolling || scrollVelocity > 400) return;
        contVec.x = Mathf.SmoothStep(rectTransform.anchoredPosition.x, pansPos[selectID].x, snapSpeed * Time.fixedDeltaTime);
        rectTransform.anchoredPosition = contVec;
    }

    public void Scrolling(bool scroll)
    {
        isScrolling = scroll;
        if (scroll) scrollRect.inertia = true;
    }

    public int GetID()
    {
        return selectID;
    }

    public void CleanPrice()
    {
        instPans[selectID].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = null;
    }

    public bool GetScroll()
    {
        return isScrolling;
    }

    
}
