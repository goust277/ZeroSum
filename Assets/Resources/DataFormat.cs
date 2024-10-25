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
    public string name;
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


#region 무기 클래스
/*Weapon 클래스(무기)*/
[Serializable]
public class Effects
{
    public string type;
    public float attackSpeed;
    public float range;
    public int[] damage;
    public int[] comboAni;
}
[Serializable]
public class Weapon
{
    public int id;
    public string name;
    public string des;
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

