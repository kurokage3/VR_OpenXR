using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolShoot : MonoBehaviour
{
    [Header("Prefab Refrences")]
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;

    [Header("Location Refrences")]
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;

    [Header("Settings")]
    [Tooltip("Specify time to destory the casing object")] [SerializeField] private float destroyTimer = 5f;
    [Tooltip("Bullet Speed")] [SerializeField] private float bulletForce = 500f;
    [Tooltip("Casing Ejection Speed")] [SerializeField] private float ejectForce = 150f;


    void Start()
    {
        if (barrelLocation == null)
        {
            barrelLocation = transform;
        }

        if (gunAnimator == null)
        {
            gunAnimator = GetComponentInChildren<Animator>();
        }
    }

    public void PullTheTrigger()
    {
        //Start the animation, which 
        gunAnimator.SetTrigger("Fire");
    }


    //This function creates the bullet behavior
    void Shoot()
    {
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

        // Create a bullet and add force on it in direction of the barrel
        GameObject spawnedBullet = Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation);
        spawnedBullet.GetComponent<Rigidbody>().AddForce(barrelLocation.forward * bulletForce);

        //Old Line of Code
        //Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation).GetComponent<Rigidbody>().AddForce(barrelLocation.forward * bulletForce);
    }

    //This function creates a casing at the ejection slot
    void CasingRelease()
    {
        //Cancels function if ejection slot hasn't been set or there's no casing
        if (!casingExitLocation || !casingPrefab)
        { return; }

        //Create the casing
        GameObject tempCasing;
        tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;
        //Add force on casing to push it out
        tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectForce * 0.7f, ejectForce), (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
        //Add torque to make casing spin in random direction
        tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);

        //Destroy casing after X seconds
        Destroy(tempCasing, destroyTimer);
    }

}
