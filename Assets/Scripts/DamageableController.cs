using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableController : MonoBehaviour
{
    [SerializeField]
    float deadAnimationDelay = 1.0F;
    HealthStatusController healthStatus;

    [SerializeField]
    bool useGlobalHealthStatus = true; 

    void Start()
    {
        if(useGlobalHealthStatus)
        {
            HealthStatusController[] controllers = FindObjectsOfType<HealthStatusController>();
            foreach (HealthStatusController controller in controllers)
            {
                if(controller.name.Equals("Health Status"))
                {
                    healthStatus = controller;
                    break;
                }
            }
        }
        else
        {
            healthStatus = GetComponent<HealthStatusController>();
        }

        healthStatus.onHealthChanged.AddListener(OnHealthChanged);
        healthStatus.onDie.AddListener(OnDie);

    }

    void OnHealthChanged(float health)
    {
        
    }

    void OnDie()
    {
        StartCoroutine(OnDieCoroutine());
    }

    IEnumerator OnDieCoroutine()
    {
        yield return new WaitForSeconds(deadAnimationDelay);
        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        healthStatus.Damage(damage);
    }
}
