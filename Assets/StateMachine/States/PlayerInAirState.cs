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
        
        player.relativeMovement = player.GetCameraRelativeVector();
        
        player.characterController.Move(player.relativeMovement * (player.movementSpeed * Time.deltaTime));
        
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
