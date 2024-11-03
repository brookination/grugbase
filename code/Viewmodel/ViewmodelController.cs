using Sandbox;

public sealed class ViewmodelAnimator : Component, IPlayerEvent
{
	[Property] public SkinnedModelRenderer Target { get; set; }
	public BodyController BodyController { get; set; }
	
	Angles EyeAngles { get; set; }

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
		Target.Set( "b_attack", Input.Pressed( "Attack1" ) );
		Target.Set( "b_grounded", BodyController.IsOnGround );
		Target.Set( "b_jump", (Input.Pressed( "jump" ) && BodyController.IsOnGround) );
		Target.Set( "move_bob", BodyController.WishVelocity.Length.Remap( 0, 320f, 0, 2f ) );
	}
	
	

	void IPlayerEvent.OnCameraMove( Player player, ref Angles angles )
	{
		if (Target == null || BodyController == null) return;
		Target.Set( "aim_pitch_inertia", angles.pitch );
		Target.Set( "aim_yaw_inertia", angles.yaw );
		
		
		EyeAngles += angles;
		EyeAngles = EyeAngles.WithPitch(EyeAngles.pitch.Clamp( -90f, 90 ));
		EyeAngles = EyeAngles.WithRoll( 0 );
		Target.Set( "aim_pitch", EyeAngles.pitch );
		Target.Set( "aim_yaw", EyeAngles.yaw );
	}
	
}
