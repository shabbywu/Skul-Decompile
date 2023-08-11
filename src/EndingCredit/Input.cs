using InControl;
using UnityEngine;
using UserInput;

namespace EndingCredit;

public class Input : MonoBehaviour
{
	[SerializeField]
	private float _speed;

	[SerializeField]
	private float _accelerationValue;

	private float _startSpeed;

	public float speed => _speed;

	private void Start()
	{
		_startSpeed = _speed;
	}

	private void Update()
	{
		if (((OneAxisInputControl)KeyMapper.Map.Attack).IsPressed || ((OneAxisInputControl)KeyMapper.Map.Jump).IsPressed || ((OneAxisInputControl)KeyMapper.Map.Submit).IsPressed || ((OneAxisInputControl)KeyMapper.Map.Down).IsPressed)
		{
			_speed = _startSpeed * _accelerationValue;
		}
		if (((OneAxisInputControl)KeyMapper.Map.Attack).WasReleased || ((OneAxisInputControl)KeyMapper.Map.Jump).WasReleased || ((OneAxisInputControl)KeyMapper.Map.Submit).WasReleased || ((OneAxisInputControl)KeyMapper.Map.Down).WasReleased)
		{
			_speed = _startSpeed;
		}
	}
}
