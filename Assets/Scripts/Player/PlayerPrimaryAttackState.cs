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
        Debug.Log("�޺� ī���� : " + comboCounter);
        player.anim.SetInteger("ComboCounter", comboCounter);


        float attackDir = player.facingDir;
        if(xInput!=0)
        {
            attackDir = xInput;
        }

        
        player.SetVelocity(player.playerMovement[comboCounter].x*attackDir, player.playerMovement[comboCounter].y);

        stateTimer = 0.1f; //���߱� �� �ҽð�
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine(player.BusyFor(0.1f));

        comboCounter++;
        lastTimeAttacked = Time.time;

        Debug.Log("������ ���� �ð� : " + lastTimeAttacked);
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
