using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics.Tracing;

#region Ĩ�� Ŭ����
[Serializable]
public class Chipset
{
    public int id;
    public string name;
    public bool type;
    public string des;
    public Dictionary<string, object> effect; // Ĩ�� ȿ��
}
#endregion

#region ��� Ŭ����
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

#region ���Ǿ� ����
[Serializable]
public class NPCInfo
{
    public int id;
    public string name;
    public string portrait;
    public string defalutDialog;
}
[Serializable]
public class NPCData
{
    public List<NPCInfo> NPCs;
}
#endregion


#region ���� Ŭ����
/*Weapon Ŭ����(����)*/
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
    public string icons;
    public Effects effects;
}
#endregion

#region �̺�Ʈ ����
[Serializable]
public class Event
{
    public int chapterNum; // é�� ��ȣ
    public Dictionary<string, bool> EventFlags; // �̺�Ʈ ����
}
[Serializable]
public class EventRoot
{
    public List<Event> Events; // �̺�Ʈ ����Ʈ
}
#endregion

