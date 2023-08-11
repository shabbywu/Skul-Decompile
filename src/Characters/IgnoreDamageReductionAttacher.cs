using System;
using Runnables.Triggers;
using UnityEngine;

namespace Characters;

[RequireComponent(typeof(Character))]
public sealed class IgnoreDamageReductionAttacher : MonoBehaviour
{
	[Serializable]
	public class DamageInfo
	{
		public string key;

		[SerializeField]
		[Range(0f, 1f)]
		public float extraIgnoreDamageReduction;
	}

	[GetComponent]
	[SerializeField]
	private Character _owner;

	[Trigger.Subcomponent]
	[SerializeField]
	private Trigger _trigger;

	[Tooltip("모든 공격에 포함되는 받는 데미지 감소 무시 값")]
	[Range(0f, 1f)]
	[SerializeField]
	private float _baseExtraIgnoreDamageReduction;

	[SerializeField]
	private DamageInfo[] _infos;

	private bool OnOwnerGiveDamage(ITarget target, ref Damage damage)
	{
		if ((Object)(object)target.character == (Object)null)
		{
			return false;
		}
		damage.ignoreDamageReduction += _baseExtraIgnoreDamageReduction;
		DamageInfo[] infos = _infos;
		foreach (DamageInfo damageInfo in infos)
		{
			string key = damageInfo.key;
			if (!string.IsNullOrWhiteSpace(key) && damage.key.Equals(key, StringComparison.OrdinalIgnoreCase))
			{
				damage.ignoreDamageReduction += damageInfo.extraIgnoreDamageReduction;
				return false;
			}
		}
		return false;
	}

	private void Awake()
	{
		if (!((Object)(object)_owner == (Object)null) && _trigger.IsSatisfied())
		{
			((PriorityList<GiveDamageDelegate>)_owner.onGiveDamage).Add(int.MaxValue, (GiveDamageDelegate)OnOwnerGiveDamage);
		}
	}
}
