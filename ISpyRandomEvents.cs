using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script takes gameobjects that are interactable and are given an extra animation called "Random".
//The script attached to the rooms Decorations gameobject.
//The script randomly runs one of these to give the player a hint of possible interactions with this object.
//If the object has been already clicked and it's clciked index is not 0, the new random will be selected.

public class ISpyRandomEvents : MonoBehaviour
{
    [SerializeField] private GameObject[] random_eventObject;
    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > 15)
        {
            timer = 0;
            RandomEvent();
        }
    }

    private void RandomEvent()
    {
        int max = random_eventObject.Length;
        int random = UnityEngine.Random.Range(0, max);

        if (random_eventObject[random].GetComponent<ISpyInteractable>().index == 0)
            random_eventObject[random].GetComponent<Animator>().Play("Random");
        else RandomEvent();

    }
}
