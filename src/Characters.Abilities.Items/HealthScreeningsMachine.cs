using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Abilities.Items;

[Serializable]
public sealed class HealthScreeningsMachine : Ability
{
	public class Instance : AbilityInstance<HealthScreeningsMachine>
	{
		private Stat.Values _stat;

		public override Sprite icon
		{
			get
			{
				if (ability._stack <= 0)
				{
					return null;
				}
				return base.icon;
			}
		}

		public override int iconStacks => ability._stack;

		public Instance(Character owner, HealthScreeningsMachine ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			_stat = ability._statPerStack.Clone();
			owner.stat.AttachValues(_stat);
			((MonoBehaviour)owner).StartCoroutine(CCheckWithinRange());
		}

		protected override void OnDetach()
		{
			owner.stat.DetachValues(_stat);
		}

		public override void Refresh()
		{
			base.Refresh();
			ability._stack++;
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
		}

		public void UpdateStack()
		{
			for (int i = 0; i < ((ReorderableArray<Stat.Value>)_stat).values.Length; i++)
			{
				((ReorderableArray<Stat.Value>)_stat).values[i].value = ((ReorderableArray<Stat.Value>)ability._statPerStack).values[i].GetStackedValue(ability._stack);
			}
			owner.stat.SetNeedUpdate();
		}

		private IEnumerator CCheckWithinRange()
		{
			UsingCollider val = default(UsingCollider);
			while (base.attached)
			{
				((UsingCollider)(ref val))._002Ector(ability._range, true);
				try
				{
					ability._overlapper.OverlapCollider(ability._range);
				}
				finally
				{
					((IDisposable)(UsingCollider)(ref val)).Dispose();
				}
				IEnumerable<Collider2D> source = ((IEnumerable<Collider2D>)ability._overlapper.results).Where(delegate(Collider2D result)
				{
					Character component = ((Component)result).GetComponent<Character>();
					if ((Object)(object)component == (Object)null)
					{
						return false;
					}
					return ((EnumArray<Character.Type, bool>)ability._characterTypes)[component.type] && component.status.hasAny;
				});
				ability._stack = Mathf.Min(ability._maxStack, source.Count());
				UpdateStack();
				yield return Chronometer.global.WaitForSeconds(ability._checkInterval);
			}
		}
	}

	[SerializeField]
	private float _checkInterval = 0.33f;

	[SerializeField]
	private Collider2D _range;

	[SerializeField]
	private int _maxStack;

	[SerializeField]
	private Stat.Values _statPerStack;

	[SerializeField]
	private CharacterStatusKindBoolArray _statusKinds;

	[SerializeField]
	private CharacterTypeBoolArray _characterTypes;

	private int _stack;

	private NonAllocOverlapper _overlapper;

	public override void Initialize()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		base.Initialize();
		_overlapper = new NonAllocOverlapper(128);
		((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(LayerMask.op_Implicit(1024));
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
