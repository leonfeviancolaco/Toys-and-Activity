using UnityEngine;
using System.Collections;
public class Cannon : MonoBehaviour
{
  
    public GameObject cannonballPrefab;

    // cannonball inst point
    public Transform firePoint;

    public float cannonballSpeed = 10f;


    void Update()
    {
       
        if (Input.GetKeyDown("space"))
        {
            FireCannonball();
        }
    }

    void FireCannonball()
    {
        GameObject cannonball = Instantiate(cannonballPrefab, firePoint.position, firePoint.rotation);

        Rigidbody rb = cannonball.GetComponent<Rigidbody>();
        rb.velocity = firePoint.forward 	* cannonballSpeed;

        
        
            
        
    }
}
