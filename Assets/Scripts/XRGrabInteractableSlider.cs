//Author name = JoshuaKenendy;
//Website portfolio = joshuatkennedy.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabInteractableSlider : XRGrabInteractable
{
	#region Variables
	// Reference to the GunInteractable script on the gun GameObject
	public XRGrabInteractableGun gunInteractableScript;
    #endregion

    [System.Obsolete]
    protected override void OnSelectEntering(SelectEnterEventArgs args)
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
}
