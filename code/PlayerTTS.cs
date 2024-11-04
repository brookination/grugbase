using dectalkTTS;

namespace Sandbox;

public sealed class PlayerTTS : Component, IPlayerEvent
{
	MusicPlayer MusicPlayer;
	Player SpeakingPlayer;

	protected override void OnUpdate()
	{
		if (MusicPlayer !=null && SpeakingPlayer != null )
		{
			MusicPlayer.Position = SpeakingPlayer.WorldPosition;


		}
	}

	void IPlayerEvent.OnPlayerTalked(Player player, string message )
	{
		MusicPlayer = MusicPlayer.PlayUrl( DecTalker.GetURL( message ) );
		SpeakingPlayer = player;


		MusicPlayer.OnFinished += Disconnect;
	}

	void Disconnect()
	{
		MusicPlayer?.Dispose();
		SpeakingPlayer = null;
	}

}
