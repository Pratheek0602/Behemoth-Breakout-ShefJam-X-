using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent navMeshAgent;
    public Animator animator;
    public float startWaitTime = 4;
    public float timeToRotate = 2;
    public float speedWalk = 6;
    public float speedRun = 9;

    public float viewRaduis = 15;
    public float viewAngle = 90;
    public LayerMask playerMask;
    public LayerMask whatIsWallMask;
    public float meshResolution = 1f;
    public int edgeIterations = 4;
    public float edgeDistance = 0.5f;

    public Transform[] waypoints;
    int m_CurrentWaypointIndex;

    Vector3 playerLastPosition = Vector3.zero;
    Vector3 m_PlayerPosition;
    
    float m_WaitTime;
    float m_TimeToRotate;
    bool m_PlayerInRange;
    bool m_PlayerNear;
    bool m_IsPatrol;
    bool m_CaughtPlayer;

    // Start is called before the first frame update
    void Start()
    {
        m_PlayerPosition = Vector3.zero;
        m_IsPatrol = true;
        m_CaughtPlayer = false;
        m_PlayerInRange = false;
        m_WaitTime = startWaitTime;
        m_TimeToRotate = timeToRotate;

        m_CurrentWaypointIndex = 0;
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speedWalk;
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    }

    // Update is called once per frame
    void Update()
    {
        EnvironmentView();

        if (!m_IsPatrol) {
            // Debug.Log("chasing");
            Chasing();
        }
        else {
            // Debug.Log("pattroling");
            Patroling();
        }
        
    }

    public void Die(){
        Stop();
        // animator.SetBool("isDead", true);
        // Debug.Log("shinda");
        // StartCoroutine(DelayedAction());
        animator.enabled = false;
        navMeshAgent.enabled = false;
        enabled = false;
        // yield return new WaitForSeconds(2f);
        // animator.enabled = false; // Disable the animator component
        // navMeshAgent.enabled = false;
    }

    // IEnumerator DelayedAction()
    // {
    //     // Wait for 3 seconds
    //     // yield return new WaitForSeconds(2f);
    //     animator.enabled = false;
    //     navMeshAgent.enabled = false;
    //     enabled = false;

    //     // Execute your action after the delay
    //     // Debug.Log("Delayed action executed!");
    // }

    // private void OnHitCollisionEnter(Collision collision)
    // {
    //     // Check if the collision is with the enemy
    //     if (collision.gameObject.CompareTag("Enemy"))
    //     {
    //         Debug.Log("GET WRECKED");
    //         // Calculate the direction away from the enemy
    //         Vector3 pushDirection = (transform.position - collision.transform.position).normalized;
    //         // Apply a force to push the player back
    //         GetComponent<Rigidbody>().AddForce(pushDirection * hitForce, ForceMode.Impulse);
    //     }
    // }

    private void Chasing() {
        animator.SetBool("isSwiping", false);
        m_PlayerNear = false;
        playerLastPosition = Vector3.zero;

        // Check if the enemy is close enough to the player
        if (Vector3.Distance(transform.position, m_PlayerPosition) <= 10) {
            // If the enemy is close enough, consider it as "reached"
            // Debug.Log("close" + m_PlayerPosition);
            Vector3 destination = m_PlayerPosition - (transform.forward * 2f);
            navMeshAgent.SetDestination(destination);
            CaughtPlayer();
        }

        if (!m_CaughtPlayer) {
            // Debug.Log("??????????????");
            Move(speedRun);
            navMeshAgent.SetDestination(m_PlayerPosition);
        }
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) {
            if (m_WaitTime <= 0 && !m_CaughtPlayer && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f) {
                m_IsPatrol = true;
                m_PlayerNear = false;
                Move(speedWalk);
                m_TimeToRotate = timeToRotate;
                m_WaitTime = startWaitTime;
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
            }
            else{
                if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f) {
                    // Stop();
                    m_WaitTime -= Time.deltaTime;
                    Move(speedRun);
                    navMeshAgent.SetDestination(m_PlayerPosition);
                }
            }
        }
        else{
            m_IsPatrol = true;
        }
    }

    private void Patroling(){
        if (m_PlayerNear) {
            if (m_TimeToRotate <= 0) {
                Move(speedWalk);
                LookingPlayer(playerLastPosition);
            }
            else{
                Stop();
                m_TimeToRotate -= Time.deltaTime;
            }
        }
        else {
            m_PlayerNear = false;
            animator.SetBool("isSwiping", false);
            playerLastPosition = Vector3.zero;
            if (m_CurrentWaypointIndex < 0 || m_CurrentWaypointIndex >= waypoints.Length) {
                m_CurrentWaypointIndex = 0;
            }
            else{
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);

            }
            
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) {
                if (m_WaitTime <= 0) {
                    NextPoint();
                    Move(speedWalk);
                    m_WaitTime = startWaitTime;
                }
                else{
                    Stop();
                    m_WaitTime -= Time.deltaTime;
                }
            }
        }
    }

    void Move(float speed) {
        // animator.SetBool("isStopping", false);
        animator.SetBool("isSwiping", false);
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }

    void Stop(){
        // Debug.Log("Should stop");
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
    }

    public void NextPoint() {
        m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    }

    void CaughtPlayer(){
        m_CaughtPlayer = true;
        animator.SetBool("isSwiping", true);
        // m_IsPatrol = true;
    }

    void LookingPlayer(Vector3 player) {
        navMeshAgent.SetDestination(player);
        if (Vector3.Distance(transform.position, player) <= 0.3) {
            if (m_WaitTime <= 0) {
                m_PlayerNear = false;
                Move(speedWalk);
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
                m_WaitTime = startWaitTime;
                m_TimeToRotate = timeToRotate;
            }
            else {
                Stop();
                m_WaitTime -= Time.deltaTime;
            
            }
        }
    }

    void EnvironmentView() {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRaduis, playerMask);

        for (int i = 0; i < playerInRange.Length; i++) {
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2) {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, whatIsWallMask)) {
                    m_PlayerInRange = true;
                    m_IsPatrol = false;
                }
                else{
                    m_PlayerInRange = false;
                }
            }
            if (Vector3.Distance(transform.position, player.position) > viewRaduis) {
                m_PlayerInRange = false;
            }
        
            if (m_PlayerInRange) {
                m_PlayerPosition = player.transform.position;
            }
        }
    }


}
