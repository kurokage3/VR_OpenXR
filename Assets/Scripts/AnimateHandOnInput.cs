//Author name = JoshuaKenendy;
//Website portfolio = joshuatkennedy.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimateHandOnInput : MonoBehaviour
{
	#region Variables
	public InputActionProperty pinchAnimationAction;
	public InputActionProperty gripAnimationAction;

	public Animator handAnimator;
	#endregion
	 
	#region UnityEngine
    void Start()
    {
		
    }

    void Update()
    {
		float triggerInputValue = pinchAnimationAction.action.ReadValue<float>();
		handAnimator.SetFloat("Trigger", triggerInputValue);

		float gripValue = gripAnimationAction.action.ReadValue<float>();
		if(gripValue < 0.55f)
        {
			handAnimator.SetFloat("Grip", gripValue);
		}
        else
        {
			handAnimator.SetFloat("Grip", 0.55f);
		}
	}
	#endregion
}
