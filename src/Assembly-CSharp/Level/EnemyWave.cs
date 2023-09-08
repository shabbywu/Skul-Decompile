using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Characters;
using Data;
using FX;
using GameResources;
using Level.Waves;
using PhysicsUtils;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Level;

public class EnemyWave : Wave
{
	[Serializable]
	private class GameObjectRandomizer
	{
		[Serializable]
		public class Reorderable : ReorderableArray<GameObjectRandomizer>
		{
			public void Randomize(Random random)
			{
				if (values.Length == 0)
				{
					return;
				}
				int maxValue = values.Sum((GameObjectRandomizer v) => v._weight);
				int num = random.Next(0, maxValue) + 1;
				int num2 = 0;
				for (int i = 0; i < values.Length; i++)
				{
					num -= values[i]._weight;
					if (num <= 0)
					{
						num2 = i;
						break;
					}
				}
				for (int j = 0; j < values.Length; j++)
				{
					if (j != num2)
					{
						GameObject gameObject = values[j]._gameObject;
						gameObject.transform.parent = null;
						Object.Destroy((Object)(object)gameObject);
					}
				}
			}
		}

		[SerializeField]
		private GameObject _gameObject;

		[SerializeField]
		private int _weight = 1;
	}

	private class Assets
	{
		internal static EffectInfo enemyAppearance = new EffectInfo(CommonResource.instance.enemyAppearanceEffect)
		{
			sortingLayerId = SortingLayer.NameToID("Summon")
		};
	}

	private const int _randomSeed = 1787508074;

	private static readonly NonAllocOverlapper _overlapper;

	private const float _enemySpawnDelay = 0.4f;

	[SerializeField]
	private bool _deactiveOnAwake = true;

	[SerializeField]
	private string[] _keys;

	[SerializeField]
	[Subcomponent(typeof(SpawnConditionInfo))]
	private SpawnConditionInfo _conditions;

	private int _remains;

	public int remains => _remains;

	public string[] keys => _keys;

	public List<Character> characters { get; private set; }

	public List<DestructibleObject> destructibleObjects { get; private set; }

	public event Action<int> onChildrenChanged;

	static EnemyWave()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		_overlapper = new NonAllocOverlapper(1);
		((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(LayerMask.op_Implicit(512));
	}

	public void Spawn(bool effect = true)
	{
		try
		{
			if (base.state != 0)
			{
				return;
			}
			base.state = State.Spawned;
			_onSpawn?.Invoke();
			if (effect)
			{
				((MonoBehaviour)this).StartCoroutine(CRun());
				return;
			}
			foreach (Character character in characters)
			{
				((Component)character).gameObject.SetActive(true);
			}
			foreach (DestructibleObject destructibleObject in destructibleObjects)
			{
				((Component)destructibleObject).gameObject.SetActive(true);
			}
		}
		catch (Exception ex)
		{
			Debug.Log((object)("Error while spawn enemy wave : " + ex.Message));
			Clear();
		}
		IEnumerator CRun()
		{
			foreach (Character character2 in characters)
			{
				if (!((Component)character2).gameObject.activeSelf)
				{
					Assets.enemyAppearance.Spawn(((Component)character2).transform.position);
				}
			}
			foreach (DestructibleObject destructibleObject2 in destructibleObjects)
			{
				if (!((Component)destructibleObject2).gameObject.activeSelf)
				{
					Assets.enemyAppearance.Spawn(((Component)destructibleObject2).transform.position);
				}
			}
			yield return Chronometer.global.WaitForSeconds(0.4f);
			foreach (Character character3 in characters)
			{
				((Component)character3).gameObject.SetActive(true);
			}
			foreach (DestructibleObject destructibleObject3 in destructibleObjects)
			{
				((Component)destructibleObject3).gameObject.SetActive(true);
			}
		}
	}

	private void Clear()
	{
		if (base.state != State.Cleared)
		{
			base.state = State.Cleared;
			_onClear?.Invoke();
		}
	}

	private void DecreaseRemains()
	{
		_remains--;
		this.onChildrenChanged?.Invoke(_remains);
		if (_remains == 0)
		{
			Clear();
		}
	}

	public override void Initialize()
	{
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		new Random(GameData.Save.instance.randomSeed + 1787508074 + (int)currentChapter.type * 256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex);
		destructibleObjects = new List<DestructibleObject>();
		characters = new List<Character>();
		AddCharacterOrDestructibleObject(((Component)this).transform);
		_remains = characters.Count + destructibleObjects.Count;
		((Component)this).gameObject.SetActive(true);
		((MonoBehaviour)this).StartCoroutine("CCheckSpawnConditions");
		void AddCharacterOrDestructibleObject(Transform transform)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected O, but got Unknown
			foreach (Transform item in transform)
			{
				Transform val = item;
				GroupSelector component = ((Component)val).GetComponent<GroupSelector>();
				Character character = ((Component)val).GetComponent<Character>();
				DestructibleObject destructibleObject = ((Component)val).GetComponent<DestructibleObject>();
				if ((Object)(object)component != (Object)null)
				{
					ICollection<Character> collection = component.Load();
					if (collection.Count == 0)
					{
						Debug.Log((object)("Wave (" + ((Object)((Component)this).gameObject).name + ")가 비어있습니다"));
						Clear();
					}
					foreach (Character enemy in collection)
					{
						characters.Add(enemy);
						enemy.health.onDiedTryCatch += delegate
						{
							DecreaseRemains();
							characters.Remove(enemy);
						};
						if (_deactiveOnAwake)
						{
							((Component)enemy).gameObject.SetActive(false);
						}
					}
				}
				else if ((Object)(object)character != (Object)null)
				{
					characters.Add(character);
					character.health.onDiedTryCatch += delegate
					{
						DecreaseRemains();
						characters.Remove(character);
					};
					if (_deactiveOnAwake)
					{
						((Component)character).gameObject.SetActive(false);
					}
				}
				else if ((Object)(object)destructibleObject != (Object)null)
				{
					destructibleObjects.Add(destructibleObject);
					destructibleObject.onDestroy += delegate
					{
						DecreaseRemains();
						destructibleObjects.Remove(destructibleObject);
					};
					if (_deactiveOnAwake)
					{
						((Component)destructibleObject).gameObject.SetActive(false);
					}
				}
				else
				{
					AddCharacterOrDestructibleObject(val);
				}
			}
		}
	}

	private IEnumerator CCheckSpawnConditions()
	{
		if (!((Object)(object)_conditions == (Object)null))
		{
			yield return null;
			while (!_conditions.IsSatisfied(this))
			{
				yield return null;
			}
			if (base.state == State.Waiting)
			{
				Spawn();
			}
		}
	}
}
