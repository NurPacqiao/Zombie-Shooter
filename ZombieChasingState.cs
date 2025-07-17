using UnityEngine;
using UnityEngine.AI;

public class ZombieChasingState : StateMachineBehaviour
{
    NavMeshAgent agent;
    Transform player;
    public float chaseSpeed = 6f;
    public float stopChasingDistance = 21f;
    public float attackingDistance = 2.5f;
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
            Debug.LogWarning($"Agent on {animator.gameObject.name} is not ready in Chasing Enter (enabled: {agent?.enabled}, isOnNavMesh: {agent?.isOnNavMesh})");
            return;
        }

        agent.speed = chaseSpeed;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (enemy == null || enemy.isDead || agent == null || !agent.enabled || !agent.isOnNavMesh)
        {
            return;
        }

        if (SoundManager.Instance != null && !SoundManager.Instance.zombieChannel.isPlaying)
        {
            SoundManager.Instance.zombieChannel.PlayOneShot(SoundManager.Instance.zombieChase);
        }

        agent.SetDestination(player.position);
        animator.transform.LookAt(player);

        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);

        if (distanceFromPlayer > stopChasingDistance)
        {
            animator.SetBool("isChasing", false);
        }

        if (distanceFromPlayer < attackingDistance)
        {
            animator.SetBool("isAttacking", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (enemy == null || enemy.isDead || agent == null || !agent.enabled || !agent.isOnNavMesh)
        {
            return;
        }

        agent.SetDestination(animator.transform.position);
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.zombieChannel.Stop();
        }
    }
}