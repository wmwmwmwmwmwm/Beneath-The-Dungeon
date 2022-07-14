using System.Collections;
using UnityEngine;

public class Special17Empower : MonoBehaviour, ISpecialStatusEventAttackDamage
{
	public void AttackDamageEffect(IntRef Damage, ElementalTypeEnum ElementalType)
	{
		Damage.Value += Damage.Value / 2;
	}
}
