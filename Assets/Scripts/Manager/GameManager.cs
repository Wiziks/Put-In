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
    float coinScore;

    [Header("Aircraft Speed")]
    [SerializeField] private float _startSpeed = 0.5f;
    public float Speed { get; private set; }
    [SerializeField] private float delta = 0.0006f;

    [Header("Game Panels")]
    [SerializeField] private GameObject _losePanel;
    [SerializeField] private TextMeshProUGUI _finalScoreText;
    public static GameManager Instance;

    [Header("Editing World")]
    [SerializeField] private Transform _obstacles;
    [SerializeField] private Transform _walls;
    [SerializeField] private Transform _roof;
    [SerializeField] private Transform _floor;

    [Header("World Borders")]
    [SerializeField] private float _horizontalBorder = 6.5f;
    [SerializeField] private float _verticalBorder = 5.5f;

    [Header("Weapons")]
    [SerializeField] private Weapon[] _weapons;
    [SerializeField] private Slide _slidePanel;
    [SerializeField] private Image _imageUnlockWeapon;
    [SerializeField] private TextMeshProUGUI _nameUnlockWeapon;


    public Dictionary<Vector2, Weapon> WeaponDictionary = new Dictionary<Vector2, Weapon>();

    private string startSpeedSave = "StartSpeedSave";
    private string deltaSpeedSave = "StartSpeedSave";

    private int scoreMultiplier = 4;
    private string scoreMultiplierSave = "ScoreMultiplierSave";

    void Start()
    {
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
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }

    public void AddScore(float currentDamage, float multiplier, Vector2 point)
    {
        float currentScore = 0;
        currentScore += currentDamage * multiplier;
        if (!Pointer.CheckHooked())
            currentScore *= 1.1f;
        score += currentScore;
        coinScore += currentScore;
        Resource.Instance.SetValue((int)(coinScore / scoreMultiplier));
        foreach (Weapon weapon in _weapons)
        {
            if (!weapon.GetActive())
            {
                if (score >= weapon.GetUnlockScore())
                {
                    weapon.SetActive(true);
                    ShowUnlockPanel(weapon);
                }
            }
        }
        UpdateScore();
        ParticleText.Instance.ShowTextParticles(currentScore, point);
    }

    [ContextMenu("Show Unlock Panel")]
    public void ShowUnlockPanel(Weapon weapon)
    {
        _imageUnlockWeapon.sprite = weapon.GetSprite();
        float temp;
        if (_imageUnlockWeapon.sprite.bounds.extents.x > _imageUnlockWeapon.sprite.bounds.extents.y)
        {
            temp = _imageUnlockWeapon.sprite.bounds.extents.y / _imageUnlockWeapon.sprite.bounds.extents.x;
            _imageUnlockWeapon.rectTransform.sizeDelta = new Vector2(75, temp * 75);
        }
        else
        {
            temp = _imageUnlockWeapon.sprite.bounds.extents.x / _imageUnlockWeapon.sprite.bounds.extents.y;
            _imageUnlockWeapon.rectTransform.sizeDelta = new Vector2(temp * 75, 75);
        }
        _nameUnlockWeapon.text = $"<b>{Localization.Instance.GetRightPhase(8)}</b>\n{weapon.GetName()}";
        _slidePanel.Sliding(1080);
        Invoke(nameof(HideSlidePanel), 3f);
    }

    void HideSlidePanel()
    {
        _slidePanel.Sliding(1180);
    }

    public void GameOver()
    {
        Resource.Instance.SaveMaxValue((int)score);
        _losePanel.SetActive(true);
        _finalScoreText.text = $"{Localization.Instance.GetRightPhase(1)}: {(int)score}";
        Body.Instance.BecomeStatic();
        Time.timeScale = 0f;
    }

    public void ContinueGame()
    {
        Time.timeScale = 1f;
        _losePanel.SetActive(false);
        CircleSelector.Instance.GetBodyPart().transform.localPosition = CircleSelector.Instance.StartPosition;
        Aircraft.Instance.transform.position = Aircraft.Instance.StartPosition;
        Body.Instance.BecomeStatic();
    }

    [ContextMenu("Reload")]
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void UpdateScore()
    {
        _scoreText.text = $"{Localization.Instance.GetRightPhase(0)}: {(int)score}";
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
        int.TryParse(value, out scoreMultiplier);
        PlayerPrefs.SetInt(scoreMultiplierSave, scoreMultiplier);
        PlayerPrefs.Save();
    }

    public float GetStartSpeed() { return _startSpeed; }
    public float GetDeltaSpeed() { return delta; }
    public float GetScoreMultiplier() { return scoreMultiplier; }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        AudioManager.Instance.StopMusic();
    }
    public void UnpauseGame()
    {
        Time.timeScale = 1f;
        AudioManager.Instance.ContinueMusic();
    }

    public void DivCoinScore(int value)
    {
        coinScore -= value * scoreMultiplier;
    }
}
