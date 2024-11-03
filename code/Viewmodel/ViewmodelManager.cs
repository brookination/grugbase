using Sandbox;

public sealed class ViewmodelManager : Component
{
	public Player Player => Player.FindLocalPlayer();
	public PlayerInventory PlayerInventory => Player.GetComponent<PlayerInventory>();
	public PlayerUse PlayerUse = null;
	
	public GameObject ViewmodelObject;
	

	public bool ViewmodelEnabled;
	private bool _viewmodelEnabled;

	void CheckViewmodel()
	{

		if ( Player != null )
		{
			PlayerUse = Player.GetComponent<PlayerUse>();
		}
		else
		{
			PlayerUse = null;
		}
		
		
		if (Player == null || Player.IsDead)
		{
			ViewmodelEnabled = true;
		}
		else
		{
			ViewmodelEnabled = false;
		}

		if ( PlayerUse == null || PlayerUse.CarryingObject )
		{
			ViewmodelEnabled = true;
		}
		else
		{
			ViewmodelEnabled = false;
		}
		
		if (!ViewmodelEnabled)
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
		if ( !ViewmodelEnabled )
		{
			Log.Info( "Creating viewmodel" );
			CreateViewmodel();
		}
		else
		{
			Log.Info( "Destroying viewmodel" );
			DestroyViewmodel();
		}
	}

	void CreateViewmodel()
	{
		if ( ViewmodelObject == null && !ViewmodelEnabled )
		{
			ViewmodelObject = new GameObject();
			
			ViewmodelObject.SetParent( Scene.Camera.GameObject );

			ViewmodelObject.LocalTransform = global::Transform.Zero;
			
			
			var armObj = new GameObject();
			var weaponObj = new GameObject();
			
			armObj.SetParent(  ViewmodelObject );
			armObj.LocalTransform = global::Transform.Zero;
			
			weaponObj.SetParent( ViewmodelObject );
			weaponObj.LocalTransform = global::Transform.Zero;
			
			var armRenderer = armObj.AddComponent<SkinnedModelRenderer>(  );
			var weaponRenderer = weaponObj.AddComponent<SkinnedModelRenderer>();

			armRenderer.Model = Model.Load( "models/first_person/first_person_arms.vmdl" );
			weaponRenderer.Model = PlayerInventory.ActiveWeapon.WeaponViewmodel;

			armRenderer.RenderOptions.Game = false;
			armRenderer.RenderOptions.Overlay = true;
			
			weaponRenderer.RenderOptions.Game = false;
			weaponRenderer.RenderOptions.Overlay = true;

			armRenderer.RenderType = ModelRenderer.ShadowRenderType.Off;
			weaponRenderer.RenderType = ModelRenderer.ShadowRenderType.Off;

			armRenderer.BoneMergeTarget = weaponRenderer;

			var animator = weaponObj.AddComponent<ViewmodelAnimator>();
			animator.Target = weaponRenderer;


		}
	}

	void DestroyViewmodel()
	{
		if ( ViewmodelObject != null )
		{
			
			ViewmodelObject.DestroyImmediate();
			ViewmodelObject = null;
		} 
	}
	
	
	protected override void OnFixedUpdate()
	{
		if (IsProxy) return;
		
		CheckViewmodel();

		
		
		
	}
}
