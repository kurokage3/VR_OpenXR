//Author name = JoshuaKenendy;
//Website portfolio = joshuatkennedy.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTargetReached : MonoBehaviour
{
	#region Variables
	public float threshold = 0.03f;
	public float coolDownTime = 3f;
	public Transform target;
	public UnityEvent onReached;

	private bool isCooldown = false;
	#endregion

	#region UnityEngine
	private void FixedUpdate()
    {
		// Check if not in cooldown
		if (!isCooldown)
		{
			float distance = Vector3.Distance(transform.position, target.position);
			if (distance < threshold)
			{
				StartCoroutine(ReachedTarget(coolDownTime));
			}
		}
	}
	#endregion

	IEnumerator ReachedTarget(float time)
	{
		// We are now in cooldown
		isCooldown = true; 

		// Reached the target
		onReached.Invoke();

		// Wait for the specified time
		yield return new WaitForSeconds(time);

		// Cooldown finished
		isCooldown = false; 
	}
}
