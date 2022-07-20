using System.Collections;
using System.Collections.Generic;
using Course_Library.Scripts;
using UnityEngine;

public class PowerUp : MonoBehaviour
{

    [SerializeField]
    [Range(-50,50)]
    private float point;
    public PowerUpType powerUpType;
    
    

    public float Point => point;

    // Start is called before the first frame update
    void Start()
    {
        
    }

}
