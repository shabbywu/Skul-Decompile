using System.Collections;
using Characters.Abilities;
using Characters.Actions;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions;

public sealed class Bone : InscriptionInstance
{
	private class Buff : Ability
	{
		public class Instance : AbilityInstance<Buff>
		{
			private int _count;

			private float _remainBuffTime;

			private bool _attached;

			public override Sprite icon
			{
				get
				{
					if (_count != 0)
					{
						return ability.defaultIcon;
					}
					return null;
				}
			}

			public override int iconStacks => _count;

			public override float iconFillAmount => (_count != ability.cycle - 1) ? 1 : 0;

			public Instance(Character owner, Buff ability)
				: base(owner, ability)
			{
			}

			protected override void OnAttach()
			{
				owner.onStartAction += OnStartAction;
			}

			protected override void OnDetach()
			{
				owner.onStartAction -= OnStartAction;
				owner.evasion.Detach(this);
			}

			public override void UpdateTime(float deltaTime)
			{
				_remainBuffTime -= deltaTime;
				if (_attached && _remainBuffTime < 0f)
				{
					_attached = false;
					DetachEvasion();
				}
			}

			private void OnStartAction(Action action)
			{
				if (action.type == Action.Type.Swap)
				{
					_count++;
					if (_count >= ability.cycle)
					{
						_count -= ability.cycle;
						Apply();
					}
				}
			}

			private void AttachEvasion()
			{
				owner.evasion.Attach(this);
			}

			private void DetachEvasion()
			{
				owner.evasion.Detach(this);
			}

			private void Apply()
			{
				_attached = true;
				_remainBuffTime = ability.buffDuration;
				((MonoBehaviour)owner).StartCoroutine(CApply());
				AttachEvasion();
				((MonoBehaviour)owner).StartCoroutine(ability.onAttach.CRun(owner));
			}

			private IEnumerator CApply()
			{
				yield return null;
				owner.playerComponents.inventory.weapon.ResetSwapCooldown();
			}
		}

		public OperationInfo.Subcomponents onAttach { get; set; }

		public int cycle { get; set; }

		public float buffDuration { get; set; }

		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new Instance(owner, this);
		}
	}

	[SerializeField]
	[Header("2세트 효과")]
	private int _cycle;

	[SerializeField]
	private Sprite _icon;

	[SerializeField]
	private float _duration;

	[Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	private OperationInfo.Subcomponents _onAttach;

	private Buff _buff;

	protected override void Initialize()
	{
		_onAttach.Initialize();
		_buff = new Buff
		{
			defaultIcon = _icon,
			buffDuration = _duration,
			cycle = _cycle,
			onAttach = _onAttach
		};
		_buff.Initialize();
	}

	public override void Attach()
	{
	}

	public override void Detach()
	{
		base.character.ability.Remove(_buff);
	}

	public override void UpdateBonus(bool wasActive, bool wasOmen)
	{
		if (keyword.isMaxStep)
		{
			base.character.ability.Add(_buff);
		}
		else
		{
			base.character.ability.Remove(_buff);
		}
	}
}
