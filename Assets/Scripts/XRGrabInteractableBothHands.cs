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
        string name = args.interactorObject.transform.name;
        Debug.Log("interactorObject Name = " + name);

        if (args.interactorObject.transform.CompareTag("Left Hand"))
        {
            attachTransform = leftAttachTransform;
        }
        else if(args.interactorObject.transform.CompareTag("Right Hand"))
        {
            attachTransform = rightAttachTransform;
        }

        base.OnSelectEntering(args);
    }

    //protected override void OnSelectEntered(SelectEnterEventArgs args)
    //{
    //    base.OnSelectEntered(args);

    //    // Try to get the ActionBasedController from the interactor
    //    var controller = args.interactorObject as ActionBasedController;
    //    if (controller == null)
    //    {
    //        Debug.LogWarning("The interactor does not have an ActionBasedController component.", this);
    //        return;
    //    }

    //    // Determine the node (left or right hand) associated with the controller
    //    var controllerNode = GetControllerNode(controller);

    //    // Set the attach transform based on the node
    //    if (controllerNode == XRNode.LeftHand && leftAttachTransform != null)
    //    {
    //        attachTransform = leftAttachTransform;
    //    }
    //    else if (controllerNode == XRNode.RightHand && rightAttachTransform != null)
    //    {
    //        attachTransform = rightAttachTransform;
    //    }
    //}

    //private XRNode GetControllerNode(ActionBasedController controller)
    //{
    //    var inputDevices = new List<InputDevice>();
    //    InputDevices.GetDevices(inputDevices);

    //    foreach (var device in inputDevices)
    //    {
    //        if (device.characteristics.HasFlag(InputDeviceCharacteristics.HeldInHand))
    //        {
    //            // Determine if the device is a left-hand or right-hand controller
    //            if (device.characteristics.HasFlag(InputDeviceCharacteristics.Left))
    //            {
    //                return XRNode.LeftHand;
    //            }
    //            else if (device.characteristics.HasFlag(InputDeviceCharacteristics.Right))
    //            {
    //                return XRNode.RightHand;
    //            }
    //        }
    //    }

    //    return XRNode.Head; // Default to head if we cannot find the controller node
    //}
}