using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace _3rdParty
{
    public class TopAdsBehaviour : MonoBehaviour
    {
        public static TopAdsBehaviour _instance;

        public GameObject ad;
        public Button closeAdButton; 
        

        private void Awake()
        {
            _instance = this;

            closeAdButton.onClick.AddListener(CloseAdClick); 
        }

        public static void InvokeAfter(Action methodToCall, float duration)
        {
            _instance.StartCoroutine(_instance.InvokeAfterCoroutine(methodToCall, duration));
        }
        private IEnumerator InvokeAfterCoroutine(Action methodToCall, float duration)
        {
            yield return new WaitForSeconds(duration);
            methodToCall();
        }

        public void ShowAd()
        {
            ad.SetActive(true);
        }

        private void CloseAdClick()
        {
            ad.SetActive(false); 
        }

    }
}