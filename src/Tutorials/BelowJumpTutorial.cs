using System.Collections;
using UnityEngine;

namespace Tutorials;

public class BelowJumpTutorial : Tutorial
{
	public override void Activate()
	{
		_state = State.Progress;
		((MonoBehaviour)this).StartCoroutine(Process());
	}

	public override void Deactivate()
	{
		base.state = State.Done;
	}

	protected override IEnumerator Process()
	{
		yield return Converse(0f);
	}
}
