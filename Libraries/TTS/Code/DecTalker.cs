using System;
using System.Linq;
using Sandbox;
using System.Threading.Tasks;

namespace dectalkTTS;

public static class DecTalker
{
	public static string Server = "https://tts.cyzon.us/tts?text=";
	
	public static string GetURL( string input )
	{
		return Server + input.UrlEncode();
	}

	public static async Task<string> Download( string text, string filename, BaseFileSystem fileSystem )
	{
		if ( fileSystem == null ) fileSystem = FileSystem.Data;
		
		text = text.RemoveBadCharacters();
		text.Substring( 0, Math.Min( text.Length, 300 ) );
		if ( !text.Any( x => char.IsLetterOrDigit( x ) ) ) return null;
		if ( string.IsNullOrEmpty( text ) ) return null;

		var folders = filename.Split( "/" );
		if (folders.Length == 1) folders = filename.Split( "\\" );
		if ( folders.Length > 1 )
		{
			string path = "";
			for ( int i = 0; i < folders.Length; i++ )
			{
				path += folders[i] + "/";
				if ( !fileSystem.DirectoryExists( path ) )
				{
					fileSystem.CreateDirectory( path );
				}
			}
		}

		try
		{
			if (fileSystem.FileExists( filename )) fileSystem.DeleteFile( filename );
			var response = await Http.RequestBytesAsync( GetURL( text ) );
			var stream = fileSystem.OpenWrite( filename );
			stream.Write( response, 0, response.Length );
			stream.Close();
			return filename;
		}
		catch ( Exception e )
		{
			Log.Error( e.Message );
			throw;
		}

	}

	public static async Task<MusicPlayer> Say( string text )
	{
		string filename = $"tts-{text.UrlEncode()}.wav";

		if ( !FileSystem.Data.FileExists( filename ) )
		{
			await Download( text, filename, FileSystem.Data );
		}

		var musicPlayer = MusicPlayer.Play( FileSystem.Data, filename );
		musicPlayer.ListenLocal = true;

		return musicPlayer;
	}
}
