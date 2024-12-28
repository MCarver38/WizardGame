using UnityEngine;

public class PlayerDodgeState : PlayerGroundedState
{
    public PlayerDodgeState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    
    public override void Enter()
    {
        base.Enter();

        player.dodgeCooldownTimer = player.dodgeCooldown;
        player.dodgeDurationTimer = player.dodgeDuration;
    }

    public override void Update()
    {
        base.Update();
        
        player.dodgeDurationTimer -= Time.deltaTime;
        
        if (player.dodgeDurationTimer <= 0)
        {
            stateMachine.ChangeState(player.idleState);
        }
        
        HandleDodge();
    }

    public override void Exit()
    {
        base.Exit();
        
        player.dodgeDurationTimer = player.dodgeDuration;
    }

    private void HandleDodge()
    {
        player.requireNewDodgePress = false;
        player.relativeMovement = player.GetCameraRelativeVector();
        
        player.characterController.Move(player.relativeMovement * (player.dodgeSpeed * Time.deltaTime));
    }
}
