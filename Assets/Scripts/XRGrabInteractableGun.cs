//Author name = JoshuaKenendy;
//Website portfolio = joshuatkennedy.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabInteractableGun : XRGrabInteractable
{
    [Header("Custom References")]
    public Transform leftAttachTransform;
    public Transform rightAttachTransform;

    // Reference to the slider's XRGrabInteractable component
    public XRGrabInteractable sliderGrabInteractable;

    // Keep track of whether the gun is currently held
    private bool isHeld = false;

    protected override void Awake()
    {
        base.Awake();
        // Ensure the slider's grab interactable is disabled by default
        if (sliderGrabInteractable != null)
        {
            sliderGrabInteractable.enabled = false;
        }
    }

    // Public method to check if the gun is being held
    public bool IsHeld()
    {
        return isHeld;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        // When the gun is picked up, enable the slider's interactable
        isHeld = true;
        if (sliderGrabInteractable != null)
        {
            sliderGrabInteractable.enabled = true;
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        // When the gun is released, disable the slider's interactable
        isHeld = false;
        if (sliderGrabInteractable != null)
        {
            sliderGrabInteractable.enabled = false;
        }
    }

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
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

}