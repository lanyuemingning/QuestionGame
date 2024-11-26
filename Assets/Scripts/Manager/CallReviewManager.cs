using AnyThinkAds.Api;
using GameDefine;
using Google.Play.Review;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Security.Cryptography;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

[MonoSingletonPath("[Review]/CallReviewManager")]
public class CallReviewManager: MonoSingleton<CallReviewManager>, ICanGetUtility, ICanSendEvent
{
    ReviewManager _reviewManager = new ReviewManager();
    PlayReviewInfo _playReviewInfo;

    private void Start()
    {
    }
    
    public IEnumerator StartReview()
    {
        if(this.GetUtility<SaveDataUtility>().GetReviewTip() == 0)
        {
            var requestFlowOperation = _reviewManager.RequestReviewFlow();
            yield return requestFlowOperation;
            if (requestFlowOperation.Error != ReviewErrorCode.NoError)
            {
                // Log error. For example, using requestFlowOperation.Error.ToString().
                yield break;
            }
            Debug.Log("requestFlowOperation");

            _playReviewInfo = requestFlowOperation.GetResult();

            var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
            yield return launchFlowOperation;
            _playReviewInfo = null; // Reset the object
            if (launchFlowOperation.Error != ReviewErrorCode.NoError)
            {
                // Log error. For example, using requestFlowOperation.Error.ToString().
                yield break;
            }
            Debug.Log("launchFlowOperation");
            this.GetUtility<SaveDataUtility>().SetReviewTip(1);
            // The flow has finished. The API does not indicate whether the user
            // reviewed or not, or even whether the review dialog was shown. Thus, no
            // matter the result, we continue our app flow.
        }

    }

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

}
