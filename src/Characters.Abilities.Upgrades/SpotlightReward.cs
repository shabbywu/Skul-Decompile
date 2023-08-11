using System;
using Data;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Upgrades;

[Serializable]
public class SpotlightReward : Ability
{
	public class Instance : AbilityInstance<SpotlightReward>
	{
		internal Instance(Character owner, SpotlightReward ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			Character character = owner;
			character.onKilled = (Character.OnKilledDelegate)Delegate.Combine(character.onKilled, new Character.OnKilledDelegate(HandleOnKilled));
		}

		private void HandleOnKilled(ITarget target, ref Damage damage)
		{
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			Character character = target.character;
			if (!((Object)(object)character == (Object)null) && ((EnumArray<Character.Type, bool>)ability._characterTypes)[character.type])
			{
				GameData.Currency.Type type = (MMMaths.RandomBool() ? GameData.Currency.Type.Bone : GameData.Currency.Type.Gold);
				Vector2Int goldrewardAmount = Singleton<Service>.Instance.levelManager.currentChapter.currentStage.goldrewardAmount;
				int x = ((Vector2Int)(ref goldrewardAmount)).x;
				goldrewardAmount = Singleton<Service>.Instance.levelManager.currentChapter.currentStage.goldrewardAmount;
				int amount = Random.Range(x, ((Vector2Int)(ref goldrewardAmount)).y);
				Singleton<Service>.Instance.levelManager.DropCurrencyBag(type, (Rarity)2, amount, 16, ((Component)character).transform.position);
			}
		}

		protected override void OnDetach()
		{
			Character character = owner;
			character.onKilled = (Character.OnKilledDelegate)Delegate.Remove(character.onKilled, new Character.OnKilledDelegate(HandleOnKilled));
		}
	}

	[SerializeField]
	private CharacterTypeBoolArray _characterTypes;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
