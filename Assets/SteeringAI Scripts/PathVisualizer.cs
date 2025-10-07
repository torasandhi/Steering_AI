using UnityEngine;
using System.Collections.Generic;

public class PathVisualizer : MonoBehaviour
{
    public Color lineColor = Color.white;

    private void OnDrawGizmos()
    {
        Gizmos.color = lineColor;

        // Get all waypoint transforms from the children of this object
        Transform[] pathTransforms = GetComponentsInChildren<Transform>();
        List<Transform> nodes = new List<Transform>();

        for (int i = 0; i < pathTransforms.Length; i++)
        {
            if (pathTransforms[i] != transform)
            {
                nodes.Add(pathTransforms[i]);
            }
        }

        // Draw lines between each node
        for (int i = 0; i < nodes.Count; i++)
        {
            Vector3 currentNode = nodes[i].position;
            Vector3 previousNode = Vector3.zero;

            if (i > 0)
            {
                previousNode = nodes[i - 1].position;
            }
            else if (i == 0 && nodes.Count > 1)
            {
                // For the first node, you can optionally connect it to the last to close the loop
                previousNode = nodes[nodes.Count - 1].position;
            }

            if (previousNode != Vector3.zero)
            {
                Gizmos.DrawLine(previousNode, currentNode);
            }
        }
    }
}