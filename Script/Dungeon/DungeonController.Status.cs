using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DungeonController
{
	public SpecialStatusData AddSpecialStatus(UserTypeEnum UserType, CommonStatus UserStatus, StatGaugeGroup GaugeGroup, string SpecialStatusID, int Duration, bool CallStartEffect)
	{
		SpecialStatusData NewSpecialStatus = Instantiate(DBManager.Instance.SpecialStatusDictionary[SpecialStatusID]);
		NewSpecialStatus.UserType = UserType;
		if (Duration < 0) NewSpecialStatus.IsPermanent = true;
		else NewSpecialStatus.Duration = Duration;
		NewSpecialStatus.IconObject = Instantiate(GaugeGroup.SpecialStatusIconPrefab, GaugeGroup.SpecialStatusIconParent, false).GetComponent<SpecialStatusIcon>();
		NewSpecialStatus.IconObject.gameObject.SetActive(true);
		NewSpecialStatus.IconObject.SpecialStatus = NewSpecialStatus;
		NewSpecialStatus.IconObject.IconImage.sprite = NewSpecialStatus.IconSprite;
		NewSpecialStatus.IconObject.SetDurationText(Duration);
		UserStatus.SpecialStatuses.Add(NewSpecialStatus.gameObject);
		if (CallStartEffect && NewSpecialStatus.GetComponent<ISpecialStatusEventStart>() != null)
		{
			StartCoroutine(NewSpecialStatus.GetComponent<ISpecialStatusEventStart>().StartEffect());
		}
		return NewSpecialStatus;
	}

	public void RemoveSpecialStatus(CommonStatus UserStatus, SpecialStatusData SpecialStatus, bool CallEndEffect = true)
	{
		if (CallEndEffect && SpecialStatus.GetComponent<ISpecialStatusEventEnd>() != null)
		{
			StartCoroutine(SpecialStatus.GetComponent<ISpecialStatusEventEnd>().EndEffect());
		}
		UserStatus.SpecialStatuses.Remove(SpecialStatus.gameObject);
		Destroy(SpecialStatus.IconObject.gameObject);
		Destroy(SpecialStatus.gameObject);
	}
}
