using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

enum EditState
{
    None,
    Buy,
    Drag
}

public class BuildManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private Weapon _prefab;
    private Weapon _currentWeapon;
    private new Camera camera;
    private EditState editState;
    CameraManager followTarget;
    Image shopPanel;
    private Vector2 playerPosition;
    private Vector2 aircraftPosition;

    void Start()
    {
        camera = Camera.main;
        _priceText.text = _prefab.GetPrice().ToString();
        followTarget = camera.GetComponent<CameraManager>();
        shopPanel = transform.parent.GetComponent<Image>();
    }

    void Update()
    {
        if (editState == EditState.None) return;

        if (Input.GetMouseButton(0))
        {
            editState = EditState.Drag;
            CameraManager.Instance.ConstantTarget(Vector2.zero);
            Vector2 newPosition = SetPoint();
            if (CheckPosition(newPosition))
                _currentWeapon.transform.position = newPosition;
        }
    }

    private Vector2 SetPoint()
    {
        Vector3 cursorWorldPosition = camera.ScreenToWorldPoint(Input.mousePosition);
        cursorWorldPosition = new Vector3(cursorWorldPosition.x, cursorWorldPosition.y, 0);

        if (_currentWeapon.GetPlaceType() == PlaceType.Walls)
        {
            cursorWorldPosition = new Vector3(GameManager.Instance.GetClosestXWall(cursorWorldPosition), cursorWorldPosition.y);
        }
        else if (_currentWeapon.GetPlaceType() == PlaceType.Floor)
        {
            cursorWorldPosition = new Vector3(cursorWorldPosition.x, GameManager.Instance.GetClosestYFloor(cursorWorldPosition) + _currentWeapon.transform.localScale.y / 2);
        }
        else if (_currentWeapon.GetPlaceType() == PlaceType.Roof)
        {
            cursorWorldPosition = new Vector3(cursorWorldPosition.x, GameManager.Instance.GetClosestYRoof(cursorWorldPosition) - _currentWeapon.transform.localScale.y / 2);
        }

        float x = Mathf.Clamp(cursorWorldPosition.x, -GameManager.Instance.GetHorizontalBorder(), GameManager.Instance.GetHorizontalBorder());
        float y = Mathf.Clamp(cursorWorldPosition.y, -GameManager.Instance.GetVerticalBorder(), GameManager.Instance.GetVerticalBorder());
        Vector2 point = new Vector2(x, y);

        return point;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!GameManager.Instance.TryBuy(_prefab.GetPrice())) return;

        editState = EditState.Buy;
        _currentWeapon = Instantiate(_prefab, GameManager.Instance.GetParent());
        _currentWeapon.transform.position = AddRandomStartPosition();
        _currentWeapon.Collider.enabled = false;
        Time.timeScale = 0;
        CameraManager.Instance.Zoom(7);
        shopPanel.color = new Color(shopPanel.color.r, shopPanel.color.g, shopPanel.color.b, 0);
        AddTemporaryRecords();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        AddToDictionary();
        _currentWeapon.Collider.enabled = true;
        _currentWeapon = null;
        Time.timeScale = 1f;
        editState = EditState.None;
        CameraManager.Instance.Zoom(5);
        shopPanel.color = new Color(shopPanel.color.r, shopPanel.color.g, shopPanel.color.b, 53f / 225f);
    }

    Vector2 AddRandomStartPosition()
    {
        Vector2 point = new Vector2();
        do
        {
            point = new Vector2(Random.Range(-GameManager.Instance.GetHorizontalBorder(), GameManager.Instance.GetHorizontalBorder()),
               Random.Range(-GameManager.Instance.GetVerticalBorder(), GameManager.Instance.GetVerticalBorder()));
            if (_currentWeapon.GetPlaceType() == PlaceType.Walls)
            {
                point = new Vector3(GameManager.Instance.GetClosestXWall(point), point.y);
            }
            else if (_currentWeapon.GetPlaceType() == PlaceType.Floor)
            {
                point = new Vector3(point.x, GameManager.Instance.GetClosestYFloor(point) + _currentWeapon.transform.localScale.y / 2);
            }
            else if (_currentWeapon.GetPlaceType() == PlaceType.Roof)
            {
                point = new Vector3(point.x, GameManager.Instance.GetClosestYRoof(point) - _currentWeapon.transform.localScale.y / 2);
            }
        } while (!CheckPosition(point));
        return point;
    }

    bool CheckPosition(Vector2 point)
    {
        RoundToHalf(point, out float x, out float y);
        for (int i = 0; i < _currentWeapon.GetSize().x; i++)
        {
            for (int j = 0; j < _currentWeapon.GetSize().y; j++)
            {
                Vector2 cell = new Vector2(x + i, y + j);
                if (GameManager.Instance.WeaponDictionary.ContainsKey(cell))
                    return false;
                else if (cell == playerPosition || cell == aircraftPosition)
                    return false;
            }
        }
        return true;
    }

    void AddToDictionary()
    {
        RoundToHalf(_currentWeapon.transform.position, out float x, out float y);
        for (int i = 0; i < _currentWeapon.GetSize().x; i++)
        {
            for (int j = 0; j < _currentWeapon.GetSize().y; j++)
            {
                GameManager.Instance.WeaponDictionary.Add(new Vector2Int((int)x + i, (int)y + j), _currentWeapon);
            }
        }
        _currentWeapon.transform.position = new Vector2(x, y);
    }

    void RoundToHalf(Vector2 point, out float x, out float y)
    {
        x = Mathf.Abs(Mathf.Ceil(point.x * 2));
        y = Mathf.Abs(Mathf.Ceil(point.y * 2));
        if (x % 2 == 0)
            x++;
        if (y % 2 == 0)
            y++;
        x /= 2;
        y /= 2;
        if (point.x < 0)
            x *= -1;
        if (point.y < 0)
            y *= -1;
    }

    void AddTemporaryRecords()
    {
        RoundToHalf(CircleSelector.Instance.transform.position, out float x, out float y);
        x = Mathf.Clamp(x, -GameManager.Instance.GetHorizontalBorder(), GameManager.Instance.GetHorizontalBorder());
        y = Mathf.Clamp(y, -GameManager.Instance.GetVerticalBorder(), GameManager.Instance.GetVerticalBorder());
        CircleSelector.Instance.transform.parent.position = new Vector2(x, y);
        playerPosition = CircleSelector.Instance.transform.parent.position;


        RoundToHalf(Aircraft.Instance.transform.position, out x, out y);
        x = Mathf.Clamp(x, -GameManager.Instance.GetHorizontalBorder(), GameManager.Instance.GetHorizontalBorder());
        y = Mathf.Clamp(y, -GameManager.Instance.GetVerticalBorder(), GameManager.Instance.GetVerticalBorder());
        Aircraft.Instance.transform.position = new Vector2(x, y);
        aircraftPosition = Aircraft.Instance.transform.position;
    }
}
