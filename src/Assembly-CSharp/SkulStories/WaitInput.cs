using System.Collections;
using InControl;
using UserInput;

namespace SkulStories;

public class WaitInput : Sequence
{
	public override IEnumerator CRun()
	{
		_narration.skippable = false;
		if (!_narration.skipped)
		{
			yield return CWaitInput();
		}
		_narration.skipped = false;
	}

	private IEnumerator CWaitInput()
	{
		do
		{
			yield return null;
			_narration.skippable = false;
		}
		while (!((OneAxisInputControl)KeyMapper.Map.Attack).WasPressed && !((OneAxisInputControl)KeyMapper.Map.Jump).WasPressed && !((OneAxisInputControl)KeyMapper.Map.Submit).WasPressed);
		_narration.textVisible = false;
	}
}
