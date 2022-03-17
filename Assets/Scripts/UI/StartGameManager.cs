using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class StartGameManager : MonoBehaviour
{
    [SerializeField] private GameObject _exitMenu;
    [SerializeField] private Slide _reviewPanel;
    [SerializeField] private GameObject _instructionPanel;
    [SerializeField] private GameObject _instructionText;
    [SerializeField] private UnityEvent _eventToStartGame;

    private string countOfGameEnter = "CountOfGameEnter";
    public static StartGameManager Instance;

    void Start()
    {
        Time.timeScale = 1f;
        Instance = this;
        if (PlayerPrefs.HasKey(countOfGameEnter))
        {
            int count = PlayerPrefs.GetInt(countOfGameEnter);
            PlayerPrefs.SetInt(countOfGameEnter, count + 1);
            PlayerPrefs.Save();
            if (count > 5 && count % 2 == 0)
                ShowReview();
        }
        else
        {
            PlayerPrefs.SetInt(countOfGameEnter, 0);
            PlayerPrefs.Save();
        }
    }

    public void StartGame()
    {
        _eventToStartGame.Invoke();
        if (!TutorialScript.Instance)
        {
            _instructionPanel.SetActive(true);
            _instructionText.SetActive(true);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Home) || Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Menu))
        {
            _exitMenu.SetActive(true);
        }
    }

    public void Yes() { Application.Quit(); }

    [ContextMenu("Show Review")]
    public void ShowReview()
    {
        _reviewPanel.Sliding(100f);
    }
}
