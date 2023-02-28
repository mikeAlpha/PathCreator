using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    public List<Transform> path_nodes = new List<Transform>();

    public static PathManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        for (int i = 0; i < path_nodes.Count; i++)
        {
            Vector3 current_node = path_nodes[i].position;
            Vector3 prev_node = Vector3.zero;

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(current_node, 0.5f);

            if(i == 0)
            {
                continue;
            }
            else
            {
                prev_node = path_nodes[i - 1].position;

                Gizmos.color = Color.white;
                Gizmos.DrawLine(prev_node, current_node);
            }
        }
    }
}
