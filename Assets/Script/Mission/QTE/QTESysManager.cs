using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTESysManager : Mission
{
    [SerializeField] private GameObject QTEMission;

    [System.Obsolete]
    private void Update()
    {
        if (QTEMission.GetComponent<QTESystem>().isClear && !isClear)
        {
            isClear = true;
            ClearMission();
            QTEMission.GetComponent<QTESystem>().isClear = false;
        }
        if (QTEMission.GetComponent<QTESystem>().isFail && player.active == false)
        {
            player.SetActive(true);
            input.SetActive(true);
            isFailed = true;
        }
    }

    private void ClearMission()
    {
        player.SetActive(true);
        input.SetActive(true);
    }
    public override void OnMission()
    {
        if (isFailed) return;

        QTEMission.SetActive(true);
        player.SetActive(false);
        input.SetActive(false);
    }
}
