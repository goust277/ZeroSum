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


#region ���� Ŭ����
/*Weapon Ŭ����(����)*/
[Serializable]
public class Animations
{
    public string idle;
    public string combo1;
    public string combo2;
    public string combo3;
    public string upSideAttack;
    public string downSideAttack;
    public string comboEnd;
}
[Serializable]
public class Effects
{
    public int level;
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
    public Animations animations;
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

#region ������
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