using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        stateTimer = player.counterAttackDuration;
        player.anim.SetBool("SuccessCounter", false);
    }

    public override void Exit()
    {
        base.Exit();

        player.anim.SetBool("SuccessCounter", false);
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                if (hit.GetComponent<Enemy>().CanBeStunned())
                {
                    stateTimer = 10f;
                    player.anim.SetBool("SuccessCounter", true);
                }
            }
        }

        if(stateTimer<0 || triggerCalled)
        {            
            stateMachine.ChangeState(player.idleState);
        }
    }
}
