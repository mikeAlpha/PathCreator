using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public WheelCollider FrontWheelRight , FrontWheelLeft;
    public float maxSteerAngle = 45f;

    PathManager mPathmanager;
    Vector3 current_point;
    int node_counter = 0;
    bool Stop = false;

    public float MaxSpeed = 100f;
    float current_speed;

    private void Start()
    {
        mPathmanager = PathManager.instance;
        current_point = mPathmanager.path_nodes[node_counter].position;
    }

    private void FixedUpdate()
    {
        Steer();
        Drive();
        CheckIfArrived();
    }

    void Steer()
    {
        Vector3 relPos = transform.InverseTransformPoint(current_point);
        var steerAngle = (relPos.x / relPos.magnitude) * maxSteerAngle;
        FrontWheelLeft.steerAngle = steerAngle;
        FrontWheelRight.steerAngle = steerAngle;
    }

    void Drive()
    {
        current_speed = 2 * Mathf.PI * FrontWheelRight.radius *FrontWheelRight.rpm * 60f / 1000f;
        Debug.Log("Currentspeed====" + current_speed);
        if (!Stop && current_speed < MaxSpeed)
        {
            FrontWheelLeft.motorTorque = 1000;
            FrontWheelRight.steerAngle = 1000;
        }
        else
        {
            FrontWheelLeft.motorTorque = 0f;
            FrontWheelRight.steerAngle = 0f;
        }
    }

    void CheckIfArrived()
    {
        Debug.Log(Vector3.Distance(transform.position, current_point));
        if(/*Vector3.Distance(transform.position , current_point)*/Mathf.Abs(transform.position.x - current_point.x) < 0.5f)
        {
            if(node_counter == mPathmanager.path_nodes.Count - 1)
            {
                Stop = true;
            }
            else
            {
                node_counter++;
                current_point = mPathmanager.path_nodes[node_counter].position;
            }
        }
    }
}
