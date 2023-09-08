using System.Collections.Generic;
using Characters.Movements;
using FX;
using FX.SpriteEffects;
using GameResources;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Statuses;

public class Discharge : IAbility, IAbilityInstance
{
	private class Info
	{
		public readonly Character attacker;

		public readonly double damagePerTick;

		public int remainTicks;

		public double remainDamage => damagePerTick * (double)remainTicks;

		public Info(Character attacker, double damagePerTick, int ticks)
		{
			this.attacker = attacker;
			this.damagePerTick = damagePerTick;
			remainTicks = ticks;
		}
	}

	private static readonly ColorBlend _colorBlend = new ColorBlend(100, new Color(0.1254902f, 1f, 0.1254902f, 1f), 0f);

	private const float _tickInterval = 0.5f;

	private const int _maxStack = 3;

	private const string _floatingTextKey = "floating/status/poision";

	private const string _floatingTextColor = "#2cbb00";

	private EffectInfo _effect;

	private ParticleEffectInfo _hitParticle;

	private readonly List<Info> _infos = new List<Info>();

	private int _stack;

	private Character _attacker;

	private List<Target> _targets;

	private readonly TargetLayer _targetLayer;

	public Character owner { get; private set; }

	public IAbility ability => this;

	public float remainTime { get; set; }

	public bool attached => true;

	public Sprite icon => null;

	public float iconFillAmount => remainTime / duration;

	public bool iconFillInversed => false;

	public bool iconFillFlipped => false;

	public int iconStacks => 0;

	public bool expired => remainTime <= 0f;

	public float duration { get; set; }

	public int iconPriority => 0;

	public bool removeOnSwapWeapon => false;

	public IAbilityInstance CreateInstance(Character owner)
	{
		return this;
	}

	public Discharge(Character owner)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		this.owner = owner;
		_hitParticle = CommonResource.instance.hitParticle;
		_targets = new List<Target>(128);
		_targetLayer = new TargetLayer(LayerMask.op_Implicit(0), allyBody: true, foeBody: false, allyProjectile: false, foeProjectile: false);
	}

	public void UpdateTime(float deltaTime)
	{
	}

	private void GiveDamage(Character attacker, double amount)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Expected O, but got Unknown
		//IL_0247: Expected O, but got Unknown
		if ((Object)(object)attacker == (Object)null || (Object)(object)owner == (Object)null)
		{
			return;
		}
		LayerMask val = _targetLayer.Evaluate(((Component)owner).gameObject);
		if (owner.type == Character.Type.Player)
		{
			val = LayerMask.op_Implicit(LayerMask.op_Implicit(val) | 0x400);
		}
		TargetFinder.FindTargetInRange(Vector2.op_Implicit(((Component)owner).transform.position), 3f, val, _targets);
		foreach (Target target in _targets)
		{
			Damage damage = new Damage(_attacker, amount * 1.0, MMMaths.RandomPointWithinBounds(target.collider.bounds), Damage.Attribute.Magic, Damage.AttackType.Additional, Damage.MotionType.Status, 1.0, 0.5f, 0.0, 1.0, 1.0, canCritical: true, @null: false, 0.0, 0.0, 0);
			_attacker.Attack(target, ref damage);
			_hitParticle.Emit(Vector2.op_Implicit(((Component)target).transform.position), target.collider.bounds, Vector2.zero);
			int num = ((!(((Component)owner).transform.position.x > ((Component)target).transform.position.x)) ? 180 : 0);
			PushForce force = new PushForce
			{
				power = new CustomFloat(3f, 4f),
				angle = new CustomFloat(num)
			};
			PushForce force2 = new PushForce
			{
				power = new CustomFloat(2f),
				angle = new CustomFloat(90f)
			};
			AnimationCurve val2 = new AnimationCurve((Keyframe[])(object)new Keyframe[4]
			{
				new Keyframe(0f, 0f),
				new Keyframe(0.25f, 0.55f),
				new Keyframe(0.5f, 0.8f),
				new Keyframe(1f, 1f)
			});
			Curve curve = new Curve(val2, 1f, 0.8f);
			Curve curve2 = new Curve(val2, 1f, 0.44f);
			PushInfo info = new PushInfo(force, curve, force2, curve2);
			if ((Object)(object)target.character != (Object)null)
			{
				target.character.movement.push.ApplyKnockback(owner, info);
			}
		}
	}

	public void Add(Character attacker, float duration, double damagePerSecond)
	{
		_attacker = attacker;
	}

	public void Refresh()
	{
		if (++_stack >= 3)
		{
			remainTime = 0f;
			GiveDamage(_attacker, ((Component)_attacker).GetComponent<IAttackDamage>().amount);
		}
		SpawnFloatingText($"방전x{_stack}");
	}

	public void Attach()
	{
		remainTime = 2.1474836E+09f;
		_stack = 1;
		owner.spriteEffectStack.Add(_colorBlend);
		SpawnFloatingText($"방전x{_stack}");
	}

	public void Detach()
	{
		owner.spriteEffectStack.Remove(_colorBlend);
	}

	public void Initialize()
	{
		_effect = new EffectInfo(CommonResource.instance.poisonEffect);
	}

	private void SpawnFloatingText(string text)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = MMMaths.RandomPointWithinBounds(((Collider2D)owner.collider).bounds);
		Singleton<Service>.Instance.floatingTextSpawner.SpawnStatus(text, Vector2.op_Implicit(val), "#2cbb00");
	}
}
