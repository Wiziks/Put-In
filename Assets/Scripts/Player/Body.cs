using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.IK;

public class Body : MonoBehaviour
{
    [SerializeField] private GameObject _instructionPanel;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private BodyPart[] bodyParts;
    [SerializeField] private Animator _animator;
    [SerializeField] private IKManager2D _ikManager2D;
    [SerializeField] private HingeJoint2D[] jointsToApart;
    public static Body Instance;

    void Start()
    {
        Time.timeScale = 1f;
        Instance = this;
        //StartCoroutine(StartCutScene());
    }
    public void BecomeDynamic()
    {
        _animator.enabled = false;
        _ikManager2D.enabled = false;
        foreach (BodyPart bodyPart in bodyParts)
            bodyPart.BecomeDynamic();
    }

    public void BecomeStatic()
    {
        foreach (BodyPart bodyPart in bodyParts)
            bodyPart.BecomeStatic();
    }

    // public void FallApart()
    // {
    //     foreach (HingeJoint2D springJoint in jointsToApart)
    //     {
    //         springJoint.breakForce = 0f;
    //         springJoint.breakTorque = 0f;
    //     }
    // }

    IEnumerator StartCutScene()
    {
        Pointer.Instance.enabled = false;
        _mainCamera.orthographicSize = 2f;
        while (transform.position.x > 7.5f)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(7.5f, transform.position.y), Time.deltaTime * _speed);
            yield return null;
        }
        _animator.speed = 0f;
        while (_mainCamera.orthographicSize < 5f)
        {
            _mainCamera.orthographicSize += 0.01f;
            yield return null;
        }
        _instructionPanel.SetActive(true);
        Pointer.Instance.enabled = true;
    }

}
