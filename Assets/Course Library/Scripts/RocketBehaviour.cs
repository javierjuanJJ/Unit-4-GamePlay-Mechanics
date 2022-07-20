using System;
using UnityEngine;

namespace Course_Library.Scripts
{
    public class RocketBehaviour : MonoBehaviour
    {
        private Transform target;
        private float speed;
        private bool homing;
        private float rocketStrength;
        private float aliveTimer;

        private void Awake()
        {
            speed = 15.0f;
            rocketStrength = 15.0f;
            aliveTimer = 5.0f;
        }

        public void Fire(Transform newTarget)
        {
            target = newTarget;
            homing = true;
            Destroy(gameObject, aliveTimer);
        }

        // Update is called once per frame
        void Update()
        {
            if(homing && target != null)
            {
                Vector3 moveDirection = (target.transform.position - transform.position).normalized;
                transform.position += moveDirection * (speed * Time.deltaTime);
                transform.LookAt(target);
            }
        }
    
        void OnCollisionEnter(Collision col)
        {
            if (target != null)
            {
                if (col.gameObject.CompareTag(target.tag))
                {
                    Rigidbody targetRigidbody = col.gameObject.GetComponent<Rigidbody>();
                    Vector3 away = -col.contacts[0].normal;
                    targetRigidbody.AddForce(away * rocketStrength, ForceMode.Impulse);
                    Destroy(gameObject);
                }
            }
        }
        
    }
}