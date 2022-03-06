using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    void Start()
    {
        camera = Camera.main;
        _priceText.text = _prefab.GetPrice().ToString();
        followTarget = camera.GetComponent<CameraManager>();
    }

    void Update()
    {
        if (editState == EditState.None) return;

        if (Input.GetMouseButton(0))
        {
            Vector3 cursorWorldPosition = camera.ScreenToWorldPoint(Input.mousePosition);
            cursorWorldPosition = new Vector3(cursorWorldPosition.x, cursorWorldPosition.y, 0);

            CameraManager.Instance.ConstantTarget(Vector2.zero);

            if (_currentWeapon.GetPlaceType() == PlaceType.Walls)
            {
                cursorWorldPosition = new Vector3(GameManager.Instance.GetClosestXWall(cursorWorldPosition), cursorWorldPosition.y);
            }
            else if (_currentWeapon.GetPlaceType() == PlaceType.Floor)
            {
                cursorWorldPosition = new Vector3(cursorWorldPosition.x, GameManager.Instance.GetClosestYFloor(cursorWorldPosition));
            }
            else if (_currentWeapon.GetPlaceType() == PlaceType.Roof)
            {
                cursorWorldPosition = new Vector3(cursorWorldPosition.x, GameManager.Instance.GetClosestYRoof(cursorWorldPosition));
            }

            float x = Mathf.Clamp(cursorWorldPosition.x, -GameManager.Instance.GetHorizontalBorder(), GameManager.Instance.GetHorizontalBorder());
            float y = Mathf.Clamp(cursorWorldPosition.y, -GameManager.Instance.GetVerticalBorder(), GameManager.Instance.GetVerticalBorder());

            _currentWeapon.transform.position = new Vector2(x, y);
            editState = EditState.Drag;
        }
    }

    public void BuyWeapon()
    {
        if (!GameManager.Instance.TryBuy(_prefab.GetPrice())) return;

        editState = EditState.Buy;
        _currentWeapon = Instantiate(_prefab, GameManager.Instance.GetParent());
        Time.timeScale = 0.1f;
        CameraManager.Instance.Zoom(7);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        BuyWeapon();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _currentWeapon = null;
        Time.timeScale = 1f;
        editState = EditState.None;
        CameraManager.Instance.Zoom(5);
    }
}
