//Author name = JoshuaKenendy;
//Website portfolio = joshuatkennedy.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabInteractableBothHands : XRGrabInteractable
{
    [Header("Custom References")]
    public Transform leftAttachTransform;
    public Transform rightAttachTransform;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        //Get the XRController from the interactorObject
        XRBaseInteractor interactor = args.interactorObject as XRBaseInteractor;
        if (interactor == null)
        {
            Debug.LogWarning("InteractorObject is not of type XRBaseInteractor");
            return;
        }
        XRController controller = interactor.GetComponent<XRController>();

        //If the controller is not found directly, attempt to find it in the parent
        if (controller == null)
        {
            controller = interactor.GetComponentInParent<XRController>();
        }

        // heck if the controller component is present
        if (controller != null)
        {
            //If Left Controller, set the left attach transform
            if (controller.controllerNode == UnityEngine.XR.XRNode.LeftHand && leftAttachTransform != null)
            {
                attachTransform = leftAttachTransform;
            }
            //Else If Right Controller, set the right attach transform
            else if (controller.controllerNode == UnityEngine.XR.XRNode.RightHand && rightAttachTransform != null)
            {
                attachTransform = rightAttachTransform;
            }
        }

        //Keep all normal XRGrabInteractable functionality
        base.OnSelectEntered(args);
    }
}