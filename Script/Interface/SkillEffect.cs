using System.Collections;
using UnityEngine;

public abstract class SkillEffect : MonoBehaviour
{
	public SkillData sd => GetComponent<SkillData>();
	public abstract IEnumerator ActivateEffect();
}
