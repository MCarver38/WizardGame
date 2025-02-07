public class PlayerNPCInteractState : PlayerState
{
    public PlayerNPCInteractState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.freeLookCamera.enabled = false;
        player.animator.SetBool("Idle", true);
        player.dialogueBox.SetActive(true);
        player.HideInteractUI();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
        
        player.freeLookCamera.enabled = true;
        player.animator.SetBool("Idle", false);
        player.dialogueBox.SetActive(false);
        player.ShowInteractUI();
    }
}
