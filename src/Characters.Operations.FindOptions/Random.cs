using System;
using System.Collections.Generic;

namespace Characters.Operations.FindOptions;

[Serializable]
public class Random : IFilter
{
	public void Filtered(List<Character> characters)
	{
		Character item = ExtensionMethods.Random<Character>((IEnumerable<Character>)characters);
		characters.Clear();
		characters.Add(item);
	}
}
