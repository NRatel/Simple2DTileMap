using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulativeServer : IServerInterface
{
    //模拟数据库
    private TileData[,] m_DataBase;

    public SimulativeServer()
    {
        m_DataBase = new TileData[(int)Config.c_MapSize.x, (int)Config.c_MapSize.y];
        CreateMap();
    }

    private void CreateMap()
    {
        //CreateMapData_Rand();
        //CreateMapData_Test1();
        CreateMapData_Test2();
    }

    #region 选择创建不同的地图
    //完全随机
    private void CreateMapData_Rand()
    {
        int mapBorderLeft = 0;
        int mapBorderRight = Config.c_MapSize.x - 1;
        int mapBorderTop = 0;
        int mapBorderBottom = Config.c_MapSize.y - 1;

        for (int i = mapBorderLeft; i <= mapBorderRight; i++)
        {
            for (int j = mapBorderTop; j <= mapBorderBottom; j++)
            {
                m_DataBase[i, j] = new TileData((TileType)Random.Range(0, 7), new Vect2Int(i, j));
            }
        }
    }
    //条纹
    private void CreateMapData_Test1()
    {
        int mapBorderLeft = 0;
        int mapBorderRight = Config.c_MapSize.x - 1;
        int mapBorderBottom = 0;
        int mapBorderTop = Config.c_MapSize.y - 1;

        for (int i = mapBorderLeft; i <= mapBorderRight; i++)
        {
            for (int j = mapBorderBottom; j <= mapBorderTop; j++)
            {
                m_DataBase[i, j] = new TileData((TileType)(i % 7), new Vect2Int(i, j));
            }
        }
    }
    //正方形四边和对称轴
    private void CreateMapData_Test2()
    {
        int mapBorderLeft = 0;
        int mapBorderRight = Config.c_MapSize.x - 1;
        int mapBorderBottom = 0;
        int mapBorderTop = Config.c_MapSize.y - 1;

        for (int i = mapBorderLeft; i <= mapBorderRight; i++)
        {
            for (int j = mapBorderBottom; j <= mapBorderTop; j++)
            {
                if (i == mapBorderLeft || i == mapBorderRight || j == mapBorderTop || j == mapBorderBottom)
                {
                    m_DataBase[i, j] = new TileData((TileType)6, new Vect2Int(i, j));
                }
                else if (i == j || i == mapBorderTop - j)
                {
                    m_DataBase[i, j] = new TileData((TileType)5, new Vect2Int(i, j));
                }
                else if (i == Mathf.FloorToInt(mapBorderRight / 2) || i == Mathf.FloorToInt(mapBorderRight / 2) + 1 || j == Mathf.FloorToInt(mapBorderTop / 2) || j == Mathf.FloorToInt(mapBorderTop / 2) + 1)
                {
                    m_DataBase[i, j] = new TileData((TileType)4, new Vect2Int(i, j));
                }
                else
                {
                    m_DataBase[i, j] = new TileData((TileType)1, new Vect2Int(i, j));
                }
            }
        }
    }
    #endregion

    #region 实现接口
    public AreaData FindAroundTileDatas(Vect2Int pos)
    {
        int mapBorderLeft = 0;
        int mapBorderRight = Config.c_MapSize.x - 1;
        int mapBorderTop = 0;
        int mapBorderBottom = Config.c_MapSize.y - 1;

        int startI = pos.x - Config.c_Range.x > mapBorderLeft ? pos.x - Config.c_Range.x : mapBorderLeft;
        int endI = pos.x + Config.c_Range.x < mapBorderRight ? pos.x + Config.c_Range.x : mapBorderRight;

        int startJ = pos.y - Config.c_Range.y > mapBorderTop ? pos.y - Config.c_Range.y : mapBorderTop;
        int endJ = pos.y + Config.c_Range.y < mapBorderBottom ? pos.y + Config.c_Range.y : mapBorderBottom;

        Vect2Int offsetIndex = new Vect2Int(startI, startJ);
        Vect2Int size = new Vect2Int(endI - startI + 1, endJ - startJ + 1);

        TileData[,] tileDatas = new TileData[size.x, size.y];

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                tileDatas[i, j] = m_DataBase[offsetIndex.x + i, offsetIndex.y + j];
            }
        }

        return new AreaData(offsetIndex, size, tileDatas);
    }
    #endregion
}
