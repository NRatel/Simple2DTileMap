using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    private Transform m_RootNodeTransform;
    private Transform m_BgTransform;
    private Player m_ThePlayer;
    private TileData[,] m_ClientTileDatas;
    private Tile[,] m_Tiles;

    public Map()
    {
        m_RootNodeTransform = GameObject.Find("RootNode").transform;
        m_BgTransform = m_RootNodeTransform.Find("Bg");
        m_BgTransform.localScale = new Vector3((Config.c_MapSize.x + 0.5f) * Config.c_MapScaleRate, 1, (Config.c_MapSize.y + 0.5f) * Config.c_MapScaleRate);
        m_BgTransform.localPosition = new Vector3((Config.c_MapSize.x - 1) * Config.c_MapScaleRate / 2, -1, (Config.c_MapSize.y - 1) * Config.c_MapScaleRate / 2);

        m_Tiles = new Tile[(int)Config.c_MapSize.x, (int)Config.c_MapSize.y];
        m_ClientTileDatas = new TileData[(int)Config.c_MapSize.x, (int)Config.c_MapSize.y];

        Vect2Int playerRandStartPos = new Vect2Int(Random.Range(0, Config.c_MapSize.x), Random.Range(0, Config.c_MapSize.y));
        m_ThePlayer = new Player("Prefabs/Player", m_RootNodeTransform, playerRandStartPos);
        CreatePlayerAroundTiles();

        Main.uiPlay.ShowPlayerMapTilePos(m_ThePlayer.MapTilePos);
        Main.uiPlay.ShowCurPosMapTileType(Tile.s_ResNameDict[(int)GetTileData(m_ThePlayer.MapTilePos).type]);
    }

    public void CreatePlayerAroundTiles()
    {
        //从服务器请求Player周围的数据
        AreaData areaData = Main.server.FindAroundTileDatas(m_ThePlayer.MapTilePos);

        Vect2Int offsetIndex = areaData.offsetIndex;
        Vect2Int size = areaData.size;
        TileData[,] tileDatas = areaData.tileDatas;

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Tile tile = m_Tiles[offsetIndex.x + i, offsetIndex.y + j];
                if (tile == null)
                {
                    TileData clientTileData = m_ClientTileDatas[offsetIndex.x + i, offsetIndex.y + j] = tileDatas[i, j];
                    m_Tiles[offsetIndex.x + i, offsetIndex.y + j] = new Tile("Prefabs/Tile", m_RootNodeTransform, clientTileData);
                }
            }
        }
    }

    public void ChangePlayerActualPos(Vector2 delta)
    {
        //调整移动速度
        delta = delta * Config.c_MoveSpeed;
        //屏幕坐标转世界坐标
        Vector3 mapDelta = new Vector3(-delta.x, 0, -delta.y);
        m_ThePlayer.ChangeActualPos(mapDelta);
    }

    public TileData GetTileData(Vect2Int mapTilePos)
    {
        return m_ClientTileDatas[mapTilePos.x, mapTilePos.y];
    }
}
