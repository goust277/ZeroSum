using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaugeBar : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private RectTransform rc;

    private bool isMoveRight = false;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (player.rotation.y != 0 && !isMoveRight)
        {
            isMoveRight = true;
            rc.rotation = Quaternion.Euler(0, 180, 0);
        }

        if (player.rotation.y == 0 && rc.rotation.y != 0)
        {
            rc.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

}
