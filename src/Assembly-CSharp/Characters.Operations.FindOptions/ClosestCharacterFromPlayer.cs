using System;
using System.Collections.Generic;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations.FindOptions;

[Serializable]
public class ClosestCharacterFromPlayer : IFilter
{
	public void Filtered(List<Character> characters)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		Character player = Singleton<Service>.Instance.levelManager.player;
		int index = 0;
		float num = 2.1474836E+09f;
		for (int i = 0; i < characters.Count; i++)
		{
			float num2 = Mathf.Abs(((Component)player).transform.position.x - ((Component)characters[i]).transform.position.x);
			if (num2 < num)
			{
				num = num2;
				index = i;
			}
		}
		Character item = characters[index];
		characters.Clear();
		characters.Add(item);
	}
}
