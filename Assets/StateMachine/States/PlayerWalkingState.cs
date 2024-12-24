using UnityEngine;

public class PlayerWalkingState : PlayerGroundedState
{
    public PlayerWalkingState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        player.animator.SetBool("Walking", true);
    }

    public override void Update()
    {
        base.Update();
        
        player.characterController.Move(player.currentMovement * (player.movementSpeed * Time.deltaTime));

        if (!player.isMovementPressed)
        {
            stateMachine.ChangeState(player.idleState);
        }

        if (player.isRunPressed)
        {
            stateMachine.ChangeState(player.sprintState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        
        player.animator.SetBool("Walking", false);
    }
}
