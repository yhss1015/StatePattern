using UnityEngine;
using UnityEngine.UIElements;

public class EnemyState : MonoBehaviour
{

    protected EnemyStateMachine stateMachine;
    protected Enemy enemy;

    protected bool triggerCalled;    
    private string animBoolName;

    protected float stateTimer;

    public EnemyState(Enemy _enemy, EnemyStateMachine _stateMachine,string _animBoolName)
    {
        this.enemy = _enemy;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        triggerCalled = true;
        enemy.anim.SetBool(animBoolName, true);
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        enemy.anim.SetBool(animBoolName, false);
    }
}
