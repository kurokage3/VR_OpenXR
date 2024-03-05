//Author name = JoshuaKenendy;
//Website portfolio = joshuatkennedy.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FireBulletOnActivate : MonoBehaviour
{
	#region Variables
	[Header("Prefab Refrences")]
	public GameObject bulletPrefab;
	public GameObject casingPrefab;
	public GameObject muzzleFlashPrefab;

	[Header("Ray Visual")]
	[SerializeField] private LineRenderer lineRenderer;
	private XRGrabInteractableGun gunInteractable;

	[Header("Location Refrences")]
	[SerializeField] private Animator gunAnimator;
	[SerializeField] private Transform barrelLocation;
	[SerializeField] private Transform casingExitLocation;

	[Header("Magazine Refrences")]
	[SerializeField] private Magazine magazine;
	[SerializeField] private XRBaseInteractor socketInteractor;
	private bool isGunRacked = true;

	[Header("Sounds")]
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip fireSound;
	[SerializeField] private AudioClip reloadSound;
	[SerializeField] private AudioClip noAmmoSound;
	[SerializeField] private AudioClip rackSlideSound;

	[Header("Settings")]
	[Tooltip("Specify time to destory the casing object")] [SerializeField] private float destroyTimer = 5f;
	[Tooltip("Bullet Speed")] [SerializeField] private float bulletInitialVelocity = 150f;
	[Tooltip("Casing Ejection Speed")] [SerializeField] private float casingEjectForce = 150f;
    #endregion

    #region UnityEngine
    [System.Obsolete]
    void Start()
    {
		//Gun Settings Initialization
		if (barrelLocation == null)
		{
			barrelLocation = transform;
		}
		if (gunAnimator == null)
		{
			gunAnimator = GetComponentInChildren<Animator>();
		}
		if(gunInteractable == null)
        {
			gunInteractable = GetComponentInParent<XRGrabInteractableGun>();
		}

		//Magazine Intitialization
		socketInteractor.onSelectEnter.AddListener(AddMagazine);
		socketInteractor.onSelectExit.AddListener(RemoveMagazine);

		//VR Settings Initialization
		XRGrabInteractable grabbable = GetComponentInParent<XRGrabInteractable>();
		grabbable.activated.AddListener(PullTheTrigger);
    }

    private void Update()
    {
		// Continuously update the ray path if the gun is being held
		UpdateBulletPath();
	}
    #endregion

    public void AddMagazine(XRBaseInteractable interactable)
    {
		magazine = interactable.GetComponent<Magazine>();
		audioSource.PlayOneShot(reloadSound);
		isGunRacked = false;

		// Disable line visuals on both hands when magazine is added
		gunInteractable.ToggleLineVisuals(false);
	}

	public void RemoveMagazine(XRBaseInteractable interactable)
    {
		magazine = null;
		audioSource.PlayOneShot(reloadSound);

		// Re-enable line visuals on both hands when magazine is removed
		gunInteractable.ToggleLineVisuals(true);
	}

	public void RackSlider()
    {
		isGunRacked = true;
		audioSource.PlayOneShot(rackSlideSound);

		// Disable line visuals on both hands when gun is racked
		gunInteractable.ToggleLineVisuals(false);

		// Check if the magazine is empty after racking
		if (magazine.numberOfBullets <= 0)
		{
			// Trigger the slide back animation
			gunAnimator.SetBool("IsEmpty", true);
		}
		else
		{
			// Make sure to reset the animation state if there's still ammo
			gunAnimator.SetBool("IsEmpty", false);
		}
	}

	public void PullTheTrigger(ActivateEventArgs arg)
	{
		//Check For Magazine & Bullets & Gun is Racked
		if(magazine && magazine.numberOfBullets > 0 && isGunRacked)
        {
			//Start the animation, which 
			gunAnimator.SetTrigger("Fire");
		}
        else
        {
			//Play No Ammo Sound
			audioSource.PlayOneShot(noAmmoSound);
		}
	}

	//This function creates the bullet behavior (Animation Event Action)
	void Shoot()
	{
		//Remove a bullet
		magazine.numberOfBullets--;

        // Check if the magazine is empty after shooting
        if (magazine.numberOfBullets <= 0)
        {
			// Trigger the slide back animation
			gunAnimator.SetBool("IsEmpty", true);
		}
        else
        {
			// Make sure to reset the animation state if there's still ammo
			gunAnimator.SetBool("IsEmpty", false);
		}

        //Play Shot Audio
        audioSource.PlayOneShot(fireSound);

		if (muzzleFlashPrefab)
		{
			//Create the muzzle flash
			GameObject tempFlash;
			tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);

			//Destroy the muzzle flash effect
			Destroy(tempFlash, destroyTimer);
		}

		//cancels if there's no bullet prefeb
		if (!bulletPrefab)
		{
			return;
		}

		// Create a bullet and set initial velocity on it in direction of the barrel
		GameObject spawnedBullet = Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation);
		Rigidbody bulletRB = spawnedBullet.GetComponent<Rigidbody>();
		bulletRB.velocity = barrelLocation.forward.normalized * bulletInitialVelocity;
	}

	//This function creates a casing at the ejection slot (Animation Event Action)
	void CasingRelease()
	{
		//Cancels function if ejection slot hasn't been set or there's no casing
		if (!casingExitLocation || !casingPrefab)
		{ return; }

		//Create the casing
		GameObject tempCasing;
		tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;
		//Add force on casing to push it out
		tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(casingEjectForce * 0.7f, casingEjectForce), (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
		//Add torque to make casing spin in random direction
		tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);

		//Destroy casing after X seconds
		Destroy(tempCasing, destroyTimer);
	}

	void UpdateBulletPath()
	{
		if (gunInteractable.IsHeld())
		{
			if (lineRenderer != null)
			{
				// Set the start position of the LineRenderer to the barrel location
				lineRenderer.SetPosition(0, barrelLocation.position);

				RaycastHit hit;
				if (Physics.Raycast(barrelLocation.position, barrelLocation.forward, out hit))
				{
					// If the raycast hits something, set the end position of the LineRenderer to the hit point
					lineRenderer.SetPosition(1, hit.point);
				}
				else
				{
					// If the raycast doesn't hit anything, extend the line far in the direction the gun is pointing
					lineRenderer.SetPosition(1, barrelLocation.position + barrelLocation.forward * 100);
				}
			}
		}
	}
}
