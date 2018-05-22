using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

//约定，TileType枚举字段名为资源名
public class Tile : ObjectBase
{
    static public Dictionary<int, string> s_ResNameDict;

    private const string c_TexturesDir = "Textures/";
    private SpriteRenderer m_SpriteRenderer;
    private TileType m_Type;

    static Tile()
    {
        s_ResNameDict = new Dictionary<int, string>();
        FieldInfo[] fields = typeof(TileType).GetFields(BindingFlags.Static | BindingFlags.Public);
        foreach (var field in fields)
        {
            s_ResNameDict.Add((int)field.GetValue(null), field.Name);
        }
    }

    public Tile(string path, Transform parentTransform, TileData tileData) : base(path, parentTransform, tileData.pos)
    {
        m_Type = tileData.type;
        m_SpriteRenderer = this.Entity.GetComponent<SpriteRenderer>();
        this.Entity.transform.localEulerAngles = new Vector3(90, 0, 0);
        Debug.Assert(m_SpriteRenderer != null, "Tile上需要SpriteRenderer组件");
        this.SetTexture();
    }

    private void SetTexture()
    {
        string resName = s_ResNameDict[(int)m_Type];
        Sprite sprite = Resources.Load<Sprite>(c_TexturesDir + resName);
        m_SpriteRenderer.sprite = sprite;
    }
}
