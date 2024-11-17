using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CannonController : MonoBehaviour
{
    [Header("Cannon Settings")]
    public Transform barrelPivot;
    public GameObject cannonballPrefab;
    public Transform firePoint;
    public Rigidbody cannonRigidbody;
    public float minVerticalAngle = -45f; 
    public float maxVerticalAngle = 45f; 
    public float minHorizontalAngle = -90f;
    public float maxHorizontalAngle = 90f; 
    public float rotationSpeed = 30f;
    public float cannonballSpeed = 20f;
    public float knockbackForce = 10f;
    public float fireDelay = 3f;
    public float resetDelay = 2f;

    // Cinemachine
    public CinemachineVirtualCamera virtualCamera; // Reference to your Cinemachine Virtual Camera
    private CinemachineBasicMultiChannelPerlin cameraNoise; // To control the camera shake
    public float shakeDuration = 0.5f;
    public float shakeAmplitude = 2.0f;
    public float shakeFrequency = 2.0f;

    // Audio
    public AudioSource fuseAudioSource; // AudioSource for the fuse sound
    public AudioSource explosionAudioSource; // AudioSource for the explosion sound
    public AudioClip fuseBurningClip; // Fuse burning sound
    public AudioClip explosionClip; 

    private bool isFiring = false;
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;

        
        if (virtualCamera != null)
        {
            cameraNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

       
        if (fuseAudioSource != null && fuseBurningClip != null)
        {
            fuseAudioSource.clip = fuseBurningClip;
        }

        if (explosionAudioSource != null && explosionClip != null)
        {
            explosionAudioSource.clip = explosionClip;
        }
    }

    void Update()
    {
       
        float verticalInput = Input.GetAxis("Vertical"); 
        float currentVerticalAngle = barrelPivot.localEulerAngles.x;
        if (currentVerticalAngle > 180) currentVerticalAngle -= 360;
        float newVerticalAngle = Mathf.Clamp(currentVerticalAngle + verticalInput * rotationSpeed * Time.deltaTime, minVerticalAngle, maxVerticalAngle);

       
        float horizontalInput = Input.GetAxis("Horizontal"); 
        float currentHorizontalAngle = barrelPivot.localEulerAngles.y;
        if (currentHorizontalAngle > 180) currentHorizontalAngle -= 360;
        float newHorizontalAngle = Mathf.Clamp(currentHorizontalAngle + horizontalInput * rotationSpeed * Time.deltaTime, minHorizontalAngle, maxHorizontalAngle);

     
        barrelPivot.localEulerAngles = new Vector3(newVerticalAngle, newHorizontalAngle, 0f);

       
        if (Input.GetKeyDown(KeyCode.Space) && !isFiring)
        {
            StartCoroutine(FireCannonWithDelay());
        }
    }

    IEnumerator FireCannonWithDelay()
    {
        isFiring = true;

        // Play the fuse burning sound
        if (fuseAudioSource != null)
        {
            fuseAudioSource.Play();
        }

        yield return new WaitForSeconds(fireDelay);

        // Stop the fuse sound and fire the cannon
        if (fuseAudioSource != null && fuseAudioSource.isPlaying)
        {
            fuseAudioSource.Stop();
        }

        FireCannon();

        // Play the explosion sound
        if (explosionAudioSource != null)
        {
            explosionAudioSource.Play();
        }

        ApplyKnockback();

        // Trigger camera shake
        StartCoroutine(ShakeCamera());

        yield return new WaitForSeconds(resetDelay);

        ResetCannonPosition();

        isFiring = false;
    }

    void FireCannon()
    {
        if (cannonballPrefab != null && firePoint != null)
        {
            GameObject cannonball = Instantiate(cannonballPrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = cannonball.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.velocity = firePoint.forward * cannonballSpeed;
            }
        }
    }

    void ApplyKnockback()
    {
        if (cannonRigidbody != null)
        {
            cannonRigidbody.AddForce(-firePoint.forward * knockbackForce, ForceMode.Impulse);
        }
    }

    void ResetCannonPosition()
    {
        if (cannonRigidbody != null)
        {
            cannonRigidbody.MovePosition(initialPosition);
            cannonRigidbody.velocity = Vector3.zero;
        }
    }

    
    IEnumerator ShakeCamera()
    {
        if (cameraNoise != null)
        {
            cameraNoise.m_AmplitudeGain = shakeAmplitude;
            cameraNoise.m_FrequencyGain = shakeFrequency;

            yield return new WaitForSeconds(shakeDuration);

            cameraNoise.m_AmplitudeGain = 0f;
            cameraNoise.m_FrequencyGain = 0f;
        }
    }
}
