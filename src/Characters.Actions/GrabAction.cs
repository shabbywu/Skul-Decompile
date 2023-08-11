using System.Collections.Generic;
using System.Linq;
using Characters.Movements;
using Characters.Operations.Attack;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Characters.Actions;

[RequireComponent(typeof(GrabBoard))]
public sealed class GrabAction : Action
{
	[SerializeField]
	[GetComponent]
	private GrabBoard _grabBoard;

	[SerializeField]
	private bool _attackHitTrigger;

	[SerializeField]
	private bool _doFailMotion;

	[SerializeField]
	[Subcomponent(typeof(Motion))]
	private Motion.Subcomponents _grabMotions;

	[Subcomponent(typeof(Motion))]
	[SerializeField]
	private Motion.Subcomponents _maintainMotions;

	[SerializeField]
	[Subcomponent(typeof(Motion))]
	private Motion.Subcomponents _grabFailMotions;

	private Character.LookingDirection _lookingDirection;

	private List<IAttack> _attacks;

	private bool _grabbing;

	public override Motion[] motions => ((SubcomponentArray<Motion>)_grabMotions).components.Concat(((SubcomponentArray<Motion>)_maintainMotions).components).Concat(((SubcomponentArray<Motion>)_grabFailMotions).components).ToArray();

	public override bool canUse
	{
		get
		{
			if (base.cooldown.canUse && !_owner.stunedOrFreezed)
			{
				return PassAllConstraints(motions[0]);
			}
			return false;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		bool blockLookBefore = false;
		JoinGrabMotion(_grabMotions);
		JoinMotion(_maintainMotions);
		JoinMotion(_grabFailMotions);
		if (_attackHitTrigger)
		{
			_attacks = new List<IAttack>();
			Motion[] components = ((SubcomponentArray<Motion>)_grabMotions).components;
			for (int i = 0; i < components.Length; i++)
			{
				IAttack componentInChildren = ((Component)components[i]).GetComponentInChildren<IAttack>();
				if (componentInChildren != null)
				{
					_attacks.Add(componentInChildren);
				}
			}
			if (_attacks.Count == 0)
			{
				Debug.LogError((object)("Attack is null " + ((Object)((Component)this).gameObject).name));
				return;
			}
			_attacks.ForEach(delegate(IAttack attack)
			{
				attack.onHit += OnAttackHit;
			});
		}
		_ = ((SubcomponentArray<Motion>)_maintainMotions).components.LongLength;
		void JoinGrabMotion(Motion.Subcomponents subcomponents)
		{
			for (int k = 0; k < ((SubcomponentArray<Motion>)subcomponents).components.Length; k++)
			{
				Motion motion2 = ((SubcomponentArray<Motion>)subcomponents).components[k];
				if (motion2.blockLook)
				{
					if (blockLookBefore)
					{
						motion2.onStart += delegate
						{
							base.owner.lookingDirection = _lookingDirection;
						};
					}
					motion2.onStart += delegate
					{
						_lookingDirection = base.owner.lookingDirection;
					};
				}
				blockLookBefore = motion2.blockLook;
				if (k + 1 < ((SubcomponentArray<Motion>)subcomponents).components.Length)
				{
					int cached2 = k + 1;
					((SubcomponentArray<Motion>)subcomponents).components[k].onEnd += delegate
					{
						DoMotion(((SubcomponentArray<Motion>)subcomponents).components[cached2]);
					};
				}
			}
			((SubcomponentArray<Motion>)subcomponents).components[((SubcomponentArray<Motion>)subcomponents).components.Length - 1].onEnd += delegate
			{
				_grabbing = false;
				if (_grabBoard.targets.Count > 0)
				{
					DoMotion(((SubcomponentArray<Motion>)_maintainMotions).components[0]);
				}
				else if (_doFailMotion && _grabBoard.failTargets.Count > 0 && ((SubcomponentArray<Motion>)_grabFailMotions).components.Length != 0)
				{
					DoMotion(((SubcomponentArray<Motion>)_grabFailMotions).components[0]);
				}
			};
		}
		void JoinMotion(Motion.Subcomponents subcomponents)
		{
			for (int j = 0; j < ((SubcomponentArray<Motion>)subcomponents).components.Length; j++)
			{
				Motion motion = ((SubcomponentArray<Motion>)subcomponents).components[j];
				if (motion.blockLook)
				{
					if (blockLookBefore)
					{
						motion.onStart += delegate
						{
							base.owner.lookingDirection = _lookingDirection;
						};
					}
					motion.onStart += delegate
					{
						_lookingDirection = base.owner.lookingDirection;
					};
				}
				blockLookBefore = motion.blockLook;
				if (j + 1 < ((SubcomponentArray<Motion>)subcomponents).components.Length)
				{
					int cached = j + 1;
					((SubcomponentArray<Motion>)subcomponents).components[j].onEnd += delegate
					{
						DoMotion(((SubcomponentArray<Motion>)subcomponents).components[cached]);
					};
				}
			}
		}
	}

	private void OnAttackHit(Target target, ref Damage damage)
	{
		if (_grabbing)
		{
			_grabbing = false;
			if (target.character.movement.config.type == Movement.Config.Type.Static || target.character.stat.GetFinal(Stat.Kind.KnockbackResistance) == 0.0)
			{
				_grabBoard.AddFailed(target);
				_grabMotions.EndBehaviour();
			}
			else
			{
				_grabBoard.Add(target);
				_grabMotions.EndBehaviour();
			}
		}
	}

	public override void Initialize(Character owner)
	{
		base.Initialize(owner);
		for (int i = 0; i < motions.Length; i++)
		{
			motions[i].Initialize(this);
		}
	}

	public override bool TryStart()
	{
		if (!((Component)this).gameObject.activeSelf || !canUse || !ConsumeCooldownIfNeeded())
		{
			return false;
		}
		_grabbing = true;
		DoAction(((SubcomponentArray<Motion>)_grabMotions).components[0]);
		return true;
	}

	private void OnDestroy()
	{
		if (_attacks != null)
		{
			_attacks.ForEach(delegate(IAttack attack)
			{
				attack.onHit -= OnAttackHit;
			});
		}
	}
}
