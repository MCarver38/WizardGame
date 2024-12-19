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
    }

    public override void Update()
    {
        base.Update();
        
        player.characterController.Move(player.currentMovement * (player.sprintSpeed * Time.deltaTime));
        
        if (!player.isMovementPressed)
        {
            stateMachine.ChangeState(player.idleState);
        }

        if (!player.isRunPressed)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
    
    
}
