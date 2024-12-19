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
        
        float previousYVelocity = player.currentMovement.y;
        float newYVelocity = player.currentMovement.y + (player.gravity * player.fallMultiplier * Time.deltaTime);
        float nextYVelocity = (previousYVelocity + newYVelocity) * .5f;
        player.currentMovement.y = nextYVelocity;
    }

    public override void Exit()
    {
        base.Exit();
    }
}
