using System;
using System.Collections.Generic;
using Characters.Operations.Attack;
using Characters.Operations.BehaviorDesigner;
using Characters.Operations.Customs;
using Characters.Operations.Customs.GlacialSkull;
using Characters.Operations.Customs.GrimReaper;
using Characters.Operations.Decorator;
using Characters.Operations.Decorator.Deprecated;
using Characters.Operations.Essences;
using Characters.Operations.Fx;
using Characters.Operations.Gauge;
using Characters.Operations.Health;
using Characters.Operations.Movement;
using Characters.Operations.ObjectTransform;
using Characters.Operations.Summon;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations;

public abstract class TargetedCharacterOperation : MonoBehaviour
{
	public class SubcomponentAttribute : SubcomponentAttribute
	{
		public SubcomponentAttribute()
			: base(types, names)
		{
		}
	}

	[Serializable]
	public class Subcomponents : SubcomponentArray<TargetedCharacterOperation>
	{
		public void Initialize()
		{
			for (int i = 0; i < base.components.Length; i++)
			{
				base.components[i].Initialize();
			}
		}

		public void Run(Character owner, Character target)
		{
			for (int i = 0; i < base.components.Length; i++)
			{
				base.components[i].Run(owner, target);
			}
		}
	}

	public static readonly Type[] types;

	public static readonly string[] names;

	[NonSerialized]
	public float runSpeed = 1f;

	static TargetedCharacterOperation()
	{
		types = new Type[124]
		{
			typeof(InstantAttack),
			typeof(InstantAttack2),
			null,
			typeof(SweepAttack),
			typeof(SweepAttack2),
			null,
			typeof(CastAttack),
			null,
			typeof(FireProjectile),
			typeof(FireProjectileInBounds),
			typeof(FireBouncyProjectile),
			typeof(MultipleFireProjectile),
			typeof(BreakPlayerShield),
			typeof(CameraShake),
			typeof(CameraShakeCurve),
			typeof(CameraZoom),
			null,
			typeof(PlayMusic),
			typeof(PauseBackgroundMusic),
			typeof(StopMusic),
			typeof(PlaySound),
			null,
			typeof(MotionTrail),
			typeof(ScreenFlash),
			typeof(ShaderEffect),
			typeof(SpawnEffect),
			typeof(SpawnEffectOnScreen),
			null,
			typeof(Vignette),
			typeof(Vibration),
			typeof(SpawnLineText),
			typeof(SpawnEnemyLineText),
			typeof(Repeater),
			typeof(Repeater2),
			typeof(Chance),
			typeof(Characters.Operations.Decorator.Random),
			typeof(SeedRandom),
			typeof(HealthFilter),
			typeof(CharacterTypeFilter),
			typeof(Sequencer),
			typeof(InHardmode),
			typeof(RandomlyRunningOperation),
			typeof(SummonCharacter),
			typeof(SummonMinion),
			typeof(SummonOperationRunner),
			typeof(SummonMultipleOperationRunners),
			typeof(SummonOperationRunnerAtTarget),
			typeof(ApplyStatusFromEssenceOwner),
			typeof(Move),
			typeof(DualMove),
			null,
			typeof(Knockback),
			typeof(KnockbackTo),
			typeof(Smash),
			typeof(SmashTo),
			null,
			typeof(GrabTo),
			null,
			typeof(ChangeGravity),
			typeof(ModifyVerticalVelocity),
			typeof(OverrideMovementConfig),
			typeof(SetCharacterColliderLayerMask),
			typeof(Jump),
			typeof(JumpDown),
			typeof(Teleport),
			typeof(TeleportOverTime),
			typeof(MoveTransform),
			typeof(MoveTransformFromPosition),
			typeof(ResetGlobalTransformToLocal),
			typeof(RotateAngle),
			typeof(SetPositionTo),
			typeof(Heal),
			typeof(Invulnerable),
			typeof(Suicide),
			typeof(Kill),
			typeof(AddGaugeValue),
			typeof(SetGaugeValue),
			typeof(ClearGaugeValue),
			typeof(SetBDVariableToTarget),
			typeof(ModifyTimeScale),
			typeof(ApplyStatus),
			typeof(AddMarkStack),
			null,
			typeof(AttachCurseOfLight),
			typeof(AttachAbility),
			typeof(AttachAbilityWithinCollider),
			null,
			typeof(Polymorph),
			typeof(ReduceCooldownTime),
			typeof(SetRemainCooldownTime),
			null,
			typeof(ActivateGameObjectOperation),
			typeof(DeactivateGameObject),
			null,
			typeof(SpawnCharacter),
			typeof(SpawnRandomCharacter),
			typeof(DestoryCharacter),
			typeof(SetCharacterVisible),
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
			typeof(SpawnGoldAtTarget),
			typeof(ConsumeGold),
			null,
			typeof(InvokeUnityEvent),
			typeof(LerpCollider),
			null,
			typeof(SetAirJumpCount),
			null,
			typeof(StopAnotherOperation),
			typeof(PrintDebugLog),
			typeof(AttachSilence),
			typeof(SpawnThiefGoldAtTarget),
			typeof(SpawnPropAtTarget),
			typeof(SlimeMagic),
			typeof(GrimReaperSentence3),
			typeof(AddFreezeRemainHitStack)
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

	public virtual void Initialize()
	{
	}

	public abstract void Run(Character owner, Character target);

	public virtual void Run(Character owner, IList<Character> targets)
	{
		for (int i = 0; i < targets.Count; i++)
		{
			Run(owner, targets[i]);
		}
	}

	public override string ToString()
	{
		return ExtensionMethods.GetAutoName((object)this);
	}
}
