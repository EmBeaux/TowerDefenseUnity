using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.EventSystems;
public class Tower : MonoBehaviour
{
    public Transform bullet;
    public float range = 80f;
    public float fireRate = 1f;
    public float bulletDelay = 2f;
    public int cost = 1;
    public Transform target;
    public Enemy[] enemies;
    [HideInInspector]
    public bool drawRangeInGame = true;
    private Transform activeCannon;

    public void Start()
    {
        activeCannon = transform.Find("Head");
        GetClosestEnemy();
    }

    public void OnMouseDown()
    {
        Debug.Log("Click");
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public void GetClosestEnemy()
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Pow(range, 2);
        Vector3 currentPosition = transform.position;
        enemies = FindObjectsOfType<Enemy>();

        foreach (Enemy potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget.transform;
            }
        }
        target = bestTarget;
    }

    public void Update()
    {
        GetClosestEnemy();
        if (target == null)
        {
            return;
        }

        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 5f).eulerAngles;
        transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        if (bulletDelay <= 0f)
        {
            Shoot();
            bulletDelay = 1f / fireRate;
        }
        bulletDelay -= Time.deltaTime;
    }

    public virtual void Shoot()
    {
        Transform bulletGO = Instantiate(bullet, activeCannon.position, activeCannon.rotation);
        Bullet bulletScript = bulletGO.GetComponent<Bullet>();

        if (bulletScript != null)
        {
            bulletScript.Seek(target);
        }
    }
}
