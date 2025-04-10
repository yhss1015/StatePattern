using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private Transform player;
    private Enemy_Skeleton enemy;
    private float moveDir;
    

    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
       

    public override void Update()
    {
        base.Update();

        if(enemy.IsPlayerDetected())
        {

            stateTimer = enemy.battleTime;

            if(enemy.IsPlayerDetected().distance<enemy.attackDistance)
            {
                Debug.Log("����");
                if(CanAttack()) // ��ٿ��� �ƴϸ� ����
                {
                    stateMachine.ChangeState(enemy.attackState);
                }
                
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position,enemy.transform.position)>7)
            {
                stateMachine.ChangeState(enemy.idleState);
            }
        }

        if (player.position.x > enemy.transform.position.x)
        {
            moveDir = 1;
        }
        else if (player.position.x < enemy.transform.position.x)
        {
            moveDir = -1;
        }
        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.linearVelocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool CanAttack()
    {
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.lastTimeAttacked = Time.time;
            return true;
        }
        Debug.Log("���� ��ٿ�");
        return false;
    }
}
