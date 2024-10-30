using Grugchamber.GrugUtil;

public sealed class PistolWeapon : BaseWeapon, IPlayerEvent
{
	public override void OnControl( Player player )
	{
		if ( Input.Pressed( "attack1" ) )
		{
			var trace = Rays.TraceFromCenter();

			if ( trace.Hit )
			{
				DebugOverlay.Line( trace.StartPosition, trace.HitPosition, Color.Magenta, duration: 2000f );
				
				// do a bullet hole
				var decalObj = new GameObject();

				decalObj.WorldTransform = new Transform( trace.HitPosition + trace.Normal * 2.0f, Rotation.LookAt( -trace.Normal, Vector3.Random ));
				
				decalObj.SetParent( trace.GameObject );
				
				var decalRenderer = decalObj.AddComponent<DecalRenderer>();
				
				decalObj.Tags.Add( "bullethole" );

				var decal = Material.Load( WeaponEffects.GetDecalFromSurface( trace.Surface.ResourcePath ));
				
				decalRenderer.Size = new Vector3( decalRenderer.Size.x/4, decalRenderer.Size.y/4, 16 );
				
				decalRenderer.Material = decal;




			}
		}
	}
}
