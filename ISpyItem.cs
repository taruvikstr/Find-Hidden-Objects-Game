using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yle.Lapset;
using Yle.BUU;


public class ISpyItem : MonoBehaviour
{
    private ISpyHiddenItems hiddenItemsScript;

    [SerializeField]
    private GameObject uiController;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private AnimationClip itemFoundClip;

    public AudioClip audioHint, itemNameAudio;
    private Collider2D col; // collider is deactivated by default in the prefab
    private Camera mainCamera;

    private Vector3 originalParentPos;
    private bool itemFound;

    public Sprite itemShapeImage, uiSprite;

    private void Start()
    {
        hiddenItemsScript = GameObject.FindGameObjectWithTag("ISpyItemContainer").GetComponent<ISpyHiddenItems>();
        uiController = GameObject.Find("UI");
        animator = GetComponent<Animator>();
        originalParentPos = transform.parent.gameObject.transform.position;
        itemFound = false;
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (itemFound) MoveItemToCenter();
    }

    public void ActivateItem()
    {
        col = GetComponent<BoxCollider2D>();
        col.enabled = true;
    }

    public void ItemFound() // Bound to the easy touch component of the item prefab, called when clicked
    {
        uiController.GetComponent<ISpyUIController>().ItemFoundSFX();
        hiddenItemsScript.itemsFound++;
        col.enabled = false;
        
        GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
        GetComponent<SpriteRenderer>().sortingLayerName = "Front Near";
        StartCoroutine("ItemFoundEvents");
    }

    IEnumerator ItemFoundEvents()
    {
        AudioSource UIaudioSource = uiController.GetComponent<AudioSource>();
        itemFound = true;
        yield return new WaitForSeconds(1f);

        if (UIaudioSource.isPlaying) UIaudioSource.Stop();
        UIaudioSource.PlayOneShot(itemNameAudio);
        animator.SetBool("ItemFound", true);

        yield return new WaitForSeconds(2.7f);

        uiController.GetComponent<ISpyUIController>().FoundItemUI();

        hiddenItemsScript.NewSearch();

        yield return new WaitForSeconds(1f);
        transform.parent.gameObject.transform.position = originalParentPos;
        itemFound = false;
        Destroy(this.gameObject, itemFoundClip.length);
    }

    private void MoveItemToCenter() //Found item is moved to the center after being found
    {
        Vector3 centerPos = new Vector3(0, 0, 0);

        if (mainCamera.GetComponent<ISpyCameraScrolling>().ableSwipe)
            centerPos = new Vector3(mainCamera.transform.position.x/2, 0f, 0f);

        transform.parent.gameObject.transform.position = Vector3.MoveTowards(transform.parent.gameObject.transform.position, centerPos, 10f * Time.deltaTime);
    }
}
