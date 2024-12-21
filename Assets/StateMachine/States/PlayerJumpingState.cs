using UnityEditor.Timeline.Actions;
using UnityEngine;

public class PlayerJumpingState : PlayerInAirState
{
    public PlayerJumpingState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        player.jumpCooldownTimer = player.jumpCooldown;
        
       HandleJump();
    }

    public override void Update()
    {
        base.Update();

        if (player.characterController.velocity.y < 0.0f)
        {
            stateMachine.ChangeState(player.fallingState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
    
    private void HandleJump()
    {
        player.currentMovement.y = Mathf.Sqrt(player.jumpForce * -2 * player.gravity);
        player.requireNewJumpPress = false;
    }
}
