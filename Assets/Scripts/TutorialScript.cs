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
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _shopButton;
    [SerializeField] private GameObject _shopPanel;
    [SerializeField] private Button _closeShopButton;
    [SerializeField] private GameObject _gameShopPanel;

    [SerializeField] private RectTransform _handRect;
    [SerializeField] private Image _hand;
    [SerializeField] private Sprite _tapSprite;
    [SerializeField] private Sprite _untapSprite;

    [SerializeField] private TextMeshProUGUI[] _tutorialTexts;
    [SerializeField] private Image _tutorialImage;
    [SerializeField] private Weapon _saw;
    [SerializeField] private Button _ingameSettings;

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
        _hand.enabled = true;
        _saw.SetActive(true);
        AudioManager.Instance.SetMusicVolume(0f);
        _ingameSettings.interactable = false;
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
        else if (phases == Phases.Seven)
            PhaseSeven();
        else if (phases == Phases.Eight)
            PhaseEight();
    }
    bool oneTime = true;
    void PhaseOne()
    {
        if (Pointer.CheckHooked())
        {
            ShopPanel.Instance.gameObject.SetActive(false);
            _tutorialTexts[0].gameObject.SetActive(false);
            _hand.enabled = false;
            _instructionPanel.SetActive(false);
            if (oneTime)
            {
                Invoke(nameof(PhaseTwo), 0.5f);
                oneTime = false;
            }
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
        phases = Phases.Two;
        if (Pointer.CheckHooked())
        {
            _instructionPanel.SetActive(false);
            Body.Instance.BecomeDynamic();
            _instructionPanel.SetActive(false);
            _tutorialTexts[1].gameObject.SetActive(false);
        }
        else
        {
            _instructionPanel.SetActive(true);
            if (!Pointer.CheckHooked())
                Body.Instance.BecomeStatic();
            else
                Body.Instance.BecomeDynamic();
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
        Pointer.Instance.enabled = false;
        Untap();
        _tutorialTexts[1].gameObject.SetActive(false);
        _tutorialTexts[2].gameObject.SetActive(true);
        _tutorialTexts[2].text = $"{Localization.Instance.GetRightPhase(15)}";
        _handRect.position = new Vector2(0 + Screen.width / 2, 340 + Screen.height / 2);
    }

    void PhaseTwoTwo()
    {
        Body.Instance.BecomeStatic();
        _tutorialTexts[1].gameObject.SetActive(false);
        _tutorialTexts[2].gameObject.SetActive(false);
        _tutorialTexts[3].gameObject.SetActive(true);
        _tutorialTexts[3].text = $"{Localization.Instance.GetRightPhase(16)}";
        _handRect.position = new Vector2(-800 + Screen.width / 2, 340 + Screen.height / 2);
    }

    void PhaseThree()
    {
        _hand.enabled = false;
        _tutorialTexts[1].gameObject.SetActive(false);
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
        phases = Phases.Four;
        _tutorialImage.rectTransform.localScale = Vector3.zero;
        _aircraft.SetSpeedMultiplier(1f);
        _UIPanel.SetActive(true);
        _gamePanel.SetActive(false);
        _playButton.interactable = false;
        _settingsButton.interactable = false;
        _shopButton.interactable = true;
        _shopButton.onClick.AddListener(PhaseFive);
        _tutorialTexts[4].gameObject.SetActive(false);
        _tutorialTexts[5].gameObject.SetActive(true);
        _tutorialTexts[5].text = $"{Localization.Instance.GetRightPhase(18)}";
        _hand.enabled = true;
        _handRect.position = new Vector2(-500 + Screen.width / 2, -300 + Screen.height / 2);
        GameManager.Instance.ContinueGame();
    }

    public void PhaseFive()
    {
        phases = Phases.Five;
        _shopPanel.SetActive(true);
        _tutorialTexts[5].gameObject.SetActive(false);
        _tutorialTexts[6].gameObject.SetActive(true);
        _tutorialTexts[6].text = $"{Localization.Instance.GetRightPhase(19)}";
        _closeShopButton.onClick.AddListener(PhaseSix);
        _hand.enabled = true;
        _handRect.position = new Vector2(900 + Screen.width / 2, 350 + Screen.height / 2); ;
    }

    public void PhaseSix()
    {
        phases = Phases.Six;
        _shopPanel.SetActive(false);
        _tutorialTexts[6].gameObject.SetActive(false);
        _tutorialTexts[7].gameObject.SetActive(true);
        _tutorialTexts[7].text = $"{Localization.Instance.GetRightPhase(20)}";
        _UIPanel.SetActive(true);
        _playButton.interactable = true;
        _shopButton.interactable = false;
        _playButton.onClick.AddListener(GameManager.Instance.ContinueGame);
        _playButton.onClick.AddListener(SetSeven);
        _handRect.position = new Vector2(Screen.width / 2, -350 + Screen.height / 2);
    }

    public void SetSeven()
    {
        phases = Phases.Seven;
        Resource.Instance.SetValue(50);
        GameManager.Instance.AddScore(_saw.GetCost(), GameManager.Instance.GetScoreMultiplier(), new Vector2(Screen.width, Screen.height) * 2f);
        startBodyPosition = new Vector2(40 + Screen.width / 2, -500 + Screen.height / 2);
        _handRect.position = startBodyPosition;
    }

    public void PhaseSeven()
    {
        AudioManager.Instance.SetMusicVolume(1f);
        Pointer.Instance.enabled = true;
        _gameShopPanel.SetActive(true);
        _tutorialTexts[7].gameObject.SetActive(false);
        _tutorialTexts[8].gameObject.SetActive(true);
        _tutorialTexts[8].text = $"{Localization.Instance.GetRightPhase(21)}";
        _hand.enabled = true;
        if (_handRect.position != (Vector3)(screenCenter - new Vector2(250, 0)) && canMove)
        {
            Tap();
            _handRect.position = Vector2.MoveTowards(_handRect.position, screenCenter - new Vector2(250, 0), Time.deltaTime * 100f);
        }
        else
        {
            Untap();
            canMove = false;
            Invoke(nameof(AccessMove), 1f);
        }
    }

    void PhaseEight()
    {
        _ingameSettings.interactable = true;
        Body.Instance.BecomeDynamic();
        Aircraft.Instance.enabled = true;
        Destroy(Instance);
        Destroy(_instructionPanel);
        Destroy(_hand.gameObject);
        FinishTutorial();
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
        else if (phases == Phases.Three)
            phases = Phases.Four;
        _instructionPanel.SetActive(false);
        _hand.gameObject.SetActive(false);
        canTap = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        canTap = true;
        _instructionPanel.SetActive(true);
        _hand.gameObject.SetActive(true);
    }

    public string GetKeyName() { return keyName; }
}
