using System.Security.Cryptography;

namespace Sandbox;

public sealed class WeaponViewmodel : Component
{
	
	[Property, ReadOnly] Player localPlayer;
	[Property, ReadOnly] PlayerInventory inventory;
	[Property, ReadOnly] private GameObject ViewmodelGameObject;
	[Property, ReadOnly] private BaseWeapon? ActiveWeapon;
	private bool ViewmodelForActiveWeapon;

	// TODO: THIS FUCKING SUCKS
	
	protected override void OnAwake()
	{
		UpdateVariables();
	}

	void UpdateVariables()
	{
		if ( localPlayer == null )
		{
			localPlayer = Player.FindLocalPlayer();
			Log.Info( "Finding local player..." );
			if ( localPlayer == null )
			{
				Log.Error( "There is no local player (?)" );
			}
			else
			{
				Log.Info( "Local player found!" );
			}
		}

		if ( localPlayer != null )
		{
			inventory = localPlayer.GetComponent<PlayerInventory>();
			if ( inventory == null )
			{
				Log.Error( "Local player inventory component not found!" );
			}

			if ( inventory != null )
			{
				if ( inventory.ActiveWeapon != null )
				{
					if ( inventory.ActiveWeapon != inventory.ActiveWeapon )
					{
						if ( ViewmodelGameObject != null )
						{
							ViewmodelGameObject.DestroyImmediate();
						}
					}
					ActiveWeapon = inventory.ActiveWeapon;
					Log.Info( "We have an active weapon!" );
					ViewmodelForActiveWeapon = false;
				}
			}
		}

		if ( ActiveWeapon != null && ActiveWeapon.WeaponViewmodel.IsValid() )
		{
			Log.Info( $"Active weapon {inventory.ActiveWeapon.GetType().ToString()} has a viewmodel!" );
			ViewmodelForActiveWeapon = true;

		}
		else
		{
			ViewmodelForActiveWeapon = false;
		}

		if ( !ViewmodelForActiveWeapon && ViewmodelGameObject != null )
		{
			ViewmodelGameObject.DestroyImmediate();
		}
		
		Log.Info( ViewmodelForActiveWeapon );
	}
	
	protected override void OnFixedUpdate()
	{
		if (IsProxy) return;

		UpdateVariables();
		SpawnViewmodel();





	}

	void SpawnViewmodel()
	{
		if ( ViewmodelGameObject == null && ViewmodelForActiveWeapon && ActiveWeapon != null)
		{
			ViewmodelGameObject = new GameObject();
			ViewmodelGameObject.SetParent( Scene.Camera.GameObject );
			ViewmodelGameObject.LocalTransform = global::Transform.Zero;

			var ArmsObject = new GameObject();
			var WeaponObject = new GameObject();
			
			ArmsObject.SetParent( ViewmodelGameObject );
			ArmsObject.LocalTransform = global::Transform.Zero;
			WeaponObject.SetParent( ViewmodelGameObject );
			WeaponObject.LocalTransform = global::Transform.Zero;

			var ArmsRenderer = ArmsObject.AddComponent<SkinnedModelRenderer>();
			ArmsRenderer.Model = Model.Load( "models/first_person/first_person_arms.vmdl" );
			
			var WeaponRenderer = WeaponObject.AddComponent<SkinnedModelRenderer>();
			WeaponRenderer.Model = ActiveWeapon.WeaponViewmodel;

			ArmsRenderer.BoneMergeTarget = WeaponRenderer;

			var controller = ViewmodelGameObject.AddComponent<ViewmodelController>();
			controller.Target = WeaponRenderer;
			controller.BodyController = localPlayer.GetComponent<BodyController>();
			
			



		}
	}
	
	/* void UpdateVariables()
	{
		if ( localPlayer == null )
		{
			localPlayer = Player.FindLocalPlayer();
		}

		if ( activeWeapon == null || localPlayer.GetComponent<PlayerInventory>().ActiveWeapon != activeWeapon )
		{
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
	} */ 

}
