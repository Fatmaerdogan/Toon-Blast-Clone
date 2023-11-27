using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FillManager : MonoBehaviour
{
    [SerializeField] private GameObject blockPrefab;

    public static FillManager Instance { get; private set; }
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
    public void Fill()
    {

        for (int i = 0; i < GridManager.Instance.changingColumns.Count; i++)
        {
            var item = GridManager.Instance.changingColumns.ElementAt(i);
            var itemKey = item.Key;
            var itemValue = item.Value;

            int rowLength = GridManager.Instance.allBlocks[itemKey].rows.Length;
            for (int j = rowLength - 1; j >= 0; j--)
            {
                if (GridManager.Instance.allBlocks[itemKey].rows[j] == null || (GridManager.Instance.allBlocks[itemKey].rows[j]
                    && GridManager.Instance.allBlocks[itemKey].rows[j].GetComponent<Block>().target == null))
                {
                    for (int k = j; k >= 0; k--)
                    {
                        if (GridManager.Instance.allBlocks[itemKey].rows[k]
                            && GridManager.Instance.allBlocks[itemKey].rows[k].GetComponent<Block>().target != null)
                        {
                            GameObject newTargetObj = GridManager.Instance.allPosObjs[itemKey].rows[j].gameObject;
                            Block curBlock = GridManager.Instance.allBlocks[itemKey].rows[k].gameObject.GetComponent<Block>();
                            curBlock.target = newTargetObj.transform;
                            curBlock.gridIndex = new Vector2(itemKey, j);

                            GridManager.Instance.allBlocks[itemKey].rows[j] = GridManager.Instance.allBlocks[itemKey].rows[k];
                            GridManager.Instance.allBlocks[itemKey].rows[k] = null;
                            curBlock.UpdateSortingOrder();
                            curBlock.MoveToTarget(0.5f);
                            break;
                        }
                    }

                }
            }
        }
        
        FallManager.Instance.Fall();
    }
    public void FillOnlyOneBlock(BlockTypes blockType, Vector2 gridIndex)
    {
        int x = (int)gridIndex.x;
        int y = (int)gridIndex.y;
        Vector3 spawnPos = GridManager.Instance.allPosObjs[(int)gridIndex.x].rows[(int)gridIndex.y].transform.position;
        Block currentBlock = BlockFactory.Instance.GetBlock(blockType, CubeTypes.Null);
        currentBlock.transform.localPosition = spawnPos;
        currentBlock.transform.SetParent(GridManager.Instance.SpawnedBlocksParent);
        GridManager.Instance.allBlocks[x].rows[y] = currentBlock.gameObject;
        currentBlock.gridIndex = gridIndex;
        currentBlock.target = GridManager.Instance.allPosObjs[x].rows[y].transform;
        currentBlock.SetupBlock();

        GridManager.Instance.DecreaseChangingColumn((int)gridIndex.x);
    }

}
