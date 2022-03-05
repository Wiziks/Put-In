using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EditState
{
    None,
    Buy,
    Drag
}

public class BuyByButton : MonoBehaviour
{
    [SerializeField] private int _price;
    [SerializeField] private TakeDamage _prefab;
    private TakeDamage _currentObject;
    private new Camera camera;
    private EditState editState;

    void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {
        //if (Time.timeScale == 0.01f) Debug.Log("+");

        if (editState == EditState.None) return;

        if (Input.GetMouseButton(0))
        {
            Vector3 cursorWorldPosition = camera.ScreenToWorldPoint(Input.mousePosition);
            cursorWorldPosition = new Vector3(cursorWorldPosition.x, cursorWorldPosition.y, 0);
            _currentObject.transform.position = cursorWorldPosition;
            editState = EditState.Drag;
        }

        if (editState == EditState.Buy) return;
        if (Input.GetMouseButtonUp(0))
        {
            _currentObject = null;
            Time.timeScale = 1f;
            editState = EditState.None;
        }
    }

    public void BuyWeapon()
    {
        if (!GameManager.Instance.TryBuy(_price)) return;

        editState = EditState.Buy;
        _currentObject = Instantiate(_prefab, new Vector2(-10000, -10000), Quaternion.identity);
        Time.timeScale = 0.01f;
    }
}
