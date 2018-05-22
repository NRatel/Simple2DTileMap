using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPlay : MonoBehaviour
{
    public GameObject m_Bg;
    public Text m_Text_MapTilePos;
    public Text m_Text_TileType;

    // Use this for initialization
    void Start ()
    {
        RegisterEvent(m_Bg, EventTriggerType.BeginDrag, OnBgBeginFrag);
        RegisterEvent(m_Bg, EventTriggerType.Drag, OnBgDrag);
        RegisterEvent(m_Bg, EventTriggerType.EndDrag, OnBgEndDrag);
    }

    public void OnBgBeginFrag(BaseEventData data)
    {
        PointerEventData eventData = (PointerEventData)data;
        Debug.Log("OnBgBeginFrag");
    }

    public void OnBgDrag(BaseEventData data)
    {
        PointerEventData eventData = (PointerEventData)data;
        //Debug.Log("OnBgDrag");
        Main.client.ChangePlayerActualPos(eventData.delta);
    }

    public void OnBgEndDrag(BaseEventData data)
    {
        PointerEventData eventData = (PointerEventData)data;
        Debug.Log("OnBgEndDrag");
    }

    private void RegisterEvent(GameObject gameObject, EventTriggerType eventTriggerType, UnityAction<BaseEventData> action)
    {
        EventTrigger evnetTrigger = m_Bg.GetComponent<EventTrigger>();
        if (evnetTrigger == null)
        {
            evnetTrigger = m_Bg.AddComponent<EventTrigger>();
        }
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventTriggerType;
        EventTrigger.TriggerEvent triggerEvent = new EventTrigger.TriggerEvent();
        triggerEvent.AddListener(action);
        entry.callback = triggerEvent;
        evnetTrigger.triggers.Add(entry);
    }

    //直接调UI的方法不好，简便起见这么干
    public void ShowPlayerMapTilePos(Vect2Int mapTilePos)
    {
        m_Text_MapTilePos.text = "当前位置: (" + mapTilePos.x + ", " + mapTilePos.y + ")";
    }

    public void ShowCurPosMapTileType(string TypeName)
    {
        m_Text_TileType.text = "当前地块类型: " + TypeName;
    }
}
