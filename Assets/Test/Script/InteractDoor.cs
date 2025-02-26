using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class InteractDoor : MonoBehaviour
{
    [Header("��� ������")]
    [SerializeField] private GameObject item;

    [HideInInspector] public bool isDropReady;
    private GameObject target;
    private bool isTakeItem;
    void Start()
    {
        isTakeItem = true;
        isDropReady = false;
        target = GameObject.Find("Items");
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void OnInteract()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        isTakeItem = false;

        GameObject _item = Instantiate(item, transform.position, Quaternion.identity, target.transform);
    }
}
