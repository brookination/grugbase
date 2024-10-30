namespace Grugchamber.GrugUtil;

public static class WeaponEffects
{
	/// <summary>
	/// The dictionary of decal effects, the key is the surface resource path, the value is the decal path.
	/// </summary>
	public static readonly Dictionary<string, string> DecalEffectsKey = new Dictionary<string, string>()
	{
		{ "everything", "materials/decals/bullethole.vmat" },
		{ "surfaces/flesh.surface", "materials/decals/flesh/flesh1.vmat" }
	};

	/// <summary>
	/// Get the bullet hole decal from surface.
	/// </summary>
	/// <param name="surfacePath"></param>
	/// <returns></returns>
	public static string GetDecalFromSurface( string surfacePath )
	{
		if ( DecalEffectsKey.TryGetValue( surfacePath, out var decalPath ) )
		{
			return decalPath;
		}

		// If no specific surface is found, fall back to "everything"
		return DecalEffectsKey["everything"];
	}
}
