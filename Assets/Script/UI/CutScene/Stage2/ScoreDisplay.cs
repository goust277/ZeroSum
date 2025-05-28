using UnityEngine;
using TMPro;  // 텍스트 출력용

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private DefMission defMission; // 외부 클래스 참조
    [SerializeField] private TextMeshProUGUI scoreText; // UI 텍스트

    private bool isLocked = false;
    private float elapsedTime = 30.0f;
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
            elapsedTime = elapsedTime - defMission.curMissionTime;
            scoreText.text = Mathf.RoundToInt(elapsedTime).ToString();
        }
    }
}
