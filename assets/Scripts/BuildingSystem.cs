using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    private GameObject towerToBuild;
    public static BuildingSystem instance;
    public Transform GetTowerToBuild() { return towerToBuild != null ? towerToBuild.transform : null; }
    public void SetTowerToBuild(GameObject tower) { towerToBuild = tower; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one BuildingSystem in scene!");
            return;
        }

        instance = this;
    }
}
