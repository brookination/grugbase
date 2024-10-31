namespace Sandbox;

public sealed class WeaponViewmodel : Component
{
	
	Player localPlayer;
	private GameObject viewmodelGameObject;
	private BaseWeapon activeWeapon;

	protected override void OnAwake()
	{
		
		localPlayer = Player.FindLocalPlayer();
	}

	protected override void OnFixedUpdate()
	{
		if (IsProxy) return;

		try
		{
			UpdateVariables();
		}
		catch ( Exception e )
		{
			Log.Error( e );
		}
		

		

	}

	void UpdateVariables()
	{
		if ( localPlayer == null )
		{
			localPlayer = Player.FindLocalPlayer();
		}

		if ( activeWeapon == null || localPlayer.GetComponent<PlayerInventory>().ActiveWeapon != activeWeapon )
		{
			Log.Info( "Updating variables" );
			activeWeapon = localPlayer.GetComponent<PlayerInventory>().ActiveWeapon;
		}
		
		if ( viewmodelGameObject == null && activeWeapon.WeaponViewmodel != null )
		{
			SpawnViewmodel();
		}

		if ( activeWeapon.WeaponViewmodel == null && viewmodelGameObject != null )
		{
			viewmodelGameObject.Enabled = false;
		}
		if ( activeWeapon.WeaponViewmodel != null && viewmodelGameObject.Enabled == false )
		{
			viewmodelGameObject.Enabled = true;
		}
	}

	void SpawnViewmodel()
	{
		if ( activeWeapon != null )
		{
			if ( activeWeapon.WeaponViewmodel != null )
			{
				viewmodelGameObject = SceneUtility.GetPrefabScene( activeWeapon.WeaponViewmodel ).Clone();
				viewmodelGameObject.SetParent( Scene.Camera.GameObject );
				viewmodelGameObject.LocalPosition = Vector3.Zero;
				viewmodelGameObject.LocalRotation = Rotation.Identity;
			}
		}
	} 

}
