using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject deathPanel = null;
    public Player player { get; set; }
    public static UIManager instance;
    public bool uiIsOpen = false;
    public Chat chat;
    public List<ActionButton> buttons = new List<ActionButton>();
    [SerializeField]
    GameObject spellBook = null;
    [SerializeField]
    SkillSlot skillPrefab = null;
    // [SerializeField]
    public Transform content = null;
    List<GameObject> toDeleteSkills = new List<GameObject>();
    ActionController controller;
    bool initialized = false;
    [SerializeField]
    DisableOverTime banner = null;
    // [SerializeField]
    // PlayerPanel playerPanelPrefab = null;
    // PlayerPanel playerPanel;
    // [SerializeField]
    Slider hpBar = null, manaBar = null;
    [SerializeField]
    Text level = null;
    //photo
    // [SerializeField]
    // PopUpBase popUpBase = null;
    GameObject currentPopUp;
    // [SerializeField]
    // BuffSlot buffSlot = null;
    // [SerializeField]
    // Transform grid = null;
    // [SerializeField]
    // TalentBook talentBook = null;
    [SerializeField]
    Text manaText = null, hpText = null;
    // [SerializeField]
    // DisableOverTime drop_banner = null;
    // [SerializeField]
    // DropPanel dropPrefab = null;
    // DropPanel dropPanel { get; set;}

    private void Awake() 
    {
        instance = this;    
    }

    public void SetActions(ActionController controller)
    {
        this.controller = controller;
        for(int i=0; i < buttons.Count; i++)
        {
            if (i < controller.actions.Count)
            {
                buttons[i].SetUpButton(controller, controller.actions[i]);
                controller.actions[i].button = buttons[i];
            }
        }
        initialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!initialized) return;

        foreach (var item in buttons)
        {
            item.FadeCheck();
        }
    }

    public void Respawn()
    {
        deathPanel.SetActive(false);
        player.Respawn();
    }

    public void SpellBook()
    {
        spellBook.SetActive(true);
        List<Skill> allSkills = controller.inventory.playerSKills;
        foreach (var s in allSkills)
        {
            SkillSlot slot = Instantiate(skillPrefab, content);
            slot.Init(s);
            toDeleteSkills.Add(slot.gameObject);
        }
    }

    public void CloseBook()
    {
        foreach (var item in toDeleteSkills)
        {
            Destroy(item);
        }
        toDeleteSkills.Clear();
        spellBook.SetActive(false);
    }

    public void SaveGame()
    {
        var inventory = controller.inventory;
        var data = player.data;
        data.stat = player.stats;
        RecordString<Equip>(ref data.equip, inventory.AllEquip());
        SaveManager.SaveData<SaveData>(data.characterName, data);
        ShowBanner();
    }

    public void ShowBanner(string message = "gg", float lifetime = 1)
    {
        banner.gameObject.SetActive(true);
        banner.Init(message, lifetime);
    }

    void RecordString<T>(ref List<string>list, List<T>template)where T: ScriptableObject
    {
        list.Clear();
        foreach (var item in template)
        {
            if (item != null)
                list.Add(item.name);
        }
    }
}
