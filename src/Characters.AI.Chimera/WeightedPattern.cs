using UnityEngine;

namespace Characters.AI.Chimera;

public class WeightedPattern : MonoBehaviour
{
	private WeightedRandomizer<Pattern> _weightedRandomizer;

	[Range(0f, 100f)]
	[SerializeField]
	private float _idleWeight;

	[Range(0f, 100f)]
	[SerializeField]
	private float _biteWeight;

	[SerializeField]
	[Range(0f, 100f)]
	private float _slamWeight;

	[Range(0f, 100f)]
	[SerializeField]
	private float _venomFallWeight;

	[SerializeField]
	[Range(0f, 100f)]
	private float _venomBallWeight;

	[Range(0f, 100f)]
	[SerializeField]
	private float _venomCannonWeight;

	[SerializeField]
	[Range(0f, 100f)]
	private float _subjectDropWeight;

	[SerializeField]
	[Range(0f, 100f)]
	private float _wreckDropWeight;

	[Range(0f, 100f)]
	[SerializeField]
	private float _wreckDestroyWeight;

	[SerializeField]
	[Range(0f, 100f)]
	private float _venomBreathWeight;

	private void Awake()
	{
		_weightedRandomizer = WeightedRandomizer.From<Pattern>((Pattern.Idle, _idleWeight), (Pattern.Bite, _biteWeight), (Pattern.Stomp, _slamWeight), (Pattern.VenomFall, _venomFallWeight), (Pattern.VenomBall, _venomBallWeight), (Pattern.VenomCannon, _venomCannonWeight), (Pattern.SubjectDrop, _subjectDropWeight), (Pattern.WreckDrop, _wreckDropWeight), (Pattern.WreckDestroy, _wreckDestroyWeight), (Pattern.VenomBreath, _venomBreathWeight));
	}

	public Pattern TakeOne()
	{
		return _weightedRandomizer.TakeOne();
	}
}
