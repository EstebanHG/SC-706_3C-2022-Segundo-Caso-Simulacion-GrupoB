using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CounterAttackController : MonoBehaviour
{
    [SerializeField]
    float damage = 5.0F;

    [SerializeField]
    Transform attackPoint;

    [SerializeField]
    float attackRadius = 0.3F;

    [SerializeField]
    LayerMask whatIsTarget;

    [SerializeField]
    float attackTimeout = 1.05F;

    [SerializeField]
    float attackLifeTime = 2.15F;

    [HideInInspector]
    public UnityEvent OnAttackEnded;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }

    public IEnumerator Attack()
    {
        yield return new WaitForSeconds(attackTimeout);

        Collider[] collisions =
            Physics.OverlapSphere
               (transform.position, attackRadius, whatIsTarget);

        if (collisions.Length == 0)
        {
            yield return null;
        }

        foreach (var collision in collisions)
        {

            DamageableController controller =
               collision.gameObject.GetComponent<DamageableController>();

            if (controller != null)
            {
                controller.TakeDamage(damage);
            }

        }
        yield return new WaitForSeconds(attackLifeTime - attackTimeout);
        OnAttackEnded?.Invoke();

    }

    /**void OnCollisionEnter(Collision collision) 
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            DamageableController controller =
               collision.gameObject.GetComponent<DamageableController>();

            if(controller != null)
            {
                controller.TakeDamage(damage);
            }
        }
    } **/
}
