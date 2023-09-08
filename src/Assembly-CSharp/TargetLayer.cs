using System;
using UnityEngine;

[Serializable]
public class TargetLayer
{
	[SerializeField]
	private LayerMask _rawMask;

	[SerializeField]
	private bool _allyBody;

	[SerializeField]
	private bool _foeBody;

	[SerializeField]
	private bool _allyProjectile;

	[SerializeField]
	private bool _foeProjectile;

	public static bool IsPlayer(int layer)
	{
		return !IsMonster(layer);
	}

	public static bool IsMonster(int layer)
	{
		if (layer != 10)
		{
			return layer == 16;
		}
		return true;
	}

	public TargetLayer()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		_rawMask = LayerMask.op_Implicit(0);
		_allyBody = false;
		_foeBody = true;
		_allyProjectile = false;
		_foeProjectile = false;
	}

	public TargetLayer(LayerMask rawMask, bool allyBody, bool foeBody, bool allyProjectile, bool foeProjectile)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		_rawMask = rawMask;
		_allyBody = allyBody;
		_foeBody = foeBody;
		_allyProjectile = allyProjectile;
		_foeProjectile = foeProjectile;
	}

	public LayerMask Evaluate(GameObject owner)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		LayerMask val = _rawMask;
		if (IsMonster(owner.layer))
		{
			if (_allyBody)
			{
				val = LayerMask.op_Implicit(LayerMask.op_Implicit(val) | 0x400);
			}
			if (_foeBody)
			{
				val = LayerMask.op_Implicit(LayerMask.op_Implicit(val) | 0x200);
			}
			if (_allyProjectile)
			{
				val = LayerMask.op_Implicit(LayerMask.op_Implicit(val) | 0x10000);
			}
			if (_foeProjectile)
			{
				val = LayerMask.op_Implicit(LayerMask.op_Implicit(val) | 0x8000);
			}
		}
		else
		{
			if (_allyBody)
			{
				val = LayerMask.op_Implicit(LayerMask.op_Implicit(val) | 0x200);
			}
			if (_foeBody)
			{
				val = LayerMask.op_Implicit(LayerMask.op_Implicit(val) | 0x400);
			}
			if (_allyProjectile)
			{
				val = LayerMask.op_Implicit(LayerMask.op_Implicit(val) | 0x8000);
			}
			if (_foeProjectile)
			{
				val = LayerMask.op_Implicit(LayerMask.op_Implicit(val) | 0x10000);
			}
		}
		return val;
	}
}
