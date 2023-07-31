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

    void Start()
    {
        var files = Directory.GetFiles(location).Where(x => !x.Contains(".meta")).ToArray();
        dropDown.onValueChanged.AddListener(SelectCharacter);
        if (files.Length < 1)
        {
            startButton.SetActive(false);
        }
        else 
        {
            int id = 0;
            foreach (var f in files)
            {
                // Debug.Log(Path.GetFileName(f));
                string name = Path.GetFileName(f);
                SaveData data = SaveManager.LoadData<SaveData>(name);
                allData.Add(data);
                var button = Instantiate(charButton, charParent);
                button.Init(this, id, name);
                id ++;
                buttons.Add(button);
            }
            Select(0);
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
    }

    public void CreateSampleCharacter()
    {
        if (selectedData == null)
        {
            selectedData = new SaveData();
            string Name = string.IsNullOrEmpty(field.text) ? "Guest00" : field.text;
            selectedData.characterName = Name;
            selectedData.stat = GetStat(currentClass);
            SaveManager.SaveData<SaveData>(Name, selectedData);
        }
    }

    Stats GetStat(CharacterClass cla)
    {
        Stats newStat = new Stats();
        switch (cla)
        {
            case CharacterClass.warrior:
                break;
            case CharacterClass.knight:
                break;
            case CharacterClass.mercenary:
                break;
            case CharacterClass.magician:
                break;
            case CharacterClass.priest:
                break;
            case CharacterClass.assassin:
                break;
            case CharacterClass.hunter:
                break;
            case CharacterClass.warlock:
                break;
            case CharacterClass.sage:
                break;
        }

        return newStat;
    }

    public void SelectCharacter(int val)
    {
        currentClass = (CharacterClass) val;
    }
}
