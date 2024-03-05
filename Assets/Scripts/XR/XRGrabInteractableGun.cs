//Author name = JoshuaKenendy;
//Website portfolio = joshuatkennedy.com

using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabInteractableGun : XRGrabInteractable
{
    #region Variables
    [Header("Custom References")]
    public Transform leftAttachTransform;
    public Transform rightAttachTransform;

    // Reference to the slider's XRGrabInteractable component
    public XRGrabInteractable sliderGrabInteractable;

    // Keep track of whether any gun is currently held
    public static bool isHeld = false;

    // Track which hand is holding the gun
    //private XRBaseInteractor holdingInteractor = null;
    #endregion

    protected override void Awake()
    {
        base.Awake();

        // Ensure the slider's grab interactable is disabled by default
        if (sliderGrabInteractable != null)
        {
            sliderGrabInteractable.enabled = false;
        }
    }

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        // Prevent grabbing if the gun is already held by any hand
        if (isHeld)
        {
            // The gun is already held by another interactor, so prevent this interaction
            // This relies on the interaction manager to respect this prevention

            // Cancel the interaction attempt
            //args.interactor.CancelInteractableSelection(this);
            Debug.Log("THIS IS WHERE I LEFT OFF, MAKE IT SO WHEN A HAND IS HOLDIONG THE GUN YOU CANT PICK IT UP");


            return;
        }

        // Determine which hand is holding the object and assign the correct attach transform
        if (args.interactorObject.transform.CompareTag("Left Hand"))
        {
            attachTransform = leftAttachTransform;
        }
        else if (args.interactorObject.transform.CompareTag("Right Hand"))
        {
            attachTransform = rightAttachTransform;
        }

        // Original OnSelectEntering functionality
        base.OnSelectEntering(args);
    }

    // Public method to check if the gun is being held
    public bool IsHeld()
    {
        return isHeld;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        // Store the interactor that is holding the gun
        //holdingInteractor = args.interactorObject as XRBaseInteractor;


        // When the gun is picked up, enable the slider's interactable
        isHeld = true;

        // Enable the slider's interactable if it exists
        if (sliderGrabInteractable != null)
        {
            sliderGrabInteractable.enabled = true;
        }

        // XR Grab Interactable Base Logic
        base.OnSelectEntered(args);

        // Disable line visuals on both hands when the gun is grabbed
        ToggleLineVisuals(false);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        // The gun is no longer held, clear the interactor and the flag
        //holdingInteractor = null;
        isHeld = false;

        // Disable the slider's interactable if it exists
        if (sliderGrabInteractable != null)
        {
            sliderGrabInteractable.enabled = false;
        }

        // XR Grab Interactable Base Logic
        base.OnSelectExited(args);

        // Re-enable line visuals on both hands when the gun is released
        ToggleLineVisuals(true);
    }

    public void ToggleLineVisuals(bool enabled)
    {
        var rayInteractors = FindObjectsOfType<XRRayInteractorGunMode>();
        foreach (var interactor in rayInteractors)
        {
            var lineVisual = interactor.GetComponent<XRInteractorLineVisual>();
            if (lineVisual != null)
            {
                lineVisual.enabled = enabled;
            }
        }
    }
}