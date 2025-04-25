using System.Collections;
using UnityEngine;

public class Player : Entity
{
    public bool isBusy { get; private set; }
    [Header("���� ����")]
    public Vector2[] attackMovement;
    public float counterAttackDuration = 0.2f;
    


    [Header("�̵� ����")]
    public float moveSpeed = 10f;
    public float jumpForce = 10f;
    public float swordReturnImpact;

    [Header("��� ����")]    
    public float dashSpeed;
    public float dashDuration;
    public float dashDir { get; private set; }
    public float slidingSpeed;         

    public SkillManager skill { get; private set; }

    public GameObject sword;
    
        
    #region States
    // �÷��̾��� ���¸� �����ϴ� ���� �ӽ�
    public PlayerStateMachine stateMachine { get; private set; }

    // �÷��̾��� ���� (��� ����, �̵� ����)
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJump { get; private set; }
    public PlayerPrimaryAttackState primaryAttack { get; private set; }
    public PlayerCounterAttackState counterAttack { get; private set; }
    public PlayerAimSwordState aimSword { get; private set; }
    public PlayerCatchSwordSate catchSword { get; private set; }
    public PlayerBlackholeState blackHole { get; private set; }

    public PlayerDeadState deadState { get; private set; }
    
    #endregion

    protected override void Awake()
    {
        // ���� �ӽ� �ν��Ͻ� ����
        stateMachine = new PlayerStateMachine();

        // �� ���� �ν��Ͻ� ���� (this: �÷��̾� ��ü, stateMachine: ���� �ӽ�, "Idle"/"Move": ���� �̸�)
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJump = new PlayerWallJumpState(this, stateMachine, "Jump");

        primaryAttack = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttack = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");

        aimSword = new PlayerAimSwordState(this, stateMachine, "AimSword");
        catchSword = new PlayerCatchSwordSate(this, stateMachine, "CatchSword");
        blackHole = new PlayerBlackholeState(this, stateMachine, "Jump");
        deadState = new PlayerDeadState(this, stateMachine, "Die");
        
    }

    protected override void Start()
    {
        base.Start();

        skill = SkillManager.instance;

        stateMachine.Initialize(idleState); //idle���·� �ʱ�ȭ
    }



    protected override void Update()
    {
        base.Update();

        stateMachine.currentstate.Update();
        CheckForDashInput();

        if (Input.GetKeyDown(KeyCode.H))
        {
            skill.crystal.CanUseSkill();
        }
    }

    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }

    public void ClearTheSword()
    {
        stateMachine.ChangeState(catchSword);
        Destroy(sword);
    }

    
    
    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }

    public void AnimationTrigger() => stateMachine.currentstate.AnimationFinishTrigger();

    private void CheckForDashInput()
    {
        if(IsWallDetected())
        {
            return;
        }

        

        if (Input.GetKeyDown(KeyCode.LeftShift)&&skill.dash.CanUseSkill())
        {
            

            dashDir = Input.GetAxisRaw("Horizontal");
            if (dashDir == 0)
            {
                dashDir = facingDir;
            }
            stateMachine.ChangeState(dashState);
        }
    }


    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);
    }




}
