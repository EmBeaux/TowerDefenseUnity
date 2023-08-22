using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 10f;
    private Transform target;
    public int waypointIndex = 0;

    private void Start()
    {
        target = Navigation.waypoints[0];
    }

    private void Update()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= 0.2f)
        {
            GetNextWaypoint();
        }
    }

    private void GetNextWaypoint()
    {
        if (waypointIndex >= Navigation.waypoints.Length - 1)
        {
            Destroy(gameObject);
            return;
        }

        waypointIndex++;
        target = Navigation.waypoints[waypointIndex];
    }
}
