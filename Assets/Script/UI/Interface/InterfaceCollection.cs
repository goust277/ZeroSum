using System;
using System.Collections.Generic;
using UnityEngine;

#region 무기, 칩셋
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


#region 유저 스탯관련 인터페이스
public interface IHealth
{
    float MaxHP { get; set; }
    float CurrentHP { get; set; }
    void GetDamage(int damage, Vector2 attackerPosition);
}
#endregion