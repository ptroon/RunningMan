using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OptionManager : MonoBehaviour
{
    public TMP_Dropdown tdrop;
    private DDOLGameManager game;
    private GroundTileManager ground;

    public TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        // point to the cenrtral game object
        game = Object.FindObjectOfType<DDOLGameManager>();
        ground = Object.FindObjectOfType<GroundTileManager>();
        LoadDropdown();
    }

    public void LoadDropdown ()
    {
        tdrop.options.Clear();
        foreach (string t in System.Enum.GetNames(typeof(GroundTileManager.ETileComplexity)))
        {
            tdrop.options.Add(new TMP_Dropdown.OptionData() { text = t });
        }

        SetDifficulty(1, "ShouldBeAnEnum..");
        tdrop.value = game.gameDifficulty[0].optionValue; 
        tdrop.RefreshShownValue();
    }

    // loads the List from the dropdown currently chosen item
    public void GetDifficulty()
    {
        game.gameDifficulty.Clear(); // clear the list
        DDOLGameManager.Choices choices = new DDOLGameManager.Choices(tdrop.value, tdrop.options[tdrop.value].text);
        game.gameDifficulty.Add(choices);
    }

    // loads the List from the provided items
    public void SetDifficulty (int optionValue, string optionName)
    {
        game.gameDifficulty.Clear(); // clear the list
        DDOLGameManager.Choices choices = new DDOLGameManager.Choices(optionValue, optionName);
        game.gameDifficulty.Add(choices);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
