using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    private Transform towerToBuild;
    public Transform GetTowerToBuild() { return towerToBuild; }
    public void SetTowerToBuild(Transform tower) { towerToBuild = tower; }
}
