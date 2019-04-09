using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using zebaroo;

public class CameraHandler : MonoBehaviour
{
    private GroundTileManager ground;
    private PlayerHealth health;
    private MenuHandler menuH;
    private DDOLGameManager game;

    private Camera myCamera;
    float ZOOM = 5f;
    public float cameraMovementSpeed =  1f;
    Vector3 vectPos, initVectPos;

    // Start is called before the first frame update
    void Start()
    {
        // init objects
        ground = Object.FindObjectOfType<GroundTileManager>();
        health = Object.FindObjectOfType<PlayerHealth>();
        menuH = Object.FindObjectOfType<MenuHandler>();
        game = Object.FindObjectOfType<DDOLGameManager>();

        // set up camera
        myCamera = this.GetComponent<Camera>();
        myCamera.orthographicSize = ZOOM;
        initVectPos = vectPos = myCamera.transform.position; // get the position of the camera into both vasriables
        cameraMovementSpeed += game.gameDifficulty[0].optionValue;
        Debug.Log("CAMERA START");

    }

    public void RestartLevelCamera ()
    {
        vectPos = initVectPos;
        myCamera.transform.position = initVectPos;
    }

    // Update is called once per frame
    void Update()
    {
        vectPos.x += cameraMovementSpeed * Time.deltaTime;
        myCamera.transform.position = vectPos;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if(collision.gameObject.tag == "Player")
        {
            health.KillPlayer("Player is dead!");
            RestartLevelCamera();
            ground.BuildPlatform();
        }

        if (collision.gameObject.tag == "Plain" ||
            collision.gameObject.tag == "Danger" ||
            collision.gameObject.tag == "Score" ||
            collision.gameObject.tag == "Jump" )
        {
            ground.RemoveTileFromList(collision.gameObject);
        }
    }

}
