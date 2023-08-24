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
    private Transform target;
    private Enemy[] enemies;
    [HideInInspector]
    public bool drawRangeInGame = true;

    private void Start()
    {
        InvokeRepeating("GetClosestEnemy", 0f, 1f);
    }


    public void OnMouseDown()
    {
        Debug.Log("Click");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    private void GetClosestEnemy()
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

    private void Update()
    {
        if (target == null)
        {
            return;
        }

        // Vector3 dir = target.position - transform.position;
        // Quaternion lookRotation = Quaternion.LookRotation(dir);
        // Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 5f).eulerAngles;
        // transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        if (bulletDelay <= 0f)
        {
            Shoot();
            bulletDelay = 1f / fireRate;
        }
        bulletDelay -= Time.deltaTime;
    }

    private void Shoot()
    {
        Vector3 bulletSpawnPos = transform.position + transform.forward * 2f;
        Vector3 bulletPos = new Vector3(bulletSpawnPos.x, bulletSpawnPos.y, bulletSpawnPos.z);
        Transform bulletGO = Instantiate(bullet, bulletPos, transform.rotation);
        // bulletGO.parent = transform;
        Bullet bulletScript = bulletGO.GetComponent<Bullet>();

        if (bulletScript != null)
        {
            Debug.Log("boutta seek");
            bulletScript.Seek(target);
        }
    }
}
