using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shop : MonoBehaviour
{
    public GameObject towerOne;
    public GameObject towerTwo;
    public GameObject towerThree;
    public TextMeshProUGUI towerOneCost;
    public TextMeshProUGUI towerTwoCost;
    public TextMeshProUGUI towerThreeCost;

    private BuildingSystem buildingSystem;
    private InfoManager infoManager;

    private void Start()
    {
        infoManager = InfoManager.instance;
        buildingSystem = BuildingSystem.instance;
        towerOneCost.text = towerOne.GetComponent<Tower>().cost.ToString();
        towerTwoCost.text = towerTwo.GetComponent<Tower>().cost.ToString();
        towerThreeCost.text = towerThree.GetComponent<Tower>().cost.ToString();
    }
    public void SetTowerOne()
    {
        if (infoManager.coins < towerOne.GetComponent<Tower>().cost) return;

        infoManager.SubtractCoins(towerOne.GetComponent<Tower>().cost);
        buildingSystem.SetTowerToBuild(towerOne);
    }

    public void SetTowerTwo()
    {
        if (infoManager.coins < towerTwo.GetComponent<Tower>().cost) return;
        
        infoManager.SubtractCoins(towerTwo.GetComponent<Tower>().cost);
        buildingSystem.SetTowerToBuild(towerTwo);
    }

    public void SetTowerThree()
    {
        if (infoManager.coins < towerThree.GetComponent<Tower>().cost) return;

        infoManager.SubtractCoins(towerThree.GetComponent<Tower>().cost);
        buildingSystem.SetTowerToBuild(towerThree);
    }
}
