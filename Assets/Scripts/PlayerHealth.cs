using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public GameObject Heart1, Heart2, Heart3;   // heart prefabs to show / hide
    private GameObject heart1, heart2, heart3;

    public int livesLeft = 3;                   // actual lives counter
    public float xPos, yPos;            // where to initialise the player on screen

    public GameObject PlayerObject;
    private GameObject tPlayerObj;

    private SoundManager sounds;
    // private GroundTileManager ground;

    // Start is called before the first frame update
    void Start()
    {
        sounds = Object.FindObjectOfType<SoundManager>();
        // ground = Object.FindObjectOfType<GroundTileManager>();
    }

    public void InitialisePlayer ()
    {
        tPlayerObj = Instantiate(PlayerObject, new Vector3(xPos, yPos, 0f), transform.rotation);
    }

    public void DrawHeartPrefabs (Canvas canvas)
    {
        heart1 = Instantiate(Heart1, new Vector3(400, 320, 0f), canvas.transform.rotation) as GameObject;
        heart1.transform.SetParent(canvas.transform, false);
        heart1.SetActive(true);

        heart2 = Instantiate(Heart2, new Vector3(450, 320, 0f), canvas.transform.rotation) as GameObject;
        heart2.transform.SetParent(canvas.transform, false);
        heart2.SetActive(true);

        heart3 = Instantiate(Heart3, new Vector3(500, 320, 0f), canvas.transform.rotation) as GameObject;
        heart3.transform.SetParent(canvas.transform, false);
        heart3.SetActive(true);
    }

    public int LostLife ()
    {
        livesLeft--;
        if (livesLeft == 2)
        {
            heart3.SetActive(false);
        }
        else if (livesLeft == 1)
        {
            heart2.SetActive(false);
        }
        else if (livesLeft == 0)
        {
            heart1.SetActive(false);
        }
        sounds.PlayAudioClip(2);
        return livesLeft;
    }

    public void KillPlayer (string message)
    {
        Debug.Log(this.name + " " + message);
        Destroy(tPlayerObj);

        livesLeft = LostLife();

        if (livesLeft >= 1) // not fully dead..
        {
            RespawnPlayer();
        }
        else
        {
            sounds.PlayAudioClip(3); // play dead clip

        } // RETURN TO MAIN SCREEN
    }

    public void RespawnPlayer ()
    {
        InitialisePlayer();
    }

    public void ResetGame ()
    {
        livesLeft = 3;
    }
}
