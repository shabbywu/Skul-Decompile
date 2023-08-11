using System;
using FX;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.CharacterStat;

[Serializable]
public class StatBonusAndTimeBonusByTrigger : Ability
{
	public class Instance : AbilityInstance<StatBonusAndTimeBonusByTrigger>
	{
		public Instance(Character owner, StatBonusAndTimeBonusByTrigger ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			owner.stat.AttachValues(ability._stat);
			ability.trigger.Attach(owner);
			ability.trigger.onTriggered += OnTriggered;
			base.remainTime = ability.duration;
			PersistentSingleton<SoundManager>.Instance.PlaySound(ability._effectAudioClipInfo, ((Component)owner).transform.position);
		}

		private void OnTriggered()
		{
			base.remainTime += ability._bounsTime;
			if (ability._clampToAbilityDuration)
			{
				base.remainTime = Mathf.Min(base.remainTime, ability.duration);
			}
		}

		public override void UpdateTime(float deltaTime)
		{
			base.remainTime -= deltaTime;
		}

		protected override void OnDetach()
		{
			owner.stat.DetachValues(ability._stat);
			ability.trigger.Detach();
			ability.trigger.onTriggered += OnTriggered;
		}
	}

	[SerializeField]
	private SoundInfo _effectAudioClipInfo;

	[SerializeField]
	private Stat.Values _stat;

	[SerializeField]
	private int _bounsTime;

	[SerializeField]
	private bool _clampToAbilityDuration;

	public ITrigger trigger;

	public StatBonusAndTimeBonusByTrigger()
	{
	}

	public StatBonusAndTimeBonusByTrigger(Stat.Values stat)
	{
		_stat = stat;
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
