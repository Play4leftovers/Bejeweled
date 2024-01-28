using UnityEngine;

public class BoardGrid
{
    public int width, height;
    public float cellSize;
    private GameObject[,] grid;

    public BoardGrid(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        
        grid = new GameObject[width, height];
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize;
    }

    public void SetJewelWorldPosition(int x, int y)
    {
        grid[x, y].gameObject.transform.position = GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f;
    }

    public GameObject GetJewelFromGrid(int[] position)
    {
        return grid[position[0], position[1]];
    }

    public void RemoveJewelFromGrid(int[] position)
    {
        grid[position[0], position[1]] = null;
    }
    
    public void AddJewelToGrid(GameObject jewel, int[] position)
    {
        grid[position[0], position[1]] = jewel;
    }
}