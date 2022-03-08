using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class StartGameManager : MonoBehaviour
{
    [SerializeField] private UnityEvent _eventToStartGame;

    void Start()
    {
        Time.timeScale = 1f;
    }

    public void StartGame()
    {
        _eventToStartGame.Invoke();
        Destroy(gameObject);
    }
}
