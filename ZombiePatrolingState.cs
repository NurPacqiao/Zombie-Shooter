using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombiePatrolingState : StateMachineBehaviour
{
    float timer;
    public float patrolingTime = 10f;
    Transform player;
    NavMeshAgent agent;
    public float detectionArea = 18f;
    public float patrolSpeed = 2f;
    List<Transform> waypointsList = new List<Transform>();
    Enemy enemy;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponent<Enemy>();
        if (enemy == null || enemy.isDead)
        {
            Debug.LogWarning("Enemy component missing or zombie is dead on " + animator.gameObject.name);
            return;
        }

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player not found! Ensure a GameObject with tag 'Player' exists.");
            return;
        }

        agent = animator.GetComponent<NavMeshAgent>();
        if (agent == null || !agent.enabled || !agent.isOnNavMesh)
        {
            Debug.LogWarning($"Agent on {animator.gameObject.name} is not ready in Patroling Enter (enabled: {agent?.enabled}, isOnNavMesh: {agent?.isOnNavMesh})");
            return;
        }

        agent.speed = patrolSpeed;
        timer = 0;

        GameObject waypointCluster = GameObject.FindGameObjectWithTag("Waypoints");
        if (waypointCluster == null)
        {
            Debug.LogError("Waypoint cluster with tag 'Waypoints' not found!");
            return;
        }

        waypointsList.Clear();
        foreach (Transform t in waypointCluster.transform)
        {
            waypointsList.Add(t);
        }

        if (waypointsList.Count == 0)
        {
            Debug.LogError("No waypoints found in the Waypoints cluster!");
            return;
        }

        Vector3 nextPosition = waypointsList[Random.Range(0, waypointsList.Count)].position;
        agent.SetDestination(nextPosition);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (enemy == null || enemy.isDead || agent == null || !agent.enabled || !agent.isOnNavMesh)
        {
            return;
        }

        if (SoundManager.Instance != null && !SoundManager.Instance.zombieChannel.isPlaying)
        {
            SoundManager.Instance.zombieChannel.clip = SoundManager.Instance.zombieWalking;
            SoundManager.Instance.zombieChannel.PlayDelayed(1f);
        }

        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            Vector3 nextPosition = waypointsList[Random.Range(0, waypointsList.Count)].position;
            agent.SetDestination(nextPosition);
        }

        timer += Time.deltaTime;
        if (timer > patrolingTime)
        {
            animator.SetBool("isPatroling", false);
        }

        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceFromPlayer < detectionArea)
        {
            animator.SetBool("isChasing", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (enemy == null || enemy.isDead || agent == null || !agent.enabled || !agent.isOnNavMesh)
        {
            return;
        }

        agent.SetDestination(agent.transform.position);
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.zombieChannel.Stop();
        }
    }
}