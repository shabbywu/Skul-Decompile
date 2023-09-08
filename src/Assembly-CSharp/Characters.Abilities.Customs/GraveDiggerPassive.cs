using System;
using System.Collections.Generic;
using Characters.Abilities.Constraints;
using Characters.Actions;
using Characters.Gear.Weapons;
using Characters.Minions;
using Characters.Monsters;
using Characters.Operations;
using Level;
using Services;
using Singletons;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Customs;

[Serializable]
public class GraveDiggerPassive : Ability
{
	public class Instance : AbilityInstance<GraveDiggerPassive>
	{
		private float _remainCorpseSpawnTime;

		public Instance(Character owner, GraveDiggerPassive ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			Singleton<Service>.Instance.levelManager.onMapChangedAndFadedIn += SpawnCorpseOnMap;
			if (ability._chanceToSpawnCorpseByKill > 0)
			{
				Character character = owner;
				character.onKilled = (Character.OnKilledDelegate)Delegate.Combine(character.onKilled, new Character.OnKilledDelegate(OnKilled));
			}
		}

		protected override void OnDetach()
		{
			Singleton<Service>.Instance.levelManager.onMapChangedAndFadedIn -= SpawnCorpseOnMap;
			if (ability._chanceToSpawnCorpseByKill > 0)
			{
				Character character = owner;
				character.onKilled = (Character.OnKilledDelegate)Delegate.Remove(character.onKilled, new Character.OnKilledDelegate(OnKilled));
			}
			ability._container.Clear();
			ability.corpse.minion.poolObject.DespawnAllSiblings();
			if ((Object)(object)ability._landOfDeadCorpseSpawner != (Object)null)
			{
				ability._landOfDeadCorpseSpawner.DespawnAllSiblings();
			}
		}

		public override void UpdateTime(float deltaTime)
		{
			if (ability._constraints.Pass() && !((Object)(object)Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.weapon.polymorphOrCurrent != (Object)(object)ability._weapon))
			{
				base.UpdateTime(deltaTime);
				HandlePeriodicCorpseSpawn(deltaTime);
			}
		}

		private void SpawnCorpseOnMap(Map old, Map @new)
		{
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			if (Map.Instance.type != 0)
			{
				return;
			}
			List<Character> allEnemies = Map.Instance.waveContainer.GetAllEnemies();
			allEnemies.PseudoShuffle();
			float num = math.min((float)allEnemies.Count, ability._corpseCountOnMap.value);
			for (int i = 0; (float)i < num; i++)
			{
				if (FindGroundSpawnPosition(allEnemies[i], out var position))
				{
					ability.SpawnCorpse(Vector2.op_Implicit(position));
				}
			}
		}

		private void OnKilled(ITarget target, ref Damage damage)
		{
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			Character character = target.character;
			if (!((Object)(object)character == (Object)null) && MMMaths.PercentChance(ability._chanceToSpawnCorpseByKill) && ability._characterTypeFilter[character.type] && !((Object)(object)Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.weapon.polymorphOrCurrent != (Object)(object)ability._weapon) && FindGroundSpawnPosition(character, out var position))
			{
				ability.SpawnCorpse(Vector2.op_Implicit(position));
			}
		}

		private void HandlePeriodicCorpseSpawn(float deltaTime)
		{
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			_remainCorpseSpawnTime -= deltaTime;
			if (!(_remainCorpseSpawnTime > 0f))
			{
				_remainCorpseSpawnTime += ability._corpseSpawnInterval.value;
				if (!FindGroundSpawnPosition(owner, out var position))
				{
					_remainCorpseSpawnTime += 0.5f;
				}
				ability.SpawnCorpse(Vector2.op_Implicit(position));
			}
		}

		private Collider2D FindGround(Vector2 position)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			Vector2 val = position + Vector2.up;
			if ((Object)(object)Physics2D.OverlapPoint(val, LayerMask.op_Implicit(Layers.groundMask)) != (Object)null)
			{
				return null;
			}
			RaycastHit2D val2 = Physics2D.Raycast(val, Vector2.down, ability._groundFindingDistance, LayerMask.op_Implicit(Layers.groundMask));
			if (!RaycastHit2D.op_Implicit(val2))
			{
				return null;
			}
			return ((RaycastHit2D)(ref val2)).collider;
		}

		private bool FindGroundSpawnPosition(Character character, out Vector2 position)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected O, but got Unknown
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			position = Vector2.op_Implicit(((Component)character).transform.position);
			BoxCollider2D val = (BoxCollider2D)ability.corpse.collider;
			Vector2 val2 = default(Vector2);
			((Vector2)(ref val2))._002Ector(Random.Range(0f - ability._spawnRange, ability._spawnRange), 0f);
			Collider2D val3 = FindGround(position + val2);
			if ((Object)(object)val3 == (Object)null)
			{
				val3 = ((!((Object)(object)character.movement == (Object)null)) ? character.movement.controller.collisionState.lastStandingCollider : FindGround(position));
			}
			if ((Object)(object)val3 == (Object)null)
			{
				return false;
			}
			Bounds bounds = val3.bounds;
			float num = math.max(((Bounds)(ref bounds)).min.x + val.size.x, position.x - ability._spawnRange);
			float num2 = math.min(((Bounds)(ref bounds)).max.x - val.size.x, position.x + ability._spawnRange);
			position.x = Random.Range(num, num2);
			position.y = ((Bounds)(ref bounds)).max.y;
			return true;
		}

		public void ReduceSkillCooldown()
		{
			foreach (SkillInfo currentSkill in owner.playerComponents.inventory.weapon.current.currentSkills)
			{
				Characters.Actions.Action action = currentSkill.action;
				if (action.cooldown.time != null)
				{
					action.cooldown.time.ReduceCooldown(ability._reduceSkillCooldown);
				}
			}
		}
	}

	[SerializeField]
	private Weapon _weapon;

	[SerializeField]
	[Header("Grave Setting")]
	private GraveDiggerGrave _graveDiggerGrave;

	[SerializeField]
	[Header("Corpse Setting")]
	private GraveDiggerCorpse _corpse;

	[SerializeField]
	private MinionSetting _corpseSetting;

	[SerializeField]
	private CustomFloat _corpseSpawnInterval;

	[SerializeField]
	private float _groundFindingDistance = 7f;

	[SerializeField]
	private float _spawnRange = 2f;

	[Header("Corpse Setting")]
	[SerializeField]
	private PoolObject _landOfDeadCorpseSpawner;

	[SerializeField]
	[Header("망령을 처치 시 스킬 쿨타임 감소")]
	private float _reduceSkillCooldown;

	[SerializeField]
	[Header("맵 진입 시 생성되는 망령 개수")]
	private CustomFloat _corpseCountOnMap = new CustomFloat(3f, 5f);

	[SerializeField]
	[Tooltip("적 처치 시 망령 생성확률")]
	[Range(0f, 100f)]
	[Header("Spawn Corpse By Kill")]
	private int _chanceToSpawnCorpseByKill;

	[SerializeField]
	private CharacterTypeBoolArray _characterTypeFilter = new CharacterTypeBoolArray(true, true, true, true, true, false, false, false);

	[Space]
	[SerializeField]
	[Constraint.Subcomponent]
	private Constraint.Subcomponents _constraints;

	[SerializeField]
	private GraveDiggerGraveContainer _container;

	[SerializeField]
	private Transform _corpsePosition;

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _onCorpseDied;

	private Instance _instance;

	private Character _owner;

	public GraveDiggerCorpse corpse => _corpse;

	public override void Initialize()
	{
		base.Initialize();
		_onCorpseDied.Initialize();
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		if (_instance == null)
		{
			_instance = new Instance(owner, this);
		}
		_owner = owner;
		return _instance;
	}

	public void HandleCorpseDie(Vector3 position)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		if (_instance != null)
		{
			SpawnGrave(position);
			if (!(_reduceSkillCooldown <= 0f))
			{
				_instance.ReduceSkillCooldown();
			}
		}
	}

	public void SpawnCorpse(Vector3 position)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		Monster summonMinion = _corpse.minion.Summon(position);
		((Component)summonMinion).GetComponent<GraveDiggerCorpse>().SetPassive(this, _owner);
		summonMinion.OnDespawn -= OnCorpseDespawn;
		summonMinion.OnDespawn += OnCorpseDespawn;
		void OnCorpseDespawn()
		{
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			summonMinion.OnDespawn -= OnCorpseDespawn;
			if (!((Object)(object)summonMinion.character == (Object)null))
			{
				if ((Object)(object)_corpsePosition != (Object)null)
				{
					_corpsePosition.position = ((Component)summonMinion).transform.position;
				}
				((MonoBehaviour)_owner).StartCoroutine(_onCorpseDied.CRun(_owner));
			}
		}
	}

	public void SpawnGrave(Vector3 position)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		_graveDiggerGrave.Spawn(position, _container);
	}

	public void OnDestroy()
	{
		if ((Object)(object)_corpsePosition != (Object)null)
		{
			Object.Destroy((Object)(object)_corpsePosition);
		}
	}
}
