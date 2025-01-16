using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventoryState : PlayerState
{
    public PlayerInventoryState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        player.animator.SetBool("Idle", true);
        player.inventoryScreen.SetActive(true);
        player.freeLookCamera.enabled = false;
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.I))
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        
        player.requireNewInventoryPress = false;
        player.animator.SetBool("Idle", false);
        player.inventoryScreen.SetActive(false);
        player.freeLookCamera.enabled = true;
    }
}
