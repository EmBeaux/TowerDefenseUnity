using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerOne : Tower
{
    private Transform cannon1;
    private Transform cannon2;
    private bool isCannon1Active = true;

    new public void Start()
    {
        cannon1 = transform.Find("Head/Cannon_1");
        cannon2 = transform.Find("Head/Cannon_2");
        GetClosestEnemy();
    }


    public override void Shoot()
    {
        Transform activeCannon = isCannon1Active ? cannon1 : cannon2;
        Transform bulletGO = Instantiate(bullet, activeCannon.position, activeCannon.rotation);
        Bullet bulletScript = bulletGO.GetComponent<Bullet>();

        if (bulletScript != null)
        {
            bulletScript.Seek(target);
        }

        isCannon1Active = !isCannon1Active; // Toggle the cannon for next time
    }
}
