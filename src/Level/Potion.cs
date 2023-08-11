using Characters;
using Characters.Abilities;
using Characters.Abilities.Savable;
using Characters.Abilities.Upgrades;
using Characters.Operations.Fx;
using Data;
using FX.SpriteEffects;
using GameResources;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Level;

public class Potion : DroppedGear
{
	public enum Size
	{
		Small,
		Medium,
		Large
	}

	[FormerlySerializedAs("_healthHealingPercent")]
	[SerializeField]
	private int _healAmount;

	[SerializeField]
	private int priority;

	[SerializeField]
	private Color _startColor;

	[SerializeField]
	private Color _endColor;

	[SerializeField]
	private Curve _curve;

	[SerializeField]
	[Subcomponent(typeof(SpawnEffect))]
	private SpawnEffect _spawn;

	[SerializeField]
	[Header("생명전환 : 저주 요소")]
	private int _potionToPowerAmount = 1;

	private const string _prefix = "Potion";

	protected string _keyBase => "Potion/" + ((Object)this).name;

	public string displayName => Localization.GetLocalizedString(_keyBase + "/name");

	public string description => Localization.GetLocalizedString(_keyBase + "/desc");

	protected override bool _interactable
	{
		get
		{
			Character player = Singleton<Service>.Instance.levelManager.player;
			if (player.ability.GetInstance<LifeChange>() != null)
			{
				return true;
			}
			if (player.ability.GetInstance<Glutton>() != null)
			{
				return true;
			}
			if (player.health.percent == 1.0)
			{
				return false;
			}
			return true;
		}
	}

	public override void InteractWith(Character character)
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		if (!GameData.Currency.gold.Has(price))
		{
			return;
		}
		IAbilityInstance instance = character.ability.GetInstance<LifeChange>();
		IAbilityInstance instance2 = character.ability.GetInstance<Glutton>();
		if (instance != null || instance2 != null || character.health.percent != 1.0)
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(_interactSound, ((Component)this).transform.position);
			_spawn.Run(character);
			character.spriteEffectStack.Add(new EasedColorBlend(priority, _startColor, _endColor, _curve));
			if (character.ability.GetInstance<LifeChange>() != null)
			{
				character.playerComponents.savableAbilityManager.IncreaseStack(SavableAbilityManager.Name.LifeChange, 1f);
			}
			else
			{
				character.health.Heal(new Health.HealInfo(Health.HealthGiverType.Potion, _healAmount));
			}
			Object.Destroy((Object)(object)((Component)this).gameObject);
		}
	}

	public override void OpenPopupBy(Character character)
	{
	}

	public override void ClosePopup()
	{
	}

	public void Initialize()
	{
	}
}
