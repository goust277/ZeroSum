using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioClip normalBGM;
    [SerializeField] private AudioClip dangerBGM;

    private bool isDangerBGM = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
            PlayNormalBGM(); // 원상복귀도 필요하다면
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
