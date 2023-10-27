using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    public Transform[] wayPoints; // An array to store the waypoints
    public float speed = 2f; // Speed of the obstacle's movement
    public float waitDuration = 0.5f; // Duration to wait at each waypoint
    [Range(0.1f, 2f)]
    public float speedMultiplier = 1f;

    private int pointIndex = 0; // Index of the current waypoint
    private float step; // Current step value for movement
    private bool waiting = false;

    void Awake()
    {
        wayPoints = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            wayPoints[i] = transform.GetChild(i);
        }
    }

    void Start()
    {
        StartCoroutine(MoveToNextPoint());
    }

    IEnumerator MoveToNextPoint()
    {
        while (true)
        {
            if (!waiting)
            {
                if (pointIndex >= wayPoints.Length)
                    pointIndex = 0;

                Vector3 targetPosition = wayPoints[pointIndex].position;
                float distance = Vector3.Distance(transform.position, targetPosition);
                step = speed * Time.deltaTime * speedMultiplier;

                transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

                if (distance <= 0.01f)
                {
                    waiting = true;
                    StartCoroutine(WaitNextPoint());
                }
            }

            yield return null;
        }
    }

    IEnumerator WaitNextPoint()
    {
        yield return new WaitForSeconds(waitDuration);
        waiting = false;
        pointIndex++;

        if (pointIndex >= wayPoints.Length)
            pointIndex = 0;
    }
}
