using System.Collections.Generic;

namespace Characters.Operations.FindOptions;

public interface IFilter
{
	void Filtered(List<Character> characters);
}
