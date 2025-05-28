using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [Header("Flash")]
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private int flashCount = 4;

    [Header("Sprite")]
    [SerializeField] private SpriteRenderer sprite;
    private Color originalColor;
   
    private void Start()
    {
        originalColor = sprite.color;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        sprite.color = originalColor;
    }

    // Update is called once per frame
    public void TriggerFlash(float duration)
    {
        Debug.Log("Flash");
        StopAllCoroutines();
        StartCoroutine(FlashCoroutine(duration));
    }

    IEnumerator FlashCoroutine(float flashDuration)
    {
        float interval = flashDuration / (flashCount * 2f);

        for (int i = 0; i < flashCount; i++)
        {
            sprite.color = flashColor;
            yield return new WaitForSeconds(interval);
            sprite.color = originalColor;
            yield return new WaitForSeconds(interval);

        }
        sprite.color = originalColor;
    }
}
