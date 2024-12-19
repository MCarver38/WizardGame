using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (player.isMovementPressed)
        {
            stateMachine.ChangeState(player.walkingState);
        }

        if (player.isJumpPressed)
        {
            stateMachine.ChangeState(player.jumpingState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
