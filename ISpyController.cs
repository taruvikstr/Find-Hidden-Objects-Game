using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Yle.Lapset;
using Yle.BUU;


public class ISpyController : MonoBehaviour
{
    [SerializeField]
    private GameObject uiController;

    public GameObject[] gameRooms;
    public int roomIndex;

    [SerializeField]
    private AudioClip introClip1, introClip2, roomFinishedClip, fanfar;

    private AudioSource audioSource;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private AnimationClip roomChangeAnim;

    [HideInInspector]
    public bool introPlayed = false;

    private void Awake()
    {
        roomIndex = 0;
        audioSource = uiController.GetComponent<AudioSource>();
        
        foreach (GameObject room in gameRooms) // Making sure all the gameroom objects are unactive
        {
            room.SetActive(false);
        }

        StartCoroutine("GameIntro");
    }

    public IEnumerator GameIntro() // When the very first room is opened, the intro speak is played. Not played anymore in next rooms.
    {
        ShuffleRooms();
        StartCoroutine("NextRoom");

        yield return new WaitForSeconds(7f);

        audioSource.PlayOneShot(introClip1);
        yield return new WaitForSeconds(introClip1.length);
        audioSource.PlayOneShot(introClip2);
        yield return new WaitForSeconds(4f);
        introPlayed = true;
        

    }

    private void ShuffleRooms()
    {
        gameRooms = gameRooms.OrderBy(x => Guid.NewGuid()).ToArray(); // This randomizes the order of the rooms in the list
    }

    private IEnumerator NextRoom()
    {
        gameRooms[roomIndex].SetActive(true);

        yield return new WaitForSeconds(4f);
        uiController.GetComponent<ISpyUIController>().AnimateUI();
    }

    public IEnumerator RoomFinished() // Called when all the items have been found
                                      // Room is finished, incrementing index and shutting previous room and opening a new one
    {
        audioSource.PlayOneShot(fanfar);
        yield return new WaitForSeconds(3.5f);
        audioSource.PlayOneShot(roomFinishedClip);

        uiController.GetComponent<ISpyUIController>().AnimateUI(); //UI panels go away

        yield return new WaitForSeconds(2.5f);
        StartCoroutine("CloseRoom");
    }

    public IEnumerator CloseRoom()
    {
        animator.SetTrigger("RoomChange");
        yield return new WaitForSeconds(6f);
        gameRooms[roomIndex].SetActive(false);
        if (roomIndex < (gameRooms.Length - 1)) roomIndex++;
        else roomIndex = 0;
        yield return new WaitForSeconds(1f);
        StartCoroutine("NextRoom");
    }
}
