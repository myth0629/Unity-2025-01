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

    public bool isAttacking = false; // 공격 중인지 확인하는 변수
    private bool canMove = false; // 이동 가능 여부
    private HealthSystem healthSystem;
    public float destroyDelay;
    public float attackCooldown = 1.5f; // 공격 쿨타임
    private float nextAttackTime = 0f; // 다음 공격 가능 시간
    public int damage = 10;

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
            Collider playerCollider = player.GetComponent<Collider>();
            // 공격 범위 내에 있으면 계속 Attack() 호출
            if(playerCollider != null)
            {
                Attack(playerCollider);
            }
        }
        else
        {
            isAttacking = false;
            agent.isStopped = false;
            agent.SetDestination(player.position);
            
            if (agent.velocity.magnitude > 0.1f)
            {
                animator.SetBool("Walk", true);
            }
            else
            {
                animator.SetBool("Walk", false);
            }
        }
    }

    private void Attack(Collider playerCollider)
    {
        if (Time.time >= nextAttackTime) // 현재 시간이 다음 공격 가능 시간보다 크거나 같으면
        {
            healthSystem = playerCollider.GetComponent<HealthSystem>();

            if(healthSystem != null)
            {
                healthSystem.DoDamage(damage, GetComponent<Actor>());
            }

            isAttacking = true;
            animator.SetTrigger("Attack");
            agent.isStopped = true; // 공격 시 이동 중지
            
            // 다음 공격 가능 시간 설정
            nextAttackTime = Time.time + attackCooldown;
            
            // 공격 애니메이션이 끝나면 이동 재개
            Invoke("ResetIsStopped", animator.GetCurrentAnimatorStateInfo(0).length);
            Invoke("AttackCheck", animator.GetCurrentAnimatorStateInfo(0).length);

        }
    }

    void AttackCheck()
    {
        isAttacking = false;
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
        agent.enabled = false;
        Debug.Log("Die");
        animator.SetTrigger("Death");
        

        canMove = false; // 이동 중지
        animator.SetBool("Walk", false);
        agent.isStopped = true; // 이동 멈춤
        

        
        Destroy(gameObject, destroyDelay);
    }
}