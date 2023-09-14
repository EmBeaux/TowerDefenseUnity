using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    public static PlacementSystem instance;
    [SerializeField]
    private GameObject cellIndicator;
    public Material cellIndicatorMaterial;

    [SerializeField]
    private InputManager inputManager;

    [SerializeField]
    private Grid grid;
    private Vector2Int gridSize;

    private GenerationSystem generationSystem;

    private BuildingSystem buildingSystem;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one PlacementSystem in scene!");
            return;
        }
        instance = this;
    }

    private void Start()
    {
        generationSystem = GenerationSystem.instance;
        buildingSystem = BuildingSystem.instance;
        gridSize = generationSystem.gridSize;
        InitializeIndicator();
    }

    private void Update() {
        UpdateIndicator();
        if (Input.GetMouseButtonDown(0) && IsWithinGrid(MouseToMap()))
        {
            OnMouseDown();
        }
    }

    private void UpdateIndicator()
    {
        Vector3Int gridPos = MouseToGrid();
        Vector2Int mapPos = MouseToMap();
        if (!IsWithinGrid(mapPos) || buildingSystem.GetTowerToBuild() == null)
        {
            cellIndicator.SetActive(false);  // Hide the indicator when out of grid bounds
            return;
        }

        cellIndicator.SetActive(true);
        cellIndicator.GetComponent<MeshRenderer>().material.color = IsValidPosition(mapPos) ? Color.green : Color.red;
        
        Vector3 gridCellCenter = MouseToGridCellCenter();
        Vector3 cellIndicatorPos = grid.CellToWorld(gridPos);
        cellIndicatorPos.y = 3.8f;
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
        return !generationSystem.pathDictionary.ContainsKey(mapPos);
    }

    private Vector3 MouseToGridCellCenter()
    {
        Vector3Int gridPos = MouseToGrid();
        Vector3 cellCenter = grid.GetCellCenterWorld(gridPos);

        return cellCenter;
    }

    public Vector2Int MouseToMap(Vector3? optional = null)
    {
        Vector3Int gridPos = MouseToGrid();
        if (optional != null)
        {
            gridPos = MouseToGrid((Vector3)optional);
        }
        return GridToMap(new Vector2Int(gridPos.x, gridPos.z));
    }

    private Vector3Int MouseToGrid(Vector3? optional = null)
    {
        Vector3 mousePos = inputManager.GetSelectedMapPosition();
        if (optional != null)
        {
            mousePos = (Vector3)optional;
        }
        if (gridSize.x % 2 == 0) mousePos.x += grid.cellSize.x / 2;
        if (gridSize.y % 2 == 0) mousePos.z += grid.cellSize.z / 2;
        Vector3Int gridPos = grid.WorldToCell(mousePos);

        return gridPos;
    }

    public bool IsWithinGrid(Vector2Int mapPos)
    {
        if (mapPos.x < 0 || mapPos.x >= gridSize.x || mapPos.y < 0 || mapPos.y >= gridSize.y)
        {
            return false;
        }
        return true;
    }

    public void OnMouseDown()
    {
        Vector2Int mapPos = MouseToMap();
        if (IsValidPosition(mapPos) && buildingSystem.GetTowerToBuild() != null) {
            Instantiate(buildingSystem.GetTowerToBuild(), MouseToGridCellCenter(), Quaternion.identity);
            buildingSystem.SetTowerToBuild(null);
        }
    }
}
