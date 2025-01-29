using Akila.FPSFramework;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    public float attackRange = 2.0f;
    public float startMoveDelay = 1.0f; // 몬스터가 이동을 시작하기 전 지연 시간

    private bool isAttacking = false; // 공격 중인지 확인하는 변수
    private bool canMove = false; // 이동 가능 여부
    private HealthSystem healthSystem;
    public float destroyDelay;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        healthSystem = GetComponent<HealthSystem>();

        // 1초 뒤에 이동 시작
        Invoke("StartMoving", startMoveDelay);
    }

    void Update()
    {
        if (canMove) // 이동이 가능할 때만 Move() 호출
        {
            Move();
        }
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

    void StartMoving()
    {
        canMove = true;
    }

    public void ifDie() // 몬스터가 죽었을 때 호출할 메서드
    {
        healthSystem = GetComponent<HealthSystem>();
        canMove = false; // 이동 중지
        agent.isStopped = true; // 이동 멈춤
        
        Destroy(gameObject, destroyDelay);
    }
}