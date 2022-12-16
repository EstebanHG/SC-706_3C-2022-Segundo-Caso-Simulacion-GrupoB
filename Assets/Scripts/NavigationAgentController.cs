using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationAgentController : MonoBehaviour
{
    [SerializeField]
    Transform target;

    [SerializeField]
    float chaseRange = 5.0F;

    [SerializeField]
    float rotationSpeed = 2.5F;

    [SerializeField]
    Transform attackPoint;

    [SerializeField]
    float attackRange = 0.5F;

    [SerializeField]
    float attackDamage =  1.0F;

    NavMeshAgent navAgent;
    Animator animator; 

    CounterAttackController counterAttack;

    float distanceToTarget;

    bool chaseTarget;
    bool wasChaseTarget;
    bool isAttacking;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.updateRotation = false;

        counterAttack = GetComponent<CounterAttackController>();
        counterAttack.OnAttackEnded.AddListener(OnAttackEnded);
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        distanceToTarget = 
            Vector3.Distance(target.position, transform.position);
        
        wasChaseTarget = chaseTarget;

        chaseTarget = 
            (distanceToTarget < chaseRange);
        

        if (chaseTarget)
        {
            EngageTarget();
        }
        else 
        {
           StopChasingTarget();
        }
        
        if(distanceToTarget <= navAgent.stoppingDistance)
        {
            AttackTarget();
        }
        

    }

    void StopChasingTarget()
    {
        if (!wasChaseTarget)
        {
            animator.SetBool("isWalking", false);
            ChaseTarget(transform.position);
        }

    }

    void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }

    void EngageTarget()
    {
        //if(!wasChaseTarget)
        //{
            animator.SetBool("isWalking", true);
        //}
        
        FaceTarget();
        ChaseTarget(target.position);
    }

    void FaceTarget()
    {
        Vector3 direction = 
           -(target.position - transform.position).normalized;

        Quaternion lookRotation =
            Quaternion.LookRotation
                (new Vector3(direction.x, 0.0F, direction.z));
        
        transform.rotation = 
            Quaternion.Slerp 
               (transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

    void AttackTarget()
    {
        if(isAttacking)
        {
            return;
        }

        if(!isAttacking)
        {
           isAttacking = true;
           animator.SetTrigger("Attack");
           StartCoroutine(counterAttack.Attack());
        }
        
    }

    void ChaseTarget(Vector3 position)
    {
        navAgent.SetDestination(position);
    }

    void OnAttackEnded()
    {
        isAttacking = false;
    }
}
