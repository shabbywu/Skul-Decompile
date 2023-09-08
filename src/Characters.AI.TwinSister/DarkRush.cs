using System.Collections;
using Characters.AI.Behaviours;
using Characters.Actions;
using Data;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.TwinSister;

public class DarkRush : MonoBehaviour
{
	[SerializeField]
	private Action _standing;

	[Subcomponent(typeof(TeleportBehind))]
	[SerializeField]
	[Header("MeleeAttack")]
	[Space]
	private TeleportBehind _teleportBehind;

	[SerializeField]
	private Action _fristAttack;

	[SerializeField]
	private Action _secondAttack;

	[Header("Last Attack")]
	[SerializeField]
	private Action _finishAttack;

	[Space]
	[SerializeField]
	private ParentPool _parentPool;

	[SerializeField]
	private float _attackLength_normal = 1f;

	[SerializeField]
	private float _attackLength_hard = 1f;

	public IEnumerator CRun(DarkAideAI darkAideAI)
	{
		darkAideAI.character.status.unstoppable.Attach(this);
		yield return _teleportBehind.CRun(darkAideAI);
		_fristAttack.TryStart();
		while (_fristAttack.running)
		{
			yield return null;
		}
		yield return _teleportBehind.CRun(darkAideAI);
		_secondAttack.TryStart();
		while (_secondAttack.running)
		{
			yield return null;
		}
		yield return CFinishAttack();
		darkAideAI.character.status.unstoppable.Detach(this);
		while (_standing.running)
		{
			yield return null;
		}
	}

	private IEnumerator CFinishAttack()
	{
		_finishAttack.TryStart();
		while (_finishAttack.running)
		{
			yield return null;
		}
		DarkRushEffect[] componentsInChildren = ((Component)_parentPool.currentEffectParent).GetComponentsInChildren<DarkRushEffect>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].HideSign();
		}
		_standing.TryStart();
		componentsInChildren = ((Component)_parentPool.currentEffectParent).GetComponentsInChildren<DarkRushEffect>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].ShowImpact();
		}
		float seconds = (GameData.HardmodeProgress.hardmode ? _attackLength_hard : _attackLength_normal);
		yield return Chronometer.global.WaitForSeconds(seconds);
		componentsInChildren = ((Component)_parentPool.currentEffectParent).GetComponentsInChildren<DarkRushEffect>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].HideImpact();
		}
	}

	public bool CanUse()
	{
		return _finishAttack.canUse;
	}
}
