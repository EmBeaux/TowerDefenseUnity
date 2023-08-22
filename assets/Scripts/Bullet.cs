using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 100f;
    public Vector3 launchDir;
    public Transform target;
    public Rigidbody bulletRB;
    public float lifetime = 5f;

    private void Awake()
    {
        bulletRB = GetComponent<Rigidbody>();
    }
    public void Seek(Transform enemy)
    {
        target = enemy;
        Vector3 dir = enemy.position - transform.position;
        launchDir = dir.normalized;

        // Move the bullet manually
        // transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
        bulletRB.velocity = dir.normalized * speed;
        StartCoroutine(DestroyAfterTime());
    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(lifetime);

        // Destroy the bullet after the specified lifetime
        Destroy(gameObject);
    }

    // private void Update()
    // {
    //     // Continue moving the bullet manually
    //     Vector3 dir = target.position - transform.position;
    //     transform.Translate(launchDir * speed * Time.deltaTime, Space.World);
    // }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the bullet collided with an enemy
        if (other.CompareTag("Enemy"))
        {
            // Destroy the bullet when it collides with an enemy
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
    }
}
