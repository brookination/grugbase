using Sandbox;

public sealed class ViewmodelManager : Component
{
	public Player Player => Player.FindLocalPlayer();
	public PlayerInventory PlayerInventory => Player.GetComponent<PlayerInventory>();
	
	public GameObject ViewmodelObject;
	

	public bool ViewmodelEnabled;
	private bool _viewmodelEnabled;

	void CheckViewmodel()
	{
		
		// check if player is alive; if not null
		if (Player == null || Player.IsDead)
		{
			ViewmodelEnabled = false;
		}
		else
		{
			ViewmodelEnabled = true;
		}

		// is viewmodel there?
		if (ViewmodelEnabled)
		{
			ViewmodelEnabled = PlayerInventory.ActiveWeapon?.WeaponViewmodel == null;
		}
		
		
		
		if ( ViewmodelEnabled != _viewmodelEnabled )
		{
			OnViewmodelChanged();
			_viewmodelEnabled = ViewmodelEnabled;
		}
		
		
	}
	
	void OnViewmodelChanged()
	{
		Log.Info( _viewmodelEnabled );
	}
	
	
	
	protected override void OnFixedUpdate()
	{
		CheckViewmodel();

		
		
		
	}
}
