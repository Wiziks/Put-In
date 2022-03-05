using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private GameObject _instructionPanel;
    [SerializeField] private Camera _camera;
    [SerializeField] private BodyPart _selectedBodyPart;
    private SpringJoint2D springJoint2D;
    private static bool isHooked;
    public static Pointer Instance;
    private bool firstTime;

    void Awake()
    {
        Instance = this;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector3 cursorWorldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        cursorWorldPosition = new Vector3(cursorWorldPosition.x, cursorWorldPosition.y, 0);
        transform.position = cursorWorldPosition;

        RaycastHit2D hit = Physics2D.Raycast(cursorWorldPosition, Vector2.zero);
        if (hit)
        {
            _selectedBodyPart = null;
            _selectedBodyPart = hit.collider.GetComponent<BodyPart>();
            if (_selectedBodyPart)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (!firstTime)
                    {
                        Body.Instance.BecomeDynamic();
                        _instructionPanel.SetActive(false);
                        _gameManager.gameObject.SetActive(true);
                        firstTime = true;
                    }
                    DestroySpringJoint();
                    springJoint2D = gameObject.AddComponent<SpringJoint2D>();
                    springJoint2D.connectedBody = _selectedBodyPart.GetComponent<Rigidbody2D>();
                    springJoint2D.autoConfigureDistance = false;
                    springJoint2D.distance = 0f;
                    springJoint2D.frequency = 5f;
                    springJoint2D.dampingRatio = 1f;
                    isHooked = true;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            DestroySpringJoint();
            isHooked = false;
        }
    }

    void DestroySpringJoint()
    {
        if (springJoint2D)
            Destroy(springJoint2D);
    }

    public static bool CheckHooked() { return isHooked; }
}
