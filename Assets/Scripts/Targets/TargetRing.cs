//Author name = JoshuaKenendy;
//Website portfolio = joshuatkennedy.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRing : MonoBehaviour
{
    #region Variables
    public GameObject explosionEffect;
    public string ringName; // Assign a unique name in the inspector to each ring like "Bulls_Eye", "Second_Ring", etc.
    #endregion

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if (explosionEffect != null)
            {
                Instantiate(explosionEffect, transform.position, transform.rotation);
            }

            // Example of different behavior based on the ring hit
            switch (ringName)
            {
                case "Bulls_Eye":
                    // Handle bull's eye hit
                    Debug.Log("Bull's eye hit!");
                    break;
                case "Second_Ring":
                    // Handle second ring hit
                    Debug.Log("Second ring hit!");
                    break;
                case "Third_Ring":
                    // Handle third ring hit
                    Debug.Log("Third ring hit!");
                    break;
                case "Fourth_Ring":
                    // Handle fourth ring hit
                    Debug.Log("Fourth ring hit!");
                    break;
                default:
                    Debug.Log("Other part of the target hit");
                    break;
            }

            // Destroy the whole target (all rings):
            Destroy(transform.root.gameObject);
        }
    }
}
