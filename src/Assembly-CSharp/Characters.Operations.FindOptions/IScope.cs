using System.Collections.Generic;

namespace Characters.Operations.FindOptions;

public interface IScope
{
	List<Character> GetEnemyList();
}
