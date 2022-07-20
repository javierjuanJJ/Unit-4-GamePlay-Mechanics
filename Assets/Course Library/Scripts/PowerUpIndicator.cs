using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpIndicator : MonoBehaviour
{

    private PlayerController player;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ChangeActive(true);
        player.PowerUpIndicators = this;
        ChangeActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    public void ChangeActive(bool active)
    {
        gameObject.SetActive(active);
    }
    
    public void ChangePosition(Vector3 position)
    {
        gameObject.transform.position = position;
    }
    
}
