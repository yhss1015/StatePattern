using UnityEngine;
/// <summary>
/// �÷��̾��� ���¸� �����ϴ� �⺻ Ŭ����.
/// �� Ŭ������ ����Ͽ� �پ��� �÷��̾� ����(��: Idle, Running, Jumping ��)�� ������ �� ����.
/// </summary>
public class PlayerState 
{
    
    protected PlayerStateMachine stateMachine;  //�÷��̾��� ���¸� �����ϴ� ���� �ӽ� ����
    protected Player player;    //���� �÷��̾� ��ü ����

    protected Rigidbody2D rb;

    protected float xInput;
    protected float yInput;
    //�ִϸ��̼ǿ��� ����� ���� ���� �̸� (�ִϸ��̼� Ʈ���ŷ� Ȱ�� ����)
    private string animBoolName;

    protected float stateTimer;
    protected bool triggerCalled;


    /// <summary>
    /// ���� Ŭ������ ������. 
    /// �÷��̾� ��ü, ���� �ӽ�, �ִϸ��̼� Ʈ���� �̸��� �ʱ�ȭ��.
    /// </summary>
    /// <param name="_player">�÷��̾� ��ü</param>
    /// <param name="_stateMachine">�÷��̾� ���� �ӽ�</param>
    /// <param name="_animBoolName">�ִϸ��̼� Ʈ���� ���� �̸�</param>
    public PlayerState(Player _player,PlayerStateMachine _stateMachine,string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }


    /// <summary>
    /// ���°� ���۵� �� ȣ��Ǵ� �Լ�.
    /// �ش� ���°� Ȱ��ȭ�� �� �ʿ��� �ʱ�ȭ ������ ����.
    /// </summary>
    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);
        rb = player.rb;
        triggerCalled = false;
    }
    /// <summary>
    /// �� �����Ӹ��� ȣ��Ǵ� �Լ�.
    /// �ش� ���¿��� ���������� �����ؾ� �� ������ ����.
    /// </summary>
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;


        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        player.anim.SetFloat("yVelocity", rb.linearVelocityY);
    }
    /// <summary>
    /// ���°� ����� �� ȣ��Ǵ� �Լ�.
    /// ���°� ����� �� �ʿ��� ���� �۾��� ����.
    /// </summary>
    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }

}
