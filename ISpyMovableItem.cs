using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ISpyMovableItem : MonoBehaviour
{
    private AudioSource audioSource;

    private Vector3 origPosition;
    private Vector3 moveLocation;
    [SerializeField] private float moveX, moveY;
    [SerializeField] private bool clicked = false;

    private void Start()
    {
        origPosition = transform.position;
        audioSource = GetComponent<AudioSource>();

        moveLocation = new Vector3(origPosition.x + moveX, origPosition.y + moveY, origPosition.z);
    }

    private void Update()
    {
        if (clicked) MoveItem(moveLocation, 10f);
        else if (!clicked) MoveItem(origPosition, 3f);
    }

    public void Click()
    {
        
        if(transform.position == origPosition && clicked == false)
        {
            clicked = true;
            GetComponent<BoxCollider2D>().enabled = false;
            audioSource.Play();

        }

    }

    public void MoveItem(Vector3 location, float time)
    {
        transform.position = Vector3.MoveTowards(transform.position,
            location, time * Time.deltaTime);

        if (transform.position == moveLocation && GetComponent<BoxCollider2D>().enabled == false)
        {
            StartCoroutine("WaitBetween");
        }
    }

    private IEnumerator WaitBetween()
    {
        GetComponent<BoxCollider2D>().enabled = true;

        yield return new WaitForSeconds(2f);
        GetComponent<BoxCollider2D>().enabled = true;
        clicked = false;

        //yield return new WaitForSeconds(3f);

        

    }

}
