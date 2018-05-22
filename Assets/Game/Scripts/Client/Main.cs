using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Config
{
    static public Vect2Int c_MapSize = new Vect2Int(30, 30);    //地图大小，单位是Tile
    static public Vect2Int c_Range = new Vect2Int(2, 2);        //视野范围，实际范围为(x*2+1, y*2+1), 单位是Tile
    static public float c_MapScaleRate = 2.4f;                  //地图坐标系相对Tile坐标缩放系数，和贴图原大小有关，直接修改可调整间距
    static public float c_MoveSpeed = 0.02f;                    //移动速度
    static public float c_FOV = 15;                             //正交摄像机视野
}

public class Main : MonoBehaviour
{
    static public Map client;
    static public SimulativeServer server;
    static public UIPlay uiPlay;

    void Start () {
        //没UI框架，简单用
        uiPlay = GameObject.Find("Canvas/UIPlay").GetComponent<UIPlay>();

        server = new SimulativeServer();
        client = new Map();
    }
}
