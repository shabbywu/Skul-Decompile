using System;
using System.Collections;
using System.Collections.Generic;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Upgrades;

[Serializable]
public sealed class RiskyDarkEnemy : Ability
{
	public sealed class Instance : AbilityInstance<RiskyDarkEnemy>
	{
		private HashSet<Character> _targets;

		private CoroutineReference _applyReference;

		public Instance(Character owner, RiskyDarkEnemy ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			ability._targetAbility.Initialize();
			_targets = new HashSet<Character>();
			if (owner.playerComponents != null)
			{
				owner.ability.Add(ability._targetAbility.ability);
				Singleton<Service>.Instance.levelManager.onMapLoaded += HandleOnMapLoaded;
			}
		}

		private void HandleOnMapLoaded()
		{
			_applyReference.Stop();
			_applyReference = ((MonoBehaviour)(object)owner).StartCoroutineWithReference(Apply());
		}

		private IEnumerator Apply()
		{
			_targets.Clear();
			while (true)
			{
				foreach (Character allSpawnedEnemy in Map.Instance.waveContainer.GetAllSpawnedEnemies())
				{
					if (!_targets.Contains(allSpawnedEnemy) && !allSpawnedEnemy.health.dead && allSpawnedEnemy.type == Character.Type.Named)
					{
						allSpawnedEnemy.ability.Add(ability._targetAbility.ability);
						_targets.Add(allSpawnedEnemy);
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
	[AbilityComponent.Subcomponent]
	private AbilityComponent _targetAbility;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
