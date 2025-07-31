using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class EnemyAi : MonoBehaviour
{
    [SerializeField] GameObject core;

    [SerializeField] AudioSource enemyHiding;
    [SerializeField] AudioSource enemyReveal;
    [SerializeField] AudioSource enemyHunting;
    [SerializeField] AudioSource enemyDeath;    

    [SerializeField] Transform target;
    [SerializeField] float chaseRange = 5f;

    [SerializeField] bool showGizmos = true;

    [SerializeField] GameObject mesh;
    [SerializeField] float turnSpeed = 5f;
    NavMeshAgent navMeshAgent;
    CapsuleCollider enemyCollider;

    [SerializeField] ParticleSystem unHideParticles;
    [SerializeField] Material[] material;
    
    [SerializeField] GameObject cloneHead;
    [SerializeField] GameObject cloneBody;
    Renderer headRenderer;
    Renderer bodyRenderer;

    float distanceToTarget = Mathf.Infinity;
    bool isProvoked = false;
    bool isHided = true;
    EnemyHealth health;

    EnemyBurn burnEnemy;

    bool restored = false;

    void Awake()
    {
        if (cloneHead != null)
            headRenderer = cloneHead.GetComponent<SkinnedMeshRenderer>();

        if (cloneBody != null)
            bodyRenderer = cloneBody.GetComponent<SkinnedMeshRenderer>();

        burnEnemy = GetComponent<EnemyBurn>();

        core.SetActive(false);

        target = FindObjectOfType<PlayerHealth>().transform;

        headRenderer = cloneHead.GetComponent<Renderer>();
        bodyRenderer = cloneBody.GetComponent<SkinnedMeshRenderer>();

        headRenderer.enabled = true;
        bodyRenderer.enabled = true;

        headRenderer.sharedMaterial = material[0];
        bodyRenderer.sharedMaterial = material[0];

        headRenderer.shadowCastingMode = ShadowCastingMode.Off;
        headRenderer.receiveShadows = false;

        bodyRenderer.shadowCastingMode = ShadowCastingMode.Off;
        bodyRenderer.receiveShadows = false;

        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyCollider = GetComponent<CapsuleCollider>();
        health = GetComponent<EnemyHealth>();
        //collider.enabled = false;
        //mesh.SetActive(false);


    }
   
    void Update()
    {
        if (restored == false)
        {
            RestoreState(health.isDead, burnEnemy.isBurned);
        }

        if (health.IsDead())
        {
            enemyCollider.center = new Vector3 (0f, -0.75f, 0f);
            enemyCollider.radius = 1.5f;
            enemyCollider.height = 4f;
            enemyDeath.Play();
            StopSounds();
            if (isHided)
            {
                StopHiding();

            }
            enabled = false;
            navMeshAgent.enabled = false;
            return;
        }
        distanceToTarget = Vector3.Distance(target.position, transform.position);

        if (isProvoked)
        {
            EngageTarget();
        }
        else if (distanceToTarget <= chaseRange)
        {
            isProvoked = true;
        }
    }

    public void Die()
    {
        if (enemyCollider == null)
            enemyCollider = GetComponent<CapsuleCollider>();

        if (enemyCollider != null)
        {
            enemyCollider.radius = 1f;
            enemyCollider.height = 1f;
            enemyCollider.enabled = true;
        }

        if (enemyDeath != null)
            enemyDeath.Play();

        StopSounds();

        if (isHided)
        {
            StopHiding();
        }

        if (navMeshAgent == null)
            navMeshAgent = GetComponent<NavMeshAgent>();

        if (navMeshAgent != null)
            navMeshAgent.enabled = false;

        enabled = false;
    }

    private void StopSounds()
    {
        enemyHunting.Stop();
        enemyHiding.Stop();
    }

    public void OnDamageTaken()
    {
        isProvoked = true;
    }

    private void EngageTarget()
    {
        FaceTarget();
        if (distanceToTarget >= navMeshAgent.stoppingDistance)
        {
            GetComponent<Animator>().SetBool("attack", false);
            ChaseTarget();
        }

        if (distanceToTarget <= (navMeshAgent.stoppingDistance * 2) && isHided)
        {
            StopHiding();
        }

        if (distanceToTarget <= navMeshAgent.stoppingDistance)
        {
            AttackTarget();
        }
    }

    private void StopHiding()
    {
        if (core != null)
        {
            core.SetActive(true);
            core.GetComponent<Rigidbody>().isKinematic = true;
        }

        if (unHideParticles != null)
            unHideParticles.Play();

        if (enemyReveal != null)
            enemyReveal.Play();

        if (enemyHunting != null)
            enemyHunting.Play();

        if (headRenderer != null && material.Length > 1)
        {
            headRenderer.sharedMaterial = material[1];
            headRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            headRenderer.receiveShadows = true;
        }

        if (bodyRenderer != null && material.Length > 1)
        {
            bodyRenderer.sharedMaterial = material[1];
            bodyRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            bodyRenderer.receiveShadows = true;
        }

        isHided = false;
    }

    private void ChaseTarget()
    {
        GetComponent<Animator>().SetTrigger("move");
        //mesh.SetActive(true);
        //collider.enabled = true;
        navMeshAgent.SetDestination(target.position);
    }


    private void AttackTarget()
    {
        GetComponent<Animator>().SetBool("attack", true);
    }

    private void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    void OnDrawGizmosSelected()
    {
        if (showGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseRange);
        }
    }

    public void RestoreState(bool isDead, bool isBurned)
    {
        if (isBurned)
        {
            gameObject.SetActive(false);
            restored = true;
            return;
        }

        if (isDead)
        {
            health.ForceDeadState();
            Die();
            restored = true;
        }
    }

}
