using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yle.Lapset;
using Yle.BUU;

/*
 * This handles all in the I Spy UI
 */

public class ISpyUIController : MonoBehaviour
{

    [SerializeField]
    private Image[] hiddenItemsUIImage;

    [SerializeField]
    private Sprite[] hiddenItemsUI;

    [SerializeField]
    private AudioClip itemToUISound;

    [SerializeField]
    private AudioClip[] audioHints;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private ParticleSystem particleHint;

    [SerializeField]
    private AudioClip particleHintSound, itemFoundSFX;

    [SerializeField]
    private Animator animator;

    private int uiIndex;

    [SerializeField]
    private Animator hintAnimator;

    private float hintTimer;

    private void Start()
    {
        ResetUI();
        audioSource = GetComponent<AudioSource>();
        hintTimer = 40f;
    }

    private void Update()
    {
        hintTimer -= Time.deltaTime;

        if (hintTimer <= 0)
        {
            hintAnimator.Play("HintPulse");
            hintTimer = 15f;
        }

    }
    public void AddUIItem(GameObject item) // This is called from Hidden Items -script once the items are being instanciated
    {
        Image UIimage = hiddenItemsUIImage[uiIndex].GetComponent<Image>();
        UIimage.sprite = item.GetComponent<ISpyItem>().itemShapeImage; // items ui shape
        //hiddenItemsUI[uiIndex] = item.GetComponent<SpriteRenderer>().sprite; // items actual sprite added to a list for later use
        hiddenItemsUI[uiIndex] = item.GetComponent<ISpyItem>().uiSprite;
        UIimage.enabled = false; // disabling the ui picture
        audioHints[uiIndex] = item.GetComponent<ISpyItem>().audioHint;
        uiIndex++;
        if (uiIndex >= 5) uiIndex = 0;
    }

    public void VisualHint() // Bound to visual hint ui button
    {
        hintTimer = 30f;

        Image UIimage = hiddenItemsUIImage[uiIndex].GetComponent<Image>();

        if (UIimage.sprite == hiddenItemsUI[uiIndex])
        {
            //moving the particle system to the location of the currently searched object
            particleHint.transform.position = GameObject.FindGameObjectWithTag("ISpyItemContainer").GetComponent<ISpyHiddenItems>().
                itemInstances[uiIndex].transform.position;

            particleHint.Stop();
            particleHint.Play();

            audioSource.Stop();
            audioSource.PlayOneShot(particleHintSound);
        }
        else if (UIimage.enabled == false) UIimage.enabled = true; //if the image is disabled, reveals the silhouette
        else UIimage.sprite = hiddenItemsUI[uiIndex]; // revealing the item picture
    }

    public void AudioHint()
    {
        hintTimer = 30f;
        // here play audio clip of the current item
        if (audioSource.isPlaying) audioSource.Stop();
        audioSource.PlayOneShot(audioHints[uiIndex]);
    }

    public void FoundItemUI() // When item is found, reveal the picture in the UI and increment the index
    {
        hintTimer = 30f;
        Image UIimage = hiddenItemsUIImage[uiIndex].GetComponent<Image>();
        UIimage.sprite = hiddenItemsUI[uiIndex];
        audioSource.PlayOneShot(itemToUISound);
        UIimage.enabled = true;
        uiIndex++;
    }

    public void ItemFoundSFX()
    {
        audioSource.PlayOneShot(itemFoundSFX);
    }

    public void ResetUI()
    {
        uiIndex = 0;
        hintTimer = 60f;
    }

    public void AnimateUI()
    {
        if (animator.GetBool("showUI"))
        {
            animator.SetBool("showUI", false);
            ResetUI();
        }
        else animator.SetBool("showUI", true);
    }

}
