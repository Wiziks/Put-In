using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleText : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private ParticleTextElement _textPrefab;
    private ParticleTextElement[] textPool;
    private int currentElement = 0;
    public static ParticleText Instance;
    void Start()
    {
        Instance = this;
        textPool = new ParticleTextElement[_poolCapacity];
        for (int i = 0; i < _poolCapacity; i++)
        {
            textPool[i] = Instantiate(_textPrefab, transform);
        }
    }

    public void ShowTextParticles(float value, Vector2 point)
    {
        Vector2 target = _mainCamera.WorldToScreenPoint(point);
        textPool[currentElement].Activate(value.ToString(), target);
        currentElement++;
        if (currentElement == _poolCapacity)
            currentElement = 0;
    }
}
