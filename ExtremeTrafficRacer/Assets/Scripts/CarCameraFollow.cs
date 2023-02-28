using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCameraFollow : MonoBehaviour
{
    [SerializeField] private Transform Target;
    private float speed = 15f;

    Vector3 dist;

    private void Start()
    {
        dist = transform.position - Target.position;
    }

    private void FixedUpdate()
    {
        if (Target != null)
        {
            Vector3 target = Target.position + dist;
            transform.position = Vector3.Lerp(transform.position, target, speed * Time.smoothDeltaTime);

            Vector3 targetRot = Target.localEulerAngles;
            targetRot.x = 90;
            targetRot.z = Mathf.Clamp(targetRot.z, -5, 5);

            Quaternion targetQuatRot = Quaternion.Euler(targetRot);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetQuatRot, speed * Time.smoothDeltaTime);
        }
    }
}
