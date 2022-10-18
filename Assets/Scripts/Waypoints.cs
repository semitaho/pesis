using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    [Range(1f, 2f)]
    [SerializeField] private float waypointSize = 1f;
    private void OnDrawGizmos()
    {
        foreach (Transform childTransform in transform)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(childTransform.position, waypointSize);
        }

        Gizmos.color = Color.red;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == transform.childCount - 1)
            {
                Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(0).position);
                break;

            }
            Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);


        }

    }

    public Transform GetNextWaypoint(Transform currentWaypoint)
    {
        if (currentWaypoint == null && transform.childCount > 0)
        {
            return transform.GetChild(0);
        }
        if (transform.childCount == 0)
        {
            return null;
        }
        if (currentWaypoint.GetSiblingIndex() < transform.childCount - 1)
        {
            return transform.GetChild(currentWaypoint.GetSiblingIndex() + 1);
        }
        else
        {
            return transform.GetChild(0);
        }

    }
}
