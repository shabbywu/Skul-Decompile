using System;

[AttributeUsage(AttributeTargets.Field)]
public class GetComponentInParentAttribute : ReadOnlyAttribute
{
	public readonly bool includeInactive;

	public GetComponentInParentAttribute(bool includeInactive = false)
	{
		this.includeInactive = includeInactive;
	}
}
