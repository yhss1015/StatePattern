using UnityEngine;

public class PlayerCatchSwordSate : PlayerState
{
    private Transform sword;

    public PlayerCatchSwordSate(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        sword = player.sword.transform;

        if (player.transform.position.x > sword.position.x && player.facingDir == 1)
        {
            player.Flip();
        }
        else if (player.transform.position.x < sword.position.x && player.facingDir == -1)
        {
            player.Flip();
        }

        rb.linearVelocity = new Vector2(player.swordReturnImpact * -player.facingDir, rb.linearVelocity.y);
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", 0.1f);
        
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }


    }
}
