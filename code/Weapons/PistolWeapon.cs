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
				Log.Info( "Woah" );
			}
		}
	}
}
