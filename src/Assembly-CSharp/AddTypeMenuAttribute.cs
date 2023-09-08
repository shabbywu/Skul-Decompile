using System;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
public sealed class AddTypeMenuAttribute : Attribute
{
	private static readonly char[] k_Separeters = new char[1] { '/' };

	public string MenuName { get; }

	public int Order { get; }

	public AddTypeMenuAttribute(string menuName, int order = 0)
	{
		MenuName = menuName;
		Order = order;
	}

	public string[] GetSplittedMenuName()
	{
		if (string.IsNullOrWhiteSpace(MenuName))
		{
			return Array.Empty<string>();
		}
		return MenuName.Split(k_Separeters, StringSplitOptions.RemoveEmptyEntries);
	}

	public string GetTypeNameWithoutPath()
	{
		string[] splittedMenuName = GetSplittedMenuName();
		if (splittedMenuName.Length == 0)
		{
			return null;
		}
		return splittedMenuName[^1];
	}
}
