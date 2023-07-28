using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    void Start()
    {
        var files = Directory.GetFiles(location).Where(x => !x.Contains(".meta")).ToArray();
        if (files.Length < 1)
        {

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
}
