using Characters.Actions;
using Level;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions;

public sealed class Manatech : SimpleStatBonusKeyword
{
	[SerializeField]
	private float _cooldownReducingAmount;

	[SerializeField]
	private DroppedManatechPart _manatechPart;

	[SerializeField]
	private double[] _countBystep;

	[SerializeField]
	private double[] _statBonusBystep;

	protected override double[] statBonusByStep => _statBonusBystep;

	protected override Stat.Category statCategory => Stat.Category.PercentPoint;

	protected override Stat.Kind statKind => Stat.Kind.SkillAttackSpeed;

	public override void UpdateBonus(bool wasActive, bool wasOmen)
	{
		UpdateStat();
	}

	public override void Attach()
	{
		base.Attach();
		base.character.onStartAction += OnStartAction;
	}

	public override void Detach()
	{
		base.Detach();
		base.character.onStartAction -= OnStartAction;
	}

	private void OnStartAction(Action action)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		if (action.type == Action.Type.Skill && !action.cooldown.usedByStreak)
		{
			for (int i = 0; (double)i < _countBystep[keyword.step]; i++)
			{
				Vector3 position = ((Component)this).transform.position;
				position.y += 0.5f;
				((Component)_manatechPart.poolObject.Spawn(position, true)).GetComponent<DroppedManatechPart>().cooldownReducingAmount = _cooldownReducingAmount;
			}
		}
	}
}
