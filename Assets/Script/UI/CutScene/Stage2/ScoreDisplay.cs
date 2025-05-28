using UnityEngine;
using TMPro;  // �ؽ�Ʈ ��¿�

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private DefMission defMission; // �ܺ� Ŭ���� ����
    [SerializeField] private TextMeshProUGUI scoreText; // UI �ؽ�Ʈ

    private bool isLocked = false;
    private float elapsedTime = 30.0f;
    void Update()
    {
        if (isLocked) return;

        if (elapsedTime >= 30f)
        {
            isLocked = true; // 30�ʰ� ������ ����
            gameObject.SetActive(false);
            return;
        }

        if (defMission != null && scoreText != null)
        {
            elapsedTime = elapsedTime - defMission.curMissionTime;
            scoreText.text = Mathf.RoundToInt(elapsedTime).ToString();
        }
    }
}
