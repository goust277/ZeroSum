using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.UI;
using System.Reflection;


public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; private set; }


    public int[] activeWeapons = new int[2]; // ���� �������� ������ ID �迭
    [SerializeField] private int currentActiveLayer; // 1 Ȥ�� 2
    [SerializeField] private List<int> acquiredWeaponIds; // �÷��̾ ���� ���� ����Ʈ
    private List<Weapon> allWeapons = new List<Weapon>(); // ��� Ĩ�� ����Ʈ

    public RuntimeAnimatorController originalAnimatorController;
    private AnimatorOverrideController currentOverrideController;
    private Animator animator;

    [SerializeField] private GameObject[] activeWeaponObject = new GameObject[2];


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ�ص� ����
        }
        else
        {
            Destroy(gameObject);
        }

        LoadData("Ver00/Dataset/Weapons.json"); // ���� ������ �ε�

    }

    private void Start()
    {
        animator = GameObject.FindWithTag("Player")?.GetComponent<Animator>();
        currentActiveLayer = 1;
    }

    private void LoadData(string path)
    {
        TextAsset weaponJson = Resources.Load<TextAsset>("Json/Ver00/Dataset/Weapons");

        //string basePath = Application.dataPath + "/Resources/Json/";
        //string fullPath = basePath + path;

        //string jsonData = File.ReadAllText(fullPath);
        string jsonData = weaponJson.text;


        // T�� ������ �迭�� ��ȯ
        WeaponsArray items = JsonConvert.DeserializeObject<WeaponsArray>(jsonData);

        // items ��ü���� Items��� ������Ƽ�� ã�� �� ���� ������
        //�� ���� IEnumerable<T> Ÿ������ ��ȯ ������ ��� �� ��Ҹ� ��ȸ�ϸ� item���� ���
        foreach (var item in items.Items)
        {
            allWeapons.Add(item);
        }
    }

    private void AddAnimationOverride(List<KeyValuePair<AnimationClip, AnimationClip>> overrides, string key, string animationName, int id)
    {
        string resourcePath = $"Animations/Weapon{id}/{animationName}";

        AnimationClip newClip = Resources.Load<AnimationClip>(resourcePath);
        if (newClip != null)
        {
            overrides.Add(new KeyValuePair<AnimationClip, AnimationClip>(currentOverrideController[key], newClip));
        }
        else
        {
            Debug.Log($"Failed to load animation from path: {resourcePath}");
        }
    }

    private void SetWeaponAnimations(int switchIdx, int id)
    {
        if (id < 0 || id >= allWeapons.Count)
        {
            Debug.Log("Invalid weapon ID: " + id);
            return;
        }

        if (originalAnimatorController == null)
        {
            Debug.Log("Original Animator Controller is null.");
            return;
        }

        // AnimatorOverrideController�� ����
        if (currentOverrideController == null || currentOverrideController.runtimeAnimatorController != originalAnimatorController)
        {
            // �ִϸ��̼� �������̵� ��Ʈ�ѷ��� ����
            currentOverrideController = new AnimatorOverrideController(originalAnimatorController);
            animator.runtimeAnimatorController = currentOverrideController;
        }

        // �ش� ������ �ִϸ��̼� ����
        Animations setWeapon = allWeapons[id].animations;
        List<KeyValuePair<AnimationClip, AnimationClip>> overrides = new();

        string layerPrefix = $"Slot{switchIdx}";

        // �� �ִϸ��̼��� �ε�
        AddAnimationOverride(overrides, $"{layerPrefix}Idle", setWeapon.idle, id);
        AddAnimationOverride(overrides, $"{layerPrefix}Combo1", setWeapon.combo1, id);
        AddAnimationOverride(overrides, $"{layerPrefix}Combo2", setWeapon.combo2, id);
        AddAnimationOverride(overrides, $"{layerPrefix}Combo3", setWeapon.combo3, id);
        AddAnimationOverride(overrides, $"{layerPrefix}UpSideAttack", setWeapon.upSideAttack, id);
        AddAnimationOverride(overrides, $"{layerPrefix}DownSideAttack", setWeapon.downSideAttack, id);
        AddAnimationOverride(overrides, $"{layerPrefix}ComboEnd", setWeapon.comboEnd, id);

        // �ִϸ��̼��� �����
        foreach (var pair in overrides)
        {
            if (pair.Value != null)
                currentOverrideController[pair.Key] = pair.Value;
        }

        //Debug.Log($"SetWeaponAnimations applied for WeaponSlot{switchIdx} with weapon ID {id}");
    }


    public void ToggleWeaponLayer(int layerIndex, bool activate)
    {
        // ���̾� Ȱ��ȭ ���� ����
        float weight = activate ? 1f : 0f;
        animator.SetLayerWeight(layerIndex, weight);
    }

    public void SwapWeapon()
    {
        (activeWeapons[1], activeWeapons[0]) = (activeWeapons[0], activeWeapons[1]);

        // �� ������Ʈ�� Image ������Ʈ�� ��������Ʈ�� ���� ��ȯ�Ϸ���
        // �� ������Ʈ���� Image ������Ʈ�� ������
        Image img1 = activeWeaponObject[0].GetComponent<Image>();
        Image img2 = activeWeaponObject[1].GetComponent<Image>();

        // �ӽ� ������ �� ��������Ʈ�� ��ȯ
        (img2.sprite, img1.sprite) = (img1.sprite, img2.sprite);

        activeWeaponObject[0].GetComponent<AdjustSpriteSize>().SetSprite();
        activeWeaponObject[1].GetComponent<AdjustSpriteSize>().SetSprite();
        Debug.Log("SwapWeapon ȣ��");


        if (currentActiveLayer == 1)
        {
            ToggleWeaponLayer(1, false); // WeaponSlot1 ���̾ ��Ȱ��ȭ
            ToggleWeaponLayer(2, true);  // WeaponSlot2 ���̾ Ȱ��ȭ
            Debug.Log("WeaponSlot2 Ȱ��ȭ");

            currentActiveLayer = 2;
        }
        else if (currentActiveLayer == 2) {
            ToggleWeaponLayer(1, true);  // WeaponSlot1 ���̾ Ȱ��ȭ
            ToggleWeaponLayer(2, false); // WeaponSlot2 ���̾ ��Ȱ��ȭ
            Debug.Log("WeaponSlot1 Ȱ��ȭ");

            currentActiveLayer = 1;
        }
        else
        {
            Debug.Log("Invalid currentActiveLayer value ����������������");
        }
    }

    // ������ ��ü �޼ҵ�
    public void SwitchActiveItem(int switchIdx, int id)
    {
        //switchIdx = Ȱ��ȭ ���⽽��, id = �ٲ� ���� ���̵�
        //if (acquiredWeaponIds.Contains(id))
        //if (acquiredWeaponIds.Contains(id))
        //{
        //    activeWeapons[switchIdx] = id;
        //}
        //else
        //{
        //    return;
        //}

        //�̹��� ������Ʈ 
        activeWeapons[switchIdx] = id;
        Weapon wp = WeaponManager.Instance.GetActiveItem(id);
        Sprite sprite = Resources.Load<Sprite>(wp.weaponIcons);
        activeWeaponObject[switchIdx].GetComponent<Image>().sprite = sprite;
        activeWeaponObject[switchIdx].GetComponent<AdjustSpriteSize>().SetSprite();

        //�ִϸ��̼� ������Ʈ
        SetWeaponAnimations(switchIdx+1, id);
    }

    //���� ������ �߰�
    public void AddAcquiredItemIds(int AddItemid)
    {
        if (!acquiredWeaponIds.Contains(AddItemid))
        {
            acquiredWeaponIds.Add(AddItemid);
        }
    }

    //���� ��忡 ����ִ� �����۵� ���� ����� ���� ���� ������
    public Weapon GetActiveItem(int slot)
    {
        //if (acquiredItemIds.Contains(slot))
        //{
        //      return acquiredItemIds[slot];
        if( slot == -1 ) return null;

        return allWeapons[slot];
        //}
        //return null;
    }

    public List<Weapon> GetAcquiredWeapons()
    {
        //������ �ִ� ���� �迭 �ݺ��� �ѹ��� ������
        //���̵� �´� �������� ����Ʈ�� �����ؼ� ��ȯ

        return allWeapons;
    }

}