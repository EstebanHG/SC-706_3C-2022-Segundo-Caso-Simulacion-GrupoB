using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField]
    float speed = 1500.0F;

    [SerializeField]
    float damage = 25.0F;

    Rigidbody rb;

    void Awake() 
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start() 
    {
        rb.velocity = transform.forward * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other) 
    {
        Transform otherObject = other.transform;
        if(otherObject != null)
        {
            DamageableController controller = 
               otherObject.GetComponent<DamageableController>();
            if(controller != null)
            {
                controller.TakeDamage(damage);
            }
        }
        Destroy(gameObject);
    }
}
