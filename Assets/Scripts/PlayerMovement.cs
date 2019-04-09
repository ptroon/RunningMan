using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using zebaroo;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D ccon;
    float hmove = 0f;
    public float runSpeed = 40f;
    private bool paused = false;
    private CameraHandler camera;

    private GroundTileManager ground;
    private PlayerHealth health;
    private MenuHandler menuH;
    private SoundManager sounds;
    private Animator animator;        

    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraHandler>();

        // init objects
        ground = Object.FindObjectOfType<GroundTileManager>();
        health = Object.FindObjectOfType<PlayerHealth>();
        menuH = Object.FindObjectOfType<MenuHandler>();
        sounds = Object.FindObjectOfType<SoundManager>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        hmove = CrossPlatformInputManager.GetAxisRaw("Horizontal") * runSpeed;

        if ( hmove != 0 )
        {
            animator.SetBool("isWalking", true);
        }

        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            animator.SetBool("isJumping", true);
            sounds.PlayAudioClip(1);
        }

      
    }

     private void FixedUpdate()
    {
        ccon.Move(hmove * Time.fixedDeltaTime, false, animator.GetBool("isJumping"));

        animator.SetBool("isJumping", false);
        animator.SetBool("isWalking", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Danger")
        {
            health.KillPlayer("Hit a danger tile");
            camera.RestartLevelCamera();
            ground.BuildPlatform();
        }
    }
}
