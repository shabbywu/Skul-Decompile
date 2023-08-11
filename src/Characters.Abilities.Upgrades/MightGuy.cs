using System;
using System.Linq;
using Characters.Actions;
using Characters.Operations;
using FX;
using Level;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Upgrades;

[Serializable]
public sealed class MightGuy : Ability
{
	public sealed class Instance : AbilityInstance<MightGuy>
	{
		private EffectPoolInstance _loopEffect;

		private int _remainFreeDash;

		public override int iconStacks
		{
			get
			{
				if (_remainFreeDash > 0)
				{
					return _remainFreeDash;
				}
				return 0;
			}
		}

		public override float iconFillAmount => (_remainFreeDash <= 0) ? 1 : 0;

		public Instance(Character owner, MightGuy ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			Singleton<Service>.Instance.levelManager.onMapChangedAndFadedIn += HandleOnMapLoaded;
			_remainFreeDash = ability._freeDashCountDefault;
			owner.onStartAction += HandleOnStartAction;
		}

		private void HandleOnStartAction(Characters.Actions.Action action)
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			if (action.type == Characters.Actions.Action.Type.Dash)
			{
				_remainFreeDash--;
				if (_remainFreeDash == 0)
				{
					_loopEffect = ability._debuffLoopEffect.Spawn(((Component)owner).transform.position, owner);
					PersistentSingleton<SoundManager>.Instance.PlaySound(ability._debuffAttachAudioClipInfo, ((Component)owner).transform.position);
				}
				if (_remainFreeDash < 0)
				{
					owner.health.TakeHealth(ability._loseHealthAmount);
					Singleton<Service>.Instance.floatingTextSpawner.SpawnPlayerTakingDamage(ability._loseHealthAmount, Vector2.op_Implicit(((Component)owner).transform.position));
					((MonoBehaviour)owner).StartCoroutine(ability._onLoseHealth.CRun(owner));
				}
			}
		}

		private void HandleOnMapLoaded(Map old, Map @new)
		{
			if ((Object)(object)_loopEffect != (Object)null)
			{
				_loopEffect.Stop();
				_loopEffect = null;
			}
			if (@new.waveContainer.GetAllEnemies().Any((Character character) => character.type == Character.Type.Adventurer || character.type == Character.Type.Boss))
			{
				_remainFreeDash = ability._freeDashCountInBossMap;
			}
			else
			{
				_remainFreeDash = ability._freeDashCountDefault;
			}
		}

		protected override void OnDetach()
		{
			if ((Object)(object)_loopEffect != (Object)null)
			{
				_loopEffect.Stop();
				_loopEffect = null;
			}
			Singleton<Service>.Instance.levelManager.onMapChangedAndFadedIn -= HandleOnMapLoaded;
			owner.onStartAction -= HandleOnStartAction;
		}
	}

	[SerializeField]
	private int _loseHealthAmount;

	[SerializeField]
	private int _freeDashCountDefault;

	[SerializeField]
	private int _freeDashCountInBossMap;

	[SerializeField]
	private EffectInfo _debuffLoopEffect = new EffectInfo
	{
		subordinated = true
	};

	[SerializeField]
	private SoundInfo _debuffAttachAudioClipInfo;

	[Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	private OperationInfo.Subcomponents _onLoseHealth;

	public override void Initialize()
	{
		base.Initialize();
		_onLoseHealth.Initialize();
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
