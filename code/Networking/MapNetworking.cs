using Sandbox;

public sealed class MapNetworking : Component
{
	protected override void OnAwake()
	{
		var props = GameObject.GetComponentsInChildren<Prop>().ToArray();
		

		foreach ( var prop in props )
		{
			prop.GameObject.SetParent( Scene );
			prop.Tags.Add( "dynamic" );
			prop.GameObject.NetworkMode = NetworkMode.Object;
		}
		
		
	}
}
