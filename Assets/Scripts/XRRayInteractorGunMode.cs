//Author name = JoshuaKenendy;
//Website portfolio = joshuatkennedy.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

//Currently Left Hand Controller Only for Testing Purposes
public class XRRayInteractorGunMode : XRRayInteractor
{
    #region Variables
              
    #endregion

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        Debug.Log("Grabbed Object = " + args.interactableObject.transform.name);
        Debug.Log("Grabbed Object Tag = " + args.interactableObject.transform.tag);


        Debug.Log("Currently Left Hand Controller Only for Testing Purposes");


        //Check if the player is trying to pick up the gun by the slider
        Transform currentTransform = args.interactableObject.transform;
        bool isSlider = false;
        while (currentTransform != null)
        {
            if (currentTransform.CompareTag("Slider"))
            {
                isSlider = true;
                break;
            }
            currentTransform = currentTransform.parent;
        }
        if (isSlider)
        {
            Debug.Log("Slider interacted, ignoring...");
            return; // Do not proceed with selection
        }

        //Disable XR Interactor Line Visual
        XRInteractorLineVisual lineVisual = GetComponent<XRInteractorLineVisual>();
        if (lineVisual != null)
        {
            lineVisual.enabled = false;
        }

        //Disable XR Ray Interactor Anchor Control (Lock Translation & Rotation)
        allowAnchorControl = false;

        // Original OnSelectEntered functionality
        base.OnSelectEntered(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        //Enable XR Interactor Line Visual
        XRInteractorLineVisual lineVisual = GetComponent<XRInteractorLineVisual>();
        if (lineVisual != null)
        {
            lineVisual.enabled = true;
        }

        //Enable XR Ray Interactor Anchor Control (Lock Translation & Rotation)
        allowAnchorControl = true;

        // Original OnSelectExited functionality
        base.OnSelectExited(args);
    }
}
