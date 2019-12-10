using System;
using UnityEngine;

namespace _3rdParty
{
    public static class TopAnalytics
    {
        private static bool _isInitialized;
        private static bool? _consent; 

        public static void InitWithConsent(bool consent)
        {
            _isInitialized = true; 
            _consent = consent; 
        }

        public static void TrackEvent(string eventName)
        {
            if (!_consent.HasValue)
            {
                throw new Exception("TopAnalytics no consent value given"); 
            }
            
            if (_isInitialized)
            {
                Debug.Log("MoroAnalytics tracking: " + eventName);     
            }
        }
    }
}