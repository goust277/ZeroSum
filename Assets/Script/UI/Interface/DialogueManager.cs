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

    #region ���̽� ���â
    [SerializeField] protected TextMeshProUGUI nameTXT; //prtivate
    [SerializeField] protected TextMeshProUGUI desTXT; //prtivate
    #endregion

    public List<Image> portraits; //��ȭâ�� ��� �ʻ�ȭ

    private bool isInteracting = false; // ��ȣ�ۿ� ���� üũ
    private bool isConversation = false;

    private int secneID = 0;
    [SerializeField] private GameObject conversationUI; // ��ȭǥ�� UI
    [SerializeField] private GameObject interactPrompt; // ��ȣ�ۿ� Ű ǥ�� UI


    void Start()
    {
        LoadDialogueData("Dialog.json");

        // SecneID ���� n(=0) �� Dialog�� ��������
        requiredScenes = GetDialogBySecneID(secneID);
        conversationUI.SetActive(false);
        interactPrompt.SetActive(false);

    }
    // JSON ������ �ҷ��� �Ľ�
    private void LoadDialogueData(string filename)
    {
        string Path = Application.dataPath + "/Resources/Json/" + filename;
        string jsonData = File.ReadAllText(Path);
        dialogueRoot = JsonConvert.DeserializeObject<DialogueRoot>(jsonData);
    }

    // Ư�� SecneID�� Dialog ��ȯ
    private List<DialogData> GetDialogBySecneID(int secneID)
    {
        //dialogueRoot�� Secnes���� SecneID�� �Ķ���� secneID�� ���� ã�Ƽ� scene �� ����..
        SecneData secne = dialogueRoot.Secnes.Find(scene => scene.SecneID == secneID);
        //secne�� ���̾ƴϸ� ���̾�α� ��ȯ ���̸� �� ��ȯ
        return secne?.Dialog;
    }

    IEnumerator TypeWriter()
    {
        foreach (var dialog in requiredScenes)
        {
            nameTXT.text = dialog.name;
            UpdatePortraits(dialog.pos);

            //��� Ÿ���� �ִ�
            foreach (var line in dialog.log)
            {   //desTXT.text = line;
                desTXT.text = "";
                for (int index = 0; index < line.Length; index++)
                {
                    desTXT.text += line[index].ToString();
                    yield return new WaitForSeconds(0.05f); //Ÿ���� �ӵ� = 1�ڴ� 0.05��
                }
                yield return new WaitUntil(() => Input.GetKey(KeyCode.E));
            }
        }

        conversationUI.SetActive(false);
        //�ʿ��� ��ȭ �������� ����Ʈ �����
        requiredScenes.Clear();
        portraits.Clear();
    }

    //���ϴ� ��� �ٲ�� �ʻ�ȭ ����ٲ������
    private void UpdatePortraits(int speakingPortraits)
    {
        for (int i = 0; i < portraits.Count; i++)
        {
            //pos�� ���� ������� ���ʺ��� 0~n��. 
            // �ش���ġ�� ĳ���Ͱ� ���ϰ� ������ ���� ��(1,1,1,1)����ϰ� �ƴϸ� ��Ӱ� ó����
            portraits[i].color = (i == speakingPortraits) ? Color.white : new Color(0.3f, 0.3f, 0.3f, 1);

        }
    }

    // ��ȣ�ۿ��� ���� �޼ҵ�
    public void EnableInteraction()
    {

        isInteracting = true;
        interactPrompt.SetActive(true); // ��ȣ�ۿ� ������Ʈ UI Ȱ��ȭ

    }

    public void DisableInteraction()
    {
        isInteracting = false;
        interactPrompt.SetActive(false); // ��ȣ�ۿ� ������Ʈ UI ��Ȱ��ȭ
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started && isInteracting && !isConversation) // Action�� ����� ���
        {
            StartConversation();
        }
    }

    // ��ȭ ����
    private void StartConversation()
    {
        isConversation = true;
        conversationUI.SetActive(true);
        StartCoroutine(TypeWriter());
    }

    // ��ȭ ����
    private void EndConversation()
    {
        conversationUI.SetActive(false);
        requiredScenes.Clear();
        portraits.Clear();
        isConversation = false;
    }

}
