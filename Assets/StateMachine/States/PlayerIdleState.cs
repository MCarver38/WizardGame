using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        player.animator.SetBool("Idle", true);
    }

    public override void Update()
    {
        base.Update();

        if (player.isMovementPressed)
        {
            stateMachine.ChangeState(player.walkingState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        
        player.animator.SetBool("Idle", false);
    }
}
