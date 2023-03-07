using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshCreator))]
public class MeshCreatorEditor : Editor
{
    private MeshCreator mCreator;
    List<Vector3> points = new List<Vector3>();

    private void OnEnable()
    {
        mCreator = target as MeshCreator;
        mCreator.InitMesh();
        mCreator.transform.position = Vector3.zero;
    }

    public override void OnInspectorGUI()
    {
        //mCreator.UpdateMesh();
        DrawDefaultInspector();
        if(GUILayout.Button("Build Path"))
        {
            mCreator.UpdateMesh();
        }
    }

    private void OnSceneGUI()
    {
        Event e = Event.current;
        if(e.shift && e.button == 1 && e.type == EventType.MouseDown)
        {
            if (Selection.activeGameObject.tag == "Ground")
                return;

            Ray mRay = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            RaycastHit hitinfo;
            if(Physics.Raycast(mRay , out hitinfo, Mathf.Infinity))
            {
                Debug.Log("Hit Something..." + hitinfo.collider.name);
                points.Add(hitinfo.point);
                mCreator.init(mCreator.transform.InverseTransformPoint(hitinfo.point));
            }
            else
            {
                var yDir = mRay.direction.y;
                if(yDir != 0)
                {
                    var dirXZ = Mathf.Abs(mRay.origin.y / yDir);
                    Vector3 point = mRay.GetPoint(dirXZ);
                    points.Add(point);
                    mCreator.init(mCreator.transform.InverseTransformPoint(point));
                }
            }
        }

        Handles.color = Color.green;
        for (int i = 0; i < points.Count; i++)
        {
            Handles.SphereHandleCap(0, points[i], Quaternion.identity, 0.2f, EventType.Repaint);
            points[i] = Handles.DoPositionHandle(points[i], Quaternion.identity);
            mCreator.SetVertex(i, points[i]);
            //mCreator.UpdatePath();
            mCreator.UpdateMesh();
        }
    }
}
