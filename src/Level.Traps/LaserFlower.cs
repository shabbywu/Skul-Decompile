using System.Collections;
using Characters;
using Characters.Actions;
using UnityEngine;

namespace Level.Traps;

public class LaserFlower : MonoBehaviour
{
	private enum FireDirection
	{
		Up,
		Down,
		Right,
		Left
	}

	[SerializeField]
	private Character _character;

	[SerializeField]
	private GameObject _horizontalBody;

	[SerializeField]
	private GameObject _verticalBody;

	[SerializeField]
	[Range(0f, 1f)]
	private float _startTiming;

	[SerializeField]
	private float _interval = 4f;

	[SerializeField]
	private int _laserSize = 3;

	[SerializeField]
	private Action _attackAction;

	[SerializeField]
	private Action _idleAction;

	[SerializeField]
	private Transform _attackRangeTransform;

	[SerializeField]
	private Transform _effectHead;

	[SerializeField]
	private Transform _effectBody;

	private FireDirection _fireDirection;

	private void OnEnable()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		_attackRangeTransform.localScale = new Vector3(1f, (float)(_laserSize + 1), 1f);
		_effectBody.localScale = new Vector3(1f, (float)_laserSize, 1f);
		Vector3 position = _effectBody.position;
		Vector3 zero = Vector3.zero;
		Quaternion localRotation = ((Component)this).transform.localRotation;
		float z = ((Quaternion)(ref localRotation)).eulerAngles.z;
		if (z != 0f)
		{
			if (z != 180f)
			{
				if (z != 90f)
				{
					if (z == 270f)
					{
						((Vector3)(ref zero))._002Ector(position.x + (float)_laserSize, position.y, position.z);
					}
				}
				else
				{
					((Vector3)(ref zero))._002Ector(position.x - (float)_laserSize, position.y, position.z);
				}
			}
			else
			{
				((Vector3)(ref zero))._002Ector(position.x, position.y - (float)_laserSize, position.z);
			}
		}
		else
		{
			((Vector3)(ref zero))._002Ector(position.x, position.y + (float)_laserSize, position.z);
		}
		_effectHead.position = zero;
	}

	private void Awake()
	{
		((MonoBehaviour)this).StartCoroutine(CAttack());
	}

	private IEnumerator CAttack()
	{
		yield return Chronometer.global.WaitForSeconds(_startTiming * _interval);
		while (true)
		{
			_attackAction.TryStart();
			yield return Chronometer.global.WaitForSeconds(_interval);
			_idleAction.TryStart();
		}
	}
}
