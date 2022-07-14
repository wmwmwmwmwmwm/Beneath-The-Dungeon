using System.Collections;
using UnityEngine;

public class Skill116Reassemble : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		sd.UserBattleStatus.SetValues(sd.UserStatus);
		yield break;
    }
}
