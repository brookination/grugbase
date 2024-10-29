using Sandbox;

public sealed class MapNetworking : Component
{
	protected override void OnAwake()
	{
		var props = GameObject.GetComponentsInChildren<Prop>().ToArray();
		

		foreach ( var prop in props )
		{
			prop.GameObject.SetParent( Scene );
			prop.Tags.Add( "dynamic" );
			prop.GameObject.NetworkMode = NetworkMode.Object;
		}
		
		
	}
}

public sealed class PropNetworking : Component
{
	/// <summary>
	/// How big the search radius for the player should be
	/// </summary>
	[Property, DefaultValue(100f)] public float SphereRadius = 100f;
	
	[Property, DefaultValue(20f)] public float RayLength = 20f;
	
	/// <summary>
	/// How fast the ray should spin
	/// </summary>
	[Property] public Angles SpinVelocity = new Angles( 0, 20f, 0 );

	private Angles CurrentAngles;

	protected override void OnFixedUpdate()
	{

		CurrentAngles += SpinVelocity * Time.Delta;
		

		var tr = Scene.Trace.Sphere( SphereRadius, WorldPosition, WorldPosition + CurrentAngles.Forward * RayLength )
			.Run();

		if ( tr.Hit )
		{
			var player = tr.GameObject.GetComponent<Player>();
			if ( player == null ) return;

			GameObject.Network.AssignOwnership( player.GameObject.Network.Owner );
		}
	}

	protected override void DrawGizmos()
	{
		if (!Gizmo.IsSelected) return;
		
		CurrentAngles += SpinVelocity * Time.Delta;
		
		Gizmo.Draw.WorldText( "This isn't entirely accurate.\n It'll be done every fixed update, not frame, as demo'd here", global::Transform.Zero.WithPosition( Vector3.Zero + Vector3.Up * 50 ).WithRotation( Angles.Zero.WithRoll( 90f ) ).WithScale( Vector3.One*0.5f ), size:12 );
		
		Gizmo.Draw.Line( new Line( Vector3.Zero, CurrentAngles.Forward * RayLength  ) );
		Gizmo.Draw.LineSphere( Vector3.Zero + CurrentAngles.Forward * RayLength, SphereRadius );
	}
}
