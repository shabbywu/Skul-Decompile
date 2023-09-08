using System;
using FX;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Items;

[Serializable]
public sealed class OmenManaCycle : Ability
{
	public sealed class Instance : AbilityInstance<OmenManaCycle>
	{
		private bool _isA;

		private EffectPoolInstance _effectInstance;

		public Instance(Character owner, OmenManaCycle ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			_isA = true;
			owner.stat.AttachValues(ability._statOnA);
			_effectInstance = ((ability._effectOnA == null) ? null : ability._effectOnA.Spawn(((Component)owner).transform.position, owner));
			PersistentSingleton<SoundManager>.Instance.PlaySound(ability._effectASoundInfo, ((Component)owner).transform.position);
		}

		public override void Refresh()
		{
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			if (_isA)
			{
				if ((Object)(object)_effectInstance != (Object)null)
				{
					_effectInstance.Stop();
				}
				_effectInstance = ((ability._effectOnB == null) ? null : ability._effectOnB.Spawn(((Component)owner).transform.position, owner));
				PersistentSingleton<SoundManager>.Instance.PlaySound(ability._effectBSoundInfo, ((Component)owner).transform.position);
				owner.stat.DetachValues(ability._statOnA);
				owner.stat.AttachValues(ability._statOnB);
				_isA = false;
			}
			else
			{
				if ((Object)(object)_effectInstance != (Object)null)
				{
					_effectInstance.Stop();
				}
				_effectInstance = ((ability._effectOnA == null) ? null : ability._effectOnA.Spawn(((Component)owner).transform.position, owner));
				PersistentSingleton<SoundManager>.Instance.PlaySound(ability._effectASoundInfo, ((Component)owner).transform.position);
				owner.stat.DetachValues(ability._statOnB);
				owner.stat.AttachValues(ability._statOnA);
				_isA = true;
			}
		}

		protected override void OnDetach()
		{
			if ((Object)(object)_effectInstance != (Object)null)
			{
				_effectInstance.Stop();
				_effectInstance = null;
			}
			owner.stat.DetachValues(ability._statOnA);
			owner.stat.DetachValues(ability._statOnB);
		}
	}

	[SerializeField]
	private EffectInfo _effectOnA = new EffectInfo
	{
		subordinated = true
	};

	[SerializeField]
	private SoundInfo _effectASoundInfo;

	[SerializeField]
	private Stat.Values _statOnA;

	[SerializeField]
	private EffectInfo _effectOnB = new EffectInfo
	{
		subordinated = true
	};

	[SerializeField]
	private SoundInfo _effectBSoundInfo;

	[SerializeField]
	private Stat.Values _statOnB;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
