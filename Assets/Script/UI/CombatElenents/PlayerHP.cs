using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;

public class PlayerHP : MonoBehaviour, IDamageAble
{
    [SerializeField] private GameObject DamageValuePrefab;

    public void Damage(int value)
    {
        GameStateManager.Instance.getChangedHP(value);
        VisualDamage(value);
    }

    private void VisualDamage(int value)
    {
        Debug.Log("VisualDamage");
        Vector3 offset = new Vector3(0, 1, 0); // 위로 1만큼 오프셋
        GameObject newText = Instantiate(DamageValuePrefab, gameObject.transform.position + offset, Quaternion.identity);

        newText.transform.SetParent(GameObject.Find("UI").transform, false);

        TextMeshProUGUI textComponent = newText.GetComponentInChildren<TextMeshProUGUI>();
        if (textComponent == null)
        {
            Debug.LogError("TextMeshProUGUI component not found in prefab!");
            return;
        }

        textComponent.text = value.ToString();
    }

    public void Health(int value)
    {
        GameStateManager.Instance.getChangedHP(value);
    }

}
