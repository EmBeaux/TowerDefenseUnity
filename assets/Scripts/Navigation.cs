using UnityEngine;

public class Navigation : MonoBehaviour
{
    public static Transform[] waypoints;

    public void SetWaypoints()
    {
        waypoints = new Transform[transform.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = transform.GetChild(i);
        }
    }
}
