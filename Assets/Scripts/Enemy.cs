using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] protected LayerMask whatIsPlayer;

    [Header("�̵� ����")]
    public float moveSpeed = 10f;
    public float idleTime;
    public float battleTime;

    [Header("���� ����")]
    public float attackDistance;
    public float attackCooldown;
    [HideInInspector]public float lastTimeAttacked;

    public EnemyStateMachine stateMachine { get;private set; }


    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();

        //Debug.Log(IsPlayerDetected().collider.gameObject.name + "�÷��̾� ��������.");
    }

    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 50, whatIsPlayer);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));
    }
}
