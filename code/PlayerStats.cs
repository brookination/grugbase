/// <summary>
/// Record stats for the local player
/// </summary>
public sealed class PlayerStats : Component, IPlayerEvent
{
	[RequireComponent] public Player Player { get; set; }

	float metersTravelled;
	Vector3 lastPosition;

	protected override void OnFixedUpdate()
	{
		if ( IsProxy ) return;

		var delta = WorldPosition - lastPosition;
		lastPosition = WorldPosition;


		if ( !Player.Controller.IsOnGround )
		{
			return;
		}

		var groundDelta = delta.WithZ( 0 ).Length.InchToMeter();
		if ( groundDelta > 10 ) groundDelta = 0;

		metersTravelled += groundDelta;

		if ( metersTravelled > 10 )
		{
			Sandbox.Services.Stats.Increment( "meters_walked", metersTravelled );
			metersTravelled = 0;
		}

	}

	void IPlayerEvent.OnJump( Player player )
	{
		if ( player != Player ) return;

		Sandbox.Services.Stats.Increment( "jump", 1 );
	}

	void IPlayerEvent.OnTakeDamage( Player player, float damage )
	{
		if ( player != Player ) return;

		Sandbox.Services.Stats.Increment( "damage_taken", damage );
	}

	void IPlayerEvent.OnDied( Player player )
	{
		if ( player != Player ) return;

		Sandbox.Services.Stats.Increment( "deaths", 1 );
	}

	void IPlayerEvent.OnPlayerMove( Player player, Vector3 velocity )
	{
		if ( player != Player ) return;

		if ( velocity.Length >= 700f )
		{
			Sandbox.Services.Achievements.Unlock( "game_toofast" );
		}
	}
	void IPlayerEvent.OnSuicide( Player player )
	{
		Sandbox.Services.Stats.Increment( "suicides", 1 );
		if ( Sandbox.Services.Stats.LocalPlayer.Get( "suicides" ).Value >= 1 )
		{
			Sandbox.Services.Achievements.Unlock( "game_ragdolling" );
		} else if ( Sandbox.Services.Stats.LocalPlayer.Get( "suicides" ).Value >= 50 )
		{
			
			Sandbox.Services.Achievements.Unlock( "game_manyragdoll" );
		}
		{
			
		}
	}

}
