using UnityEngine;

public class InformationAttribute : PropertyAttribute
{
	public enum InformationType
	{
		Error,
		Info,
		None,
		Warning
	}

	public InformationAttribute(string message, InformationType type, bool messageAfterProperty)
	{
	}
}
