using System.Collections.Generic;
using Characters;
using UnityEngine;

namespace Level;

public class SelectableInWave : MonoBehaviour
{
	[SerializeField]
	private Character[] _characters;

	[SerializeField]
	private SpriteRenderer[] _additionalIcons;

	public Character[] characters => _characters;

	private void Awake()
	{
		Object.Destroy((Object)(object)this);
	}

	public SpriteRenderer[] GetIcons()
	{
		List<SpriteRenderer> list = new List<SpriteRenderer>();
		Character[] array = _characters;
		foreach (Character character in array)
		{
			list.Add(((Component)character.@base).GetComponentInChildren<SpriteRenderer>());
		}
		list.AddRange(_additionalIcons);
		return list.ToArray();
	}
}
