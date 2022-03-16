using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
    public static SnapScrolling Instance;
    [SerializeField] private Weapon[] _weapons;

    void Start()
    {
        Instance = this;
        panCount = _weapons.Length;

        rectTransform = GetComponent<RectTransform>();
        instPans = new GameObject[panCount];
        pansPos = new Vector2[panCount];
        pansScale = new Vector2[panCount];

        for (int i = 0; i < panCount; i++)
        {
            instPans[i] = Instantiate(panPrefab, transform, false);
            instPans[i].transform.GetChild(1).GetComponent<Image>().sprite = _weapons[i].GetSprite();
            SetSize(_weapons[i].GetSprite(), instPans[i].transform.GetChild(1).GetComponent<Image>());
            instPans[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = _weapons[i].GetName();
            if (_weapons[i].GetActive())
            {
                instPans[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = $"{Localization.Instance.GetRightPhase(4)} - {_weapons[i].GetCost()}";
                instPans[i].transform.GetChild(4).gameObject.SetActive(false);
                instPans[i].transform.GetChild(5).GetComponent<Button>().enabled = false;
                instPans[i].transform.GetChild(5).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{Localization.Instance.GetRightPhase(6)}";
            }
            else
            {
                instPans[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = $"{Localization.Instance.GetRightPhase(7)} {_weapons[i].GetUnlockScore()}";
                instPans[i].transform.GetChild(4).gameObject.SetActive(true);
                instPans[i].transform.GetChild(5).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{Localization.Instance.GetRightPhase(31)}";
                instPans[i].transform.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
                instPans[i].transform.GetChild(0).GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
                instPans[i].transform.GetChild(1).GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
                instPans[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().color = new Color(0.5f, 0.5f, 0.5f, 1);
                instPans[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().color = new Color(0.5f, 0.5f, 0.5f, 1);
            }
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

    // public void Refresh()
    // {
    //     for (int i = 0; i < panCount; i++)
    //     {
    //         if (_weapons[i].GetActive())
    //         {
    //             instPans[i].transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Куплено";
    //             instPans[i].transform.GetChild(5).gameObject.SetActive(false);
    //         }
    //         else
    //         {
    //             instPans[i].transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = $"Цена: {_weapons[i].GetUnlockScore()}";
    //             instPans[i].transform.GetChild(5).gameObject.SetActive(true);
    //         }
    //     }
    // }

    public void RefreshAll()
    {
        for (int i = 0; i < panCount; i++)
        {
            instPans[i].transform.GetChild(1).GetComponent<Image>().sprite = _weapons[i].GetSprite();
            SetSize(_weapons[i].GetSprite(), instPans[i].transform.GetChild(1).GetComponent<Image>());
            instPans[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = _weapons[i].GetName();
            if (_weapons[i].GetActive())
            {
                instPans[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = $"{Localization.Instance.GetRightPhase(4)} - {_weapons[i].GetCost()}";
                instPans[i].transform.GetChild(4).gameObject.SetActive(false);
                instPans[i].transform.GetChild(5).GetComponent<Button>().enabled = false;
                instPans[i].transform.GetChild(5).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{Localization.Instance.GetRightPhase(6)}";
            }
            else
            {
                instPans[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = $"{Localization.Instance.GetRightPhase(7)} {_weapons[i].GetUnlockScore()}";
                instPans[i].transform.GetChild(4).gameObject.SetActive(true);
                instPans[i].transform.GetChild(5).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{Localization.Instance.GetRightPhase(31)}";
                instPans[i].transform.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
                instPans[i].transform.GetChild(0).GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
                instPans[i].transform.GetChild(1).GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
                instPans[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().color = new Color(0.5f, 0.5f, 0.5f, 1);
                instPans[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().color = new Color(0.5f, 0.5f, 0.5f, 1);
            }
        }
    }

    public void SetSize(Sprite sprite, Image image)
    {
        float temp;
        if (sprite.bounds.extents.x > sprite.bounds.extents.y)
        {
            temp = sprite.bounds.extents.y / sprite.bounds.extents.x;
            image.rectTransform.sizeDelta = new Vector2(400, temp * 400);
        }
        else
        {
            temp = sprite.bounds.extents.x / sprite.bounds.extents.y;
            image.rectTransform.sizeDelta = new Vector2(temp * 400, 400);
        }
    }
}
