using UnityEngine;
/// <summary>
/// �÷��̾��� ���¸� �����ϴ� ���� �ӽ� Ŭ����.
/// �÷��̾ ���� �� �ִ� �پ��� ���¸� ��ȯ�ϴ� ������ ��.
/// </summary>
public class PlayerStateMachine
{

    /// <summary>
    /// ���� Ȱ��ȭ�� ���¸� �����ϴ� ������Ƽ.
    /// �ܺο��� ���� ������ �� ������ private set ���.
    /// </summary>
    public PlayerState currentstate { get; private set; }

    public void Initialize(PlayerState _startState)
    {
        currentstate = _startState;
        currentstate.Enter();
    }

    public void ChangeState(PlayerState _newState)
    {
        currentstate.Exit();
        currentstate = _newState;
        currentstate.Enter();
    }

}
