using UnityEngine;
using TMPro;  // 텍스트 출력용

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private DefMission defMission; // 외부 클래스 참조
    [SerializeField] private TextMeshProUGUI scoreText; // UI 텍스트
    [SerializeField] private float maxTime = 30f; // 최대 시간 (30초)

    private bool isLocked = false;
    private float elapsedTime = 0f;
    void Update()
    {
        if (isLocked) return;

        if (elapsedTime >= 30f)
        {
            isLocked = true; // 30초가 지나면 멈춤
            gameObject.SetActive(false);
            return;
        }

        if (defMission != null && scoreText != null)
        {
            // defMission.curMissionTime이 누적 시간이라고 가정
            float timeLeft = maxTime - defMission.curMissionTime;
            timeLeft = Mathf.Clamp(timeLeft, 0f, maxTime); // 0보다 작아지지 않게

            scoreText.text = Mathf.RoundToInt(timeLeft).ToString();
        }
    }
}
