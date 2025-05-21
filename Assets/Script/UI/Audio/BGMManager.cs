using UnityEngine;


public class BGMManager : MonoBehaviour
{
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioClip normalBGM;
    [SerializeField] private AudioClip dangerBGM;

    private bool isDangerBGM = false;

    private void Start()
    {
        PlayNormalBGM();
    }

    private void Update()
    {
        int currentHP = Ver01_DungeonStatManager.Instance.GetCurrentHP();

        if (!isDangerBGM && currentHP <= 3)
        {
            PlayDangerBGM();
        }
        else if (isDangerBGM && currentHP > 3)
        {
            PlayNormalBGM(); // ���󺹱͵� �ʿ��ϴٸ�
        }
    }

    private void PlayNormalBGM()
    {
        if (bgmSource.clip == normalBGM) return;

        bgmSource.clip = normalBGM;
        bgmSource.loop = true;
        bgmSource.Play();
        isDangerBGM = false;
    }

    private void PlayDangerBGM()
    {
        if (bgmSource.clip == dangerBGM) return;

        bgmSource.clip = dangerBGM;
        bgmSource.loop = true;
        bgmSource.Play();
        isDangerBGM = true;
    }
}
