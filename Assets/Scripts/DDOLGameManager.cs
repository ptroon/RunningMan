using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DDOLGameManager : MonoBehaviour
{

    // current difficulty of the game from the meny option.
    public struct Choices
    {
        public int optionValue;
        public string optionName;

        public Choices (int _optionValue, string _optionName)
        {
            this.optionValue = _optionValue;
            this.optionName = _optionName;
        }
    }

    public List<Choices> gameDifficulty;

    public int sessionBestScore = 0;     // best score this session
    public int currentScore = 0;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        gameDifficulty = new List<Choices>();

        Debug.Log("Initialising _Preload..");
    }

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene("MenuScene"); // load the menu scene as the first scene
    }

    public int UpdateScore (float timer)
    {
        currentScore = (int) timer * (gameDifficulty[0].optionValue + 1);
        return currentScore;
        // return the score multiplied by the difficulty
    }


    // Update is called once per frame
    void Update()
    {
        
    }


}
