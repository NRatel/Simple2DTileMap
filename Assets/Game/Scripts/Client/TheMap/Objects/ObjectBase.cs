using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public abstract class ObjectBase
{
    private string m_PrefabPath;            //资源路径
    private GameObject m_Entity;            //绑定实际物体
    private Transform m_ParentTransform;    //父节点
    private Vect2Int m_MapTilePos;          //在Map上的坐标

    #region public 封装字段
    public string PrefabPath
    {
        get
        {
            return m_PrefabPath;
        }

        set
        {
            m_PrefabPath = value;
        }
    }

    public GameObject Entity
    {
        get
        {
            return m_Entity;
        }

        set
        {
            m_Entity = value;
        }
    }

    public Transform ParentTransform
    {
        get
        {
            return m_ParentTransform;
        }

        set
        {
            m_ParentTransform = value;
        }
    }

    public Vect2Int MapTilePos
    {
        get
        {
            return m_MapTilePos;
        }

        set
        {
            m_MapTilePos = value;
        }
    }
    #endregion

    #region public 
    public ObjectBase(string prefabPath, Transform parentTransform, Vect2Int mapTilePos)
    {
        m_PrefabPath = prefabPath;
        m_ParentTransform = parentTransform;
        m_MapTilePos = mapTilePos;
        m_Entity = CreateEntity(MapTilePosToActualPos(mapTilePos));
    }

    public void ChangeActualPos(Vector3 delta)
    {
        int mapBorderLeft = 0;
        int mapBorderRight = Config.c_MapSize.x - 1;
        int mapBorderBottom = 0;
        int mapBorderTop = Config.c_MapSize.y - 1;

        Vector3 calcedActualPos = m_Entity.transform.localPosition + delta;

        //将实际坐标限定在地图内
        float x, z;
        if (calcedActualPos.x < mapBorderLeft * Config.c_MapScaleRate)
        {
            x = mapBorderLeft * Config.c_MapScaleRate;
        }
        else if (calcedActualPos.x > mapBorderRight * Config.c_MapScaleRate)
        {
            x = mapBorderRight * Config.c_MapScaleRate;
        }
        else
        {
            x = calcedActualPos.x;
        }

        if (calcedActualPos.z < mapBorderBottom * Config.c_MapScaleRate)
        {
            z = mapBorderBottom * Config.c_MapScaleRate;
        }
        else if (calcedActualPos.z > mapBorderTop * Config.c_MapScaleRate)
        {
            z = mapBorderTop * Config.c_MapScaleRate;
        }
        else
        {
            z = calcedActualPos.z;
        }

        m_Entity.transform.localPosition = new Vector3(x, 0, z);

        Vect2Int newMapTilePos = ActualPosToMapTilePos(m_Entity.transform.localPosition);
        if (newMapTilePos.x != m_MapTilePos.x || newMapTilePos.y != m_MapTilePos.y)
        {
            m_MapTilePos = newMapTilePos;
            OnMapTilePosChanged(newMapTilePos);
        }
    }
    
    public virtual void OnMapTilePosChanged(Vect2Int newMapTilePos)
    {

    }
    #endregion

    #region private
    private GameObject CreateEntity(Vector3 startPosition)
    {
        GameObject prefab = Resources.Load<GameObject>(m_PrefabPath);
        GameObject entity = GameObject.Instantiate<GameObject>(prefab);
        entity.transform.SetParent(m_ParentTransform);
        entity.transform.localPosition = startPosition;
        entity.transform.localEulerAngles = new Vector3(0, 0, 0);
        entity.transform.localScale = Vector3.one;
        return entity;
    }

    public Vect2Int ActualPosToMapTilePos(Vector3 actualPos)
    {
        int mapBorderLeft = 0;
        int mapBorderRight = Config.c_MapSize.x - 1;
        int mapBorderBottom = 0;
        int mapBorderTop = Config.c_MapSize.y - 1;

        int calcedMapTilePosX = Mathf.RoundToInt(actualPos.x / Config.c_MapScaleRate);
        int calcedMapTilePosZ = Mathf.RoundToInt(actualPos.z / Config.c_MapScaleRate);

        //将MapTile坐标限定在地图内
        int x, y;
        if (calcedMapTilePosX < mapBorderLeft)
        {
            x = mapBorderLeft;
        }
        else if (calcedMapTilePosX > mapBorderRight)
        {
            x = mapBorderRight;
        }
        else
        {
            x = calcedMapTilePosX;
        }

        if (calcedMapTilePosZ < mapBorderBottom)
        {
            y = mapBorderBottom;
        }
        else if (calcedMapTilePosZ > mapBorderTop)
        {
            y = mapBorderTop;
        }
        else
        {
            y = calcedMapTilePosZ;
        }
        
        return new Vect2Int(x, y);
    }

    public Vector3 MapTilePosToActualPos(Vect2Int mapTilePos)
    {
        return new Vector3(mapTilePos.x * Config.c_MapScaleRate, 0, mapTilePos.y * Config.c_MapScaleRate);
    }
    #endregion
}
