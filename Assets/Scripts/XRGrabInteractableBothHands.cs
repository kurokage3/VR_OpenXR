//Author name = JoshuaKenendy;
//Website portfolio = joshuatkennedy.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabInteractableBothHands : XRGrabInteractable
{
    [Header("Custom References")]
    public Transform leftAttachTransform;
    public Transform rightAttachTransform;

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