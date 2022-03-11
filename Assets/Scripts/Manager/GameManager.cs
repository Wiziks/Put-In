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
    float score;
    float rawScore;

    [Header("Aircraft Speed")]
    [SerializeField] private float _startSpeed = 0.5f;
    public float Speed { get; private set; }
    private float delta = 0.0001f;

    [Header("Game Panels")]
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _losePanel;
    public static GameManager Instance;

    [Header("Editing World")]
    [SerializeField] private Transform _obstacles;
    [SerializeField] private Transform _walls;
    [SerializeField] private Transform _roof;
    [SerializeField] private Transform _floor;

    [Header("World Borders")]
    [SerializeField] private float _horizontalBorder = 6.5f;
    [SerializeField] private float _verticalBorder = 5.5f;


    public Dictionary<Vector2, Weapon> WeaponDictionary = new Dictionary<Vector2, Weapon>();

    private string startSpeedSave = "StartSpeedSave";
    private string deltaSpeedSave = "StartSpeedSave";

    private float scoreMultiplier = 10f;
    private string scoreMultiplierSave = "ScoreMultiplierSave";

    void Start()
    {
        if (PlayerPrefs.HasKey(startSpeedSave))
            _startSpeed = PlayerPrefs.GetFloat(startSpeedSave);

        if (PlayerPrefs.HasKey(deltaSpeedSave))
            delta = PlayerPrefs.GetFloat(deltaSpeedSave);

        if (PlayerPrefs.HasKey(scoreMultiplierSave))
            scoreMultiplier = PlayerPrefs.GetFloat(scoreMultiplierSave);

        Instance = this;
        _scoreText.enabled = true;
        Time.timeScale = 1f;
        Speed = _startSpeed;
        UpdateScore();
    }

    IEnumerator SpeedChanger()
    {
        while (true)
        {
            Speed += delta;
            yield return null;
        }
    }

    public void AddScore(float currentDamage, float multiplier, Vector2 point)
    {
        float currentScore = 0;
        currentScore += currentDamage * multiplier;
        if (!Pointer.CheckHooked())
            currentScore *= 1.1f;
        score += currentScore;

        rawScore += currentScore;
        if (rawScore % scoreMultiplier == 0)
        {
            Resource.Instance.ChangeValue(1);
        }

        UpdateScore();
        ParticleText.Instance.ShowTextParticles(currentScore, point);
    }

    public bool TryBuy(int price)
    {
        if (score < price)
        {
            AudioManager.Instance.PlaySoundNEM();
            InformationText.Instance.Activate();
            return false;
        }
        score -= price;
        UpdateScore();
        return true;
    }

    // private void WinGame()
    // {
    //     Body.Instance.FallApart();
    //     Invoke(nameof(ShowWinPanel), 1f);
    // }

    private void ShowWinPanel()
    {
        _winPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void GameOver()
    {
        _losePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    [ContextMenu("Reload")]
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void UpdateScore()
    {
        _scoreText.text = $"Прогрес:{(int)score}";
    }

    public Transform GetParent() { return _obstacles; }

    public float GetClosestXWall(Vector3 position)
    {
        float minDistance = float.MaxValue;
        float closestX = float.MaxValue;
        for (int i = 0; i < _walls.childCount; i++)
        {
            Transform childTransform = _walls.GetChild(i).transform;
            float distance = Vector2.Distance(childTransform.position, position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestX = childTransform.position.x + childTransform.localScale.x;
            }
        }
        return closestX;
    }

    public float GetClosestYFloor(Vector3 position)
    {
        float minDistance = float.MaxValue;
        float closestY = float.MaxValue;
        for (int i = 0; i < _floor.childCount; i++)
        {
            Transform childTransform = _floor.GetChild(i).transform;
            float distance = Vector3.Distance(childTransform.position, position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestY = childTransform.position.y + childTransform.localScale.y / 2;
            }
        }
        return closestY;
    }

    public float GetClosestYRoof(Vector3 position)
    {
        float minDistance = float.MaxValue;
        float closestY = float.MaxValue;
        for (int i = 0; i < _roof.childCount; i++)
        {
            Transform childTransform = _roof.GetChild(i).transform;
            float distance = Vector3.Distance(childTransform.position, position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestY = childTransform.position.y - childTransform.localScale.y / 2;
            }
        }
        return closestY;
    }

    public float GetVerticalBorder() { return _verticalBorder; }
    public float GetHorizontalBorder() { return _horizontalBorder; }

    [ContextMenu("Delete all")]
    public void DeleteAll() { PlayerPrefs.DeleteAll(); }

    public void SetStartSpeed(string value)
    {
        float.TryParse(value, out _startSpeed);
        PlayerPrefs.SetFloat(startSpeedSave, _startSpeed);
        PlayerPrefs.Save();
    }

    public void SetDeltaSpeed(string value)
    {
        float.TryParse(value, out delta);
        PlayerPrefs.SetFloat(deltaSpeedSave, delta);
        PlayerPrefs.Save();
    }

    public void SetScoreMultiplier(string value)
    {
        float.TryParse(value, out scoreMultiplier);
        PlayerPrefs.SetFloat(scoreMultiplierSave, scoreMultiplier);
        PlayerPrefs.Save();
    }

    public float GetStartSpeed() { return _startSpeed; }
    public float GetDeltaSpeed() { return delta; }
    public float GetScoreMultiplier() { return scoreMultiplier; }

    public void StopGame()
    {
        Time.timeScale = 0f;
        AudioManager.Instance.StopMusic();
    }
    public void ContinueGame()
    {
        Time.timeScale = 1f;
        AudioManager.Instance.ContinueMusic();
    }
}
