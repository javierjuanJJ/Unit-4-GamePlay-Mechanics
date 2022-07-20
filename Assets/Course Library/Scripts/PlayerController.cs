using System;
using System.Collections;
using System.Collections.Generic;
using Course_Library.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    [Range(0,50)]
    private float speed = 5.0f;
    
    private float verticalInput, powerUpStrenght;
    
    private Rigidbody playerRb;

    private GameObject focalPoint;
    
    private PowerUpIndicator powerUpIndicators;
    
    private bool hasPowerUp;
    
    public PowerUpType currentPowerUp = PowerUpType.None;
    
    public GameObject rocketPrefab;
    private GameObject tmpRocket;
    private Coroutine powerupCountdown;
    
    
    public float hangTime;
    public float smashSpeed;
    public float explosionForce;
    public float explosionRadius;
    bool smashing = false;
    float floorY;


    public PowerUpIndicator PowerUpIndicators
    {
        get => powerUpIndicators;
        set => powerUpIndicators = value;
    }

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
        powerUpStrenght = 15.0f;
        hasPowerUp = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public float Speed => speed;

    // Update is called once per frame
    void Update()
    {
        verticalInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * (speed * verticalInput));
        
        if (PowerUpIndicators != null)
        {
            ChangePosition(new Vector3(0, -0.5f, 0) + transform.position);
        }
        
        if (currentPowerUp == PowerUpType.Rockets && Input.GetKeyDown(KeyCode.F))
        {
            LaunchRockets();
        }
        
        if(currentPowerUp == PowerUpType.Smash && Input.GetKeyDown(KeyCode.Space) && !smashing)
        {
            smashing = true;
            StartCoroutine(Smash());
        }

        if (transform.position.y < -10)
        {
            SceneManager.LoadScene(0);
        }
        
        
    }

    
    private void ChangeActive(bool active)
    {
        PowerUpIndicators.ChangeActive(active);
    }

    private void ChangePosition(Vector3 position)
    {
        PowerUpIndicators.ChangePosition(position);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            GameObject powerUp = other.gameObject;
            hasPowerUp = true;
            currentPowerUp = other.gameObject.GetComponent<PowerUp>().powerUpType;
            powerUpStrenght += powerUp.GetComponent<PowerUp>().Point;
            Destroy(powerUp);
            
            if(powerupCountdown != null)
            {
                StopCoroutine(powerupCountdown);
            }
            powerupCountdown = StartCoroutine(PowerupCountdownRoutine());
            
            StartCoroutine(PowerupCountdownRoutine());
            ChangeActive(true);
        }
        
    }
    
    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerUp = false;
        currentPowerUp = PowerUpType.None;
        ChangeActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject collisionGameObject = collision.gameObject;
        
        if (collisionGameObject.CompareTag("Enemy") && hasPowerUp && currentPowerUp == PowerUpType.Pushback)
        {
            Rigidbody enemyRigidbody = collisionGameObject.GetComponent<Rigidbody>();
            Vector3 awayFrom = collision.transform.position - transform.position;
            
            Debug.Log("Collided with " + collisionGameObject.name + " with powerup set to " + hasPowerUp);
            
            enemyRigidbody.AddForce(powerUpStrenght * awayFrom, ForceMode.Impulse);
            Debug.Log("Player collided with: " + collisionGameObject.name + " with powerup set to " + currentPowerUp);
        }
    }
    
    void LaunchRockets()
    {
        foreach(var enemy in FindObjectsOfType<Enemy>())
        {
            tmpRocket = Instantiate(rocketPrefab, transform.position + Vector3.up, Quaternion.identity);
            tmpRocket.GetComponent<RocketBehaviour>().Fire(enemy.transform);
        }
    }
    
    IEnumerator Smash()
    {
        var enemies = FindObjectsOfType<Enemy>();
        
        //Store the y position before taking off
        floorY = transform.position.y;
        
        //Calculate the amount of time we will go up
        float jumpTime = Time.time + hangTime;
        
        while(Time.time < jumpTime)
        {
            //move the player up while still keeping their x velocity.
            playerRb.velocity = new Vector2(playerRb.velocity.x, smashSpeed);
            yield return null;
        }
        
        //Now move the player down
        while(transform.position.y > floorY)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, -smashSpeed * 2);
            yield return null;
        }
        
        //Cycle through all enemies.
        for (int i = 0; i < enemies.Length; i++)
        {
            //Apply an explosion force that originates from our position.
            if(enemies[i] != null)
                enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius, 0.0f, ForceMode.Impulse);
        }
        
        //We are no longer smashing, so set the boolean to false
        smashing = false;
    }
    
}
