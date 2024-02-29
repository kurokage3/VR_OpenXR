//Author name = JoshuaKenendy;
//Website portfolio = joshuatkennedy.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabInteractableOffset : XRGrabInteractable
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

        base.OnSelectEntering(args);
    }

    //Future Non-Obsolete Potential Solution
    /*
    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        // Prevent grabbing the slider if the gun is not being held
        if (!gunInteractableScript.IsHeld())
        {
            // Correct method to cancel the selection of this interactable
            interactionManager.CancelSelection(this);
            return;
        }

        // Continue with the base method implementation
        base.OnSelectEntering(args);
    }
    */
}
