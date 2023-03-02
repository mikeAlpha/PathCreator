using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public WheelCollider FrontWheelRight , FrontWheelLeft , RearWheelLeft , RearWheelRight;
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
        //Debug.Log("Currentspeed====" + current_speed);
        if (!Stop && current_speed < MaxSpeed)
        {
            FrontWheelLeft.motorTorque = 150;
            FrontWheelRight.motorTorque = 150;
        }
        else if(Stop)
        {
            FrontWheelLeft.motorTorque = 0f;
            FrontWheelRight.motorTorque = 0f;

            FrontWheelLeft.brakeTorque = 1000f;
            FrontWheelRight.brakeTorque = 1000f;
            RearWheelLeft.brakeTorque = 1000f;
            RearWheelRight.brakeTorque = 1000f;
        }
    }

    void CheckIfArrived()
    {
        //if (transform.position.y < current_point.y && Mathf.Abs(transform.position.y - current_point.y) < 0.8f)
        //{
        //    Debug.Log("Less than Y");
        //    FrontWheelLeft.motorTorque = 500;
        //    FrontWheelRight.steerAngle = 500;

        //}
        //else if (transform.position.y >= current_point.y)
        //{
        //    Debug.Log("More than Y");
        //    FrontWheelLeft.motorTorque = 1000;
        //    FrontWheelRight.steerAngle = 1000;
        //}


        Debug.Log("Distance====" + Vector3.Distance(transform.position, current_point) + " Node:" + node_counter + " Y Distance:" + Mathf.Abs(transform.position.y - current_point.y));
        var dist = Vector3.Distance(transform.position, current_point);
        float arriveDist = 0.85f;
        if (dist > 1f && dist < 1.5f)
        {
            arriveDist = 1.5f;
        }


        if(dist /*Mathf.Abs(transform.position.x - current_point.x)*/ < arriveDist)
        {
            if(node_counter == mPathmanager.path_nodes.Count - 1)
            {
                Stop = true;
                Destroy(gameObject);
            }
            else
            {
                node_counter++;
                current_point = mPathmanager.path_nodes[node_counter].position;
            }
        }
    }
}
