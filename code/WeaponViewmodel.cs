namespace Sandbox;

public sealed class WeaponViewmodel : Component
{
    [Property, ReadOnly] Player localPlayer;
    [Property, ReadOnly] PlayerInventory inventory;
    [Property, ReadOnly] private GameObject viewmodelGameObject;
    [Property, ReadOnly] private BaseWeapon? activeWeapon;
    private bool hasViewmodel;

    protected override void OnAwake()
    {
        UpdatePlayerData();
    }

    private void UpdatePlayerData()
    {
        localPlayer ??= Player.FindLocalPlayer();

        if (localPlayer != null)
        {
            inventory = localPlayer.GetComponent<PlayerInventory>();
            if (inventory == null)
            {
                Log.Error("Local player inventory component not found!");
                return;
            }

            UpdateActiveWeapon();
        }
    }

    private void UpdateActiveWeapon()
    {
        var currentWeapon = inventory.ActiveWeapon;

        if (currentWeapon != activeWeapon)
        {
            activeWeapon = currentWeapon;
            DestroyViewmodel();
            CheckViewmodel();
        }
    }

    private void CheckViewmodel()
    {
        hasViewmodel = activeWeapon?.WeaponViewmodel.IsValid() ?? false;

        if (hasViewmodel)
        {
            SpawnViewmodel();
        }
        else
        {
            DestroyViewmodel();
        }
    }

    protected override void OnFixedUpdate()
    {
        if (IsProxy) return;

        UpdatePlayerData();
    }

    private void SpawnViewmodel()
    {
        if (viewmodelGameObject == null)
        {
            viewmodelGameObject = new GameObject();
            viewmodelGameObject.SetParent(Scene.Camera.GameObject);
            viewmodelGameObject.LocalTransform = global::Transform.Zero;

            CreateArmsAndWeaponObjects();
        }
    }

    private void CreateArmsAndWeaponObjects()
    {
        var armsObject = new GameObject();
        var weaponObject = new GameObject();

        armsObject.SetParent(viewmodelGameObject);
        weaponObject.SetParent(viewmodelGameObject);

        var armsRenderer = armsObject.AddComponent<SkinnedModelRenderer>();
        armsRenderer.Model = Model.Load("models/first_person/first_person_arms.vmdl");
        armsRenderer.LocalTransform = global::Transform.Zero;

        var weaponRenderer = weaponObject.AddComponent<SkinnedModelRenderer>();
        weaponRenderer.Model = activeWeapon.WeaponViewmodel;
        weaponRenderer.LocalTransform = global::Transform.Zero;

        armsRenderer.BoneMergeTarget = weaponRenderer;

        var controller = viewmodelGameObject.AddComponent<ViewmodelController>();
        controller.Target = weaponRenderer;
        controller.BodyController = localPlayer.GetComponent<BodyController>();
    }

    private void DestroyViewmodel()
    {
        if (viewmodelGameObject != null)
        {
            viewmodelGameObject.DestroyImmediate();
            viewmodelGameObject = null;
        }
    }

}
