using UnityEngine;

public class PlayerInAirState : PlayerState
{
    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        
    }

    public override void Update()
    {
        base.Update();
        
        player.currentMovement.y += player.gravity;
        
        player.characterController.Move(player.currentMovement * (player.movementSpeed * player.clampedDeltaTime()));
        
        if (player.CheckIfGrounded())
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
