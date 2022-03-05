using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{

    public Transform Target;
    public float LerpRate;

    void LateUpdate()
    {
        Vector3 targetVector = new Vector3(Target.position.x, Target.position.y, -10);
        transform.position = Vector3.Lerp(transform.position, targetVector, Time.deltaTime * LerpRate);

    }
}
