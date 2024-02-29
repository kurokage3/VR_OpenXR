//Author name = JoshuaKenendy;
//Website portfolio = joshuatkennedy.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRRayInteractorGunMode : XRRayInteractor
{
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        //Testing Debug Log
        LogInteractionDetails(args);

        //Check if the player is trying to pick up the gun by the slider
        if (IsInteractingWithSlider(args.interactableObject.transform))
        {
            Debug.Log("Slider interacted, ignoring...");
            return; // Do not proceed with selection
        }

        // Disable visual and anchor control
        SetVisualAndAnchorControl(false);

        // Original OnSelectEntered functionality
        base.OnSelectEntered(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        // Enable visual and anchor control
        SetVisualAndAnchorControl(true);

        // Original OnSelectExited functionality
        base.OnSelectExited(args);
    }

    private void SetVisualAndAnchorControl(bool enabled)
    {
        // Toggle XR Interactor Line Visual
        XRInteractorLineVisual lineVisual = GetComponent<XRInteractorLineVisual>();
        if (lineVisual != null)
        {
            lineVisual.enabled = enabled;
        }

        // Toggle XR Ray Interactor Anchor Control (Lock Translation & Rotation)
        allowAnchorControl = enabled;
    }

    private bool IsInteractingWithSlider(Transform interactableObjectTransform)
    {
        // Check if the interaction is with the slider component of the gun
        while (interactableObjectTransform != null)
        {
            if (interactableObjectTransform.CompareTag("Slider"))
            {
                return true; // Slider interaction confirmed
            }
            interactableObjectTransform = interactableObjectTransform.parent;
        }
        return false; // No slider interaction detected
    }

    private void LogInteractionDetails(SelectEnterEventArgs args)
    {
        Debug.Log($"Grabbed Object = {args.interactableObject.transform.name}");
        Debug.Log($"Grabbed Object Tag = {args.interactableObject.transform.tag}");
    }
}
