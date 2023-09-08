using Characters;
using FX.SpriteEffects;
using UnityEngine;

public class AlphaThresholdSetter : MonoBehaviour
{
	[SerializeField]
	private Character _character;

	[SerializeField]
	private float _value = 0.8f;

	[SerializeField]
	private int _priority = -2147483647;

	private void Start()
	{
		_character.spriteEffectStack.Add(new AlphaThreshold(_priority, _value, 0f));
	}
}
