using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;

public class PlayerHP : MonoBehaviour, IDamageAble
{
    [SerializeField] private GameObject DamageValuePrefab;
    [SerializeField] private Transform canvasTransform;

    public void Damage(int value)
    {
        GameStateManager.Instance.getChangedHP(value);
        VisualDamage(value);
    }

    private void VisualDamage(int value)
    {
        Debug.Log("VisualDamage");
        Vector3 offset = gameObject.transform.position; // z축을 -0.1로 설정
        GameObject newText = Instantiate(DamageValuePrefab, offset, Quaternion.identity);
        Debug.Log($"★★★★★ New Damage Text instantiated at: {newText.transform.position}");


        if (canvasTransform == null)
        {
            Debug.LogError("Canvas Transform is not assigned!");
            return;
        }

        newText.transform.SetParent(canvasTransform, false);
        //Debug.Log($"★ ★Parent set to: {newText.transform.parent.name}");
        //TextMeshProUGUI textComponent = newText.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI textComponent = newText.GetComponent<TextMeshProUGUI>();
        if (textComponent == null)
        {
            Debug.LogError("TextMeshProUGUI component not found in prefab!");
            return;
        }

        textComponent.text = value.ToString();
    }
    //public void Health(int value)
    //{
    //    GameStateManager.Instance.getChangedHP(value);
    //}
}
