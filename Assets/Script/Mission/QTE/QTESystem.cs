using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QTESystem : MonoBehaviour
{
    [Header("UI Settings")]
    public GameObject keyUIPrefab; // 키 UI 프리팹
    public Transform keyUIParent; // UI가 배치될 부모 객체 (Horizontal Layout Group 권장)
    public Color defaultColor = Color.white;
    public Color successColor = Color.green;
    public Color failColor = Color.red;

    [Header("Time")]
    [SerializeField] private Image timeBar;
    [SerializeField] private Image timeBarSec;
    [SerializeField] private float maxTime;
    [SerializeField] private float clearTime;
    private float _curTime;
    private float curTime;

    [Header("Image")]
    [SerializeField] private GameObject completed;
    [SerializeField] private GameObject failed;
    private List<KeyCode> qteSequence = new List<KeyCode>();
    private List<TextMeshProUGUI> keyUIElements = new List<TextMeshProUGUI>();
    private int currentIndex = 0;
    private KeyCode[] possibleKeys = { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };

    public bool isClear;
    private bool isDone;
    public bool isFail;
    public bool isFailed;

    private void Update()
    {
        if (!isDone && !isFailed)
        {
            if (curTime > 0f)
            {
                curTime -= Time.deltaTime;

                timeBar.fillAmount = curTime / maxTime;
                timeBarSec.fillAmount = curTime / maxTime;
            }

            else if (curTime <= 0f)
            {
                isFailed = true;
            }

            if (currentIndex < qteSequence.Count)
            {
                if (Input.GetKeyDown(qteSequence[currentIndex]))
                {
                    // 올바른 키 입력 시
                    keyUIElements[currentIndex].color = successColor;
                    currentIndex++;

                    if (currentIndex == qteSequence.Count)
                    {
                        isDone = true;

                    }
                }
                else if (Input.anyKeyDown)
                {
                    // 잘못된 키 입력 시
                    foreach (KeyCode key in possibleKeys)
                    {
                        if (Input.GetKeyDown(key) && key != qteSequence[currentIndex])
                        {
                            keyUIElements[currentIndex].color = failColor;

                            isFailed = true;
                        }
                    }
                }
            }
        }

        if (isDone || isFailed)
        {
            if (_curTime < clearTime)
            {
                _curTime += Time.deltaTime;
                if (_curTime >= clearTime / 10)
                {
                    if (isFailed)
                    {
                        failed.SetActive(true);
                    }
                    if (isDone)
                    {
                        completed.SetActive(true);
                    }
                }
            }
            else if (_curTime >= clearTime)
            {
                if (isFailed)
                {
                    IsFailed();
                }
                if (isDone) 
                {
                    IsClear();
                }
            }
        }
    }
    private void OnEnable()
    {
        GenerateQTESequence();
        CreateSequenceUI();
        curTime = maxTime;
        isFail = false;
        isDone = false;
        _curTime = 0f;
    }

    private void IsFailed()
    {
        Debug.Log("QTE 실패!");

        gameObject.SetActive(false);
        failed.SetActive(false);
        isFail = true;
    }

    private void IsClear()
    {
        Debug.Log("QTE 성공!");

        gameObject.SetActive(false);
        completed.SetActive(false);
        isClear = true;
    }

    private void GenerateQTESequence()
    {
        qteSequence.Clear();
        for (int i = 0; i < 5; i++)
        {
            int rand = Random.Range(0, possibleKeys.Length);
            qteSequence.Add(possibleKeys[rand]);
        }
    }

    private void CreateSequenceUI()
    {
        // 기존 UI 삭제
        foreach (Transform child in keyUIParent)
        {
            Destroy(child.gameObject);
        }
        keyUIElements.Clear();
        currentIndex = 0;
        isClear = false;
        isFailed = false;
        // 새 UI 생성
        foreach (KeyCode key in qteSequence)
        {
            GameObject uiObj = Instantiate(keyUIPrefab, keyUIParent);
            TextMeshProUGUI textComp = uiObj.GetComponent<TextMeshProUGUI>();
            textComp.text = key.ToString();
            textComp.color = defaultColor;
            keyUIElements.Add(textComp);
        }
    }
}
