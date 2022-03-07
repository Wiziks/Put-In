using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSelector : MonoBehaviour
{
    [SerializeField] private BodyPart _majorBodyPart;
    public static CircleSelector Instance;
    void Start()
    {
        Instance = this;
    }

    public BodyPart GetBodyPart() { return _majorBodyPart; }
}