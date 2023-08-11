using UnityEngine;
using UnityEngine.Tilemaps;

namespace Level;

public class PasteTile : MonoBehaviour
{
	[SerializeField]
	private Tilemap _original;

	public void Paste(Tilemap to)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		TileBase[] tilesBlock = _original.GetTilesBlock(_original.cellBounds);
		for (int i = 0; i < tilesBlock.Length; i++)
		{
			TileBase val = tilesBlock[i];
			if (!((Object)(object)val == (Object)null))
			{
				BoundsInt cellBounds = _original.cellBounds;
				Vector3Int size = ((BoundsInt)(ref cellBounds)).size;
				int x = ((Vector3Int)(ref size)).x;
				Vector3Int origin = _original.origin;
				((Vector3Int)(ref origin)).x = ((Vector3Int)(ref origin)).x + i % x;
				((Vector3Int)(ref origin)).y = ((Vector3Int)(ref origin)).y + i / x;
				to.SetTile(origin, val);
			}
		}
	}
}
