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

    private Transform generationSystem;
    private GenerationSystem generationSystemComponent;

    private void Awake()
    {
        generationSystem = GameObject.Find("Generation System").transform;
        generationSystemComponent = generationSystem.GetComponent<GenerationSystem>();
        InitializeIndicator();
    }

    private void Update() {
        Vector3 mousePos = inputManager.GetSelectedMapPosition();
        Vector3Int gridPos = grid.WorldToCell(mousePos);

        Vector3 cellCenter = grid.GetCellCenterWorld(gridPos);
        cellCenter.y = grid.transform.position.y + 0.55f;
        cellIndicator.transform.position = cellCenter;

        // Update the position of the cell indicator based on the actual cell the mouse is over
        Vector2Int gridSize = generationSystemComponent.gridSize;
        Vector2Int graphPos = GridToGraph(new Vector2Int(gridPos.x, gridPos.z), gridSize);
        if (generationSystemComponent.pathDictionary.ContainsKey(graphPos))
        {
            cellIndicator.GetComponent<MeshRenderer>().material.color = Color.red;
        }
        else
        {
            cellIndicator.GetComponent<MeshRenderer>().material.color = Color.green;
        }
        cellIndicator.transform.position = grid.CellToWorld(gridPos);
        cellIndicator.transform.position = new Vector3(cellIndicator.transform.position.x, cellCenter.y, cellIndicator.transform.position.z);
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

    private Vector2Int GridToGraph(Vector2Int gridPos, Vector2Int gridSize)
    {
        return new Vector2Int(gridPos.x + ((gridSize.x - 1) / 2), gridPos.y + ((gridSize.y - 1) / 2));
    }
}
