using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointMover : MonoBehaviour
{
    [SerializeField] private Waypoints waypoints;
    [Range(5f, 15f)] [SerializeField] private float moveSpeed = 1f;

    [SerializeField] private float waitTimeInOnePlace = 1f;

    [SerializeField] private Transform targetToLookat;

    [SerializeField] private float dampingTime = 0.3f;

    private float distanceThreshold = 0.1f;

    private float currentSpendTime = 0;

    private bool isInWaypoint = false;

    private Animator animator;

    private Transform currentWaypoint;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        if (waypoints == null) return;
        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        if (currentWaypoint == null) return;

        transform.position = currentWaypoint.position;
        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        isInWaypoint = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWaypoint == null) return;
        if (targetToLookat != null)
        {
            transform.LookAt(targetToLookat);
        }
        HandleWaypoints();
        var moveVector = currentWaypoint.position - transform.position;


        UpdateAnimation(moveVector);
    }

    void UpdateAnimation(Vector3 dir)
    {
        var direction = transform.InverseTransformDirection(dir.normalized);
        if (!isInWaypoint)
        {
            this.animator.SetFloat("Sivulle", direction.x * moveSpeed / 10, dampingTime, Time.deltaTime);
            this.animator.SetFloat("Eteentaakse", direction.z * moveSpeed / 10, dampingTime, Time.deltaTime);

        }
        else
        {
            this.animator.SetFloat("Eteentaakse", 0, dampingTime, Time.deltaTime);
            this.animator.SetFloat("Sivulle", 0, dampingTime, Time.deltaTime);

        }

    }

    private void HandleWaypoints()
    {
        if (isInWaypoint)
        {
            currentSpendTime += Time.deltaTime;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, moveSpeed * Time.deltaTime);
        }

        if (currentSpendTime >= waitTimeInOnePlace)
        {
            isInWaypoint = false;
            currentSpendTime = 0f;
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, moveSpeed * Time.deltaTime);
        }
        if (!isInWaypoint && HasEnoughCloseToWaypoint())
        {
            isInWaypoint = true;
            currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);

        }

    }

    private bool HasEnoughCloseToWaypoint()
    {
        return Vector3.Distance(transform.position, currentWaypoint.position) < distanceThreshold;
    }
}
