using UnityEngine;

namespace Characters.Operations.Customs;

public class SetRotation : CharacterOperation
{
	private enum Type
	{
		Manually,
		Random
	}

	[SerializeField]
	private Type _type;

	[SerializeField]
	private Transform _transform;

	[Range(-180f, 180f)]
	[SerializeField]
	private float _rotateValue;

	public override void Run(Character owner)
	{
		if (_type == Type.Manually)
		{
			RotateEulerAngles();
		}
		else if (_type == Type.Random)
		{
			RotateRandomEulerAngles();
		}
	}

	private void RotateEulerAngles()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		_transform.rotation = Quaternion.Euler(0f, 0f, _rotateValue);
	}

	private void RotateRandomEulerAngles()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		int num = Random.Range(-180, 180);
		_transform.rotation = Quaternion.Euler(0f, 0f, (float)num);
	}
}
