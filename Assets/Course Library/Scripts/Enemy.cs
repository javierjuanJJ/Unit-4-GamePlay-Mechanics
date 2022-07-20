using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    [SerializeField]
    [Range(0,50)]
    private float speed;
    private Rigidbody playerRb;

    private GameObject player;
    
    public bool isBoss = false;
    public float spawnInterval;
    private float nextSpawn;
    public int miniEnemySpawnCount;
    private SpawnManager spawnManager;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        
        if (isBoss)
        {
            spawnManager = FindObjectOfType<SpawnManager>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
        
        if(isBoss)
        {
            if(Time.time > nextSpawn)
            {
                nextSpawn = Time.time + spawnInterval;
                spawnManager.SpawnEnemyWave(spawnManager.PrefabRandom(spawnManager.miniEnemyPrefabs),miniEnemySpawnCount);
            }
        }
        
        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }

    private void FollowPlayer()
    {
        Vector3 positionEnemy = (player.transform.position - transform.position).normalized;
        playerRb.AddForce(positionEnemy * speed);
    }
}
