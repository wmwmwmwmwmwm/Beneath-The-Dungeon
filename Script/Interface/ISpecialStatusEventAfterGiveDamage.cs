using System.Collections;

public interface ISpecialStatusEventAfterGiveDamage
{
	IEnumerator AfterGiveDamageEffect(bool IsDamageGiver, IntRef Damage);
}
