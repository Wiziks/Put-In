using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public enum Phases
{
    One,
    Two,
    TwoOne,
    TwoTwo,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
}

public class TutorialScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private StartGameManager _startGameManager;
    [SerializeField] private GameObject _UIPanel;
    [SerializeField] private GameObject _gamePanel;
    [SerializeField] private GameObject _instructionPanel;
    [SerializeField] private Aircraft _aircraft;

    [SerializeField] private RectTransform _handRect;
    [SerializeField] private Image _hand;
    [SerializeField] private Sprite _tapSprite;
    [SerializeField] private Sprite _untapSprite;

    [SerializeField] private TextMeshProUGUI[] _tutorialTexts;

    private bool needTutorial = false;
    private string keyName = "TutorialKey";
    private Vector2 startBodyPosition;
    private Vector2 screenCenter;
    private bool canMove;
    private bool canTap;
    public Phases phases;
    public static TutorialScript Instance;

    private void Start()
    {
        phases = Phases.One;
        Instance = this;

        if (PlayerPrefs.HasKey(keyName))
            needTutorial = PlayerPrefs.GetInt(keyName) == 1 ? true : false;
        else
            needTutorial = true;

        if (!needTutorial)
        {
            Destroy(gameObject);
            return;
        }

        _startGameManager.gameObject.SetActive(false);
        _UIPanel.SetActive(false);
        _gamePanel.SetActive(true);
        CameraManager.Instance.Zoom(5);
        Body.Instance.gameObject.SetActive(true);
        startBodyPosition = CameraManager.Instance.Camera.WorldToScreenPoint(CircleSelector.Instance.GetBodyPart().transform.position);
        _instructionPanel.SetActive(true);
        _tutorialTexts[0].gameObject.SetActive(true);
        _tutorialTexts[0].text = $"{Localization.Instance.GetRightPhase(13)}";
        screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        canMove = true;
        canTap = true;
        _handRect.position = startBodyPosition;
        Pointer.Instance.gameObject.SetActive(true);
    }

    void Update()
    {
        if (phases == Phases.One)
            PhaseOne();
        else if (phases == Phases.Two)
            PhaseTwo();
        else if (phases == Phases.TwoOne)
            PhaseTwoOne();
        else if (phases == Phases.TwoTwo)
            PhaseTwoTwo();
        else if (phases == Phases.Three)
            PhaseThree();
        else if (phases == Phases.Four)
            PhaseFour();
    }

    void PhaseOne()
    {
        if (Pointer.CheckHooked())
        {
            ShopPanel.Instance.gameObject.SetActive(false);
            _tutorialTexts[0].gameObject.SetActive(false);
            _hand.enabled = false;
            Invoke(nameof(PhaseTwo), 1f);
        }
        if (_handRect.position != (Vector3)screenCenter && canMove)
        {
            Tap();
            _handRect.position = Vector2.MoveTowards(_handRect.position, screenCenter, Time.deltaTime * 100f);
        }
        else
        {
            Untap();
            canMove = false;
            Invoke(nameof(AccessMove), 1f);
        }
    }


    void PhaseTwo()
    {
        if (Pointer.CheckHooked() && canTap)
        {
            Body.Instance.BecomeDynamic();
            _instructionPanel.SetActive(false);
            _tutorialTexts[1].gameObject.SetActive(false);
        }
        else
        {
            phases = Phases.Two;
            Body.Instance.BecomeStatic();
            _instructionPanel.SetActive(true);
            _tutorialTexts[1].gameObject.SetActive(true);
            _tutorialTexts[1].text = $"{Localization.Instance.GetRightPhase(14)}";
        }
    }

    void PhaseTwoOne()
    {
        Body.Instance.BecomeStatic();
        _instructionPanel.SetActive(true);
        _hand.enabled = true;
        Untap();
        _tutorialTexts[1].gameObject.SetActive(false);
        _tutorialTexts[2].gameObject.SetActive(true);
        _tutorialTexts[2].text = $"{Localization.Instance.GetRightPhase(15)}";
        _handRect.position = new Vector2(Screen.width / 2, Screen.height - 100);
    }

    void PhaseTwoTwo()
    {
        _tutorialTexts[2].gameObject.SetActive(false);
        _tutorialTexts[3].gameObject.SetActive(true);
        _tutorialTexts[3].text = $"{Localization.Instance.GetRightPhase(16)}";
        _handRect.position = new Vector2(100, Screen.height - 100);
    }

    void PhaseThree()
    {
        _tutorialTexts[3].gameObject.SetActive(false);
        _tutorialTexts[4].gameObject.SetActive(true);
        _tutorialTexts[4].text = $"{Localization.Instance.GetRightPhase(17)}";
        Pointer.Instance.enabled = false;
        Body.Instance.BecomeStatic();
        _aircraft.gameObject.SetActive(true);
        _aircraft.enabled = true;
        _aircraft.SetSpeedMultiplier(5f);
    }

    void PhaseFour()
    {
        _aircraft.SetSpeedMultiplier(1f);
    }

    void AccessMove()
    {
        _handRect.position = startBodyPosition;
        canMove = true;
    }


    void Tap()
    {
        _hand.sprite = _tapSprite;
    }

    void Untap()
    {
        _hand.sprite = _untapSprite;
    }

    void FinishTutorial()
    {
        PlayerPrefs.SetInt(keyName, needTutorial ? 1 : 0);
        PlayerPrefs.Save();
        Destroy(gameObject);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!canTap) return;
        if (phases == Phases.TwoTwo)
            phases = Phases.Three;
        else if (phases == Phases.TwoOne)
            phases = Phases.TwoTwo;
        canTap = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        canTap = true;
    }
}
