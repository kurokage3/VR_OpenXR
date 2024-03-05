//Author name = JoshuaKenendy;
//Website portfolio = joshuatkennedy.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabInteractableSlider : XRGrabInteractable
{
    #region Variables
    private Vector3 initialAttachLocalPos;
    private Quaternion initialAttachLocalRot;

    // Reference to the GunInteractable script on the gun GameObject
    public XRGrabInteractableGun gunInteractableScript;
    #endregion

    #region UnityEngine
    void Start()
    {
        //Create attach point
        if (!attachTransform)
        {
            GameObject grab = new GameObject("Grab Pivot");
            grab.transform.SetParent(transform, false);
            attachTransform = grab.transform;
        }

        initialAttachLocalPos = attachTransform.localPosition;
        initialAttachLocalRot = attachTransform.localRotation;

        Debug.Log("Convert XRGrabInteractableOffset to XRGrabInteractableSlider eventually. ");
        Debug.Log("Including the Select Entered events (Animator.enabled = false && OnTargetReached.enabled = true)");
        Debug.Log("Including the Select Exited events (Animator.enabled = true && OnTargetReached.enabled = false)");
    }
    #endregion

    protected override void OnSelectEntered(SelectEnterEventArgs interactor)
    {
        // Disable line visuals on both hands when the slider is grabbed
        ToggleLineVisuals(false);

        //Offset the transform
        if (interactor.interactorObject is XRDirectInteractor)
        {
            attachTransform.position = GetAttachTransform(interactor.interactorObject).position;
            attachTransform.rotation = GetAttachTransform(interactor.interactorObject).rotation;
        }
        else
        {
            attachTransform.localPosition = initialAttachLocalPos;
            attachTransform.localRotation = initialAttachLocalRot;
        }

        // Original OnSelectEntered functionality
        base.OnSelectEntered(interactor);
    }


    [System.Obsolete]
#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
    protected override void OnSelectEntering(SelectEnterEventArgs args)
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member
    {
        // Prevent grabbing the slider if the gun is not being held
        if (!gunInteractableScript.IsHeld())
        {

            // Correct method to cancel the selection of this interactable
            args.interactable.interactionManager.CancelInteractableSelection(this);
            return;
        }

        // Disable line visuals on both hands when the slider is grabbed
        ToggleLineVisuals(false);

        base.OnSelectEntering(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        // Re-enable line visuals on both hands when the gun is released
        ToggleLineVisuals(false);
    }

    private void ToggleLineVisuals(bool enabled)
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