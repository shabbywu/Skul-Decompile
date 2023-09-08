using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Gear.Weapons;
using Characters.Movements;
using Data;
using Level;
using PhysicsUtils;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Weapons;

[Serializable]
public class PrisonerChestPassive : Ability
{
	public class Instance : AbilityInstance<PrisonerChestPassive>
	{
		public Instance(Character owner, PrisonerChestPassive ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			Singleton<Service>.Instance.levelManager.onMapLoadedAndFadedIn += ability.SpawnChest;
		}

		protected override void OnDetach()
		{
			Singleton<Service>.Instance.levelManager.onMapLoadedAndFadedIn -= ability.SpawnChest;
		}
	}

	private const int _randomSeed = -612673708;

	[SerializeField]
	private Weapon _weapon;

	[SerializeField]
	[Space]
	private PrisonerChest _chest;

	[SerializeField]
	private PrisonerChest _cursedChest;

	[SerializeField]
	[Space]
	private PrisonerSkillInfosByGrade[] _skills;

	[Range(0f, 100f)]
	[Header("상자가 나올 확률")]
	[SerializeField]
	private int _chestChance;

	[Header("상자가 나왔을 때, 저주받은 상자일 확률")]
	[Range(0f, 100f)]
	[SerializeField]
	private int _cursedChestChance;

	[SerializeField]
	[Header("상자 연출을 위해 필요한 좌우 너비")]
	private float _widthForChest;

	[SerializeField]
	private BoxCollider2D _cutSceneArea;

	private NonAllocOverlapper _overlapper;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		_overlapper = new NonAllocOverlapper(1);
		return new Instance(owner, this);
	}

	private bool TryGetChestSpawnPosition(Random random, out Vector3 position)
	{
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		List<Character> allEnemies = Map.Instance.waveContainer.GetAllEnemies();
		allEnemies.PseudoShuffle(random);
		for (int i = 0; i < allEnemies.Count; i++)
		{
			Character character = allEnemies[i];
			if (character.type != 0 || (Object)(object)character.movement == (Object)null || character.movement.baseConfig.type == Movement.Config.Type.AcceleratingFlying || character.movement.baseConfig.type == Movement.Config.Type.Flying)
			{
				continue;
			}
			RaycastHit2D val = Physics2D.Raycast(Vector2.op_Implicit(((Component)character).transform.position), Vector2.down, 10f, LayerMask.op_Implicit(Layers.groundMask));
			if (!RaycastHit2D.op_Implicit(val) || (Object)(object)((Component)((RaycastHit2D)(ref val)).collider).GetComponent<PolygonCollider2D>() == (Object)null)
			{
				continue;
			}
			Bounds bounds = ((RaycastHit2D)(ref val)).collider.bounds;
			if (!(((Bounds)(ref bounds)).size.x < _widthForChest))
			{
				bounds = ((RaycastHit2D)(ref val)).collider.bounds;
				float x = ((Bounds)(ref bounds)).center.x;
				bounds = ((RaycastHit2D)(ref val)).collider.bounds;
				position = Vector2.op_Implicit(new Vector2(x, ((Bounds)(ref bounds)).max.y));
				Cage cage = Map.Instance.cage;
				if ((!((Object)(object)cage != (Object)null) || !((Behaviour)cage).isActiveAndEnabled || !(Vector2.SqrMagnitude(Vector2.op_Implicit(position - ((Component)cage).transform.position)) < 9f)) && CheckPlayableArea(Vector2.op_Implicit(position)))
				{
					return true;
				}
			}
		}
		position = Vector3.zero;
		return false;
	}

	private bool CheckPlayableArea(Vector2 point)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		((Component)_cutSceneArea).transform.position = Vector2.op_Implicit(point);
		((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(LayerMask.op_Implicit(256));
		Physics2D.SyncTransforms();
		if (_overlapper.OverlapCollider((Collider2D)(object)_cutSceneArea).results.Count > 0)
		{
			return false;
		}
		return true;
	}

	private void SpawnChest()
	{
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		Random random = new Random(GameData.Save.instance.randomSeed + -612673708 + (int)currentChapter.type * 256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex);
		if (Map.Instance.type != 0 || !MMMaths.PercentChance(random, _chestChance))
		{
			return;
		}
		if (!TryGetChestSpawnPosition(random, out var position))
		{
			Debug.Log((object)"spawn chest failed");
			return;
		}
		PrisonerChest prisonerChest = Object.Instantiate<PrisonerChest>(MMMaths.PercentChance(random, _cursedChestChance) ? _cursedChest : _chest, position, Quaternion.identity, ((Component)Map.Instance).transform);
		int gradeBonus = prisonerChest.GetGradeBonus();
		PrisonerSkill skill1 = ((Component)_weapon.currentSkills[0]).GetComponent<PrisonerSkill>();
		PrisonerSkill skill2 = ((Component)_weapon.currentSkills[1]).GetComponent<PrisonerSkill>();
		int scrollGrade = (skill1.level + skill2.level + gradeBonus) / 2;
		int num = skill1.parent.skillInfos.Count - 1;
		if (scrollGrade > num)
		{
			scrollGrade = num;
		}
		PrisonerSkillInfosByGrade prisonerSkillInfosByGrade = _skills.Where(delegate(PrisonerSkillInfosByGrade s)
		{
			if (skill1.level >= scrollGrade && skill1.parent.key.Equals(s.key, StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			return (skill2.level < scrollGrade || !skill2.parent.key.Equals(s.key, StringComparison.OrdinalIgnoreCase)) ? true : false;
		}).Random(random);
		prisonerChest.SetSkillInfo(skillInfo: prisonerSkillInfosByGrade.skillInfos[scrollGrade], weapon: _weapon, skills: prisonerSkillInfosByGrade);
	}
}
