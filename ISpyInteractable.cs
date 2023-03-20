using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script can be used for all interactbale items in I spy minigame.
 * Give the interactable the sounds, collider, easytouch and possible animations.
 */

public class ISpyInteractable : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip[] interactableAudio;

    private Animator interactableAnimator;

    [SerializeField]
    private AnimationClip[] interactableAnimations;

    [SerializeField]
    private bool hasAnimation, hasAudio, notRepeatable;

    public int index;

    void Start()
    {
        if(hasAnimation) interactableAnimator = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();

        index = 0;
    }


    public void PlayInteraction() //Playing the sounds in correct order and invokes a possible loopable sound
    {
        if (index == 0)
        {
            audioSource.Stop();
            if (hasAudio)
            {
                audioSource.PlayOneShot(interactableAudio[0]);
                Invoke("PlayLoopable", interactableAudio[0].length);
            }

            if (hasAnimation) interactableAnimator.Play("0");

            if (notRepeatable) GetComponent<Collider2D>().enabled = false;
        }
        else
        {
            if (hasAudio)
            {
                audioSource.Stop();
                audioSource.PlayOneShot(interactableAudio[index]);
            }
            
            if (hasAnimation) interactableAnimator.Play("1");

        }

        if (interactableAudio.Length > index + 1) index++;
        else index = 0;
        
    }

    private void PlayLoopable() // For this to work, tick the looping box and add the audio to the audiosource components audioclip slot
    {
        if(index == 1) audioSource.Play(); //Needs an extra check if the interactable is being spammed
    }

    private void OnDisable()
    {
        index = 0;
    }

    private void OnEnable()
    {
        GetComponent<Collider2D>().enabled = true;
    }

}
