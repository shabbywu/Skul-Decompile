using System.Collections;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours;

public sealed class TeleportAttack : Behaviour
{
	[Subcomponent(typeof(TeleportBehind))]
	[SerializeField]
	private TeleportBehind _teleportBehind;

	[SerializeField]
	[Subcomponent(typeof(ActionAttack))]
	private ActionAttack _attack;

	[Subcomponent(typeof(EscapeTeleport))]
	[SerializeField]
	private EscapeTeleport _escapeTeleport;

	private Behaviour[] _behaviours;

	private void Awake()
	{
		_behaviours = new Behaviour[3] { _teleportBehind, _attack, _escapeTeleport };
	}

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		for (int i = 0; i < _behaviours.Length; i++)
		{
			yield return _behaviours[i].CRun(controller);
			if (base.result != Result.Doing)
			{
				yield break;
			}
		}
		base.result = Result.Done;
	}

	public bool CanUse()
	{
		return _attack.CanUse();
	}
}
