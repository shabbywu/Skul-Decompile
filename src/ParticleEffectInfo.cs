using System.Collections.Generic;
using Characters.Movements;
using Data;
using Level;
using UnityEngine;

[CreateAssetMenu]
public class ParticleEffectInfo : ScriptableObject
{
	[SerializeField]
	private float multiplier = 1f;

	[SerializeField]
	private DroppedParts[] _parts;

	public void Emit(Vector2 position, Bounds bounds, Push push)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		Vector2 force = Vector2.zero;
		if (push != null && !push.expired)
		{
			force = push.direction * push.totalForce;
		}
		Emit(position, bounds, force);
	}

	public void Emit(Vector2 position, Bounds bounds, Vector2 force, bool interpolate = true)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		if (GameData.Settings.particleQuality != 0)
		{
			DroppedParts[] parts = _parts;
			foreach (DroppedParts parts2 in parts)
			{
				SpawnParts(position, bounds, force, interpolate, parts2);
			}
		}
	}

	public void EmitRandom(Vector2 position, Bounds bounds, Vector2 force, bool interpolate = true)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		if (GameData.Settings.particleQuality != 0)
		{
			DroppedParts parts = ExtensionMethods.Random<DroppedParts>((IEnumerable<DroppedParts>)_parts);
			SpawnParts(position, bounds, force, interpolate, parts);
		}
	}

	private void SpawnParts(Vector2 position, Bounds bounds, Vector2 force, bool interpolate, DroppedParts parts)
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)parts == (Object)null)
		{
			Debug.LogError((object)(((Object)this).name + " : A parts is missing!"));
			return;
		}
		bool flag = parts.collideWithTerrain;
		if (GameData.Settings.particleQuality == 2)
		{
			if (parts.priority == DroppedParts.Priority.Low)
			{
				flag = false;
			}
		}
		else if (GameData.Settings.particleQuality == 1 && parts.priority == DroppedParts.Priority.Low)
		{
			flag = false;
		}
		Vector2Int count = parts.count;
		int x = ((Vector2Int)(ref count)).x;
		count = parts.count;
		int num = Random.Range(x, ((Vector2Int)(ref count)).y + 1);
		int layer = (flag ? ((GameData.Settings.particleQuality > 1) ? 11 : 27) : ((((Component)parts).gameObject.layer != 11) ? ((Component)parts).gameObject.layer : 0));
		for (int i = 0; i < num; i++)
		{
			Vector2 val;
			Quaternion val2;
			if (parts.randomize)
			{
				val = MMMaths.RandomPointWithinBounds(bounds) + Vector2.op_Implicit(((Component)parts).transform.position);
				val2 = Quaternion.AngleAxis((float)Random.Range(0, 360), Vector3.forward);
			}
			else
			{
				val = position + Vector2.op_Implicit(((Component)parts).transform.position);
				val2 = ((Component)parts).transform.rotation;
			}
			PoolObject obj = parts.poolObject.Spawn(Vector2.op_Implicit(val), val2, true);
			((Component)obj).GetComponent<DroppedParts>().Initialize(force, multiplier * 3f, interpolate);
			((Component)obj).gameObject.layer = layer;
		}
	}
}
