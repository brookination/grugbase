namespace Grugchamber.GrugUtil
{
	public static class Rays
	{
		/// <summary>
		/// Trace a ray from the scene's main camera's center.
		/// </summary>
		/// <returns></returns>
		public static SceneTraceResult TraceFromCenter()
		{
			var ray = Game.ActiveScene.Trace.Ray(
				Game.ActiveScene.Camera.ScreenPixelToRay( new Vector2( Screen.Width / 2, Screen.Height / 2 ) ), 1000f );

			return ray.Run();

		}
	}

	public static class WeaponEffects
	{
		/// <summary>
		/// Create a bullet hole
		/// </summary>
		/// <param name="HitPosition"></param>
		/// <param name="HitNormal"></param>
		/// <param name="surface"></param>
		public static async void BulletHoleFx( Vector3 HitPosition, Vector3 HitNormal, Surface surface, GameObject hitObject )
		{
			Scene scene = Game.ActiveScene;

			GameObject holeObj = new GameObject();

			string BulletHolePath = String.Empty;
			
			
			foreach ( var keys in DecalEffectsKey )
			{
				if ( keys.Key == "everythingelse" )
				{
					BulletHolePath = keys.Value;
				}

				if ( keys.Key == surface.ResourcePath )
				{
					BulletHolePath = keys.Value;
				}
			}
			
			// safety first
			if (BulletHolePath == string.Empty) return;
			
			holeObj.SetParent( hitObject );

			holeObj.WorldTransform = new Transform( HitPosition + HitNormal * 2.0f, Rotation.LookAt( HitNormal ) );

			var decalRenderer = holeObj.AddComponent<DecalRenderer>();

			decalRenderer.Material = await Material.LoadAsync( BulletHolePath );
			
			return;



		}

		public static Dictionary<string, string> DecalEffectsKey = new Dictionary<string, string>
		{
			{"everythingelse", "materials/decals/bullethole.vmat" },
			{"surfaces/flesh.surface", "materials/decals/flesh/flesh1.vmat"}
		};
		
	}
}

