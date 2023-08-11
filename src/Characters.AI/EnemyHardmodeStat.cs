using Data;
using UnityEngine;

namespace Characters.AI;

[RequireComponent(typeof(Character))]
public sealed class EnemyHardmodeStat : MonoBehaviour
{
	[SerializeField]
	[GetComponent]
	private Character _character;

	private void Start()
	{
		if (!GameData.HardmodeProgress.hardmode)
		{
			Object.Destroy((Object)(object)this);
		}
		else
		{
			new EnemyBonusStatInHardmode().AttachTo(_character);
		}
	}
}
