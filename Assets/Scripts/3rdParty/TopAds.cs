using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _3rdParty
{
    public static class TopAds
    {
        public static event Action OnAdLoadedEvent;
        public static event Action OnAdFailedEvent;
        public static event Action OnAdShownEvent;

        private static bool? isConsent; 
        private static bool isInitialized;
        private static bool isAdLoaded;

        private static string recognizedAdUnitId = "f4280fh0318rf0h2"; 
        
        public static void GrantConsent()
        {
            isConsent = true; 
        }

        public static void RevokeConsent()
        {
            isConsent = false; 
        }

        public static void InitializeSDK()
        {
            isInitialized = true;

            if (TopAdsBehaviour._instance == null)
            {
                throw new Exception("You must include TopAdsBehaviour in your scene"); 
            }
        }

        public static void RequestAd(string adUnitId)
        {
            if (!isConsent.HasValue)
            {
                throw new Exception("TopAds - No user consent set!"); 
            }

            if (!adUnitId.Equals(recognizedAdUnitId))
            {
                throw new Exception("TopAds - Unrecognized ad unit"); 
            }

            if (isInitialized)
            {
                TopAdsBehaviour.InvokeAfter(AdLoaded, Random.Range(0, 10)); 
            }
        }

        private static void AdLoaded()
        {
            if (Random.Range(0, 10) > 2)
            {
                isAdLoaded = true;
                OnAdLoadedEvent?.Invoke();    
            }
            else
            {
                isAdLoaded = false;
                OnAdFailedEvent?.Invoke(); 
            }
        }

        public static void ShowAd(string adUnitId)
        {
            if (!adUnitId.Equals(recognizedAdUnitId))
            {
                throw new Exception("TopAds - Unrecognized ad unit"); 
            }

            if (isInitialized && isAdLoaded)
            {
                TopAdsBehaviour._instance.ShowAd();
                OnAdShownEvent?.Invoke(); 
                isAdLoaded = false; 
            }
        }
    }
}