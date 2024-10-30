using Grugchamber.GrugUtil;

public sealed class PistolWeapon : BaseWeapon, IPlayerEvent
{
	[Property] public float Damage { get; set; } = 50f;
	public override void OnControl( Player player )
	{
		if ( Input.Pressed( "attack1" ) )
		{
			var tr = Rays.TraceFromCenter().IgnoreGameObject( GameObject.GetComponentsInParent<Player>(  ).FirstOrDefault().GameObject ).Run();

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
				decalObj.AddComponent<BulletHole>();

				if ( tr.Body.IsValid() )
				{
					tr.Body.ApplyImpulseAt( tr.HitPosition, tr.Direction * 200.0f * tr.Body.Mass.Clamp( 0, 200 ) );
				}

				var damage = new DamageInfo( Damage, GameObject, GameObject, tr.Hitbox );
				damage.Position = tr.HitPosition;
				damage.Shape = tr.Shape;

				foreach ( var damagable in tr.GameObject.Components.GetAll<IDamageable>(  ) )
				{
					damagable.OnDamage( damage );
				}


			}
		}
	}
}
