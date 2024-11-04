using dectalkTTS;
using Sandbox.Audio;

namespace Sandbox;

public class PlayerTTS : GameObjectSystem, IPlayerEvent
{
	public PlayerTTS( Scene scene ) : base( scene )
	{
		
	}


	async void IPlayerEvent.OnPlayerTalked( Player player, string talk )
	{
		

		var musicPlayer = await DecTalker.Say( talk );

		musicPlayer.TargetMixer = Mixer.FindMixerByName( "TTS" );
		
		musicPlayer.Volume = 2;
		musicPlayer.Position = musicPlayer.Position;
		
	}
}
