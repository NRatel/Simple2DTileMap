using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//只是用来标记当前视野中点
public class Player :ObjectBase
{
    private FollowCamera m_FollowCammer;    //跟随相机

    public Player(string prefabPath, Transform parentTransform, Vect2Int mapTilePos) : base(prefabPath, parentTransform, mapTilePos)
    {
        m_FollowCammer = GameObject.Find("MainCamera").GetComponent<FollowCamera>();
        m_FollowCammer.SetTarget(this.Entity.transform);
    }

    public override void OnMapTilePosChanged(Vect2Int newMapTilePos)
    {
        Main.uiPlay.ShowPlayerMapTilePos(newMapTilePos);
        Main.uiPlay.ShowCurPosMapTileType(Tile.s_ResNameDict[(int)Main.client.GetTileData(MapTilePos).type]);
        Main.client.CreatePlayerAroundTiles();
    }
}
