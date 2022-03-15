using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class StartGameManager : MonoBehaviour
{
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
        if(!TutorialScript.Instance)
            Destroy(gameObject);
    }
}
