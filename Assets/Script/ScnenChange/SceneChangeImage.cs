using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneChangeImage : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private float speed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (image.fillAmount <= 1) 
        {
            image.fillAmount += Time.deltaTime * speed;

        }
    }


}
