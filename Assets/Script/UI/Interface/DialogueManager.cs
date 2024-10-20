using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class DialogueManager : MonoBehaviour
{
    private DialogueRoot dialogueRoot;
    private List<DialogData> requiredScenes;

    #region 제이슨 출력및 전처리 UI
    [SerializeField] protected TextMeshProUGUI nameTXT; //prtivate
    [SerializeField] protected TextMeshProUGUI desTXT; //prtivate
    [SerializeField] private GameObject conversationUI; // 대화표시 UI
    [SerializeField] private GameObject interactPrompt; // 상호작용 키 표시 UI
    #endregion

    public bool isConversation = false; // 대화창이 지금 떠있는지 여부 

    [SerializeField] private int currentEventID = 0;
    public List<Image> portraits; //대화창에 띄울 초상화


    void Start()
    {
        LoadDialogueData("Chap0Dialog.json");

        // SecneID 값이 n(=0) 인 Dialog만 가져오기

        requiredScenes = GetDialogBySecneID(currentEventID);
        conversationUI.SetActive(false);
        interactPrompt.SetActive(false);
    }


    #region 제이슨 파싱
    // JSON 파일을 불러와 파싱
    private void LoadDialogueData(string filename)
    {
        string Path = Application.dataPath + "/Resources/Json/Ver00/Dialog/" + filename;
        string jsonData = File.ReadAllText(Path);
        dialogueRoot = JsonConvert.DeserializeObject<DialogueRoot>(jsonData);
    }

    // 조건을 모두 만족하는지 확인하는 함수
    private bool CheckEventConditions(EventConditions eventConditions, Dictionary<string, bool> currentEventConditions)
    {
        // 필요 조건들을 모두 확인
        foreach (string condition in eventConditions.needEventConditions)
        {
            // currentEventConditions에 없는 조건이거나 값이 false면 조건을 만족하지 않음
            if (!currentEventConditions.ContainsKey(condition) || !currentEventConditions[condition])
            {
                return false;
            }
        }

        // 모든 조건이 만족되면 true 반환
        return true;
    }

    // 특정 SecneID의 Dialog 반환
    private List<DialogData> GetDialogBySecneID(int secneID)
    {
        //dialogueRoot의 Secnes에서 SecneID가 파라미터 secneID인 것을 찾아서 scene 를 저장..
        SecneData secne = dialogueRoot.Secnes.Find(scene => scene.SecneID == secneID);
        //secne이 널이아니면 다이어로그 반환 널이면 널 반환

        return secne?.dialog;
    }
    #endregion


    #region 대사 출력 및 UI 업데이트
    IEnumerator TypeWriter()
    {
        foreach (var dialog in requiredScenes)
        {
            nameTXT.text = dialog.name;
            UpdatePortraits(dialog.pos);

            //대사 타이핑 애니
            foreach (var line in dialog.log)
            {   //desTXT.text = line;
                desTXT.text = "";
                for (int index = 0; index < line.Length; index++)
                {
                    desTXT.text += line[index].ToString();
                    yield return new WaitForSeconds(0.05f); //타이핑 속도 = 1자당 0.05초
                }
                yield return new WaitUntil(() => Input.GetKey(KeyCode.E));
            }
        }

        EndConversation();
    }

    //말하는 사람 바뀌면 초상화 색깔바꿔줘야함
    private void UpdatePortraits(int speakingPortraits)
    {
        for (int i = 0; i < portraits.Count; i++)
        {
            //pos값 보고 순서대로 왼쪽부터 0~n임. 
            // 해당위치의 캐릭터가 말하고 있으면 원래 값(1,1,1,1)출력하고 아니면 어둡게 처리함
            portraits[i].color = (i == speakingPortraits) ? Color.white : new Color(0.3f, 0.3f, 0.3f, 1);
        }
    }

    // 대화 시작
    public void StartConversation()
    {
        isConversation = true;
        conversationUI.SetActive(true);
        StartCoroutine(TypeWriter());
    }

    // 대화 종료
    private void EndConversation()
    {
        conversationUI.SetActive(false);
        requiredScenes.Clear();
        portraits.Clear();
        isConversation = false;
    }
    #endregion
}
