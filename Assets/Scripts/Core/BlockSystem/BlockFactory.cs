using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;

public class BlockFactory:MonoBehaviour
{
    public BlockDictionary[] BlockDictionary;
    public static BlockFactory Instance { get; private set; }
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
    public Block GetBlock(BlockTypes blockType,CubeTypes cubeTypes)
    {
        Block block = null;
        foreach (var item in BlockDictionary)
        {
            if(item.cubeType==cubeTypes && item.blockType == blockType)
            {
                block= Instantiate(item.blockObject,item.blockObject.transform.position,Quaternion.identity);
            }
        }
        return block;
    }
}

[System.Serializable]
public class BlockDictionary
{
    public BlockTypes blockType;
    public CubeTypes cubeType;
    public Block blockObject;
}