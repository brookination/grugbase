namespace Grugchamber.GrugUtil;

public static class Rays
{
	/// <summary>
	///     Trace a ray from the active scene's main camera's center.
	/// </summary>
	/// <returns></returns>
	public static SceneTraceResult TraceFromCenter()
	{
		return Game.ActiveScene.Trace.Ray(
			Game.ActiveScene.Camera.ScreenPixelToRay( new Vector2( Screen.Width / 2, Screen.Height / 2 ) ), 1000f )
			.Run();

	}
}
