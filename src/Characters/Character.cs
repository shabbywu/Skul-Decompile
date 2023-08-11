using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Actions;
using Characters.Gear.Weapons;
using Characters.Marks;
using Characters.Movements;
using Characters.Player;
using FX;
using Level;
using Services;
using Singletons;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace Characters;

[RequireComponent(typeof(CharacterAnimationController))]
[DisallowMultipleComponent]
public class Character : MonoBehaviour
{
	public enum Type
	{
		TrashMob,
		Named,
		Adventurer,
		Boss,
		Summoned,
		Trap,
		Player,
		Dummy,
		PlayerMinion
	}

	public enum LookingDirection
	{
		Right,
		Left
	}

	public enum SizeForEffect
	{
		Small,
		Medium,
		Large,
		ExtraLarge,
		None
	}

	public delegate void OnGaveStatusDelegate(Character target, CharacterStatus.ApplyInfo applyInfo, bool result);

	public delegate void OnKilledDelegate(ITarget target, ref Damage damage);

	public delegate void OnStartMotionDelegate(Motion motion, float runSpeed);

	private const float sizeLerpSpeed = 10f;

	public readonly GiveDamageEvent onGiveDamage = new GiveDamageEvent();

	public GaveDamageDelegate onGaveDamage;

	public OnGaveStatusDelegate onGaveStatus;

	public OnKilledDelegate onKilled;

	public Action<Characters.Actions.Action> onStartCharging;

	public Action<Characters.Actions.Action> onStopCharging;

	public Action<Characters.Actions.Action> onCancelCharging;

	public readonly TrueOnlyLogicalSumList cinematic = new TrueOnlyLogicalSumList(false);

	public readonly TrueOnlyLogicalSumList invulnerable = new TrueOnlyLogicalSumList(false);

	public readonly TrueOnlyLogicalSumList evasion = new TrueOnlyLogicalSumList(false);

	public readonly TrueOnlyLogicalSumList blockLook = new TrueOnlyLogicalSumList(false);

	public readonly TrueOnlyLogicalSumList stealth = new TrueOnlyLogicalSumList(false);

	public readonly Stat stat;

	public readonly CharacterChronometer chronometer = new CharacterChronometer();

	[SerializeField]
	private Key _key;

	[GetComponent]
	[SerializeField]
	protected CharacterHealth _health;

	[SerializeField]
	[GetComponent]
	protected CharacterHit _hit;

	[SerializeField]
	protected BoxCollider2D _collider;

	[GetComponent]
	[SerializeField]
	protected Movement _movement;

	[SerializeField]
	[GetComponent]
	private CharacterAnimationController _animationController;

	[GetComponent]
	[SerializeField]
	private CharacterStatus _status;

	[SerializeField]
	private Type _type;

	[SerializeField]
	private SizeForEffect _sizeForEffect;

	[SerializeField]
	private SortingGroup _sortingGroup;

	[SerializeField]
	protected Stat.Values _baseStat = new Stat.Values(new Stat.Value(Stat.Category.Constant, Stat.Kind.Health, 0.0), new Stat.Value(Stat.Category.Constant, Stat.Kind.MovementSpeed, 0.0));

	[SerializeField]
	protected Transform _base;

	private LookingDirection _lookingDirection;

	[SerializeField]
	protected Weapon _weapon;

	[SerializeField]
	private GameObject _attach;

	private CoroutineReference _cWaitForEndOfAction;

	public Key key => _key;

	public CharacterHealth health => _health;

	public CharacterHit hit => _hit;

	public BoxCollider2D collider => _collider;

	public Movement movement => _movement;

	public CharacterAnimationController animationController => _animationController;

	public CharacterStatus status => _status;

	public bool stunedOrFreezed
	{
		get
		{
			if ((Object)(object)_status != (Object)null)
			{
				if (!_status.stuned && !_status.freezed)
				{
					return _status.unmovable;
				}
				return true;
			}
			return false;
		}
	}

	public Type type
	{
		get
		{
			return _type;
		}
		set
		{
			_type = value;
		}
	}

	public SizeForEffect sizeForEffect => _sizeForEffect;

	public SortingGroup sortingGroup => _sortingGroup;

	public Transform @base => _base;

	public LookingDirection lookingDirection
	{
		get
		{
			return _lookingDirection;
		}
		set
		{
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			desiringLookingDirection = value;
			if (!blockLook.value)
			{
				_lookingDirection = value;
				if (_lookingDirection == LookingDirection.Right)
				{
					_animationController.parameter.flipX = false;
					attachWithFlip.transform.localScale = Vector3.one;
				}
				else
				{
					_animationController.parameter.flipX = true;
					attachWithFlip.transform.localScale = new Vector3(-1f, 1f, 1f);
				}
			}
		}
	}

	public PlayerComponents playerComponents { get; private set; }

	public LookingDirection desiringLookingDirection { get; private set; }

	public List<Characters.Actions.Action> actions { get; private set; } = new List<Characters.Actions.Action>();


	public ISpriteEffectStack spriteEffectStack { get; private set; }

	public Mark mark { get; private set; }

	public CharacterAbilityManager ability { get; private set; }

	public Silence silence { get; private set; }

	public Motion motion { get; private set; }

	public Motion runningMotion
	{
		get
		{
			if ((Object)(object)motion == (Object)null || !motion.running)
			{
				return null;
			}
			return motion;
		}
	}

	public GameObject attach
	{
		get
		{
			return _attach;
		}
		set
		{
			_attach = value;
		}
	}

	public GameObject attachWithFlip { get; private set; }

	public bool liveAndActive
	{
		get
		{
			if ((Object)(object)health != (Object)null)
			{
				if (!health.dead)
				{
					return ((Component)this).gameObject.activeSelf;
				}
				return false;
			}
			return ((Component)this).gameObject.activeSelf;
		}
	}

	public event System.Action onDie;

	public event EvadeDamageDelegate onEvade;

	public event Action<Characters.Actions.Action> onStartAction;

	public event Action<Characters.Actions.Action> onEndAction;

	public event Action<Characters.Actions.Action> onCancelAction;

	public event OnStartMotionDelegate onStartMotion;

	private Character()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		stat = new Stat(this);
	}

	protected virtual void Awake()
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		if ((Object)(object)hit == (Object)null)
		{
			cinematic.Attach((object)this);
		}
		if ((Object)(object)_attach == (Object)null)
		{
			_attach = new GameObject("_attach");
			_attach.transform.SetParent(((Component)this).transform, false);
		}
		if ((Object)(object)attachWithFlip == (Object)null)
		{
			attachWithFlip = new GameObject("attachWithFlip");
			attachWithFlip.transform.SetParent(attach.transform, false);
		}
		spriteEffectStack = ((Component)this).GetComponent<ISpriteEffectStack>();
		if ((Object)(object)health != (Object)null)
		{
			mark = Mark.AddComponent(this);
		}
		ability = CharacterAbilityManager.AddComponent(this);
		stat.AttachValues(_baseStat);
		stat.Update();
		InitializeActions();
		_animationController.Initialize();
		_animationController.onExpire += OnAnimationExpire;
		if ((Object)(object)_health != (Object)null)
		{
			_health.owner = this;
			_health.SetMaximumHealth(stat.Get(Stat.Category.Final, Stat.Kind.Health));
			_health.ResetToMaximumHealth();
			_health.onDie += OnDie;
			_health.onTakeDamage.Add(0, (TakeDamageDelegate)delegate(ref Damage damage)
			{
				return stat.ApplyDefense(ref damage);
			});
			_health.onTakeDamage.Add(int.MaxValue, (TakeDamageDelegate)CancelDamage);
		}
		if (type == Type.Player)
		{
			playerComponents = new PlayerComponents(this);
			playerComponents.Initialize();
		}
		silence = new Silence(this);
		bool CancelDamage(ref Damage damage)
		{
			if (cinematic.value)
			{
				return true;
			}
			if (damage.priority > Damage.evasionPriority)
			{
				return false;
			}
			if (evasion.value)
			{
				this.onEvade?.Invoke(ref damage);
				return true;
			}
			if (damage.priority > Damage.invulnerablePriority)
			{
				return false;
			}
			if (invulnerable.value)
			{
				damage.@null = true;
			}
			return false;
		}
	}

	private void OnDestroy()
	{
		if (type == Type.Player)
		{
			playerComponents.Dispose();
		}
	}

	protected virtual void Update()
	{
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		float deltaTime = ((ChronometerBase)chronometer.master).deltaTime;
		stat.TakeTime(deltaTime);
		playerComponents?.Update(deltaTime);
		if ((Object)(object)_health == (Object)null)
		{
			stat.UpdateIfNecessary();
		}
		else if (stat.UpdateIfNecessary())
		{
			double num = math.ceil(stat.Get(Stat.Category.Final, Stat.Kind.Health));
			double current = math.ceil(_health.percent * num);
			_health.SetHealth(current, num);
		}
		double final = stat.GetFinal(Stat.Kind.CharacterSize);
		Vector3 localScale = ((Component)this).transform.localScale;
		((Component)this).transform.localScale = Vector3.one * Mathf.Lerp(localScale.x, (float)final, Time.deltaTime * 10f);
	}

	protected void OnDie()
	{
		this.onDie?.Invoke();
	}

	public void Attack(ITarget target, ref Damage damage)
	{
		if ((Object)(object)target.character != (Object)null)
		{
			TryAttackCharacter(target, ref damage);
		}
		else if ((Object)(object)target.damageable != (Object)null)
		{
			AttackDamageable(target, ref damage);
		}
	}

	public void Attack(Character character, ref Damage damage)
	{
		TryAttackCharacter(new TargetStruct(character), ref damage);
	}

	public void Attack(DestructibleObject damageable, ref Damage damage)
	{
		AttackDamageable(new TargetStruct(damageable), ref damage);
	}

	public bool TryAttackCharacter(ITarget target, ref Damage damage)
	{
		Character character = target.character;
		if (character.health.dead)
		{
			return false;
		}
		Damage originalDamage = damage;
		if (onGiveDamage.Invoke(target, ref damage))
		{
			return false;
		}
		character.hit.Stop(damage.stoppingPower);
		if (character.health.TakeDamage(ref damage, out var dealtDamage))
		{
			return false;
		}
		if (character.type == Type.Player)
		{
			Singleton<Service>.Instance.floatingTextSpawner.SpawnPlayerTakingDamage(in damage);
		}
		else
		{
			Singleton<Service>.Instance.floatingTextSpawner.SpawnTakingDamage(in damage);
		}
		onGaveDamage?.Invoke(target, in originalDamage, in damage, dealtDamage);
		if (target.character.health.dead)
		{
			onKilled?.Invoke(target, ref damage);
		}
		return true;
	}

	public void AttackDamageable(ITarget target, ref Damage damage)
	{
		DestructibleObject damageable = target.damageable;
		Damage originalDamage = damage;
		GiveDamageEvent giveDamageEvent = onGiveDamage;
		if (giveDamageEvent == null || giveDamageEvent.Invoke(target, ref damage))
		{
			damageable.Hit(this, ref damage);
			if (damage.amount > 0.0)
			{
				Singleton<Service>.Instance.floatingTextSpawner.SpawnTakingDamage(in damage);
			}
			onGaveDamage?.Invoke(target, in originalDamage, in damage, 0.0);
			if (target.damageable.destroyed)
			{
				onKilled?.Invoke(target, ref damage);
			}
		}
	}

	public void TryKillTarget(ITarget target, ref Damage damage)
	{
		if (!((Object)(object)target.character == (Object)null))
		{
			target.character.health.TryKill();
			onKilled?.Invoke(target, ref damage);
		}
	}

	public bool GiveStatus(Character target, CharacterStatus.ApplyInfo status)
	{
		if ((Object)(object)target.status == (Object)null)
		{
			onGaveStatus?.Invoke(target, status, result: false);
			return false;
		}
		bool result = target.status.Apply(this, status);
		onGaveStatus?.Invoke(target, status, result);
		return result;
	}

	private IEnumerator CWaitForEndOfAction(Characters.Actions.Action action)
	{
		yield return motion.action.CWaitForEndOfRunning();
		this.onEndAction?.Invoke(action);
	}

	public void DoAction(Motion motion, float speedMultiplier)
	{
		DoAction(motion, speedMultiplier, triggerOnStartAction: true);
	}

	public void DoAction(Motion motion, float speedMultiplier, bool triggerOnStartAction)
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		Motion motion2 = this.motion;
		if ((Object)(object)motion2 != (Object)null && motion2.running)
		{
			CancelAction();
		}
		DoMotion(motion, speedMultiplier);
		if ((Object)(object)motion.action != (Object)null)
		{
			((CoroutineReference)(ref _cWaitForEndOfAction)).Stop();
			if (((Behaviour)this).isActiveAndEnabled)
			{
				_cWaitForEndOfAction = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)this, CWaitForEndOfAction(motion.action));
			}
			else
			{
				Debug.LogWarning((object)"Coroutine couldn't be started because the character is not active or disabled, so use onEnd event");
				motion.action.onEnd += onActionEnd;
			}
			if (triggerOnStartAction)
			{
				this.onStartAction?.Invoke(motion.action);
			}
		}
		void onActionEnd()
		{
			motion.action.onEnd -= onActionEnd;
			this.onEndAction?.Invoke(motion.action);
		}
	}

	public void TriggerOnStartActionManually(Characters.Actions.Action action)
	{
		this.onStartAction?.Invoke(action);
	}

	public void DoActionNonBlock(Motion motion)
	{
		this.onStartAction?.Invoke(motion.action);
		motion.StartBehaviour(1f);
		motion.EndBehaviour();
		this.onEndAction?.Invoke(motion.action);
	}

	public void DoMotion(Motion motion, float speedMultiplier = 1f)
	{
		Motion motion2 = this.motion;
		if ((Object)(object)motion2 != (Object)null && motion2.running)
		{
			if ((Object)(object)motion2.action != (Object)(object)motion.action)
			{
				CancelAction();
			}
			else
			{
				_animationController.StopAll();
				Movement obj = movement;
				if (obj != null)
				{
					obj.blocked.Detach((object)this);
				}
				blockLook.Detach((object)this);
				motion2.CancelBehaviour();
			}
		}
		this.motion = motion;
		float num = motion.speed * speedMultiplier;
		if (motion.stay)
		{
			_animationController.Play(motion.animationInfo, num);
		}
		else
		{
			_animationController.Play(motion.animationInfo, motion.length / num, num);
		}
		blockLook.Detach((object)this);
		lookingDirection = desiringLookingDirection;
		motion.StartBehaviour(num);
		this.onStartMotion?.Invoke(motion, num);
		if ((Object)(object)movement != (Object)null)
		{
			movement.blocked.Detach((object)this);
			if (motion.blockMovement && motion.length > 0f)
			{
				movement.blocked.Attach((object)this);
			}
		}
		if (motion.blockLook)
		{
			blockLook.Attach((object)this);
		}
	}

	public void InitializeActions()
	{
		((Component)this).GetComponentsInChildren<Characters.Actions.Action>(true, actions);
		actions.Sort((Characters.Actions.Action a, Characters.Actions.Action b) => a.priority.CompareTo(b.priority));
		foreach (Characters.Actions.Action action in actions)
		{
			action.Initialize(this);
		}
	}

	public void CancelAction()
	{
		if (!((Object)(object)motion == (Object)null))
		{
			_animationController.StopAll();
			((CoroutineReference)(ref _cWaitForEndOfAction)).Stop();
			if (this.onCancelAction != null && (Object)(object)motion.action != (Object)null)
			{
				this.onCancelAction(motion.action);
			}
			Movement obj = movement;
			if (obj != null)
			{
				obj.blocked.Detach((object)this);
			}
			blockLook.Detach((object)this);
			motion.CancelBehaviour();
		}
	}

	public LookingDirection DesireToLookAt(float targetX)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		if (!(((Component)this).transform.position.x > targetX))
		{
			return desiringLookingDirection = LookingDirection.Right;
		}
		return desiringLookingDirection = LookingDirection.Left;
	}

	public LookingDirection ForceToLookAt(float targetX)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		if (!(((Component)this).transform.position.x > targetX))
		{
			return this.lookingDirection = LookingDirection.Right;
		}
		return this.lookingDirection = LookingDirection.Left;
	}

	public void ForceToLookAt(LookingDirection lookingDirection)
	{
		desiringLookingDirection = lookingDirection;
		_lookingDirection = lookingDirection;
		_animationController.parameter.flipX = _lookingDirection != LookingDirection.Right;
	}

	private void OnAnimationExpire()
	{
		Movement obj = movement;
		if (obj != null)
		{
			obj.blocked.Detach((object)this);
		}
		blockLook.Detach((object)this);
		motion?.EndBehaviour();
	}
}
