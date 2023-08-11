using System;
using System.Collections;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public class Heal : Ability
{
	public class Instance : AbilityInstance<Heal>
	{
		private CoroutineReference _cHealReference;

		public Instance(Character owner, Heal ability)
			: base(owner, ability)
		{
		}

		public override void Refresh()
		{
			base.Refresh();
			OnAttach();
		}

		protected override void OnAttach()
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			((CoroutineReference)(ref _cHealReference)).Stop();
			_cHealReference = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)owner, CHeal());
		}

		protected override void OnDetach()
		{
			((CoroutineReference)(ref _cHealReference)).Stop();
		}

		private IEnumerator CHeal()
		{
			for (int i = 0; i < ability._count; i++)
			{
				owner.health.PercentHeal((float)(ability._totalPercent / ability._count) * 0.01f);
				yield return ChronometerExtension.WaitForSeconds((ChronometerBase)(object)owner.chronometer.master, ability.duration / (float)ability._count);
			}
		}
	}

	[SerializeField]
	private int _totalPercent;

	[SerializeField]
	private int _count = 3;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
