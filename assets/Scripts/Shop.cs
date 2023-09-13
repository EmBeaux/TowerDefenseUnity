using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject towerOne;
    public GameObject towerTwo;
    public GameObject towerThree;
    private BuildingSystem buildingSystem;

    private void Start()
    {
        buildingSystem = BuildingSystem.instance;
    }
    public void SetTowerOne()
    {
        buildingSystem.SetTowerToBuild(towerOne);
    }

    public void SetTowerTwo()
    {
        buildingSystem.SetTowerToBuild(towerTwo);
    }

    public void SetTowerThree()
    {
        buildingSystem.SetTowerToBuild(towerThree);
    }
}
