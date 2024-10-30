using Grugchamber.GrugUtil;

public sealed class PistolWeapon : BaseWeapon, IPlayerEvent
{
	public override void OnControl( Player player )
	{
		if ( Input.Pressed( "attack1" ) )
		{
			var tr = Rays.TraceFromCenter();

			if ( tr.Hit )
			{
				DebugOverlay.Line( tr.StartPosition, tr.HitPosition, Color.Magenta, duration: 2000f );
				
				
				
				
				
				// do a bullet hole
				var decalObj = new GameObject();

				decalObj.WorldTransform = new Transform( tr.HitPosition + tr.Normal * 2.0f, Rotation.LookAt( -tr.Normal, Vector3.Random ));
				
				decalObj.SetParent( tr.GameObject );
				
				var decalRenderer = decalObj.AddComponent<DecalRenderer>();
				
				decalObj.Tags.Add( "bullethole" );

				var decal = Material.Load( WeaponEffects.GetDecalFromSurface( tr.Surface.ResourcePath ));
				
				decalRenderer.Size = new Vector3( decalRenderer.Size.x/4, decalRenderer.Size.y/4, 16 );
				
				decalRenderer.Material = decal;
				



			}
		}
	}
}
