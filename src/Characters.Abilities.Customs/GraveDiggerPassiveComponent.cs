using UnityEngine;

namespace Characters.Abilities.Customs;

public class GraveDiggerPassiveComponent : AbilityComponent<GraveDiggerPassive>
{
	public void SpawnGrave(Vector3 position)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		base.baseAbility.SpawnGrave(position);
	}

	public void SpawnCorpse(Vector3 position)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		base.baseAbility.SpawnCorpse(position);
	}

	private void OnDestroy()
	{
		base.baseAbility.OnDestroy();
	}
}
