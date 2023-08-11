using System;
using Characters.Gear.Weapons.Gauges;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Customs;

[Serializable]
public class YakshaPassive : Ability, IAbilityInstance
{
	[SerializeField]
	private ValueGauge _gauge;

	[SerializeField]
	private int _stacksToAttack;

	[Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	private OperationInfo.Subcomponents _operations;

	private CoroutineReference _operationRunner;

	public Character owner { get; set; }

	public IAbility ability => this;

	public float remainTime
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	public bool attached => true;

	public Sprite icon => _defaultIcon;

	public float iconFillAmount => 0f;

	public int iconStacks { get; protected set; }

	public bool expired => false;

	public override void Initialize()
	{
		base.Initialize();
		_operations.Initialize();
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return this;
	}

	public void UpdateTime(float deltaTime)
	{
	}

	public void Refresh()
	{
	}

	public void Attach()
	{
	}

	public void Detach()
	{
		((CoroutineReference)(ref _operationRunner)).Stop();
	}

	public void AddStack()
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		_gauge.Add(1f);
		iconStacks++;
		if (iconStacks >= _stacksToAttack)
		{
			_gauge.Clear();
			iconStacks = 0;
			((CoroutineReference)(ref _operationRunner)).Stop();
			_operationRunner = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)owner, _operations.CRun(owner));
		}
	}
}
