using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Monetization;
//using GoogleMobileAds.Api;

public class AdManager : Singleton<AdManager>
{
//	string AndroidGameID = "1767349"; // test
//    string IOSGameID = "2085042"; // test
//    bool TestMode = false;
//	string PlacementID = "video";

//	string AdmobAppID = "ca-app-pub-5182776098416264~2004111651";
//	string AdmobAdID = "unexpected_platform";
//	InterstitialAd AdmobAd;

//	private void Start()
//	{
//		// 유니티 ads 초기화
//#if UNITY_EDITOR
//        TestMode = true;
//#endif
//#if UNITY_IOS
//        Monetization.Initialize(IOSGameID, TestMode);
//#else
//        Monetization.Initialize(AndroidGameID, TestMode);
//#endif
//		// 구글 애드몹 초기화
//#if UNITY_EDITOR
//		AdmobAdID = "ca-app-pub-3940256099942544/8691691433";
//#elif UNITY_ANDROID
//		AdmobAdID = "ca-app-pub-5182776098416264/3459172865";
//#elif UNITY_IOS
//		AdmobAdID = "ca-app-pub-5182776098416264/6655377884";
//#endif
//		MobileAds.Initialize(AdmobAppID);
//		AdmobAd = new InterstitialAd(AdmobAdID);
//		AdmobAd.LoadAd(new AdRequest.Builder().Build());
//		AdmobAd.OnAdClosed += AdmobAdFinished;
//    }

//    Action AdCompleteCallback, AdFailedCallback;
//	public void ShowAd(Action CompleteCallback, Action FailedCallback)
//	{
//        AdCompleteCallback = CompleteCallback;
//        AdFailedCallback = FailedCallback;
//        StartCoroutine(ShowAdWhenReady());
//	}

//	IEnumerator ShowAdWhenReady()
//	{
//		if(UnityEngine.Random.value < 0.5f)
//		{
//			for (int i = 0; i < 10; i++)
//			{
//				if (!Monetization.IsReady("video"))
//				{
//					yield return new WaitForSeconds(0.5f);
//				}
//			}
//			ShowAdPlacementContent Ad = Monetization.GetPlacementContent(PlacementID) as ShowAdPlacementContent;
//			if (Ad != null)
//			{
//				Ad.Show(UnityAdFinished);
//			}
//		}
//		else
//		{
//			for (int i = 0; i < 10; i++)
//			{
//				if (!AdmobAd.IsLoaded())
//				{
//					yield return new WaitForSeconds(0.5f);
//				}
//			}
//			AdmobAd.Show();
//		}
//	}

//	void AdmobAdFinished(object sender, EventArgs args)
//	{
//		AdmobAd.Destroy();
//		AdmobAd = new InterstitialAd(AdmobAdID);
//		AdmobAd.LoadAd(new AdRequest.Builder().Build());
//		AdmobAd.OnAdClosed += AdmobAdFinished;
//		AdCompleteCallback?.Invoke();
//		AdCompleteCallback = null;
//	}

//    void UnityAdFinished(ShowResult Result)
//    {
//        switch (Result)
//        {
//            case ShowResult.Failed:
//                AdFailedCallback?.Invoke();
//                break;
//            case ShowResult.Skipped:
//            case ShowResult.Finished:
//                AdCompleteCallback?.Invoke();
//                break;
//        }
//        AdCompleteCallback = null;
//        AdFailedCallback = null;
//    }
}
