using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ISpyRandomItems : MonoBehaviour // This script i attached to each rooms Randomitems object
                                             // and needs be given the other rooms items that don't really fit in the current room.
{
    [SerializeField]
    private List<GameObject> randItems = new List<GameObject>();

    private void OnEnable()
    {
        randItems = randItems.OrderBy(i => Guid.NewGuid()).ToList();
    }

    public GameObject GiveRandomHidingItem()
    {
        return randItems[0];
    }
}
