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

    #region 제이슨 출력창
    [SerializeField] protected TextMeshProUGUI nameTXT; //prtivate
    [SerializeField] protected TextMeshProUGUI desTXT; //prtivate
    #endregion

    public List<Image> portraits; //대화창에 띄울 초상화

    private bool isInteracting = false; // 상호작용 여부 체크
    private bool isConversation = false;

    private int secneID = 0;
    [SerializeField] private GameObject conversationUI; // 대화표시 UI
    [SerializeField] private GameObject interactPrompt; // 상호작용 키 표시 UI


    void Start()
    {
        LoadDialogueData("Dialog.json");

        // SecneID 값이 n(=0) 인 Dialog만 가져오기
        requiredScenes = GetDialogBySecneID(secneID);
        conversationUI.SetActive(false);
        interactPrompt.SetActive(false);

    }
    // JSON 파일을 불러와 파싱
    private void LoadDialogueData(string filename)
    {
        string Path = Application.dataPath + "/Resources/Json/" + filename;
        string jsonData = File.ReadAllText(Path);
        dialogueRoot = JsonConvert.DeserializeObject<DialogueRoot>(jsonData);
    }

    // 특정 SecneID의 Dialog 반환
    private List<DialogData> GetDialogBySecneID(int secneID)
    {
        //dialogueRoot의 Secnes에서 SecneID가 파라미터 secneID인 것을 찾아서 scene 를 저장..
        SecneData secne = dialogueRoot.Secnes.Find(scene => scene.SecneID == secneID);
        //secne이 널이아니면 다이어로그 반환 널이면 널 반환
        return secne?.Dialog;
    }

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

        conversationUI.SetActive(false);
        //필요한 대화 다했으니 리스트 비워줌
        requiredScenes.Clear();
        portraits.Clear();
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

    // 상호작용을 위한 메소드
    public void EnableInteraction()
    {

        isInteracting = true;
        interactPrompt.SetActive(true); // 상호작용 프롬프트 UI 활성화

    }

    public void DisableInteraction()
    {
        isInteracting = false;
        interactPrompt.SetActive(false); // 상호작용 프롬프트 UI 비활성화
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started && isInteracting && !isConversation) // Action이 수행된 경우
        {
            StartConversation();
        }
    }

    // 대화 시작
    private void StartConversation()
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

}
