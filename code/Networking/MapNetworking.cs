using Sandbox;


[Group("Grugbase/Networking"), Icon("settings_input_antenna")]
public sealed class MapNetworking : Component
{
	[Property, DefaultValue(100f), Description("How big the search radius for the player is")] public float SphereRadius = 100f;
	
	[Property, DefaultValue(20f), Description("How long should we cast the sphere??? I don't really understand this...")] public float RayLength = 20f;
	
	[Property, Description("How fast should the ray spin")] public Angles SpinVelocity = new Angles( 0, 20f, 0 );

	
	protected override void OnAwake()
	{
		var props = GameObject.GetComponentsInChildren<Prop>().ToArray();
		var skinned = GameObject.GetComponentsInChildren<SkinnedModelRenderer>().ToArray();

		var obj = new GameObject(true, "Props");
		
		foreach ( var mdl in skinned )
		{
			mdl.CreateBoneObjects = true;
		}

		foreach ( var prop in props )
		{
			prop.GameObject.SetParent( obj );
			prop.Tags.Add( "dynamic" );
			prop.GameObject.NetworkMode = NetworkMode.Object;
			prop.GameObject.Network.SetOrphanedMode( NetworkOrphaned.ClearOwner );
			var networking = prop.AddComponent<PropNetworking>();
			
			networking.SphereRadius = SphereRadius;
			networking.RayLength = RayLength;
			networking.SpinVelocity = SpinVelocity;
		}
		
		
	}
}

[Group("Grugbase/Networking"),  Icon("leak_add")]
public sealed class PropNetworking : Component
{
	/// <summary>
	/// How big the search radius for the player should be
	/// </summary>
	[Property, DefaultValue(100f), Description("How big the search radius for the player is")] public float SphereRadius = 100f;
	
	[Property, DefaultValue(20f), Description("How long should we cast the sphere??? I don't really understand this...")] public float RayLength = 20f;
	
	/// <summary>
	/// How fast the ray should spin
	/// </summary>
	[Property, Description("How fast should the ray spin")] public Angles SpinVelocity = new Angles( 0, 20f, 0 );

	private Angles CurrentAngles;

	protected override void OnFixedUpdate()
	{
		// Spin a lil bit
		CurrentAngles += SpinVelocity * Time.Delta;
		
		// Trace a sphere
		var tr = Scene.Trace.Sphere( SphereRadius, WorldPosition, WorldPosition + CurrentAngles.Forward * RayLength )
			.Run();

		if ( tr.Hit )
		{
			var player = tr.GameObject.GetComponent<Player>();
			// If the GameObject isn't a player, return
			if ( player == null ) return;
			// Don't do anything if we own it already
			if (player.Network.Owner == GameObject.Network.Owner) return;

			// Assign ownership
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
