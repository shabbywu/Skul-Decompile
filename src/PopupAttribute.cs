using System.Linq;
using UnityEngine;

public class PopupAttribute : PropertyAttribute
{
	public readonly bool allowCustom;

	public readonly object[] list;

	public PopupAttribute(params object[] list)
	{
		this.list = list;
	}

	public PopupAttribute(bool allowCustom, params object[] list)
	{
		this.allowCustom = allowCustom;
		if (this.allowCustom)
		{
			this.list = list.Append("Custom").ToArray();
		}
		else
		{
			this.list = list;
		}
	}
}
