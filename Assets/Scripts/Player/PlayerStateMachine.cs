using UnityEngine;
/// <summary>
/// 플레이어의 상태를 관리하는 상태 머신 클래스.
/// 플레이어가 가질 수 있는 다양한 상태를 전환하는 역할을 함.
/// </summary>
public class PlayerStateMachine
{

    /// <summary>
    /// 현재 활성화된 상태를 저장하는 프로퍼티.
    /// 외부에서 직접 변경할 수 없도록 private set 사용.
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
