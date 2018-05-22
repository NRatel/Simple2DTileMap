using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 这个文件中定义一些Clinet和Server通用的数据类型 和 前后端通信接口，
/// 如果后期需要真实发送，只需要将这里的类型序列化。
/// </summary>

public interface IServerInterface
{
    AreaData FindAroundTileDatas(Vect2Int pos);
}

public enum TileType
{
    A = 0,
    B = 1,
    C = 2,
    D = 3,
    E = 4,
    F = 5,
    G = 6,
}

//二维Int向量
public struct Vect2Int
{
    public int x;
    public int y;

    public Vect2Int(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

//地块数据
public struct TileData
{
    public TileType type;       //和Client的 TileType取值范围一致
    public Vect2Int pos;

    public TileData(TileType type, Vect2Int pos)
    {
        this.type = type;       //Tile类型
        this.pos = pos;         //Tile坐标
    }
}

//地图上的一块区域, 包含若干TileData
public struct AreaData
{
    public Vect2Int offsetIndex;    //通过 加上offset 获得实际位置索引
    public Vect2Int size;           //下面数组的大小
    public TileData[,] tileDatas;   //从0，0开始的数组

    public AreaData(Vect2Int offsetIndex, Vect2Int size, TileData[,] tileDatas)
    {
        this.offsetIndex = offsetIndex;
        this.size = size;
        this.tileDatas = tileDatas;
    }
}



