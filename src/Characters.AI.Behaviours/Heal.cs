using System.Collections;
using System.Collections.Generic;
using Characters.Actions;
using Level;
using UnityEngine;

namespace Characters.AI.Behaviours;

public class Heal : Behaviour
{
	[Range(1f, 100f)]
	[SerializeField]
	private int _count;

	[SerializeField]
	private float _delay;

	[MinMaxSlider(0f, 100f)]
	[SerializeField]
	private Vector2 _amountRange;

	[SerializeField]
	private Action _healMotion;

	private EnemyWaveContainer _enemyWaveContainer;

	private void Start()
	{
		_enemyWaveContainer = Map.Instance.waveContainer;
	}

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		Random.Range(_amountRange.x, _amountRange.y);
		ExtensionMethods.Random<Character>((IEnumerable<Character>)_enemyWaveContainer.GetAllEnemies());
		_healMotion.TryStart();
		yield break;
	}
}
