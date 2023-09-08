using Scenes;
using UnityEngine;

namespace Characters.Abilities.Constraints;

public class EndingCredit : Constraint
{
	public override bool Pass()
	{
		return !((Component)Scene<GameBase>.instance.uiManager.endingCredit).gameObject.activeInHierarchy;
	}
}
