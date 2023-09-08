using System;
using System.Collections;
using System.Collections.Generic;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Upgrades;

[Serializable]
public class Plague : Ability
{
	public class Instance : AbilityInstance<Plague>
	{
		private HashSet<Character> _targets;

		private CoroutineReference _applyReference;

		public Instance(Character owner, Plague ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			_targets = new HashSet<Character>();
			if (owner.playerComponents != null)
			{
				Singleton<Service>.Instance.levelManager.onMapLoaded += HandleOnMapLoaded;
				HandleOnMapLoaded();
			}
		}

		private void HandleOnMapLoaded()
		{
			_applyReference.Stop();
			if ((Object)(object)owner != (Object)null)
			{
				_applyReference = ((MonoBehaviour)(object)owner).StartCoroutineWithReference(Apply());
			}
		}

		private IEnumerator Apply()
		{
			_targets.Clear();
			while (true)
			{
				foreach (Character allEnemy in Map.Instance.waveContainer.GetAllEnemies())
				{
					if (!_targets.Contains(allEnemy) && !allEnemy.health.dead && ability._characterType[allEnemy.type] && !((Object)(object)allEnemy.ability == (Object)null))
					{
						allEnemy.ability.Add(ability._plagueAbility.ability);
						_targets.Add(allEnemy);
					}
				}
				yield return Chronometer.global.WaitForSeconds(0.1f);
			}
		}

		protected override void OnDetach()
		{
			Singleton<Service>.Instance.levelManager.onMapLoadedAndFadedIn -= HandleOnMapLoaded;
		}
	}

	[SerializeField]
	private CharacterTypeBoolArray _characterType;

	[AbilityComponent.Subcomponent]
	[SerializeField]
	private AbilityComponent _plagueAbility;

	public override void Initialize()
	{
		base.Initialize();
		_plagueAbility.Initialize();
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
