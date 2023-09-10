using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public Transform towerOne;
    public Transform towerTwo;
    public Transform towerThree;
    private Transform buildingSystem;
    private BuildingSystem buildingSystemComponent;

    private void Awake()
    {
        buildingSystem = GameObject.Find("Building System").transform;
        buildingSystemComponent = buildingSystem.GetComponent<BuildingSystem>();
    }
    public void SetTowerOne()
    {
        buildingSystemComponent.SetTowerToBuild(towerOne);
    }

    public void SetTowerTwo()
    {
        buildingSystemComponent.SetTowerToBuild(towerTwo);
    }

    public void SetTowerThree()
    {
        buildingSystemComponent.SetTowerToBuild(towerThree);
    }
}
