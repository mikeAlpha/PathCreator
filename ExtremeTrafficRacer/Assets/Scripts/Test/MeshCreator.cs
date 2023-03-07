using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MeshCreator : MonoBehaviour
{
    public List<Vector3> wps = new List<Vector3>();
    public List<Vector3> totalPathpoints = new List<Vector3>();

    Mesh mesh;

    public List<Vector3> mVerts = new List<Vector3>();
    

    List<Vector3> normals = new List<Vector3>();
    public Material RoadMat;
    int[] triangles = new int[] { 0, 2, 1, 1, 2, 3 };

    int lineSteps = 10;

    public void init(Vector3 position)
    {
        wps.Add(position);
        totalPathpoints.Add(position);
        UpdatePath();
        UpdateMesh();
    }

    public void UpdatePath()
    {
        if (wps.Count > 1)
        {
            Vector3 p0 = totalPathpoints[totalPathpoints.Count - 1];
            Vector3 p1 = new Vector3(totalPathpoints[totalPathpoints.Count - 1].x + 1f, totalPathpoints[totalPathpoints.Count - 1].y, totalPathpoints[totalPathpoints.Count - 1].z);
            Vector3 p2 = new Vector3(totalPathpoints[totalPathpoints.Count - 1].x + 2f, totalPathpoints[totalPathpoints.Count - 1].y, totalPathpoints[totalPathpoints.Count - 1].z);

            Vector3 startPoint = Bezier.GetPoint(transform, p0, p1, p2, 0);
            totalPathpoints.Add(startPoint);

            for (int i = 1; i < lineSteps; i++)
            {
                p0 = totalPathpoints[totalPathpoints.Count - 1];
                p1 = new Vector3(totalPathpoints[totalPathpoints.Count - 1].x + 1f, totalPathpoints[totalPathpoints.Count - 1].y, totalPathpoints[totalPathpoints.Count - 1].z);
                p2 = new Vector3(totalPathpoints[totalPathpoints.Count - 1].x + 2f, totalPathpoints[totalPathpoints.Count - 1].y, totalPathpoints[totalPathpoints.Count - 1].z);

                Vector3 nextPoint = Bezier.GetPoint(transform, p0, p1, p2, i / (float)lineSteps);
                totalPathpoints.Add(nextPoint);
            }
        }
    }


    public void UpdateMesh(/*List<Vector3> points*/)
    {
        if (wps.Count > 1)
        {
            if (mVerts != null && normals != null)
            {
                mVerts.Clear();
                normals.Clear();
            }

            int numTris = 2 * (totalPathpoints.Count - 1);
            int[] roadTriangles = new int[numTris * 3];

            int vertIndex = 0;
            int triIndex = 0;

            for (int i = 0; i < wps.Count; i++)
            {
                if ((i + 1) < wps.Count)
                {
                    Vector3 v0 = new Vector3(totalPathpoints[i].x - 1f, totalPathpoints[i].y, totalPathpoints[i].z);
                    Vector3 v1 = new Vector3(totalPathpoints[i].x + 1f, totalPathpoints[i].y, totalPathpoints[i].z);
                    Vector3 v2 = new Vector3(totalPathpoints[i + 1].x - 1f, totalPathpoints[i + 1].y, totalPathpoints[i + 1].z);
                    Vector3 v3 = new Vector3(totalPathpoints[i + 1].x + 1f, totalPathpoints[i + 1].y, totalPathpoints[i + 1].z);


                    //normals[vertIndex + 0] = Vector3.up;
                    //normals[vertIndex + 1] = Vector3.up;

                    mVerts.Add(v0);
                    mVerts.Add(v1);
                    mVerts.Add(v2);
                    mVerts.Add(v3);

                    //normals.Add(Vector3.up);
                    //normals.Add(Vector3.up);
                    //normals.Add(Vector3.up);
                    //normals.Add(Vector3.up);

                    normals.Insert(0 + vertIndex, Vector3.up);
                    normals.Insert(1 + vertIndex, Vector3.up);
                    normals.Insert(2 + vertIndex, -Vector3.up);
                    normals.Insert(3 + vertIndex, -Vector3.up);

                    for (int j = 0; j < triangles.Length; j++)
                    {
                        roadTriangles[triIndex + j] = (vertIndex + triangles[j]) % mVerts.Count;
                    }

                    vertIndex += 4;
                    triIndex += 6;
                }
            }

            Vector3[] fVerts = mVerts.ToArray();
            Vector3[] norms = normals.ToArray();

            mesh.vertices = fVerts;
            mesh.triangles = roadTriangles;
            mesh.normals = norms;
            mesh.RecalculateNormals();
            GetComponent<MeshFilter>().mesh = mesh;
        }
    }

    public void SetVertex(int index, Vector3 point)
    {
        wps[index] = point;
    }

    public List<Vector3> GetVertices()
    {
        return wps;
    }

    public void InitMesh()
    {
        mesh = new Mesh();
    }
}
