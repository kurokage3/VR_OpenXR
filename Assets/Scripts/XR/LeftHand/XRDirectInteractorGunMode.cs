//Author name = JoshuaKenendy;
//Website portfolio = joshuatkennedy.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRDirectInteractorGunMode : XRDirectInteractor
{
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        //Testing Debug Log
        LogInteractionDetails(args);

        //Check if the player is trying to pick up the gun by the slider
        if (args.interactableObject.transform.CompareTag("Magazine"))
        {
            args.interactableObject.transform.gameObject.GetComponent<Rigidbody>().useGravity = true;
            Debug.Log("Turned Magazine Gravity On!");

            Debug.Log("Respawn a new magazine");
        }

        // Original OnSelectEntered functionality
        base.OnSelectEntered(args);
    }

    private void LogInteractionDetails(SelectEnterEventArgs args)
    {
        Debug.Log($"Grabbed Object = {args.interactableObject.transform.name}");
        Debug.Log($"Grabbed Object Tag = {args.interactableObject.transform.tag}");
    }
}
