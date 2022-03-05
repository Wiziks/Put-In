using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private int _targetScore = 1000;
    float score;

    [Header("Time")]
    [SerializeField] private Image _leftTimeScale;
    [SerializeField] private TextMeshProUGUI _timeLeftText;
    [SerializeField] private float _timeToLose = 60f;
    private float _timer;

    [Header("Game Panels")]
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _losePanel;
    public static GameManager Instance;

    void Start()
    {
        Instance = this;
        _scoreText.enabled = true;
        _leftTimeScale.enabled = true;
        _timeLeftText.enabled = true;
        Time.timeScale = 1f;
        _timeLeftText.text = $"{(int)(_timeToLose - _timer)}";
        UpdateScore();
    }

    void Update()
    {
        //_timer += Time.deltaTime;

        _leftTimeScale.fillAmount = (_timeToLose - _timer) / _timeToLose;
        _timeLeftText.text = $"{(int)(_timeToLose - _timer)}";

        if (_timer > _timeToLose)
        {
            GameOver();
        }
    }

    public void AddScore(float currentDamage, float multiplier, Vector2 point)
    {
        float currentScore = 0;
        currentScore += currentDamage * multiplier;
        if (!Pointer.CheckHooked())
            currentScore *= 1.1f;
        score += currentScore;
        UpdateScore();
        DamageText.Instance.ShowDamage(currentScore, point);
        if (score > _targetScore)
            WinGame();
    }

    public bool TryBuy(int price)
    {
        if (score < price)
            return false;
        score -= price;
        UpdateScore();
        return true;
    }

    private void WinGame()
    {
        Body.Instance.FallApart();
        Invoke(nameof(ShowWinPanel), 1f);
    }

    private void ShowWinPanel()
    {
        _winPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    private void GameOver()
    {
        _losePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void UpdateScore()
    {
        _scoreText.text = $"Прогрес:{(int)score}";
    }
}
