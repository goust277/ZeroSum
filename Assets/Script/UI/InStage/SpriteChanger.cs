using System.Collections;
using UnityEngine;

public class SpriteChanger : MonoBehaviour
{
    [SerializeField] private AudioSource spriteChangerAudioSource;
    public SpriteRenderer spriteRenderer; // ������Ʈ�� ��������Ʈ ������
    public Sprite[] sprites; // ������ ��������Ʈ �迭

    private void OnEnable()
    {
        StartCoroutine(ChangeSpriteGradually()); // ������Ʈ�� Ȱ��ȭ�� �� �ڵ� ����
    }

    private IEnumerator ChangeSpriteGradually()
    {
        float elapsedTime = 0f; // �� ���� �ð�
        float interval = 0.1f;  // �ʱ� ���� ����
        int spriteIndex = Random.Range(0, sprites.Length);

        spriteChangerAudioSource.PlayOneShot(spriteChangerAudioSource.clip);

        while (elapsedTime < 2f) // 2�� ���� ����
        {
            spriteRenderer.sprite = sprites[spriteIndex]; // ��������Ʈ ����
            spriteIndex = (spriteIndex + 1) % sprites.Length; // ���� ��������Ʈ�� ����

            yield return new WaitForSeconds(interval);

            elapsedTime += interval;
            interval += 0.05f; // ���� ���� (0.1 �� 0.15 �� 0.2 �� ...)
        }

        // ���⼭ spriteIndex�� "���� �ε���" ������
        int lastSpriteIndex = (spriteIndex - 1 + sprites.Length) % sprites.Length;

        // ���������� �ٲ� ��������Ʈ ���� 1��
        spriteRenderer.sprite = sprites[lastSpriteIndex];
        yield return new WaitForSeconds(1f);

        // �θ𿡰� ������ ��������Ʈ ����
        FarmingDoor parent = transform.parent?.GetComponent<FarmingDoor>();
        if (parent != null)
        {
            parent.ReceiveDropIndex(lastSpriteIndex);
        }

        gameObject.SetActive(false); // ��Ȱ��ȭ
    }
}
