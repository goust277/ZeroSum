using UnityEngine;
using TMPro;  // �ؽ�Ʈ ��¿�

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private DefMission defMission; // �ܺ� Ŭ���� ����
    [SerializeField] private TextMeshProUGUI scoreText; // UI �ؽ�Ʈ
    [SerializeField] private float maxTime = 30f; // �ִ� �ð� (30��)

    private bool isLocked = false;
    private float elapsedTime = 0f;
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
            // defMission.curMissionTime�� ���� �ð��̶�� ����
            float timeLeft = maxTime - defMission.curMissionTime;
            timeLeft = Mathf.Clamp(timeLeft, 0f, maxTime); // 0���� �۾����� �ʰ�

            scoreText.text = Mathf.RoundToInt(timeLeft).ToString();
        }
    }
}
