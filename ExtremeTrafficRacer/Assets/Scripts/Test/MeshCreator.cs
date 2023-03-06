using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MeshCreator : MonoBehaviour
{
    public Transform[] wps;

    Mesh mesh;

    public List<Vector3> mVerts = new List<Vector3>();

    int[] triangles = new int[] { 0, 2, 1, 1, 2, 3 };

    private void Start()
    {
        mesh = new Mesh();

        int numTris = 2 * (wps.Length - 1);
        int[] roadTriangles = new int[numTris * 3];

        int vertIndex = 0;
        int triIndex = 0;

        for (int i = 0; i < wps.Length; i++)
        {
            if ((i + 1) < wps.Length)
            {
                Vector3 v0 = new Vector3(wps[i].localPosition.x - 0.5f, 0, wps[i].localPosition.z);
                Vector3 v1 = new Vector3(wps[i].localPosition.x + 0.5f, 0, wps[i].localPosition.z);
                Vector3 v2 = new Vector3(wps[i + 1].localPosition.x - 0.5f, 0, wps[i + 1].localPosition.z);
                Vector3 v3 = new Vector3(wps[i + 1].localPosition.x + 0.5f, 0, wps[i + 1].localPosition.z);

                mVerts.Add(v0);
                mVerts.Add(v1);
                mVerts.Add(v2);
                mVerts.Add(v3);

                for (int j = 0; j < triangles.Length; j++)
                {
                    roadTriangles[triIndex + j] = (vertIndex + triangles[j]) % mVerts.Count;
                }

                vertIndex += 4;
                triIndex += 6;
            }
        } 

        Vector3[] fVerts = mVerts.ToArray();
        mesh.vertices = fVerts;
        mesh.triangles = roadTriangles;
        GetComponent<MeshFilter>().mesh = mesh;
    }
}
