using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSprintState : PlayerGroundedState
{
    public PlayerSprintState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
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
        
        player.relativeMovement = player.GetCameraRelativeVector();
        
        player.characterController.Move(player.relativeMovement * (player.sprintSpeed * Time.deltaTime));
        
        if (!player.isMovementPressed)
        {
            stateMachine.ChangeState(player.idleState);
        }

        if (!player.isRunPressed && player.isMovementPressed)
        {
            stateMachine.ChangeState(player.walkingState);
        }
        else if (!player.isRunPressed)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        
        player.animator.SetBool("Walking", false);
    }
    
    
}
