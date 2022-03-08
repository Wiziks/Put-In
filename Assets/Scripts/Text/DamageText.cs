using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private TextElement _textPrefab;
    private TextElement[] textPool;
    private int currentElement = 0;
    public static DamageText Instance;
    void Start()
    {
        Instance = this;
        textPool = new TextElement[_poolCapacity];
        for (int i = 0; i < _poolCapacity; i++)
        {
            textPool[i] = Instantiate(_textPrefab, transform);
        }
    }

    public void ShowDamage(float damage, Vector2 point)
    {
        Vector2 target = _mainCamera.WorldToScreenPoint(point);
        textPool[currentElement].Activate(damage.ToString(), target);
        currentElement++;
        if (currentElement == _poolCapacity)
            currentElement = 0;
    }
}
