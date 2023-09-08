using System;
using System.Collections.Generic;

namespace Characters.Operations.FindOptions;

[Serializable]
public class Random : IFilter
{
	public void Filtered(List<Character> characters)
	{
		Character item = characters.Random();
		characters.Clear();
		characters.Add(item);
	}
}
