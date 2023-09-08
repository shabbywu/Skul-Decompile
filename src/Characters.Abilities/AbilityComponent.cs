using System;
using Characters.Abilities.CharacterStat;
using Characters.Abilities.Customs;
using Characters.Abilities.Darks;
using Characters.Abilities.Debuffs;
using Characters.Abilities.Decorators;
using Characters.Abilities.Enemies;
using Characters.Abilities.Essences;
using Characters.Abilities.Items;
using Characters.Abilities.Upgrades;
using Characters.Abilities.Weapons;
using Characters.Abilities.Weapons.DavyJones;
using Characters.Abilities.Weapons.EntSkul;
using Characters.Abilities.Weapons.Fighter;
using Characters.Abilities.Weapons.Ghoul;
using Characters.Abilities.Weapons.GrimReaper;
using Characters.Abilities.Weapons.Minotaurus;
using Characters.Abilities.Weapons.Skeleton_Sword;
using Characters.Abilities.Weapons.Wizard;
using Characters.Abilities.Weapons.Yaksha;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities;

public abstract class AbilityComponent : MonoBehaviour
{
	public class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
	{
		public new static readonly Type[] types;

		public new static readonly string[] names;

		static SubcomponentAttribute()
		{
			types = new Type[263]
			{
				typeof(NothingComponent),
				null,
				typeof(StatBonusComponent),
				typeof(StackableStatBonusComponent),
				typeof(StackableStatBonusByTimeComponent),
				typeof(StackableStatBonusByLostHealthComponent),
				typeof(TimeGradientStatBonusComponent),
				null,
				typeof(StatBonusByAirTimeComponent),
				typeof(StatBonusByMovingComponent),
				typeof(StatBonusByShieldComponent),
				typeof(StatBonusByIncomeComponent),
				typeof(StatBonusByOutcomeComponent),
				typeof(StatBonusByBalanceComponent),
				typeof(StatBonusBySkillsInCooldownComponent),
				typeof(StatBonusByOtherStatComponent),
				typeof(StatBonusByGaveDamageComponent),
				typeof(StatBonusByKillComponent),
				typeof(StatBonusAndTimeBonusByTriggerComponent),
				null,
				typeof(StatByApplyingStatusCountsWithinRangeComponent),
				typeof(StatByCountsWithinRangeComponent),
				typeof(StatBonusByTargetStatusComponent),
				null,
				typeof(StatBonusByMaxHealthComponent),
				typeof(StatPerCurrentHealthComponent),
				typeof(StatPerLostHealthComponent),
				typeof(StatBonusPerGearRarityComponent),
				typeof(StatBonusPerGearTagComponent),
				typeof(StatBonusByInscriptionCountComponent),
				typeof(StatBonusPerInscriptionStackComponent),
				typeof(StatBonusPerHealComponent),
				null,
				typeof(StatBonusCooldownableComponent),
				null,
				typeof(StatDebuffOnStatusComponent),
				typeof(OverrideFinalStatValuesComponent),
				typeof(UniqueStatBonusComponent),
				null,
				typeof(MassacreStatBonusComponent),
				null,
				typeof(AttachAbilityOnGaveDamageComponent),
				typeof(CurseOfLightComponent),
				typeof(GiveCurseOfLightComponent),
				null,
				typeof(OmenMisfortuneComponent),
				typeof(OmenSunAndMoonComponent),
				typeof(OmenExecutionComponent),
				typeof(OmenManaCycleComponent),
				typeof(OmenArmsComponent),
				typeof(FenrirFangComponent),
				typeof(GoludaluSummonBookComponent),
				typeof(GiantsCavalryComponent),
				typeof(SymbolOfToughnessComponent),
				typeof(BigBloodComponent),
				typeof(AccelerationSwordComponent),
				typeof(HealthScreeningsMachineComponent),
				typeof(BloodEatingSwordComponent),
				typeof(MiracleGrailComponent),
				typeof(GreatHerosArmorComponent),
				typeof(ManeOfBeastKingComponent),
				typeof(VictoryBatonComponent),
				typeof(StoneMaskComponent),
				typeof(CrownOfThornsComponent),
				typeof(StigmaLegComponent),
				typeof(WindMailComponent),
				typeof(CowardlyCloakComponent),
				typeof(FrostGiantsLeatherComponent),
				typeof(SylphidsWingComponent),
				typeof(MasterPieceBerserkersGloveComponent),
				typeof(ChosenHerosCircletComponent),
				typeof(ChosenWarriorsArmorComponent),
				typeof(ChosenThiefDaggersComponent),
				typeof(ChosenClericsBibleComponent),
				typeof(CupOfFateComponent),
				typeof(OldSpellBookCoverComponent),
				typeof(DarkPriestsRobesShieldComponent),
				typeof(GraceOfLeoniaComponent),
				typeof(FightersBeltComponent),
				null,
				typeof(DwarfComponent),
				typeof(KirizComponent),
				typeof(EvileEyeComponent),
				typeof(CharmComponent),
				typeof(FaceBugComponent),
				typeof(SpectorOwnerComponent),
				typeof(PrisonerPassiveComponent),
				typeof(PrisonerChestPassiveComponent),
				typeof(PrisonerLancePassiveComponent),
				typeof(YakshaHomePassiveComponent),
				typeof(GhoulPassive2Component),
				typeof(GrimReaperPassiveComponent),
				typeof(GrimReaperHarvestPassiveComponent),
				typeof(MinotaurusPassiveComponent),
				typeof(ChallengerMarkPassiveComponent),
				typeof(StatBonusByEntGassCountComponent),
				typeof(Skeleton_SwordPassiveComponent),
				typeof(Skeleton_SwordPassiveTetanusComponent),
				typeof(Skeleton_SwordTetanusDamageComponent),
				typeof(WizardPassiveComponent),
				typeof(WizardManaChargingSpeedBonusComponent),
				typeof(WizardTranscendenceComponent),
				typeof(DavyJonesPassiveComponent),
				typeof(LazinessOfGeniusComponent),
				typeof(PlagueComponent),
				typeof(KettleOfSwampWitchComponent),
				typeof(SpotlightRewardComponent),
				typeof(GreatMountainForceComponent),
				typeof(GluttonComponent),
				typeof(BoneShieldComponent),
				typeof(StatusRegrantComponent),
				typeof(TimeChargingStatBonusComponent),
				typeof(OneHitSkillDamageComponent),
				typeof(OneHitSkillDamageMarkingComponent),
				typeof(ManyHitMarkComponent),
				typeof(TimeBombGiverComponent),
				typeof(TimeBombComponent),
				typeof(WhaleboneAmuletComponent),
				typeof(ThornsArmorComponent),
				typeof(MightGuyComponent),
				typeof(AngrySightComponent),
				typeof(DamageAttributeChangeComponent),
				typeof(HealthArmorComponnet),
				typeof(RecklessPostureComponent),
				typeof(ObsessiveCompulsiveComponent),
				typeof(KingSlayerComponent),
				typeof(LivingArmorComponent),
				typeof(GhostStoriesComponent),
				typeof(BallistaComponent),
				typeof(RechargeComponent),
				typeof(QuarantineComponent),
				typeof(ProtectionComponent),
				null,
				typeof(DotDamageComponent),
				typeof(StackableModifyTakingDamageComponent),
				typeof(DamageMissionComponent),
				typeof(ReverseHorizontalInputComponent),
				typeof(BlockCriticalComponent),
				null,
				typeof(RandomAbilitiesComponent),
				typeof(AirAndGroundAttackDamageComponent),
				typeof(ExtraDamageToBackComponent),
				typeof(ModifyDamageByStackResolverComponent),
				typeof(ModifyTakingDamageComponent),
				typeof(ModifyDamageComponent),
				typeof(ModifyDamageStackableComponent),
				typeof(ModifyDamageByDistanceComponent),
				typeof(ModifyDamageByTargetLayerComponent),
				typeof(ModifyDamageByTargetSizeComponent),
				typeof(ModifyTrapDamageComponent),
				typeof(ModifyDamageByDashDistanceComponent),
				typeof(ModifyDamageOnStatusComponent),
				typeof(ModifyActionSpeedComponent),
				typeof(ModifyDamageByOwnerAndTargetHealthComponent),
				typeof(ModifyStatusDurationComponent),
				typeof(ModifyDamageByStatComponent),
				typeof(ChangeTakingDamageToOneComponent),
				typeof(IgnoreShieldOverDamageComponent),
				typeof(IgnoreTakingDamageByDirectionComponent),
				typeof(IgnoreTrapDamageComponent),
				typeof(CriticalToMaximumHealthComponent),
				null,
				typeof(AddAirJumpCountComponent),
				typeof(AttachAbilityToTargetOnGaveDamageComponent),
				typeof(AddGaugeValueOnGaveDamageComponent),
				typeof(AddMaxGaugeValueComponent),
				typeof(StackableAdditionalHitComponent),
				typeof(AdditionalHitComponent),
				typeof(AdditionalHitOnStatusTriggerComponent),
				typeof(AdditionalHitByTargetStatusComponent),
				typeof(AdditionalHitToStatusTakerComponent),
				typeof(ApplyStatusOnGaveDamageComponent),
				typeof(ApplyStatusOnGaveEmberDamageComponent),
				typeof(ApplyStatusOnApplyStatusComponent),
				typeof(AttachAbilityWithinColliderComponent),
				typeof(AttachAbilityToMinionWithinColliderComponent),
				typeof(AttachAbilityToStatusTargetComponent),
				typeof(DetachAbilityByTriggerComponent),
				null,
				typeof(ReduceCooldownByTriggerComponent),
				typeof(ReduceSwapCooldownByTriggerComponent),
				typeof(ReduceCooldownAnotherSkillComponent),
				typeof(IgnoreSkillCooldownComponent),
				null,
				typeof(ChangeActionComponent),
				typeof(CurrencyBonusComponent),
				typeof(EnhanceComboActionComponent),
				typeof(GetInvulnerableComponent),
				typeof(GetLockoutComponent),
				typeof(GetEvasionComponent),
				typeof(GetEvasionShieldComponent),
				typeof(GetSilenceComponent),
				typeof(GetGuardComponent),
				typeof(GiveStatusOnGaveDamageComponent),
				typeof(ModifyTimeScaleComponent),
				typeof(OperationByTriggerComponent),
				typeof(OperationByTriggersComponent),
				typeof(OperationOnGuardMotionComponent),
				typeof(RunTargetOperationOnGaveDamageComponent),
				typeof(RunTargetOperationOnGiveDamageComponent),
				typeof(OverrideMovementConfigComponent),
				typeof(PeriodicHealComponent),
				typeof(ShieldComponent),
				typeof(StackableShieldComponent),
				typeof(ShieldByCountWithinRangeComponent),
				typeof(WeaponPolymorphComponent),
				typeof(ReviveComponent),
				typeof(HitBombComponent),
				null,
				typeof(AttachToOneTargetOnGaveDamageComponent),
				null,
				typeof(AlchemistGaugeBoostComponent),
				typeof(AlchemistGaugeDeactivateComponent),
				typeof(AlchemistPassiveComponent),
				null,
				typeof(RiderPassiveComponent),
				typeof(RiderSkeletonRiderComponent),
				null,
				typeof(ThiefPassiveComponent),
				typeof(SpawnThiefGoldOnTookDamageComponent),
				typeof(SpawnThiefGoldOnGaveDamageComponent),
				null,
				typeof(MummyPassiveComponent),
				typeof(MummyGunDropPassiveComponent),
				null,
				typeof(BombSkulPassiveComponent),
				typeof(ArchlichSoulLootingPassiveComponent),
				typeof(AwakenDarkPaladinPassiveComponent),
				typeof(Berserker2PassiveComponent),
				typeof(GhoulPassiveComponent),
				typeof(LivingArmorPassiveComponent),
				typeof(RecruitPassiveComponent),
				typeof(RockstarPassiveComponent),
				typeof(SamuraiPassive2Component),
				null,
				typeof(BoneOfBraveComponent),
				typeof(BoneOfManaComponent),
				typeof(BoneOfSpeedComponent),
				typeof(CriticalChanceByDistanceComponent),
				typeof(MagesManaBraceletComponent),
				typeof(MedalOfCarleonComponent),
				typeof(NonConsumptionComponent),
				null,
				typeof(Skeleton_ShieldExplosionPassiveComponent),
				typeof(Skeleton_Shield4GuardComponent),
				null,
				typeof(ElderEntsGratitudeComponent),
				typeof(OffensiveWheelComponent),
				typeof(GoldenManeRapierComponent),
				typeof(ForbiddenSwordComponent),
				typeof(ChimerasFangComponent),
				typeof(UnknownSeedComponent),
				typeof(DoomsdayComponent),
				typeof(LeoniasGraceComponent),
				typeof(CretanBullComponent),
				typeof(AttentivenessComponent),
				typeof(SpecterComponent),
				typeof(GraveDiggerPassiveComponent),
				typeof(EssenceRecruitPassiveComponent),
				typeof(ProjectileCountComponent),
				typeof(EmptyPotionComponent),
				typeof(MagicWandComponent),
				typeof(HotTagComponent)
			};
			int length = typeof(AbilityComponent).Namespace.Length;
			names = new string[types.Length];
			for (int i = 0; i < names.Length; i++)
			{
				Type type = types[i];
				if (type == null)
				{
					string text = names[i - 1];
					int num = text.LastIndexOf('/');
					if (num == -1)
					{
						names[i] = string.Empty;
					}
					else
					{
						names[i] = text.Substring(0, num + 1);
					}
				}
				else
				{
					names[i] = type.FullName.Substring(length + 1, type.FullName.Length - length - 1).Replace('.', '/');
				}
			}
		}

		public SubcomponentAttribute()
			: base(allowCustom: true, types, names)
		{
		}
	}

	[Serializable]
	public class Subcomponents : SubcomponentArray<AbilityComponent>
	{
		public void Initialize()
		{
			for (int i = 0; i < base.components.Length; i++)
			{
				base.components[i].Initialize();
			}
		}
	}

	public abstract IAbility ability { get; }

	public abstract void Initialize();

	public abstract IAbilityInstance CreateInstance(Character owner);
}
public abstract class AbilityComponent<T> : AbilityComponent where T : Ability
{
	[SerializeField]
	protected T _ability;

	public override IAbility ability => _ability;

	public T baseAbility => _ability;

	public override void Initialize()
	{
		_ability.Initialize();
	}

	private void OnDestroy()
	{
		_ability = null;
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return _ability.CreateInstance(owner);
	}
}
