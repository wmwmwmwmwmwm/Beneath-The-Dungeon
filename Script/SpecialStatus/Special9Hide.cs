using UnityEngine;

public class Special9Hide : MonoBehaviour, ISpecialStatusEventReceiveDamage
{
	public void ReceiveDamageEffect(IntRef Damage)
	{
		Damage.Value = (int)(Damage.Value * 0.5f);
	}
}
