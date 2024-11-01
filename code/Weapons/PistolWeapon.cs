using Grugchamber.GrugUtil;
using Sandbox.Audio;

public sealed class PistolWeapon : BaseWeapon, IPlayerEvent
{
	[Property] public float Damage { get; set; } = 50f;
	public override void OnControl( Player player )
	{
		if ( Input.Pressed( "attack1" ) )
		{
			Shoot();
		}
	}

	private void Shoot()
	{
		var tr = Rays.TraceFromCenter().IgnoreGameObject( GameObject.GetComponentsInParent<Player>(  ).FirstOrDefault().GameObject ).Run();

		if ( tr.Hit )
		{
			DebugOverlay.Line( tr.StartPosition, tr.HitPosition, Color.Magenta, duration: 20f );
				
				
				
				
				
			// do a bullet hole
			var decalObj = new GameObject();

			decalObj.WorldTransform = new Transform( tr.HitPosition + tr.Normal * 2.0f, Rotation.LookAt( -tr.Normal, Vector3.Random ));

			if ( tr.GameObject.GetComponent<SkinnedModelRenderer>() != null )
			{
				var skinned = tr.GameObject.GetComponent<SkinnedModelRenderer>();

				var parentObj = skinned.GetBoneObject( tr.Bone );
				
				if (parentObj != null)
				{
					decalObj.SetParent( parentObj );
				}
				else
				{
					decalObj.SetParent( tr.GameObject );
				}
			}
			else
			{
				 decalObj.SetParent( tr.GameObject );
			}
			
			// network bullet hole
			decalObj.NetworkMode = NetworkMode.Object;
			decalObj.Network.AssignOwnership( Connection.Local );
				
			var decalRenderer = decalObj.AddComponent<DecalRenderer>();
				
			
			decalObj.Tags.Add( "bullethole" );

			var decal = ResourceLibrary.Get<DecalDefinition>( "decals/bullethole.decal" );

			if ( tr.Surface.ImpactEffects.BulletDecal != null )
			{
				decal = ResourceLibrary.Get<DecalDefinition>( tr.Surface.ImpactEffects.BulletDecal.FirstOrDefault() );
			}
			
			
			// og.Info( decal );
			
			if ( !decal.IsValid() )
			{
				decal = ResourceLibrary.Get<DecalDefinition>( "decals/bullethole.decal" );
			}
			
			decalRenderer.Size = new Vector3( decalRenderer.Size.x/4, decalRenderer.Size.y/4, 16 );
				
			decalRenderer.Material = Game.Random.FromList( decal.Decals ).Material;
			decalObj.AddComponent<BulletHole>();
			// sound
			SoundEvent bullet = ResourceLibrary.Get<SoundEvent>( tr.Surface.Sounds.Bullet );
			SoundEvent pistol_shoot = ResourceLibrary.Get<SoundEvent>( "audio/weapons/pistol_shoot.sound");
			if ( bullet != null )
			{
				Task.Delay( (int)tr.StartPosition.Distance( tr.HitPosition )/1000 ).ContinueWith( ( task ) =>
				{

					var handle = decalObj.PlaySound( bullet, 0 );
					handle.TargetMixer = Mixer.FindMixerByName( "Game" );
					handle.Volume = 0.5f;
				} );

			}
			else
			{
				Log.Warning( $"There was no bullet SFX for {tr.Surface.Sounds.Bullet}!" );
				var handle = decalObj.PlaySound( ResourceLibrary.Get<SoundEvent>("sounds/error.sound"), 0 );
				handle.TargetMixer = Mixer.FindMixerByName( "Game" );
				handle.Volume = 0.5f;
			}

			// SoundHandle shootHandle = GameObject.PlaySound( pistol_shoot, 0 );
			// shootHandle.TargetMixer = Mixer.FindMixerByName( "Game" );
			

			// apply force
			if ( tr.Body.IsValid() )
			{
				tr.Body.ApplyImpulseAt( tr.HitPosition, tr.Direction * 200.0f * tr.Body.Mass.Clamp( 0, 200 ) );
			}
			// apply damage

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
