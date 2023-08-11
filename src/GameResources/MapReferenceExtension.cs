namespace GameResources;

public static class MapReferenceExtension
{
	public static bool IsNullOrEmpty(this MapReference mapReference)
	{
		return mapReference?.empty ?? true;
	}
}
