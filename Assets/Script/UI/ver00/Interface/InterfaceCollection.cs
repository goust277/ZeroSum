using System;
using System.Collections.Generic;
using UnityEngine;

#region ����, Ĩ��
public interface IItemCollection<T>
{
    IEnumerable<T> Items { get; }
}

[Serializable]
public class WeaponsArray : IItemCollection<Weapon>
{
    public Weapon[] Weapons;

    public IEnumerable<Weapon> Items => Weapons;
}

[Serializable]
public class ChipsetsArray : IItemCollection<Chipset>
{
    public Chipset[] ChipSets;

    public IEnumerable<Chipset> Items => ChipSets;
}
#endregion