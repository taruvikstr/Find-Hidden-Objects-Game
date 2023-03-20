using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Yle.Lapset;
using Yle.BUU;

/*
 * I spy minigame
 * This script is attached to the empty objects (easy, medium) in each gameroom.
 */

public class ISpyHiddenItems : MonoBehaviour
{
    [SerializeField]
    private List<Transform> hidingPlaces = new List<Transform>();

    [SerializeField]
    private List<GameObject> items = new List<GameObject>();

    public List<GameObject> itemInstances = new List<GameObject>();

    [HideInInspector]
    public int itemsFound;

    [SerializeField]
    private ISpyController iSpyControllerScript;

    [SerializeField]
    private GameObject iSpyUIController;

    public GameObject randomHidingItem;

    private void OnEnable()
    {
        itemsFound = 0;
        if (hidingPlaces.Count.Equals(0)) GetHidingSlots();
        itemInstances.Clear();

        HideItems(); // Once the room is activated agan, hiding new items
    }

    public void NewSearch() // Called after instantiating all items and after each found item (i spy item -script)
    {

        if (itemsFound == 5)
        {
            itemInstances.Clear();
            itemsFound = 0;
            iSpyControllerScript.StartCoroutine("RoomFinished");
        }
        else
        {
            itemInstances[itemsFound].GetComponent<ISpyItem>().ActivateItem();
            if(iSpyControllerScript.introPlayed) iSpyUIController.GetComponent<ISpyUIController>().Invoke("AudioHint", 0.6f);
            else iSpyUIController.GetComponent<ISpyUIController>().Invoke("AudioHint", 1.6f);
        }
    }


    private void GetHidingSlots() // Takes all child items to the list that will possible given as possible hiding places
    {
        foreach (Transform child in transform) // Each difficulty has their own hiding slots
        {

            hidingPlaces.Add(child);
        }

        hidingPlaces = hidingPlaces.OrderBy(i => Guid.NewGuid()).ToList();
    }



    private void HideItems()
    {
        randomHidingItem = transform.parent.GetChild(0).GetComponent<ISpyRandomItems>().GiveRandomHidingItem(); // Getting random, non fitting item to hide to this room
        items.Insert(4, randomHidingItem);
        items = items.OrderBy(i => Guid.NewGuid()).ToList();
        // hide items to random slots
        for (int index = 0; index < 5; index++)
        {
            GameObject itemToHide = Instantiate(items[index], hidingPlaces[index]);
            iSpyUIController.GetComponent<ISpyUIController>().AddUIItem(itemToHide); // giving the hidden gameobjects for UI
            itemInstances.Add(itemToHide);
        }

        if (iSpyControllerScript.introPlayed) Invoke("NewSearch", 6f);
        else Invoke("NewSearch", 10.5f); // First time playing, the player will hear the game intro, NewSearch is initiated later
    }

    private void OnDisable()
    {
        items.Remove(randomHidingItem);
    }
}
