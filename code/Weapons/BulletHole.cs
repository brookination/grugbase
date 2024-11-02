using Sandbox;

public sealed class BulletHole : Component
{
	[Property] public float TimeUntilDestroyed { get; set; } = 20f;
	
	TimeSince timeSinceCreated;

	private DecalRenderer renderer;
	
	protected override void OnAwake()
	{
		
		
		
		renderer = GetComponent<DecalRenderer>();
		timeSinceCreated = 0f;
	}

	protected override void OnUpdate()
	{
		var frac = (timeSinceCreated / TimeUntilDestroyed) / 6f;
		if ( renderer != null )
		{
			renderer.TintColor = Color.Lerp(Color.White, Color.White.WithAlpha( 0f ), frac  );
		}

		if ( timeSinceCreated >= TimeUntilDestroyed )
		{
			GameObject.DestroyImmediate();
		}
	}
}
