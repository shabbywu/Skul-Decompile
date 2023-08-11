using Characters.Abilities;
using Characters.Abilities.Weapons;
using Characters.Gear.Weapons.Gauges;
using Level;
using UnityEngine;

namespace CutScenes.Shots.Events.Customs;

public sealed class DropScrollAndCells : Event
{
	[SerializeField]
	private PrisonerChest _chest;

	[SerializeField]
	private PrisonerSkillScroll _skillScroll;

	[SerializeField]
	private DroppedCell _cellPrefab;

	[SerializeField]
	private CustomFloat _cellCount;

	[SerializeField]
	[Header("부여할 버프, 비워두면 부여하지 않음")]
	private AbilityBuff _buff;

	public override void Run()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		Vector3 position = ((Component)this).transform.position;
		position.y += 0.2f;
		Object.Instantiate<PrisonerSkillScroll>(_skillScroll, position, Quaternion.identity, ((Component)Map.Instance).transform).SetSkillInfo(_chest.weapon, _chest.skills, _chest.skillInfo);
		float value = _cellCount.value;
		ValueGauge gauge = (ValueGauge)_chest.weapon.gauge;
		ApplyBuff();
		for (int i = 0; (float)i < value; i++)
		{
			_cellPrefab.Spawn(((Component)this).transform.position, gauge);
		}
	}

	private void ApplyBuff()
	{
		if (!((Object)(object)_buff == (Object)null))
		{
			AbilityBuff abilityBuff = Object.Instantiate<AbilityBuff>(_buff);
			((Object)abilityBuff).name = ((Object)_buff).name;
			abilityBuff.Loot(_chest.weapon.owner);
		}
	}
}
