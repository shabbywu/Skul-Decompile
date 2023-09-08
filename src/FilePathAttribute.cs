using UnityEngine;

public class FilePathAttribute : PropertyAttribute
{
	public readonly string title;

	public readonly string defaultName;

	public FilePathAttribute(string title)
	{
		this.title = title;
		defaultName = string.Empty;
	}

	public FilePathAttribute(string title, string defaultName)
	{
		this.title = title;
		this.defaultName = defaultName;
	}
}
