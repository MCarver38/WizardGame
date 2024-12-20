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

        if (!player.CheckIfGrounded() || player.currentMovement.y > 0)
        {
            player.currentMovement.y += player.gravity;
            
            player.characterController.Move(player.currentMovement * Time.deltaTime);
        }
        
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
