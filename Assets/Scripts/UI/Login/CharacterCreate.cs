using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class CharacterCreate : MonoBehaviour
{
    const string location = "Assets/Resources/Data";
    public List<SaveData> allData = new List<SaveData>();
    [SerializeField]
    CharButton charButton = null;
    [SerializeField]
    Transform charParent = null;
    public static SaveData selectedData;
    public List<CharButton> buttons = new List<CharButton>();
    [SerializeField]
    CharacterStatsUI stats = null;
    [SerializeField]
    GameObject createCharacterPanel = null;
    [SerializeField]
    InputField field = null;
    [SerializeField]
    Dropdown dropDown = null;
    [SerializeField]
    GameObject startButton = null, createButton = null;
    [SerializeField]
    CharacterClass currentClass = CharacterClass.warrior;
    [SerializeField]
    int MaxPlayers = 5;
    string[] files;

    private void Start()
    {
        dropDown.onValueChanged.AddListener(SelectCharacter);
        selectedData = null;
        field.onSubmit.AddListener(delegate
        {
            selectedData.characterName = field.text;
            stats.SetUp(selectedData);
        }
        );
        //
        if(!Directory.Exists(location))
        {
            Directory.CreateDirectory(location);
            startButton.SetActive(false);
            selectedData = new SaveData();
            selectedData.stat = GetStat(CharacterClass.warrior);
            selectedData.characterName = "Guest";
            stats.SetUp(selectedData);
            return;
        }
        //
        files = Directory.GetFiles(location).Where(x=>!x.Contains(".meta")).ToArray();
        if(files.Length<1)
        {
            startButton.SetActive(false);
            selectedData = new SaveData();
            selectedData.stat = GetStat(CharacterClass.warrior);
            selectedData.characterName = "Guest";
        }
        else
        {
            int id = 0;
            foreach(var f in files)
            {
                string name = Path.GetFileName(f);
                SaveData data = SaveManager.LoadData<SaveData>(name);
                allData.Add(data);
                var button= Instantiate(charButton, charParent);
                button.Init(this, id,name);
                id++;
                buttons.Add(button);
            }
            Select(0);
        }
        if(files.Length>=MaxPlayers)
        {
            createButton.SetActive(false);
        }
    }

    public void Select(int ID)
    {
        foreach (var b in buttons)
        {
            b.icon.SetActive(false);
        }
        
        selectedData = allData[ID];
        buttons[ID].icon.SetActive(true);
        stats.SetUp(selectedData);
    }

    public void CreateCharacter()
    {
        createButton.SetActive(false);
        createCharacterPanel.SetActive(true);
        startButton.SetActive(true);
        stats.SetUp(selectedData);
    }

    // public void CreateSampleCharacter()
    // {
    //     if (selectedData == null)
    //     {
    //         selectedData = new SaveData();
    //         string Name = string.IsNullOrEmpty(field.text) ? "Guest00" : field.text;
    //         selectedData.characterName = Name;
    //         selectedData.stat = GetStat(currentClass);
    //         SaveManager.SaveData<SaveData>(Name, selectedData);
    //     }
    // }

    public static Stats GetStat(CharacterClass cla)
    {
        Stats newStat = new Stats();
        switch (cla)
        {
            case CharacterClass.warrior:
                newStat.Level = 1;
                newStat.HP = 10;
                newStat.Mana = 5;
                newStat.PhysicalAttack = 15;
                newStat.Wisdom = 5;
                newStat.MagicalAttack = 5;
                newStat.PhysicalDefense = 10;
                newStat.MagicalDefense = 5;
                newStat.Agility = 5;
                newStat.Aggro = 10;
                newStat.charClass = CharacterClass.warrior;
                break;

            case CharacterClass.knight:
                newStat.Level = 1;
                newStat.HP = 10;
                newStat.Mana = 5;
                newStat.PhysicalAttack = 10;
                newStat.Wisdom = 5;
                newStat.MagicalAttack = 5;
                newStat.PhysicalDefense = 15;
                newStat.MagicalDefense = 10;
                newStat.Agility = 5;
                newStat.Aggro = 10;
                newStat.charClass = CharacterClass.knight;
                break;

            case CharacterClass.mercenary:
                newStat.Level = 1;
                newStat.HP = 15;
                newStat.Mana = 5;
                newStat.PhysicalAttack = 20;
                newStat.Wisdom = 5;
                newStat.MagicalAttack = 5;
                newStat.PhysicalDefense = 10;
                newStat.MagicalDefense = 5;
                newStat.Agility = 5;
                newStat.Aggro = 10;
                newStat.charClass = CharacterClass.mercenary;
                break;

            case CharacterClass.magician:
                newStat.Level = 1;
                newStat.HP = 5;
                newStat.Mana = 15;
                newStat.PhysicalAttack = 5;
                newStat.Wisdom = 10;
                newStat.MagicalAttack = 15;
                newStat.PhysicalDefense = 5;
                newStat.MagicalDefense = 10;
                newStat.Agility = 5;
                newStat.Aggro = 5;
                newStat.charClass = CharacterClass.magician;
                break;

            case CharacterClass.priest:
                newStat.Level = 1;
                newStat.HP = 10;
                newStat.Mana = 15;
                newStat.PhysicalAttack = 5;
                newStat.Wisdom = 15;
                newStat.MagicalAttack = 10;
                newStat.PhysicalDefense = 5;
                newStat.MagicalDefense = 10;
                newStat.Agility = 5;
                newStat.Aggro = 5;
                newStat.charClass = CharacterClass.priest;
                break;

            case CharacterClass.assassin:
                newStat.Level = 1;
                newStat.HP = 10;
                newStat.Mana = 5;
                newStat.PhysicalAttack = 20;
                newStat.Wisdom = 5;
                newStat.MagicalAttack = 10;
                newStat.PhysicalDefense = 10;
                newStat.MagicalDefense = 5;
                newStat.Agility = 15;
                newStat.Aggro = 5;
                newStat.charClass = CharacterClass.assassin;
                break;

            case CharacterClass.hunter:
                newStat.Level = 1;
                newStat.HP = 10;
                newStat.Mana = 5;
                newStat.PhysicalAttack = 15;
                newStat.Wisdom = 5;
                newStat.MagicalAttack = 10;
                newStat.PhysicalDefense = 10;
                newStat.MagicalDefense = 5;
                newStat.Agility = 15;
                newStat.Aggro = 5;
                newStat.charClass = CharacterClass.hunter;
                break;

            case CharacterClass.warlock:
                newStat.Level = 1;
                newStat.HP = 5;
                newStat.Mana = 15;
                newStat.PhysicalAttack = 5;
                newStat.Wisdom = 15;
                newStat.MagicalAttack = 20;
                newStat.PhysicalDefense = 5;
                newStat.MagicalDefense = 10;
                newStat.Agility = 5;
                newStat.Aggro = 5;
                newStat.charClass = CharacterClass.warlock;
                break;

            case CharacterClass.sage:
                newStat.Level = 1;
                newStat.HP = 5;
                newStat.Mana = 20;
                newStat.PhysicalAttack = 5;
                newStat.Wisdom = 20;
                newStat.MagicalAttack = 15;
                newStat.PhysicalDefense = 5;
                newStat.MagicalDefense = 15;
                newStat.Agility = 5;
                newStat.Aggro = 5;
                newStat.charClass = CharacterClass.sage;
                break;

        }

        return newStat;
    }

    public void SelectCharacter(int val)
    {
        currentClass = (CharacterClass) val;
        Stats stat = GetStat(currentClass);
        stats.SetUp(stat);
        string name = field.text;
        stats.Name.text = "Name: " + name;
        selectedData.stat = stat;
        if (string.IsNullOrEmpty(name))
        {
            name = "Guest";
        }
        selectedData.characterName = name;
    }

    public void DeleteCharacter(int id)
    {
        var popup = Resources.Load<PopUpBase>("UI/PopUpBase");
        PopUpBase b = Instantiate(popup, transform.parent);
        b.transform.SetAsLastSibling();
        string title = "Delete this char?";
        b.Init(title, () => {DeleteData(id,b.gameObject);}, () => {Destroy(b.gameObject);});
    }

    void DeleteData(int id, GameObject popup)
    {
        Destroy(buttons[id].gameObject);
        Destroy(popup);
        File.Delete(files[id]);
        createButton.SetActive(true);
    }
}
