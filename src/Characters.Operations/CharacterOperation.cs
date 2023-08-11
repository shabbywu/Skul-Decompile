using System;
using Characters.Operations.Animation;
using Characters.Operations.Attack;
using Characters.Operations.BehaviorDesigner;
using Characters.Operations.Customs;
using Characters.Operations.Customs.AquaSkull;
using Characters.Operations.Customs.BombSkul;
using Characters.Operations.Customs.DavyJones;
using Characters.Operations.Customs.EntSkul;
using Characters.Operations.Customs.GraveDigger;
using Characters.Operations.Customs.Minotaurus;
using Characters.Operations.Customs.Skeleton_Mage;
using Characters.Operations.Customs.Skeleton_Sword;
using Characters.Operations.Decorator;
using Characters.Operations.Decorator.Deprecated;
using Characters.Operations.Essences;
using Characters.Operations.Fx;
using Characters.Operations.Gauge;
using Characters.Operations.GrabBorad;
using Characters.Operations.Health;
using Characters.Operations.Items;
using Characters.Operations.Movement;
using Characters.Operations.ObjectTransform;
using Characters.Operations.Summon;
using UnityEditor;

namespace Characters.Operations;

public abstract class CharacterOperation : TargetedCharacterOperation
{
	public new class SubcomponentAttribute : SubcomponentAttribute
	{
		public SubcomponentAttribute()
			: base(true, types, names)
		{
		}
	}

	[Serializable]
	public new class Subcomponents : SubcomponentArray<CharacterOperation>
	{
		public void Initialize()
		{
			for (int i = 0; i < base.components.Length; i++)
			{
				base.components[i].Initialize();
			}
		}

		public void Run(Character owner)
		{
			for (int i = 0; i < base.components.Length; i++)
			{
				base.components[i].Run(owner);
			}
		}

		public void Run(Character owner, Character target)
		{
			Run(target);
		}

		public void Stop()
		{
			for (int i = 0; i < base.components.Length; i++)
			{
				base.components[i].Stop();
			}
		}
	}

	public new static readonly Type[] types;

	public new static readonly string[] names;

	static CharacterOperation()
	{
		types = new Type[231]
		{
			typeof(InstantAttack),
			typeof(InstantAttack2),
			typeof(InstantAttackByCount),
			null,
			typeof(SweepAttack),
			typeof(SweepAttack2),
			null,
			typeof(RingAttack),
			typeof(TeleportAttack),
			null,
			typeof(CastAttack),
			null,
			typeof(GrabbedTargetAttack),
			null,
			typeof(PlayerAttack),
			typeof(BreakPlayerShield),
			null,
			typeof(FireProjectile),
			typeof(FireProjectileInBounds),
			typeof(FireBouncyProjectile),
			typeof(MultipleFireProjectile),
			typeof(GlobalAttack),
			typeof(WeaponMaster),
			typeof(CameraShake),
			typeof(CameraShakeCurve),
			typeof(CameraZoom),
			null,
			typeof(PlayMusic),
			typeof(PlayChapterMusic),
			typeof(PauseBackgroundMusic),
			typeof(StopMusic),
			typeof(PlaySound),
			typeof(SetInternalMusicVolume),
			null,
			typeof(MotionTrail),
			typeof(ScreenFlash),
			typeof(ShaderEffect),
			typeof(SpawnEffect),
			typeof(SpawnEffectOnScreen),
			typeof(SetOwnerColor),
			null,
			typeof(Vignette),
			typeof(Vibration),
			typeof(SpawnLineText),
			typeof(SpawnEnemyLineText),
			typeof(DropParts),
			typeof(ByAbility),
			typeof(Repeater),
			typeof(Repeater2),
			typeof(Repeater3),
			typeof(Chance),
			typeof(Characters.Operations.Decorator.Random),
			typeof(SeedRandom),
			typeof(WeightedRandom),
			typeof(ByLookingDirection),
			typeof(OneByOne),
			typeof(CharacterTypeFilter),
			typeof(HealthFilter),
			typeof(InHardmode),
			typeof(ByPositionX),
			typeof(ByCharacterSize),
			typeof(Sequencer),
			typeof(RandomlyRunningOperation),
			typeof(SummonCharacter),
			typeof(SummonMinion),
			typeof(SummonMinionToTarget),
			typeof(SummonMonster),
			typeof(SummonOperationRunner),
			typeof(SummonMultipleOperationRunners),
			typeof(SummonOperationRunnersOnGround),
			typeof(SummonOperationRunnersOnGroundOneDirection),
			typeof(SummonOperationRunnerAtChildren),
			typeof(SummonOperationRunnersAtTargetWithinRange),
			typeof(SummonOriginBasedOperationRunners),
			typeof(SummonLiquid),
			typeof(SummonBDCharacter),
			typeof(SummonBDRandomCharacter),
			typeof(SummonCharactersInRange),
			typeof(MinionGroupOperations),
			typeof(DespawnMinions),
			typeof(SummonDarkOrb),
			typeof(Move),
			typeof(Characters.Operations.Movement.MoveTo),
			typeof(DualMove),
			typeof(StopMove),
			null,
			typeof(ClearGrabBoard),
			typeof(StartToAddGrabTarget),
			typeof(StartMovingGrabbedTarget),
			null,
			typeof(ChangeGravity),
			typeof(ModifyVerticalVelocity),
			typeof(OverrideMovementConfig),
			typeof(SetCharacterColliderLayerMask),
			typeof(Jump),
			typeof(JumpDown),
			typeof(Teleport),
			typeof(TeleportToCharacter),
			typeof(TeleportOverTime),
			typeof(FlipObject),
			typeof(SetPositionTo),
			typeof(SetRotationTo),
			typeof(SetRotationToTarget),
			typeof(SetScaleToDistance),
			typeof(MoveTransform),
			typeof(MoveTransformFromPosition),
			typeof(ResetGlobalTransformToLocal),
			typeof(RotateAngle),
			typeof(RotateTransform),
			typeof(RotateToTarget),
			typeof(LerpTransform),
			typeof(ScaleByPlatformSize),
			typeof(SetParentToCharacterAttach),
			typeof(ActivateLaser),
			typeof(LerpLaser),
			typeof(SetRotationByDirection),
			typeof(Heal),
			typeof(RangeHeal),
			typeof(LoseHealth),
			typeof(Invulnerable),
			typeof(Lockout),
			typeof(Suicide),
			typeof(Cinematic),
			typeof(Evasion),
			typeof(AddGaugeValue),
			typeof(SetGaugeValue),
			typeof(ClearGaugeValue),
			typeof(Change),
			typeof(Remove),
			typeof(Discard),
			typeof(Equip),
			typeof(Drop),
			typeof(DropByRarity),
			typeof(SummonDwarfTurret),
			typeof(AttachKirizGlobal),
			typeof(Naias),
			typeof(ApplyStatusFromEssenceOwner),
			typeof(SetAnimationClip),
			typeof(Characters.Operations.GrabBorad.AddToGrabBoard),
			typeof(RunOperationOnGrabBoard),
			typeof(OverrideBDVariableInRange),
			typeof(FindEnemy),
			typeof(SetBehaviorTreeVariable),
			typeof(IncreaseSharedIntVariable),
			typeof(ModifyTimeScale),
			typeof(ApplyStatus),
			typeof(AddMarkStack),
			null,
			typeof(AttachAbility),
			typeof(AttachAbilityGlobal),
			typeof(AttachAbilityWithinCollider),
			typeof(AttachAbilityToMinions),
			null,
			typeof(Polymorph),
			typeof(StartWeaponPolymorph),
			typeof(EndWeaponPolymorph),
			null,
			typeof(SwapWeapon),
			typeof(ReduceCooldownTime),
			typeof(SetRemainCooldownTime),
			typeof(ModifyRemainCooldownTime),
			typeof(ModifyEssenceRemainCooldownTime),
			typeof(TriggerActionStart),
			typeof(DoAction),
			null,
			typeof(ActivateGameObjectOperation),
			typeof(DeactivateGameObject),
			null,
			typeof(SpawnProp),
			typeof(SpawnCharacter),
			typeof(SpawnRandomCharacter),
			typeof(DestoryCharacter),
			typeof(SetCharacterVisible),
			typeof(UpdateAnimation),
			null,
			typeof(LookAt),
			typeof(LookTarget),
			typeof(LookTargetOpposition),
			null,
			typeof(TakeAim),
			typeof(TakeAimTowardsTheFront),
			typeof(GiveBuff),
			null,
			typeof(SpawnGold),
			typeof(ConsumeGold),
			null,
			typeof(InvokeUnityEvent),
			typeof(LerpCollider),
			typeof(StopAnotherOperation),
			typeof(PrintDebugLog),
			typeof(Instantiate),
			null,
			typeof(Guard),
			null,
			typeof(TeleportToSkulHead),
			typeof(SpawnThiefGoldAtTarget),
			typeof(ArchlichPassive),
			typeof(AddYakshaStompStack),
			typeof(PrisonerPhaser),
			typeof(Samurai2IlseomInstantAttack),
			typeof(MultidimensionalPrism),
			typeof(DropManatechPart),
			null,
			typeof(AddRockstarPassiveStack),
			typeof(SummonRockstarAmp),
			typeof(FireHighTideProjectile),
			typeof(FireLowTideProjectile),
			typeof(FireWaterspoutProjectile),
			typeof(SetFloodSweepAttackDamageMultiplier),
			typeof(ReduceCooldownTimeByProjectileCount),
			typeof(Explode),
			typeof(AddDamageStack),
			typeof(RiskyUpgrade),
			typeof(SummonSmallBomb),
			typeof(EntSkulPassive),
			typeof(EntSkulThornyVine),
			typeof(SummonEntMinionAtEntSapling),
			typeof(SummonEntSapling),
			typeof(SummonMinionFromGraves),
			typeof(SummonGrave),
			typeof(SummonLandOfTheDead),
			typeof(SpawnCorpseForLandOfTheDead),
			typeof(StartRecordAttacks),
			typeof(Skeleton_SwordInstantAttack),
			typeof(Skeleton_SwordSweepAttack),
			typeof(AddManaChargingSpeedMultiplier),
			typeof(TryReduceMana),
			typeof(FillUpMana),
			typeof(PushCannonBall),
			typeof(PopCannonBall),
			typeof(FireCannonBall)
		};
		int length = typeof(CharacterOperation).Namespace.Length;
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

	public abstract void Run(Character owner);

	public override void Run(Character owner, Character target)
	{
		Run(target);
	}

	public virtual void Stop()
	{
	}

	protected virtual void OnDestroy()
	{
		Stop();
	}
}
