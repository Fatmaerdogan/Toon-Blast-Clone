using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class NeighbourManager : MonoBehaviour
{
    public static NeighbourManager Instance { get; private set; }
    List<GameObject> neighbours = new List<GameObject>();
    List<GameObject> foundedBaloons = new List<GameObject>();
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    public void DoSingleObjAction(Vector2 gridIndex)
    {
        GridManager.Instance.AddNewChangingColumn((int)gridIndex.x);
    }
    public List<GameObject> FindSameNeighbours(CubeTypes cubeType, Vector2 gridIndex)
    {
        neighbours = new List<GameObject>();
        foundedBaloons = new List<GameObject>();

        GridManager.Instance.changingColumns = new Dictionary<int, int>();
        GridManager.Instance.AddNewChangingColumn((int)gridIndex.x);
        neighbours.Add(GridManager.Instance.allBlocks[(int)gridIndex.x].rows[(int)gridIndex.y]);

        RecursiveNeighbourSearch(cubeType, gridIndex);

        return neighbours;
    }
    public List<GameObject> FindUpBlocks(Vector2 gridIndex)
    {
        List<GameObject> upBlocks = new List<GameObject>();
        for (int i = 0; i < (int)gridIndex.y; i++)
        {
            GridManager.Instance.AddNewChangingColumn((int)gridIndex.x);
            upBlocks.Add(GridManager.Instance.allBlocks[(int)gridIndex.x].rows[i]);
        }
        return upBlocks;
    }
    public List<GameObject> FindDownBlocks(Vector2 gridIndex)
    {
        List<GameObject> downBlocks = new List<GameObject>();
        for (int i = (int)gridIndex.y + 1; i < GridManager.Instance.myGrid.GridSizeY; i++)
        {
            GridManager.Instance.AddNewChangingColumn((int)gridIndex.x);
            downBlocks.Add(GridManager.Instance.allBlocks[(int)gridIndex.x].rows[i]);
        }
        return downBlocks;
    }
    public List<GameObject> FindRightBlocks(Vector2 gridIndex)
    {
        List<GameObject> rightBlocks = new List<GameObject>();
        for (int i = (int)gridIndex.x + 1; i < GridManager.Instance.myGrid.GridSizeX; i++)
        {
            GridManager.Instance.AddNewChangingColumn(i);
            rightBlocks.Add(GridManager.Instance.allBlocks[i].rows[(int)gridIndex.y]);
        }
        return rightBlocks;
    }
    public List<GameObject> FindLeftBlocks(Vector2 gridIndex)
    {
        List<GameObject> leftBlocks = new List<GameObject>();
        for (int i = 0; i < (int)gridIndex.x; i++)
        {
            GridManager.Instance.AddNewChangingColumn(i);
            leftBlocks.Add(GridManager.Instance.allBlocks[i].rows[(int)gridIndex.y]);
        }
        return leftBlocks;
    }
    public void RecursiveNeighbourSearch(CubeTypes cubeType, Vector2 gridIndex)
    {
        int x = (int)gridIndex.x;
        int y = (int)gridIndex.y;
        if (x + 1 < GridManager.Instance.allBlocks.Length)
        {
            Block curBlock = GridManager.Instance.allBlocks[x + 1].rows[y].GetComponent<Block>();
            if (curBlock is CubeBlock &&
                GridManager.Instance.allBlocks[x + 1].rows[y].GetComponent<CubeBlock>().cubeType == cubeType)
            {
                if (!neighbours.Contains(GridManager.Instance.allBlocks[x + 1].rows[y]))
                {
                    GridManager.Instance.AddNewChangingColumn(x + 1);
                    neighbours.Add(GridManager.Instance.allBlocks[x + 1].rows[y]);
                    RecursiveNeighbourSearch(cubeType, new Vector2(x + 1, y));
                }
            }
            else if (curBlock is BottleBlock)
            {

                if (!foundedBaloons.Contains(GridManager.Instance.allBlocks[x + 1].rows[y]))
                {
                    GridManager.Instance.AddNewChangingColumn(x + 1);
                    foundedBaloons.Add(GridManager.Instance.allBlocks[x + 1].rows[y]);
                }
            }
        }
        if (y + 1 < GridManager.Instance.allBlocks[0].rows.Length)
        {
            Block curBlock = GridManager.Instance.allBlocks[x].rows[y + 1].GetComponent<Block>();
            if (curBlock is CubeBlock &&
                GridManager.Instance.allBlocks[x].rows[y + 1].GetComponent<CubeBlock>().cubeType == cubeType)
            {
                if (!neighbours.Contains(GridManager.Instance.allBlocks[x].rows[y + 1]))
                {
                    GridManager.Instance.AddNewChangingColumn(x);
                    neighbours.Add(GridManager.Instance.allBlocks[x].rows[y + 1]);
                    RecursiveNeighbourSearch(cubeType, new Vector2(x, y + 1));
                }
            }
            else if (curBlock is BottleBlock)
            {

                if (!foundedBaloons.Contains(GridManager.Instance.allBlocks[x].rows[y + 1]))
                {
                    GridManager.Instance.AddNewChangingColumn(x);
                    foundedBaloons.Add(GridManager.Instance.allBlocks[x].rows[y + 1]);
                }
            }
        }
        if (x - 1 >= 0)
        {
            Block curBlock = GridManager.Instance.allBlocks[x - 1].rows[y].GetComponent<Block>();
            if (curBlock is CubeBlock &&
                GridManager.Instance.allBlocks[x - 1].rows[y].GetComponent<CubeBlock>().cubeType == cubeType)
            {
                if (!neighbours.Contains(GridManager.Instance.allBlocks[x - 1].rows[y]))
                {
                    GridManager.Instance.AddNewChangingColumn(x - 1);
                    neighbours.Add(GridManager.Instance.allBlocks[x - 1].rows[y]);
                    RecursiveNeighbourSearch(cubeType, new Vector2(x - 1, y));
                }
            }
            else if (curBlock is BottleBlock)
            {

                if (!foundedBaloons.Contains(GridManager.Instance.allBlocks[x - 1].rows[y]))
                {
                    GridManager.Instance.AddNewChangingColumn(x - 1);
                    foundedBaloons.Add(GridManager.Instance.allBlocks[x - 1].rows[y]);
                }
            }
        }
        if (y - 1 >= 0)
        {
            Block curBlock = GridManager.Instance.allBlocks[x].rows[y - 1].GetComponent<Block>();
            if (curBlock is CubeBlock &&
                 GridManager.Instance.allBlocks[x].rows[y - 1].GetComponent<CubeBlock>().cubeType == cubeType)
            {
                if (!neighbours.Contains(GridManager.Instance.allBlocks[x].rows[y - 1]))
                {
                    GridManager.Instance.AddNewChangingColumn(x);
                    neighbours.Add(GridManager.Instance.allBlocks[x].rows[y - 1]);
                    RecursiveNeighbourSearch(cubeType, new Vector2(x, y - 1));
                }
            }
            else if (curBlock is BottleBlock)
            {

                if (!foundedBaloons.Contains(GridManager.Instance.allBlocks[x].rows[y - 1]))
                {
                    GridManager.Instance.AddNewChangingColumn(x);
                    foundedBaloons.Add(GridManager.Instance.allBlocks[x].rows[y - 1]);
                }
            }
        }
    }
    public void BlowUpBallons()
    {
        foreach (GameObject balloon in foundedBaloons)
        {
            balloon.GetComponent<BottleBlock>().Explode();
        }
        foundedBaloons = new List<GameObject>();
    }

}
