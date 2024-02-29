//Author name = JoshuaKenendy;
//Website portfolio = joshuatkennedy.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingTarget : MonoBehaviour
{
    #region Variables
    public GameObject waypointsParent;
    private Transform[] waypoints;

    public float speed = 1f;

    public GameObject explosionEffect;

    private int currentWaypointIndex = 0;
    private bool movingForward = true;
    #endregion

    #region UnityEngine
    void Start()
    {
        // Initialize waypoints based on the children of waypointsParent
        if (waypointsParent != null)
        {
            waypoints = new Transform[waypointsParent.transform.childCount];
            for (int i = 0; i < waypointsParent.transform.childCount; i++)
            {
                waypoints[i] = waypointsParent.transform.GetChild(i);
            }
        }
    }

    void Update()
    {
        MoveBetweenWaypoints();
    }
    #endregion

    void MoveBetweenWaypoints()
    {
        if (waypoints.Length == 0) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            if (movingForward)
            {
                if (currentWaypointIndex < waypoints.Length - 1)
                    currentWaypointIndex++;
                else
                {
                    movingForward = false;
                    currentWaypointIndex--;
                }
            }
            else
            {
                if (currentWaypointIndex > 0)
                    currentWaypointIndex--;
                else
                {
                    movingForward = true;
                    currentWaypointIndex++;
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        if (waypointsParent != null)
        {
            waypoints = new Transform[waypointsParent.transform.childCount];
            for (int i = 0; i < waypointsParent.transform.childCount; i++)
            {
                waypoints[i] = waypointsParent.transform.GetChild(i);
            }

            if (waypoints.Length > 0)
            {
                Gizmos.color = Color.green;
                for (int i = 0; i < waypoints.Length; i++)
                {
                    if (waypoints[i] != null)
                    {
                        Gizmos.DrawSphere(waypoints[i].position, 0.25f);
                        if (i > 0 && waypoints[i - 1] != null)
                            Gizmos.DrawLine(waypoints[i - 1].position, waypoints[i].position);
                    }
                }
            }
        }
    }
}
