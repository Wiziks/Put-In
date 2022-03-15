using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class StartGameManager : MonoBehaviour
{
    [SerializeField] private GameObject _exitMenu;
    [SerializeField] private UnityEvent _eventToStartGame;
    public static StartGameManager Instance;

    void Start()
    {
        Time.timeScale = 1f;
        Instance = this;
    }

    public void StartGame()
    {
        _eventToStartGame.Invoke();
        if (!TutorialScript.Instance)
            Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Home) || Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Menu))
        {
            _exitMenu.SetActive(true);
        }
    }

    public void Yes() { Application.Quit(); }
}
