using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvBtn : BaseInteractable
{
    [SerializeField] private ControlEv target;
    [SerializeField] private int num;
    public override void Exe()
    {
        if (target == null)
            return;
        target.btnNum = num;
        target.Exe();

        Debug.Log("��ư ��ȣ�ۿ�");
    }
}
