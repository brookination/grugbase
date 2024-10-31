using Sandbox;

public sealed class ViewmodelController : Component, IPlayerEvent
{
	[Property] public SkinnedModelRenderer Target { get; set; }
	public BodyController BodyController { get; set; }

	protected override void OnAwake()
	{
		BodyController = Player.FindLocalPlayer().GetComponent<BodyController>();
	}

	protected override void OnUpdate()
	{
		if (IsProxy) return;

		if ( Target != null && BodyController != null )
		{
			Animate();
		}
	}


	void Animate()
	{
		if ( Input.Pressed( "Attack1" ) )
		{
			Target.Set( "b_attack", true );
		}
	}
}
