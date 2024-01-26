using UnityEngine;

public class BoardGrid
{
    private int width, height;
    private Jewel[,] grid;
    private float cellSize;

    public BoardGrid(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        
        grid = new Jewel[width, height];
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize;
    }

    public void SetJewelWorldPosition(int x, int y)
    {
        grid[x, y].gameObject.transform.position = GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f;
    }
}