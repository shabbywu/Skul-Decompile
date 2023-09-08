using System;

[AttributeUsage(AttributeTargets.Field)]
public class GetComponentInChildrenAttribute : ReadOnlyAttribute
{
	public readonly bool includeInactive;

	public GetComponentInChildrenAttribute(bool includeInactive = false)
	{
		this.includeInactive = includeInactive;
	}
}
