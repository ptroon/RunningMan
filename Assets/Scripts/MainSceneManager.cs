using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using zebaroo;

public class MainSceneManager : MonoBehaviour
{

    private GroundTileManager ground;
    private PlayerHealth health;
    private MenuHandler menuH;
    private DDOLGameManager game;

    public Canvas canvas;
    public GameObject pauseScreen;
    public TMPro.TMP_Text score;

    private float elapsedTime, initTime;

    private void Awake()
    {

        // init objects
        ground  = Object.FindObjectOfType<GroundTileManager>();
        health  = Object.FindObjectOfType<PlayerHealth>();
        menuH   = Object.FindObjectOfType<MenuHandler>();
        game    = Object.FindObjectOfType<DDOLGameManager>();

        initTime = Time.time;

        // run functions to draw / build screen
        // set game difficulty
        ground.SetComplexity((GroundTileManager.ETileComplexity) game.gameDifficulty[0].optionValue);
        
        // build & initialise
        ground.BuildPlatform();
        health.InitialisePlayer();
        health.DrawHeartPrefabs(canvas);

        pauseScreen.SetActive(false);
        Time.timeScale = 1f;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

     void Update()
    {
        if (Input.GetButtonDown("Cancel") || (Input.GetKeyDown(KeyCode.N) && Time.timeScale == 0))
        {
            menuH.ToggleScreen(pauseScreen);
        }

        if (Input.GetKeyDown(KeyCode.Y) && Time.timeScale == 0)
        {
            // go to main menu
            health.ResetGame();
            menuH.LoadScene("MenuScene");
        }

        if (health.livesLeft <= 0)
        {
            health.ResetGame();
            menuH.LoadScene("MenuScene");
        }

        elapsedTime = Time.time - initTime;
        score.text = "SCORE: " + game.UpdateScore(elapsedTime).ToString();

        ground.AppendTileChunk(); // checks to see if we need more tiles and appends them if so!

    }

    public void TogglePauseScreen ()
    {
        menuH.ToggleScreen(pauseScreen);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
    }

    public void ExitMainScreen ()
    {
        health.ResetGame();
        menuH.LoadScene("MenuScene");
    }
}
