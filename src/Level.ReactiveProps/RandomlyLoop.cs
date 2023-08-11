using System.Collections.Generic;
using UnityEngine;

namespace Level.ReactiveProps;

public class RandomlyLoop : MonoBehaviour
{
	[SerializeField]
	private FlyGroup[] _groups;

	[SerializeField]
	[MinMaxSlider(1f, 100f)]
	private Vector2 _termRange;

	private float _term;

	private float _elapsed;

	private void Start()
	{
		_term = Random.Range(_termRange.x, _termRange.y);
	}

	private void Update()
	{
		_elapsed += ((ChronometerBase)Chronometer.global).deltaTime;
		if (_elapsed > _term)
		{
			ExtensionMethods.Random<FlyGroup>((IEnumerable<FlyGroup>)_groups).Activate();
			_elapsed -= _term;
			_term = Random.Range(_termRange.x, _termRange.y);
		}
	}
}
