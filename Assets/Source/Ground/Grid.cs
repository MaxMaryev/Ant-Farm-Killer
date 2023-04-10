using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private int[,] _grid;
    public Grid(int width, int height) 
    { 
        _grid = new int[width, height];
    }

    public int GetValueAt(Vector2Int index) => _grid[index.y, index.x];

    public void IncreaseValueAt(Vector2Int index) => _grid[index.y, index.x] += 1;
    public void DecreaseValueAt(Vector2Int index) => _grid[index.y, index.x] -= 1;
}
