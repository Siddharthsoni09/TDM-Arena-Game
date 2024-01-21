using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    public AudioSource audioSource;

    [Header("Footsteps Source")]
    public AudioClip[] footstepsSound;

    private AudioClip GetRandomFootStep()
    {
        return footstepsSound[Random.Range(0, footstepsSound.Length)];
    }

    private void FootStep()
    {
        AudioClip clip = GetRandomFootStep();
        audioSource.PlayOneShot(clip);
    }
}
