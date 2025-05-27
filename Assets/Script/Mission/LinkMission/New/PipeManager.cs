using UnityEngine;
using UnityEngine.UI;

public class PipeManager : Mission
{
    [SerializeField] private Button[] btns;
    [SerializeField] private PipeRotate[] pipes;
    [SerializeField] private ChangeLink[] changeLinks;
    [SerializeField] private GameObject mission;
    [SerializeField] private float clearTime;

    [SerializeField] private GameObject completed;
    private float _curTime;

    private bool isChange = false;
    private int answerPipe = 0;
    void Start()
    {
        _curTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (answerPipe == pipes.Length )
        {
            if (!isClear)
            {
                ClearMission();

                //Debug.Log("Å¬¸®¾î");
            }
        }
        else if (answerPipe != pipes.Length)
        {
            if (isClear)
            {
                isClear = false;
            }
        }
    }

    private void ClearMission()
    {
        foreach (var btn in btns)
        {
            btn.enabled = false;
        }

        if(!isChange)
        {
            foreach (var change in changeLinks)
            {
                change.ChangeColor();
            }
            isChange = true;
        }

        if (_curTime < clearTime)
        {
            _curTime += Time.deltaTime;
            if (_curTime >= clearTime / 10)
            {
                completed.SetActive(true);
            }
        }
        else if (_curTime >= clearTime)
        {
            mission.SetActive(false);
            player.SetActive(true);
            input.SetActive(true);
            completed.SetActive(false);
            isClear = true;
        }
    }


    private void UpAnwerPipe()
    {
        answerPipe++;
    }

    private void DownAnwerPipe()
    { 
        answerPipe--;
    }

    private void OnEnable()
    {
        for (int i = 0; i < pipes.Length; i++)
        {
            pipes[i].answerUp += UpAnwerPipe;
            pipes[i].answerDown += DownAnwerPipe;
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < pipes.Length; i++)
        {
            pipes[i].answerUp -= UpAnwerPipe;
            pipes[i].answerDown -= DownAnwerPipe;
        }
    }

    public override void OnMission()
    {
        mission.SetActive(true);
        player.SetActive(false);
        input.SetActive(false);
    }
}
