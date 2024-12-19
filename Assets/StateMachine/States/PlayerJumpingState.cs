using UnityEngine;

public class PlayerJumpingState : PlayerInAirState
{
    public PlayerJumpingState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
       HandleJump();
    }

    public override void Update()
    {
        base.Update();
        
    }

    public override void Exit()
    {
        base.Exit();
    }
    
    private void HandleJump()
    {
        player.currentMovement.y = player.initialJumpVelocity * .5f;
    }
}
