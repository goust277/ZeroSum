using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class JsonLine
{
    public string line;
}
[Serializable]
public class JsonLines
{
    public JsonLine[] Lines;
}
[Serializable]
public class ClockWork
{
    public string name;
    public string des;
    public string[] param;
}
[Serializable]
public class ClockWorks 
{
    public ClockWork[] Works;
}
//////////////////////////////////////////////////


/*Chipset Ŭ���� (JSON �����͸� ���� Ŭ����)*/
[Serializable]
public class Chipset
{
    public int id;
    public string name;
    public bool type;
    public string des;
    public Dictionary<string, object> effect; // Ĩ�� ȿ��
}

/*Dialog Ŭ����(��� ��°�)*/
[Serializable]
public class DialogData
{
    public string name;
    public int pos;
    public List<string> log;
}
[Serializable]
public class SecneData
{
    public int SecneID;
    public List<DialogData> Dialog;
}
[Serializable]
public class DialogueRoot
{
    public List<SecneData> Secnes;
}


/*Weapon Ŭ����(����)*/
[Serializable]
public class Effects
{
    public string type;
    public float attackSpeed;
    public float range;
    public int[] damage;
    public string[] comboAni;
}
[Serializable]
public class Weapon
{
    public int id;
    public string name;
    public string des;
    public Effects effects;
}


