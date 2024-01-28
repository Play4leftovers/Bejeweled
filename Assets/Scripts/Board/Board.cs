using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Board : MonoBehaviour
{
    private BoardGrid _grid;
    
    private int[,] _jewelsToBeMoved = new int[2,2];
    public List<int[]> _cleanUpList;

    private int[] _movePosition;
    private int _matching;
    private bool _playerControlEnabled = true;

    public GameObject jewel;
    
    void Start()
    {
        _cleanUpList = new List<int[]>();
        _grid = new BoardGrid(8, 8, 1f);
        int[] array = new int[2];
        
        for (int x = 0; x < _grid.width; x++)
        {
            array[0] = x;
            for (int y = 0; y < _grid.height; y++)
            {
                array[1] = y;
                CreateJewel(array);
            }
        }
    }

    private void UpdateBoard()
    {
        MoveJewels();
        FindMatches();
        CleanUpJewels();
    }

    private void MoveJewels()
    {
        int[] cellOne = { _jewelsToBeMoved[0, 0], _jewelsToBeMoved[0, 1] };
        int[] cellTwo = { _jewelsToBeMoved[1, 0], _jewelsToBeMoved[1, 1] };
        if (!IsAdjacent(cellOne, cellTwo)) return;

        GameObject tempOne = _grid.GetJewelFromGrid(cellOne);
        _grid.RemoveJewelFromGrid(cellOne);

        GameObject tempTwo = _grid.GetJewelFromGrid(cellTwo);
        _grid.RemoveJewelFromGrid(cellTwo);
        
        _grid.AddJewelToGrid(tempOne, cellTwo);
        _grid.AddJewelToGrid(tempTwo, cellOne);
        
        _grid.SetJewelWorldPosition(cellOne[0], cellOne[1]);
        _grid.SetJewelWorldPosition(cellTwo[0], cellTwo[1]);
    }

    private bool IsAdjacent(int[] cellOne, int[] cellTwo)
    {
        int[] tempCell = {  cellOne[0] - cellTwo[0], 
            cellOne[1] - cellTwo[1] };
        
        if (tempCell[0] < 2 && tempCell[0] > -2)
        {
            if (tempCell[1] < 2 && tempCell[1] > -2)
            {
                if (tempCell[0] == 0 || tempCell[1] == 0)
                {
                    return true;
                }
            }
        }
        return false;
    }
    
    private void FindMatches()
    {
        int width = _grid.width, height = _grid.height;
        int[] position = {0,0};
        
        
        //For loop to check each element in a column against previous elements in that column
        //to see if they are the same element.
        
        //If they are not the same element, look at how many elements have previously
        //been matching each other and go back and add them to the CleanUpList for later reference.
        for (int x = 0; x < width; x++)
        {
            position[0] = x; position[1] = 0;
            _matching = 0;
            GameObject tempObject = _grid.GetJewelFromGrid(position);
            
            for (int y = 0; y < height; y++)
            {
                position[1] = y;
                GameObject tempLoopObject = _grid.GetJewelFromGrid(position);
                bool sameType = tempObject.GetComponent<Jewel>().jewelSO.JewelType ==
                                tempLoopObject.GetComponent<Jewel>().jewelSO.JewelType;
                
                if (sameType)
                {
                    _matching++;
                }

                if (y == height-1 && sameType)
                {
                    if (_matching >= 3)
                    {
                        for (int i = 0; i <= _matching-1; i++)
                        {
                            Debug.Log("Trying to add position [" + x + "]" + "[" + (y-i) + "]");
                            int[] tempArray = { x, y - i };
                            _cleanUpList.Add(tempArray);
                        }
                    }
                }
                
                if(!sameType)
                {
                    if (_matching >= 3)
                    {
                        for (int i = 1; i <= _matching; i++)
                        {
                            Debug.Log("Trying to add position [" + x + "]" + "[" + (y-i) + "]");
                            int[] tempArray = { x, y - i };
                            _cleanUpList.Add(tempArray);
                        }
                    }
                    tempObject = tempLoopObject;
                    _matching = 1;
                }
            }
        }
        
        Debug.Log("Check Rows now!");
        
        //This loop works exactly the same, but checks elements in rows instead.
        //It also checks if the position it tries to add to CleanUpList already exists in the list.
        for (int y = 0; y < height; y++)
        {
            position[1] = y; position[0] = 0;
            _matching = 0;
            GameObject tempObject = _grid.GetJewelFromGrid(position);
            
            for (int x = 0; x < width; x++)
            {
                position[0] = x;
                GameObject tempLoopObject = _grid.GetJewelFromGrid(position);
                bool sameType = tempObject.GetComponent<Jewel>().jewelSO.JewelType ==
                                tempLoopObject.GetComponent<Jewel>().jewelSO.JewelType;

                if (sameType)
                {
                    _matching++;
                }
                
                if (x == width-1 && sameType)
                {
                    if (_matching >= 3)
                    {
                        for (int i = 0; i <= _matching-1; i++)
                        {
                            int[] tempArray = { x - i, y };
                            bool sameArray = false;
                            foreach (var cleanUpArray in _cleanUpList)
                            {
                                if (tempArray[0] == cleanUpArray[0] && tempArray[1] == cleanUpArray[1])
                                {
                                    sameArray = true;
                                    break;
                                }
                            }

                            if (!sameArray)
                            {
                                Debug.Log("Trying to add position [" + (x-i) + "]" + "[" + y + "]");
                                _cleanUpList.Add(tempArray);
                            }
                        }
                    }
                }
                
                if(!sameType)
                {
                    if (_matching >= 3)
                    {
                        for (int i = 1; i <= _matching; i++)
                        {
                            int[] tempArray = { x-i, y };
                            bool sameArray = false;
                            foreach (var cleanUpArray in _cleanUpList)
                            {
                                if (tempArray[0] == cleanUpArray[0] && tempArray[1] == cleanUpArray[1])
                                {
                                    sameArray = true;
                                    break;
                                }
                            }
                            
                            if (!sameArray)
                            {
                                Debug.Log("Trying to add position [" + (x-i) + "]" + "[" + y + "]");
                                _cleanUpList.Add(tempArray);
                            }
                                
                        }
                    }
                    tempObject = tempLoopObject;
                    _matching = 1;
                }
            }
        }
    }

    private void CleanUpJewels()
    {
        foreach (var cleanUpArray in _cleanUpList)
        {
            GameObject temp = _grid.GetJewelFromGrid(cleanUpArray);
            _grid.RemoveJewelFromGrid(cleanUpArray);
            Destroy(temp);
        }
    }

    public void DestroyJewelInGrid(int[] position)
    {
        GameObject temp = _grid.GetJewelFromGrid(position);
        _grid.RemoveJewelFromGrid(position);
        Destroy(temp);
    }

    private void CreateJewel(int[] position)
    {
        GameObject tempJewel = Instantiate(jewel);
        _grid.AddJewelToGrid(tempJewel, position);
        _grid.SetJewelWorldPosition(position[0], position[1]);
    }


    private int[] GetXY(Vector3 worldPosition)
    {
        int[] temp = new int [2];
        temp[0] = Mathf.FloorToInt((worldPosition.x / _grid.cellSize));
        temp[1] = Mathf.FloorToInt((worldPosition.y / _grid.cellSize));
        return temp;
    }
    
    public void MouseBoardBridge(Vector3 worldPositionOne, Vector3 worldPositionTwo)
    {
        int[] gridPositionOne = GetXY(worldPositionOne);
        int[] gridPositionTwo = GetXY(worldPositionTwo);
        _jewelsToBeMoved[0, 0] = gridPositionOne[0];
        _jewelsToBeMoved[0, 1] = gridPositionOne[1];
        _jewelsToBeMoved[1, 0] = gridPositionTwo[0];
        _jewelsToBeMoved[1, 1] = gridPositionTwo[1];
        
        UpdateBoard();
    }
}
