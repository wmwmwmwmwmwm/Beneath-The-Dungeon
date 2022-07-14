using System.Collections;
using UnityEngine;

public class Special18Petrifaction : MonoBehaviour, ISpecialStatusEventAttackDamage
{
	public void AttackDamageEffect(IntRef Damage, ElementalTypeEnum ElementalType)
	{
		Damage.Value /= 2;
	}
}
