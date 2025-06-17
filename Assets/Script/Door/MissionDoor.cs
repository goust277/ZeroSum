using UnityEngine;

public class MissionDoor : BaseInteractable
{
    [SerializeField] private MissionDoorManager doorManager;
    [SerializeField] private GameObject monsterDoor;

    [SerializeField] private Mission mission;

    [Header("Time")]
    [SerializeField] private float coolTime = 2f;
    private float curCoolTime = 0f;

    [Header("Animator")]
    [SerializeField] private Animator animator;
    private bool isClear;

    private bool isCount = false;

    public override void Exe()
    {
        animator.SetTrigger("Open");
        mission.OnMission();
    }

    private void Update()
    {
        if (mission.isFailed)
        {
            if(curCoolTime >= coolTime)
            {
                mission.isFailed = false;
                curCoolTime = 0f;

                if (animator.GetBool("Fail"))
                    animator.SetBool("Fail", false);

                return;
            }

            if (!animator.GetBool("Fail"))
                animator.SetBool("Fail", true);

            curCoolTime += Time.deltaTime;
        }

        if (mission.isClear)
        {
            animator.SetTrigger("Clear");

            if (!isCount)
            {
                doorManager.clearMission++;
                isCount = true;
            }
            
        }
    }

    public void ActiveMonsterDoor()
    {
        if (monsterDoor != null)
        {
            monsterDoor.SetActive(true);
        }
        gameObject.SetActive(false);
    }
}
