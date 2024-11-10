using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics.Tracing;

#region 칩셋 클래스
[Serializable]
public class Chipset
{
    public int id;
    public string name;
    public bool type;
    public string des;
    public Dictionary<string, object> effect; // 칩셋 효과
}
#endregion

#region 대사 클래스
[Serializable]
public class DialogData
{
    public int id;
    public int pos;
    public List<string> log;
}
[Serializable]
public class Prerequisites
{
    public int npc;
    public List<string> needEventConditions;
    public int currentSecneID;
}
[Serializable]
public class SecneData
{
    public int SecneID;
    public List<DialogData> dialog;
    public Prerequisites prerequisites;
    public AfterConditions afterConditions;
}
[Serializable]
public class DialogueRoot
{
    public List<SecneData> Secnes;
}
[Serializable]
public class AfterConditions
{
    public List<string> changeEventConditions;
    public int changeSecneID;
}
#endregion

#region 엔피씨 정보
[Serializable]
public class NPCInfo
{
    public int NPCid;
    public string NPCname;
    public string NPCportrait;
    public string defalutDialog;
    public List<int> itemsForSale;
}
[Serializable]
public class NPCData
{
    public List<NPCInfo> NPCs;
}
#endregion


#region 무기 클래스
/*Weapon 클래스(무기)*/
[Serializable]
public class Effects
{
    public float attackSpeed;
    public float range;
    public int[] damage;
    public int[] comboAni;
}
[Serializable]
public class Weapon
{
    public int weaponId;
    public string weaponName;
    public string weaponDes;
    public string weaponIcons;
    public int type;
    public Effects effects;
}
#endregion

#region 이벤트 상태
[Serializable]
public class Event
{
    public int chapterNum; // 챕터 번호
    public Dictionary<string, bool> EventFlags; // 이벤트 조건
}
[Serializable]
public class EventRoot
{
    public List<Event> Events; // 이벤트 리스트
}
#endregion

#region 아이템
public class ItemForSale
{
    public int itemId;
    public string itemName;
    public string icons;
    public string des;
    public int price;
}
public class ItemRoot
{
    public List<ItemForSale> Items;
}
#endregion