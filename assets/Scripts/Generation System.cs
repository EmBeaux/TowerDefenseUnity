using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GenerationSystem : MonoBehaviour
{
    public Transform pathTile;
    public Transform grassTile;
    public Transform startTower;
    public Transform endTower;
    public Transform grid;
    public Transform enemyWaypoint;
    public Transform navigation;
    public Transform gameManager;
    public GameManager gameManagerComponent;
    public Navigation navigationComponent;
    public Grid gridComponent;
    public Transform startTowerTransform;
    public Transform endTowerTransform;
    public Vector3 gridCenter;
    public Vector2Int gridSize = new Vector2Int(15, 15);
    public Vector2Int startTile = new Vector2Int(0, 0);
    public Vector2Int endTile = new Vector2Int(14, 14);
    public Dictionary<Vector2Int, int> pathDictionary = new Dictionary<Vector2Int, int>();
    public int maxOverlapCount = 3;
    public float upDownWeight = 0.5f;

    private void Awake()
    {
        gridComponent = grid.GetComponent<Grid>();
        navigationComponent = navigation.GetComponent<Navigation>();
        gameManagerComponent = gameManager.GetComponent<GameManager>();
    }

    private void Start()
    {
        gridCenter = grid.position;
        GenerateGrid();
        Vector3 startTilePosition = CalculateCellPosition(startTile.x, startTile.y);
        Vector3 endTilePosition = CalculateCellPosition(endTile.x, endTile.y);
        startTilePosition.y += (gridComponent.cellSize.y * 0.5f) + (endTower.localScale.y * 0.5f);
        endTilePosition.y += (gridComponent.cellSize.y * 0.5f) + (startTower.localScale.y * 0.5f);
        startTowerTransform = Instantiate(startTower, startTilePosition, Quaternion.identity, grid);
        endTowerTransform = Instantiate(endTower, endTilePosition, Quaternion.identity, grid);

        GeneratePath();
    }

    private void GenerateGrid()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector3 tilePosition = CalculateCellPosition(x, y);
                Instantiate(grassTile, tilePosition, Quaternion.identity, grid);
            }
        }
    }

    private void GeneratePath()
    {
        List<Vector3> modifiedPath = new List<Vector3>();

        Vector2Int newDirection = Random.Range(0f, 1f) < 0.5f ? Vector2Int.up : Vector2Int.right;
        Vector2Int currentTile = new Vector2Int(0, 0);
        List<Vector2Int> previous2Tiles = new List<Vector2Int>();
        while (currentTile != endTile)
        {
            if (previous2Tiles.Count < 2) {
                currentTile = currentTile + newDirection;
                previous2Tiles.Add(newDirection);
            } else {
                Vector2Int randomDirection = GetRandomDirection(newDirection, currentTile);
                previous2Tiles.Clear();
                previous2Tiles.Add(randomDirection);
                currentTile = currentTile + randomDirection;
                newDirection = randomDirection;
            }

            Instantiate(enemyWaypoint, CalculateCellPosition(currentTile.x, currentTile.y), Quaternion.identity, navigation);
            if (!pathDictionary.ContainsKey(currentTile))
            {
                if (!pathDictionary.ContainsKey(currentTile))
                {
                    pathDictionary.Add(currentTile, 1);
                    Instantiate(pathTile, CalculateCellPosition(currentTile.x, currentTile.y), Quaternion.identity, grid);
                } else {
                    pathDictionary[currentTile] += 1;
                }
            }

        }

        navigationComponent.SetWaypoints();
        gameManagerComponent.StartGameManager(startTowerTransform, endTowerTransform);
    }

    private Vector3 CalculateCellPosition(int x, int z)
    {
        float xOffset = (x - (gridSize.x - 1) * 0.5f) * gridComponent.cellSize.x;
        float zOffset = (z - (gridSize.y - 1) * 0.5f) * gridComponent.cellSize.z;
        Vector3 offset = new Vector3(gridComponent.cellSize.x * 0.5f, 0, gridComponent.cellSize.z * 0.5f); // Offset by half the cell size
        return gridCenter + new Vector3(xOffset, 0, zOffset) + offset;
    }

    private Vector2Int GetRandomDirection(Vector2Int oldDirection, Vector2Int currentTile) {
        Vector2Int[] directions = new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.right, Vector2Int.left };
        List<Vector2Int> remainingDirections = new List<Vector2Int>();
        foreach (Vector2Int dir in directions)
        {
            if (IsValidDirection(dir, oldDirection, currentTile))
            {
                remainingDirections.Add(dir);
            }
        }

        directions = remainingDirections.ToArray();
        float probability = Random.Range(0f, 1f);
        int index = 0;
        if (directions.Length > 1)
        {
            index = probability < upDownWeight ? 0 : 1;
        } else if (directions.Length == 0) 
        {
            // BYPASS OVERLAP COUNT IF STUCK
            Vector2Int cardinalDirection = GetCardinalHeading(endTile, currentTile);
            List<Vector2Int> listDirections = directions.ToList();
            listDirections.Add(cardinalDirection);
            directions = listDirections.ToArray();
        }
        Vector2Int randomDirection = directions[index];

        return randomDirection;
    }

    private bool IsValidDirection(Vector2Int direction, Vector2Int previousDirection, Vector2Int currentTile)
    {
        Vector2Int nextTile1 = currentTile + direction;
        Vector2Int nextTile2 = nextTile1 + direction;

        return nextTile1.x >= 0 && nextTile1.x < gridSize.x &&
                nextTile1.y >= 0 && nextTile1.y < gridSize.y &&
                nextTile2.x >= 0 && nextTile2.x < gridSize.x &&
                nextTile2.y >= 0 && nextTile2.y < gridSize.y &&
                !AreOppositeDirections(direction, previousDirection) && 
                (!pathDictionary.ContainsKey(nextTile1) || pathDictionary[nextTile1] < maxOverlapCount) &&
                (!pathDictionary.ContainsKey(nextTile2) || pathDictionary[nextTile2] < maxOverlapCount);
    }

    private bool AreOppositeDirections(Vector2Int direction1, Vector2Int direction2)
    {
        return direction1.x * direction2.x + direction1.y * direction2.y == -1;
    }

    private Vector2Int GetCardinalHeading(Vector2Int endTile, Vector2Int currentTile)
    {
        Vector2Int heading = endTile - currentTile;
        float distance = heading.magnitude;

        Vector2 normalizedDirection = Vector2.zero;
        if (distance > 0)
        {
            normalizedDirection = new Vector2(heading.x / distance, heading.y / distance);
        }

        Vector2Int cardinalDirection = Vector2Int.zero;
        if (Mathf.Abs(normalizedDirection.x) > Mathf.Abs(normalizedDirection.y))
        {
            // Dominant direction is left or right
            if (normalizedDirection.x > 0)
            {
                cardinalDirection = Vector2Int.right;
            }
            else
            {
                cardinalDirection = Vector2Int.left;
            }
        }
        else
        {
            // Dominant direction is up or down
            if (normalizedDirection.y > 0)
            {
                cardinalDirection = Vector2Int.up;
            }
            else
            {
                cardinalDirection = Vector2Int.down;
            }
        }

        return cardinalDirection;
    }
}
