using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform followTarget;
    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        if(followTarget != null)
        {
            transform.position = followTarget.position;
        }
    }
}
