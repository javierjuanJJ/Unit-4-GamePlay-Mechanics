using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject powerUpIndicatorPrefab;

    [SerializeField]
    private GameObject[] powerUpsPrefab, enemiesPrefab;
    
    [SerializeField]
    [Range(0,50)]
    private float posisitonRandom = 9;
    
    [SerializeField]
    [Range(0,10)]
    private int numberEnemies = 1;
    
    private int waveNumber;
    
    public GameObject bossPrefab;
    public GameObject[] miniEnemyPrefabs;
    public int bossRound;
    
    private void Awake()
    {
        waveNumber = 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemyWave(PrefabRandom(enemiesPrefab), numberEnemies);
        SpawnEnemyWave(PrefabRandom(powerUpsPrefab), numberEnemies);
        CreatePrefab(powerUpIndicatorPrefab);
    }
    
    public GameObject PrefabRandom(GameObject[] typePrefab)
    {
        int index = RandomNumber(0 , typePrefab.Length);
        return typePrefab[index];
    }

    public void SpawnEnemyWave(GameObject prefab, int enemies)
    {
        for (int count = 0; count < enemies; count++)
        {
            CreatePrefab(prefab);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isEmptyArray(FindObjectsOfType<Enemy>()) || isEmptyArray(FindObjectsOfType<PowerUp>()))
        {
            GameObject prefab = isEmptyArray(FindObjectsOfType<Enemy>()) ? PrefabRandom(enemiesPrefab) : PrefabRandom(powerUpsPrefab);
            SpawnEnemyWave(prefab,waveNumber);
            waveNumber++;
            
            //Spawn a boss every x number of waves
            if (waveNumber % bossRound == 0)
            {
                CreatePrefab(bossPrefab).GetComponent<Enemy>().miniEnemySpawnCount = bossRound != 0 ? waveNumber / bossRound : 1;
            }
            else
            {
                SpawnEnemyWave(prefab,waveNumber);
            }
            
            //Updated to select a random powerup prefab for the Medium Challenge
            CreatePrefab(PrefabRandom(powerUpsPrefab));
        }
    }

    private bool isEmptyArray(object[] findObjectsOfType)
    {
        return findObjectsOfType.Length == 0;
    }

    private GameObject CreatePrefab(GameObject gameObjectToCreate)
    {
        Vector3 positionEnemy = PositionRandom(posisitonRandom);
        Quaternion gameObjectToCreateRotation = gameObjectToCreate.transform.rotation;
        return Instantiate(gameObjectToCreate, positionEnemy, gameObjectToCreateRotation);
    }

    private Vector3 PositionRandom(float positionRandom)
    {
        float XPosition = RandomNumber(-positionRandom,positionRandom);
        float YPosition = 0;
        float ZPosition = RandomNumber(-positionRandom,positionRandom);

        return new Vector3(XPosition, YPosition, ZPosition);
    }

    private float RandomNumber(float startNumber, float endNumber)
    {
        return Random.Range(startNumber, endNumber);
    }
    
    private int RandomNumber(int startNumber, int endNumber)
    {
        return Random.Range(startNumber, endNumber);
    }
}
