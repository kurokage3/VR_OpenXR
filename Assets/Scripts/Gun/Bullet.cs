//Author name = JoshuaKenendy;
//Website portfolio = joshuatkennedy.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region Variables
    public float damage = 10f; // Default damage
    #endregion

    #region UnityEngine
    void Start()
    {
		// This destroys the bullet after 5 seconds if it hasn't hit anything
		Destroy(gameObject, 5f); 
	}
    #endregion

    void OnCollisionEnter(Collision collision)
    {
        // Check if the bullet hits a zombie (or any other game object you're interested in). You might use tags or layers for this.
        if (collision.gameObject.tag == "Zombie")
        {
            // You could also add effects here, such as a hit effect or sound

            Destroy(gameObject);
        }

        // Destroy the bullet upon collision with any object
        //Destroy(gameObject);
    }
}
