using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSelector : MonoBehaviour
{
    [SerializeField] private BodyPart _majorBodyPart;
    public static CircleSelector Instance;
    [SerializeField] public Vector3 StartPosition;
    void Awake()
    {
        Instance = this;
    }

    public BodyPart GetBodyPart() { return _majorBodyPart; }
}
