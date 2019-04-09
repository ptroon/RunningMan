using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    private AudioSource audioSource;
    public enum EAudioClips { jump=1, loseLife, dead, hitBad, hitGood};
    public AudioClip jumpClip, loseLifeClip, deadClip, hitBadClip, hitGoodClip;
        
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudioClip(int clipID)
    {
        switch (clipID)
        {
            case 1:
                audioSource.PlayOneShot(jumpClip);
                break;
            case 2:
                audioSource.PlayOneShot(loseLifeClip);
                break;
            case 3:
                audioSource.PlayOneShot(deadClip);
                break;
            case 4:
                audioSource.PlayOneShot(hitBadClip);
                break;
            case 5:
                audioSource.PlayOneShot(hitGoodClip);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
