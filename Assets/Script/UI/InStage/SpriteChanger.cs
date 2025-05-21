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
        int spriteIndex = Random.Range(0, 4);

        spriteChangerAudioSource.PlayOneShot(spriteChangerAudioSource.clip);

        while (elapsedTime < 2f) // 2�� ���� ����
        {
            spriteRenderer.sprite = sprites[spriteIndex]; // ��������Ʈ ����
            spriteIndex = (spriteIndex + 1) % sprites.Length; // ���� ��������Ʈ�� ����

            yield return new WaitForSeconds(interval); // ���� ���ݸ�ŭ ���

            elapsedTime += interval; // ��� �ð� �߰�
            interval += 0.05f; // ���� ���� (0.1 �� 0.15 �� 0.2 �� ...)
        }

        // ���������� �ٲ� ��������Ʈ���� 1�� ���� ����
        yield return new WaitForSeconds(1f);

        // �θ𿡰� ������ ��������Ʈ ����
        FarmingDoor parent = transform.parent?.GetComponent<FarmingDoor>();
        if (parent != null)
        {
            if(spriteIndex == 0)
            {
                spriteIndex = 4;
            }
            parent.ReceiveDropIndex(spriteIndex-1);
        }

        gameObject.SetActive(false); // ��Ȱ��ȭ
    }
}
