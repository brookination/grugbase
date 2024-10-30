namespace Grugchamber.GrugUtil
{
	public static class Rays
	{
		public static SceneTraceResult TraceFromCenter()
		{
			var ray = Game.ActiveScene.Trace.Ray(
				Game.ActiveScene.Camera.ScreenPixelToRay( new Vector2( Screen.Width / 2, Screen.Height / 2 ) ), 1000f );

			return ray.Run();

		}
	}
}

