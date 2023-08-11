using Level;
using UnityEngine;

namespace Characters.Operations.SetPosition;

public class ToSavedPosition : Policy
{
	[SerializeField]
	private PositionCache _repo;

	public override Vector2 GetPosition(Character owner)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return GetPosition();
	}

	public override Vector2 GetPosition()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return _repo.Load();
	}
}
