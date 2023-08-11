using System.Collections.Generic;
using Characters;
using UnityEngine;

namespace Level.Waves;

public sealed class PinGroup : MonoBehaviour
{
	private ICollection<Pin> _pins;

	private ICollection<Character> _characters;

	public ICollection<Character> Load()
	{
		_pins = ((Component)this).GetComponentsInChildren<Pin>();
		_characters = new List<Character>(_pins.Count);
		foreach (Pin pin in _pins)
		{
			foreach (Character item in pin.Load())
			{
				_characters.Add(item);
			}
		}
		return _characters;
	}
}
