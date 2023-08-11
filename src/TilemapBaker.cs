using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapBaker : MonoBehaviour
{
	private static readonly List<Vector2> _pathesCache = new List<Vector2>(32);

	[GetComponent]
	[SerializeField]
	private Tilemap _tilemap;

	[GetComponent]
	[SerializeField]
	private TilemapCollider2D _tilemapCollider;

	[SerializeField]
	[GetComponent]
	private CompositeCollider2D _compositeCollider;

	[GetComponent]
	[SerializeField]
	private Rigidbody2D _rigidbody;

	private Bounds _bounds;

	public Bounds bounds => _bounds;

	private void Bake(ColliderFilter filter, int layer)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Expected O, but got Unknown
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		CustomColliderTile.colliderFilter = filter;
		((Collider2D)_tilemapCollider).usedByComposite = false;
		_tilemap.RefreshAllTiles();
		Bounds val = ((Collider2D)_tilemapCollider).bounds;
		if (((Bounds)(ref val)).size != Vector3.zero)
		{
			if (((Bounds)(ref _bounds)).size == Vector3.zero)
			{
				_bounds = ((Collider2D)_tilemapCollider).bounds;
			}
			else
			{
				ref Bounds reference = ref _bounds;
				Vector3 max = ((Bounds)(ref _bounds)).max;
				val = ((Collider2D)_tilemapCollider).bounds;
				((Bounds)(ref reference)).max = Vector3.Max(max, ((Bounds)(ref val)).max);
				ref Bounds reference2 = ref _bounds;
				Vector3 min = ((Bounds)(ref _bounds)).min;
				val = ((Collider2D)_tilemapCollider).bounds;
				((Bounds)(ref reference2)).min = Vector3.Min(min, ((Bounds)(ref val)).min);
			}
		}
		((Collider2D)_tilemapCollider).usedByComposite = true;
		_compositeCollider.GenerateGeometry();
		GameObject val2 = new GameObject(((object)(ColliderFilter)(ref filter)).ToString());
		val2.transform.parent = ((Component)this).transform;
		val2.transform.position = Vector3.zero;
		val2.layer = layer;
		val2.AddComponent<Rigidbody2D>().bodyType = (RigidbodyType2D)2;
		for (int i = 0; i < ((Collider2D)_compositeCollider).shapeCount; i++)
		{
			int path = _compositeCollider.GetPath(i, _pathesCache);
			GameObject val3 = new GameObject(((object)(ColliderFilter)(ref filter)).ToString());
			val3.transform.parent = val2.transform;
			val3.transform.position = Vector3.zero;
			val3.layer = layer;
			val3.AddComponent<PolygonCollider2D>().points = _pathesCache.Take(path).ToArray();
		}
	}

	private void FillPadding(int amount)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		_tilemap.CompressBounds();
		Bounds localBounds = _tilemap.localBounds;
		Vector2Int val = default(Vector2Int);
		((Vector2Int)(ref val))._002Ector((int)((Bounds)(ref localBounds)).min.x, (int)((Bounds)(ref localBounds)).min.y);
		Vector2Int val2 = default(Vector2Int);
		((Vector2Int)(ref val2))._002Ector((int)((Bounds)(ref localBounds)).max.x - 1, (int)((Bounds)(ref localBounds)).max.y - 1);
		CustomColliderTile tile;
		for (int i = ((Vector2Int)(ref val)).x; i <= ((Vector2Int)(ref val2)).x; i++)
		{
			Fill(new Vector3Int(i, ((Vector2Int)(ref val)).y, 0), Vector3Int.down, (CustomColliderTile t) => t.verticallyOpened);
			Fill(new Vector3Int(i, ((Vector2Int)(ref val2)).y, 0), Vector3Int.up, (CustomColliderTile t) => t.verticallyOpened);
		}
		for (int j = ((Vector2Int)(ref val)).y; j <= ((Vector2Int)(ref val2)).y; j++)
		{
			Fill(new Vector3Int(((Vector2Int)(ref val)).x, j, 0), Vector3Int.left, (CustomColliderTile t) => t.horizontallyOpened);
			Fill(new Vector3Int(((Vector2Int)(ref val2)).x, j, 0), Vector3Int.right, (CustomColliderTile t) => t.horizontallyOpened);
		}
		FillCorners(new Vector3Int(((Vector2Int)(ref val)).x, ((Vector2Int)(ref val)).y, 0), -1, -1);
		FillCorners(new Vector3Int(((Vector2Int)(ref val2)).x, ((Vector2Int)(ref val)).y, 0), 1, -1);
		FillCorners(new Vector3Int(((Vector2Int)(ref val)).x, ((Vector2Int)(ref val2)).y, 0), -1, 1);
		FillCorners(new Vector3Int(((Vector2Int)(ref val2)).x, ((Vector2Int)(ref val2)).y, 0), 1, 1);
		void Fill(Vector3Int point, Vector3Int direction, Func<CustomColliderTile, bool> tileFilter)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			tile = _tilemap.GetTile<CustomColliderTile>(point);
			if ((Object)(object)tile != (Object)null && tileFilter(tile))
			{
				for (int m = 1; m <= amount; m++)
				{
					_tilemap.SetTile(point + direction * m, (TileBase)(object)tile);
				}
			}
		}
		void FillCorners(Vector3Int point, int xDirection, int yDirection)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			tile = _tilemap.GetTile<CustomColliderTile>(point);
			if ((Object)(object)tile != (Object)null && (int)tile.position == 0)
			{
				for (int k = 1; k <= amount; k++)
				{
					for (int l = 1; l <= amount; l++)
					{
						_tilemap.SetTile(new Vector3Int(((Vector3Int)(ref point)).x + xDirection * k, ((Vector3Int)(ref point)).y + yDirection * l, 0), (TileBase)(object)tile);
					}
				}
			}
		}
	}

	public void Bake()
	{
		_rigidbody.bodyType = (RigidbodyType2D)2;
		_compositeCollider.generationType = (GenerationType)1;
		Bake((ColliderFilter)0, 8);
		Bake((ColliderFilter)1, 18);
		Bake((ColliderFilter)2, 19);
		Bake((ColliderFilter)3, 17);
		FillPadding(3);
		Object.Destroy((Object)(object)_compositeCollider);
		Object.Destroy((Object)(object)_tilemapCollider);
		Object.Destroy((Object)(object)_rigidbody);
		Object.Destroy((Object)(object)this);
	}
}
