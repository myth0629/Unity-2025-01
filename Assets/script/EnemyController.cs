using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    public float attackRange = 2.0f;

    private bool isAttacking = false; // 공격 중인지 확인하는 변수

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= attackRange)
        {
            if (!isAttacking) // 공격 중이 아닐 때만 공격 시작
            {
                Attack();
            }
        }
        else
        {
            isAttacking = false; // 공격 범위에서 벗어나면 공격 상태 해제
            agent.SetDestination(player.position);
            // 이동 애니메이션 재생
            if (agent.velocity.magnitude > 0.1f) // 이동 중일 때만 애니메이션 재생
            {
                 animator.SetBool("Walk", true);
            }else{
                 animator.SetBool("Walk", false);
            }
           
        }
    }

    private void Attack()
    {
       
        isAttacking = true; // 공격 시작
        agent.isStopped = true; // 공격 중에는 이동 멈춤
        animator.SetTrigger("Attack"); // 공격 애니메이션 재생
        //공격 애니메이션이 끝나면 agent.isStopped=false가 되어야함
        Invoke("ResetIsStopped", animator.GetCurrentAnimatorStateInfo(0).length);
    }
    void ResetIsStopped()
    {
        agent.isStopped = false;
    }
}