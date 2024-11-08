
using Sandbox.Diagnostics;

public sealed class PlayerInventory : Component, IPlayerEvent
{
	[RequireComponent] public Player Player { get; set; }


	public List<BaseWeapon> Weapons => Scene.Components.GetAll<BaseWeapon>( FindMode.EverythingInSelfAndDescendants ).Where( x => x.Network.OwnerId == Network.OwnerId ).ToList();

	public BaseWeapon ActiveWeapon { get; private set; }

	public void GiveDefaultWeapons()
	{
		Pickup( "weapons/hands.prefab" );
		Pickup( "weapons/camera.prefab" );
		Pickup( "weapons/pistol.prefab" );
	}

	void Pickup( string prefabName )
	{
		var prefab = GameObject.Clone( prefabName, new CloneConfig { Parent = GameObject, StartEnabled = false } );
		prefab.NetworkSpawn( false, Network.Owner );

		var weapon = prefab.Components.Get<BaseWeapon>( true );
		Assert.NotNull( weapon );

		IPlayerEvent.Post( e => e.OnWeaponAdded( Player, weapon ) );
	}

	protected override void OnUpdate()
	{

	}

	public void SwitchWeapon( BaseWeapon weapon )
	{
		if ( ActiveWeapon.IsValid() )
		{
			ActiveWeapon.GameObject.Enabled = false;
		}

		ActiveWeapon = weapon;
		IPlayerEvent.Post( x => x.OnWeaponChanged( Player, weapon ) );

		if ( ActiveWeapon.IsValid() )
		{
			ActiveWeapon.GameObject.Enabled = true;
		}
	}

	void IPlayerEvent.OnSpawned( Player player )
	{
		if ( player != Player )
			return;

		GiveDefaultWeapons();
	}
}
