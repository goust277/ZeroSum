using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class TutorialBattleKey : MonoBehaviour
{
    [SerializeField] private GameObject parring;
    [SerializeField] private GameObject down;
    [SerializeField] private GameObject shooting;

    private int currentStep = 0; // X, LeftControl, C ¼ø¼­
    private bool isDone = false;

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        if (!isDone)
        {
            ScanInputValue();
        }
    }

    private void ScanInputValue()
    {
        switch (currentStep)
        {
            case 0: // X
                if (Input.GetKeyDown(KeyCode.X))
                {
                    currentStep++;
                    UpdateUI();
                }
                break;

            case 1: // LeftControl
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    currentStep++;
                    UpdateUI();
                }
                break;

            case 2: // CÅ°
                if (Input.GetKeyDown(KeyCode.C))
                {
                    currentStep++;
                    FinishSequence();
                }
                break;
        }
    }


    private void UpdateUI()
    {
        parring.SetActive(currentStep == 0);
        down.SetActive(currentStep == 1);
        shooting.SetActive(currentStep == 2);
    }

    private void FinishSequence()
    {
        isDone = true;

        parring.SetActive(false);
        down.SetActive(false);
        shooting.SetActive(false);
    }
}
