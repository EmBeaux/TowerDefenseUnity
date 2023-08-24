using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject cellIndicator;
    public Material cellIndicatorMaterial;

    [SerializeField]
    private InputManager inputManager;

    [SerializeField]
    private Grid grid;
    private Vector2Int gridSize;

    private Transform generationSystem;
    private GenerationSystem generationSystemComponent;

    private void Awake()
    {
        generationSystem = GameObject.Find("Generation System").transform;
        generationSystemComponent = generationSystem.GetComponent<GenerationSystem>();
        gridSize = generationSystemComponent.gridSize;
        InitializeIndicator();
    }

    private void Update() {
        UpdateIndicator();
    }

    private void UpdateIndicator()
    {
        Vector3Int gridPos = MouseToGrid();
        Vector2Int mapPos = MouseToMap();
        Vector3 gridCellCenter = MouseToGridCellCenter();

        cellIndicator.GetComponent<MeshRenderer>().material.color = IsValidPosition(mapPos) ? Color.green : Color.red;
        Vector3 cellIndicatorPos = grid.CellToWorld(gridPos);
        cellIndicatorPos.y = gridCellCenter.y + 0.1f;
        cellIndicator.transform.position = cellIndicatorPos;
    }
    
    private void InitializeIndicator()
    {
        // Calculate the position of the center of the grid cell where you want to draw the square
        Vector3 gridCellCenter = new Vector3(0, 0, 0); // Adjust the cell coordinates as needed
        Mesh cellIndicatorMesh = new Mesh();
        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(0, 0, 0),
            new Vector3(4, 0, 0),
            new Vector3(0, 0, 4),
            new Vector3(4, 0, 4)
        };
        cellIndicatorMesh.vertices = vertices;

        int[] triangles = new int[6]
        {
            // lower left triangle
            0, 2, 1,
            // upper right triangle
            2, 3, 1
        };
        cellIndicatorMesh.triangles = triangles;

        Vector3[] normals = new Vector3[4]
        {
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward
        };
        cellIndicatorMesh.normals = normals;

        Vector2[] uv = new Vector2[4]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };
        cellIndicatorMesh.uv = uv;


        cellIndicator = new GameObject("Cell Indicator");
        cellIndicator.transform.position = gridCellCenter;

        MeshFilter meshFilter = cellIndicator.AddComponent<MeshFilter>();
        meshFilter.mesh = cellIndicatorMesh;

        MeshRenderer meshRenderer = cellIndicator.AddComponent<MeshRenderer>();
        meshRenderer.material = cellIndicatorMaterial;
    }

    private Vector2Int GridToMap(Vector2Int gridPos)
    {
        return new Vector2Int(gridPos.x + ((gridSize.x - 1) / 2), gridPos.y + ((gridSize.y - 1) / 2));
    }

    private bool IsValidPosition(Vector2Int mapPos)
    {
        return !generationSystemComponent.pathDictionary.ContainsKey(mapPos);
    }

    private Vector3 MouseToGridCellCenter()
    {
        Vector3Int gridPos = MouseToGrid();
        Vector3 cellCenter = grid.GetCellCenterWorld(gridPos);

        return cellCenter;
    }

    private Vector2Int MouseToMap()
    {
        Vector3Int gridPos = MouseToGrid();
        return GridToMap(new Vector2Int(gridPos.x, gridPos.z));
    }

    private Vector3Int MouseToGrid()
    {
        Vector3 mousePos = inputManager.GetSelectedMapPosition();
        Vector3Int gridPos = grid.WorldToCell(mousePos);

        return gridPos;
    }
}
