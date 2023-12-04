using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    public NavMeshAgent Agent;
    public Animator EnemyAnimator;
    public Transform player;
    public LayerMask whatIsGround, Player;
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    public float TimePerAttack;
    bool JustAttacked;
    public float SightRange, AttackRange;
    public bool playerInSightRange, playerInAttackRange;
    public float BulletSpeed;
    public string EName;
    bool walkCooldown;

    public GameObject projectile;

    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.Find("Player").GetComponent<Transform>();

        Agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(player);
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, SightRange, Player);
        playerInAttackRange = Physics.CheckSphere(transform.position, AttackRange, Player);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();

    }

    private void Patroling()
    {
        EnemyAnimator.SetBool("Patroling", true);
        EnemyAnimator.SetBool("Chasing", false);
        EnemyAnimator.SetBool("Shooting", false);
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            Agent.SetDestination(walkPoint);
        }
            

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
            
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        Agent.SetDestination(player.position);
        EnemyAnimator.SetBool("Patroling", false);
        EnemyAnimator.SetBool("Chasing", true);
        EnemyAnimator.SetBool("Shooting", false);
    }

    private void AttackPlayer()
    {
        EnemyAnimator.SetBool("Patroling", false);
        EnemyAnimator.SetBool("Chasing", false);
        EnemyAnimator.SetBool("Shooting", true);
        //Make sure enemy doesn't move
        Agent.SetDestination(transform.position);

        //transform.LookAt(player);

        //Debug.Log(player.position);

        if (!JustAttacked)
        {
            ///Attack code here
            //GB();

            //projectile.transform.SetPositionAndRotation(transform.position, Quaternion.identity);

            //Rigidbody rb = projectile.GetComponent<Rigidbody>();

            ////projectile.transform.parent = null;
            //rb.velocity = transform.forward * BulletSpeed;
            //rb.AddForce(transform.up * 0f, ForceMode.Impulse);
            /////End of attack code

            JustAttacked = true;
            Invoke(nameof(ResetAttack), TimePerAttack);
        }
    }
    private void ResetAttack()
    {
        JustAttacked = false;
    }
    private void DestroyEnemy()
    {
        EnemyAnimator.SetBool("Patroling", false);
        EnemyAnimator.SetBool("Chasing", false);
        EnemyAnimator.SetBool("Shooting", false);
        EnemyAnimator.SetBool("Patroling", false);
        EnemyAnimator.SetBool("Died", true);


        AnimatorStateInfo stateinfo = EnemyAnimator.GetCurrentAnimatorStateInfo(0);
        //change to add back to object pool
        if (stateinfo.IsName("Die") && stateinfo.normalizedTime >= 1.0f)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, SightRange);
    }

    //public void TakeDamage(float DMG, string effect)
    //{
    //    Health -= DMG;
    //    //Debug.Log(Health);
    //    if (effect.CompareTo("Incendiary") == 0)
    //    {
    //        StartCoroutine(DMGOverTime(1));
    //    }

    //    if (Health <= 0)
    //    {

    //        DestroyEnemy();
    //    }

    //}

    //IEnumerator DMGOverTime(int time)
    //{
    //    while (Health != 0 && time != 5)
    //    {
    //        Health -= 5;
    //        yield return new WaitForSeconds(1);
    //        time++;
    //        //Debug.Log(Health);

    //    }

    //}
}