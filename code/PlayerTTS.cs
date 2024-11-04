using dectalkTTS;

namespace Sandbox;

public class PlayerTTS : GameObjectSystem, IPlayerEvent
{
	public PlayerTTS( Scene scene ) : base( scene )
	{
		
	}


	async void IPlayerEvent.OnPlayerTalked( Player player, string talk )
	{
		

		var musicPlayer = await DecTalker.Say( talk );

		
		musicPlayer.Volume = 2;
		musicPlayer.Position = musicPlayer.Position;

	}
}
