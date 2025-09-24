using System.Collections;
using UnityEngine;

public class SpriteChanger : MonoBehaviour
{
    [SerializeField] private AudioSource spriteChangerAudioSource;
    public SpriteRenderer spriteRenderer; // 오브젝트의 스프라이트 렌더러
    public Sprite[] sprites; // 변경할 스프라이트 배열

    private void OnEnable()
    {
        StartCoroutine(ChangeSpriteGradually()); // 오브젝트가 활성화될 때 자동 실행
    }

    private IEnumerator ChangeSpriteGradually()
    {
        float elapsedTime = 0f; // 총 변경 시간
        float interval = 0.1f;  // 초기 변경 간격
        int spriteIndex = Random.Range(0, sprites.Length);

        spriteChangerAudioSource.PlayOneShot(spriteChangerAudioSource.clip);

        while (elapsedTime < 2f) // 2초 동안 변경
        {
            spriteRenderer.sprite = sprites[spriteIndex]; // 스프라이트 변경
            spriteIndex = (spriteIndex + 1) % sprites.Length; // 다음 스프라이트로 변경

            yield return new WaitForSeconds(interval);

            elapsedTime += interval;
            interval += 0.05f; // 간격 증가 (0.1 → 0.15 → 0.2 → ...)
        }

        // 여기서 spriteIndex는 "다음 인덱스" 상태임
        int lastSpriteIndex = (spriteIndex - 1 + sprites.Length) % sprites.Length;

        // 마지막으로 바뀐 스프라이트 유지 1초
        spriteRenderer.sprite = sprites[lastSpriteIndex];
        yield return new WaitForSeconds(1f);

        // 부모에게 마지막 스프라이트 전달
        FarmingDoor parent = transform.parent?.GetComponent<FarmingDoor>();
        if (parent != null)
        {
            parent.ReceiveDropIndex(lastSpriteIndex);
        }

        gameObject.SetActive(false); // 비활성화
    }
}
