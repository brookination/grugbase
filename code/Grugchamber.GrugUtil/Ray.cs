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

		public static Dictionary<string, string> DecalEffectsKey = new Dictionary<string, string>
		{
			{"everythingelse", "materials/decals/bullethole.vmat" },
			{"surfaces/flesh.surface", "materials/decals/flesh/flesh1.vmat"}
		};

		public static string GetDecalFromSurface( string surfacePath )
		{
			return DecalEffectsKey[surfacePath];
		} 
		
	}
}

