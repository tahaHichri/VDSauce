using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * This Script will hold our simple demo events
 */
public class MainCanvasScript : MonoBehaviour
{
    /**
     * UI
     */ 
    // when the user wishes to update his GDPR consent, the button will show the GDPR consent popup
    public Button privacyButton;

    /**
     * GDPR privacy consent Popup
     */
    private const string ADS_PERSONALIZATION_CONSENT = "Ads";



    /**
     * Lifecycle and Callbacks
     */ 
    // Start is called before the first frame update
    void Start()
    {
        // Show Android Native Toast if we are on an Android device
        #if UNITY_ANDROID
              _ShowNativeAndroidToast("Hello and Welcome to VoodooSauce demo");
        #endif

    }

    // Update is called once per frame
    void Update()
    {

    }



    public void handleStartGame()
    {
        /**
         *  Async show consent popup, get response, and start Game
         */
        StartCoroutine(AskForConsentAndInit());
    }

    public void handleEndGame()
    {
        // Async show and wait for privacy popup
        VoodooSauce.EndGame();
    }


    public void handleShowAd()
    {
        // Async show and wait for privacy popup
        VoodooSauce.ShowAd();
    }




    /**
     * Async show GDPR consent and wait for user response
     * We are using UnitySimpleGDPRConsent open source package for faster display
     * @see: https://github.com/yasirkula/UnitySimpleGDPRConsent
     */
    private IEnumerator AskForConsentAndInit()
    {
        // Show a consent dialog with two sections (and wait for the dialog to be closed):
        yield return SimpleGDPR.WaitForDialog(new GDPRConsentDialog().
            AddSectionWithToggle(ADS_PERSONALIZATION_CONSENT, "Ads Personalization", "When enabled, you'll see ads that are more relevant to you. Otherwise, you will still receive ads, but they will no longer be tailored toward you.").
            AddPrivacyPolicies("https://policies.google.com/privacy", "https://unity3d.com/legal/privacy-policy", "https://my.policy.url"));

        // Check if user has granted the Ads Personalization permission before closing the dialog
        if (SimpleGDPR.GetConsentState(ADS_PERSONALIZATION_CONSENT) == SimpleGDPR.ConsentState.Yes)
        {
            // User accepted
            VoodooSauce.Initialize("f4280fh0318rf0h2", true);

            VoodooSauce.StartGame();

        }
        else
        {
            // User denied
            VoodooSauce.Initialize("f4280fh0318rf0h2", false);

            VoodooSauce.StartGame();
    }


}



    /**
     * Native Android Toast display
     * Show messages natively by making use of the Toast class
     * @param: The message we need to display
     */
    private void _ShowNativeAndroidToast(string message)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        if (unityActivity != null)
        {
            // Prepare the Toast class by package name
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");

            // Like manipulating views, we need to make and show the toast on the UI Thread.
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                // Call the Toast.maketText and then show
                // we are passing one, since we need the duration to be "long"
                AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity,
                    message, 1);
                toastObject.Call("show");
            }));
        }
    }
}





