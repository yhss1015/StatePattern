using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{

    private int comboCounter;

    private float lastTimeAttacked;
    private float comboWindow = 2;

    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if(comboCounter>2||Time.time>=lastTimeAttacked+comboWindow)
        {
            comboCounter = 0;
        }
        Debug.Log("콤보 카운터 : " + comboCounter);
        player.anim.SetInteger("ComboCounter", comboCounter);


        float attackDir = player.facingDir;
        if(xInput!=0)
        {
            attackDir = xInput;
        }

        
        player.SetVelocity(player.playerMovement[comboCounter].x*attackDir, player.playerMovement[comboCounter].y);

        stateTimer = 0.1f; //멈추기 전 텀시간
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine(player.BusyFor(0.1f));

        comboCounter++;
        lastTimeAttacked = Time.time;

        Debug.Log("마지막 공격 시간 : " + lastTimeAttacked);
    }

    public override void Update()
    {
        base.Update();

        
        if(stateTimer<0)
        {
            player.ZeroVelocity();
        }

        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
