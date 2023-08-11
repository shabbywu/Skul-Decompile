using UnityEngine;

namespace Characters.Operations.LookAtTargets;

public sealed class Chance : Target
{
	[SerializeField]
	[Range(0f, 1f)]
	private float _lookRightChance = 0.5f;

	public override Character.LookingDirection GetDirectionFrom(Character character)
	{
		if (!MMMaths.Chance(_lookRightChance))
		{
			return Character.LookingDirection.Left;
		}
		return Character.LookingDirection.Right;
	}
}
