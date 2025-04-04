using UnityEngine;
/// <summary>
/// �÷��̾��� ���¸� �����ϴ� �⺻ Ŭ����.
/// �� Ŭ������ ����Ͽ� �پ��� �÷��̾� ����(��: Idle, Running, Jumping ��)�� ������ �� ����.
/// </summary>
public class PlayerState 
{
    //�÷��̾��� ���¸� �����ϴ� ���� �ӽ� ����
    protected PlayerStateMachine stateMachine;

    //���� �÷��̾� ��ü ����
    protected Player player;

    //�ִϸ��̼ǿ��� ����� ���� ���� �̸� (�ִϸ��̼� Ʈ���ŷ� Ȱ�� ����)
    private string animBoolName;


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
        Debug.Log("���� " + animBoolName);
    }
    /// <summary>
    /// �� �����Ӹ��� ȣ��Ǵ� �Լ�.
    /// �ش� ���¿��� ���������� �����ؾ� �� ������ ����.
    /// </summary>
    public virtual void Update()
    {
        Debug.Log("������Ʈ " + animBoolName);
    }
    /// <summary>
    /// ���°� ����� �� ȣ��Ǵ� �Լ�.
    /// ���°� ����� �� �ʿ��� ���� �۾��� ����.
    /// </summary>
    public virtual void Exit()
    {
        Debug.Log("����Ʈ " + animBoolName);
    }


}
