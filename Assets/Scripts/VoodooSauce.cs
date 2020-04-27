using System;
using System.Collections;
using _3rdParty;
using UnityEngine;

/* Introduction
 *
 * VoodooSauce is a Unity SDK that we implement into all of our games here at Voodoo.  This SDK is responsible for
 * providing Ads, Analytics, IAP, GDPR, etc. functionality in an easy to use package for internal and external studios
 * to integrate into their games. The SDK is used around the world by more than 200+ games, thus reliability and ease of use is
 * incredibly important for us. 
 *
 * For this exercise, we would like you to create a basic VoodooSauce that integrates the fake "TopAds" and "TopAnalytics"
 * SDKs.
 *
 * At the end we ask that you answer some quick questions at the bottom of this file. 
 * 
 */

/* Instructions 
 *
 * Please fill out the method implementations below 
 * Feel free to create additional classes to help with your implementation 
 * Please do not spend more than 2.5 hours on the code implementation portion of this exercise
 * Please do not modify the code in the 3rdParty folder
 * Make sure to read this entire file before starting to code.  We include important instructions on how to use the TopAds and TopAnalytics SDKs
 * 
 */

// Bonus Question : Show an android Toast when you launch the app.


public static class VoodooSauce
{

    // Developers will have to provide this when they initialize our SDK
    // using the initialize method
    // A flexible way to allow studios to provide their ad unit id to your VoodooSauce SDK
    private static string appAdUnitID;

    // will help us check with every method that the user has initialized the SDK
    private static bool isSDKinitialized = false;

    // holds the privacy user choice, can be changed with Grant and Revoke Methods
    private static bool ConsentGiven = false;


    // this will be updated based on the events from TopAds to know when we should keep requesting
    // new ads, it will be turned to true when we receive "OnAdLoadedEvent" callback
    private static bool adLoaded = false;

    // this is used when the user clicks show but no ad loaded, so we request a new one
    // and display as soon as possible
    private static bool displayAdAsSoonAsLoadedNextTime = false;


    /***
     * Prevent overloading the SDK with calls
     */
    // config by user
    private static int throttleSecondsBetweenAds=0;
    private static int throttleGamesBetweenAds=0;


    // Records and counts to compare
    private static long secondsSinceLastAd = 0;
    private static long gamesSinceLastAd = 0;



    /**
     * A flexible way to allow studios to provide their ad unit id to your VoodooSauce SDK
     * 
     * This method initializes the SDK, it is required before using it to remind the developer
     * That they should provide their app id and the consent
     * 
     * @param: adUnitID: a string. e.g.: "f4280fh0318rf0h2"
     * @param: privacyConsent: a boolean.
     */
    public static void Initialize(string adUnitID, bool privacyConsent)
    {

        // init our needed vars
        appAdUnitID = adUnitID;
        isSDKinitialized = true;
        ConsentGiven = privacyConsent;
    }



    // TODO Before calling methods in TopAds and TopAnalytics you must call their init methods 
    // TopAds requires the TopAds prefab to be created in the scene
    // TODO You also need to collect user GDPR consent and pass that boolean value to TopAds and TopAnalytics 
    // You can collect this consent by displaying a popup to the user at the start of the game and then storing that value for future use 


    /**
	 * @requires calling the Initialize method first
	 */
    public static void StartGame()
    {
        /**
         * GDPR CONSENT
         * We Asynchronously Collect this consent by displaying a popup to the user
		 * we pass the consent to the SDK in the initialization Method alongslide the app ID
		 * @see An example at [MainCanvasScript.cs]
         */

        // use the ConsentGiven to init TopAnalytics
        TopAnalytics.InitWithConsent(ConsentGiven);

        // init TopAds
        TopAds.InitializeSDK();


        // Grant/Revoke TopAds based on consentGiven value
        if (ConsentGiven)
        {
            TopAds.GrantConsent();
        }
        else
        {
            TopAds.RevokeConsent();
        }

        // Track in TopAnalytics that a game has started
        TopAnalytics.TrackEvent("Game_started");


        // Delegate TopAds events to listeners to ensure that we always have an ad ready :)
        TopAds.OnAdLoadedEvent += OnAdLoadedEvent;
        TopAds.OnAdFailedEvent += OnAdFailedEvent;
        TopAds.OnAdShownEvent += OnAdShownEvent;


        // we request an ad to prepare for the ShowAd Event :)
        TopAds.RequestAd(appAdUnitID);


        gamesSinceLastAd++;
    }

    public static void EndGame()
    {
        // Track in TopAnalytics that a game has ended
        TopAnalytics.TrackEvent("Game_ended");
    }

    public static void ShowAd()
    {


        Debug.Log(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        Debug.Log(secondsSinceLastAd);

        Debug.Log("Differentce "+ (DateTimeOffset.UtcNow.ToUnixTimeSeconds() - secondsSinceLastAd));

        // check limit
        if (throttleGamesBetweenAds <= gamesSinceLastAd ||
            (DateTimeOffset.UtcNow.ToUnixTimeSeconds() - secondsSinceLastAd) >= throttleSecondsBetweenAds)
        {
            // if an ad is available, display it!
            if (adLoaded)
            {
                TopAds.ShowAd(appAdUnitID);
            }
            else
            {
                // Request a new one and display when ready :)
                TopAds.RequestAd(appAdUnitID);

                // display as soon as an ad is ready
                displayAdAsSoonAsLoadedNextTime = true;
            }

            adLoaded = false;
        }
    }



    /*
     * ************************************************
     *
     * TopAds Action events
     * Ad readiness/display events
     *
     * An autorequest system that ensures an ad is always ready to be displayed
     *
     * ************************************************
     */

    /**
     * this is a delegate of TopAds.OnAdShownEvent
     * Track in TopAnalytics when an ad is displayed.
     */
    private static void OnAdShownEvent()
    {
        // Track in TopAnalytics when an ad is displayed.
        TopAnalytics.TrackEvent("ad_displayed");

        // since ad is displayed, we turn this to false and to allow requesting a new one
        adLoaded = false;

        // Prepare for the next ad
        TopAds.RequestAd(appAdUnitID);


        Debug.Log("Ad shown, requesting for next time");


        gamesSinceLastAd = 0;
        // The number of seconds that have elapsed since 1970-01-01T00:00:00Z.
        // needed to compare later
        secondsSinceLastAd = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }


    /**
     * this is a delegate of TopAds.OnAdLoadedEvent to keep VoodooSauce aware when an ad is available
     * or it should keep trying
	 */
    private static void OnAdLoadedEvent()
    {
        // stop retrying and display with click on show
        adLoaded = true;
        Debug.Log("An Ad is available");


        // did the user click show and we don't have an ad ready?
        if (displayAdAsSoonAsLoadedNextTime)
        {
            displayAdAsSoonAsLoadedNextTime = false;
            TopAds.ShowAd(appAdUnitID);
        }
    }


    /**
     * this is a delegate of TopAds.OnAdFailedEvent to keep VoodooSauce aware when an ad is still
	 * not available, which means we should keep trying
	 * Auto requesting until OnAdLoadedEvent is called
     */
    private static void OnAdFailedEvent()
    {
        // if we already have an ad ready, we don't try with a new one :)
        if (adLoaded) return;

        // otherwise, we send another request
        TopAds.RequestAd(appAdUnitID);

        Debug.Log("An Ad is not available yet");
    }






    public static void SetAdDisplayConditions(int secondsBetweenAds, int gamesBetweenAds)
    {
        // Sometimes studios call "ShowAd" too often and bombard players with ads 
        // Add a system that prevents the "ShowAd" method from showing an available ad 
        // Unless EITHER condition provided is true:

        throttleSecondsBetweenAds = secondsBetweenAds;
        throttleGamesBetweenAds = gamesBetweenAds;
    }


    // === Please answer these quick questions within the code file ===  

    // In the VoodooSauce we integrate many 3rd party SDKs of varying reliability that display Ads, Analytics, etc.
    // What processes would you suggest to ensure that the VoodooSauce SDK is minimally affected by crashes 
    // in another SDK?
    /***
     * As much as this depends on how deep is our interactions with the other SDKs
     * We can ensure that we are prepared for unexpected returns from these SDKs such as erronous/absent values.
     * We need to implement alternative fallbacks for these scenarios (a fallback logic, execution)
     * When we have checks for on our interaction with these SDKs, we minimize the risk of affecting the rest of our logic.
     *
     * That is why implementing tests can be critical. They make our code pass through many scenarios
     */ 


    // What are some pitfalls/shortcomings in your above implementation?
    /**
     * I would have preferred to better improve/optimize my code performance-wise and portability wise.
     * This is a simple project that we can make more flexible by exposing more configuration calls,
     * and cleanups attached to the app's lifecycle.
     */ 

    // How would you improve your implementation if you had more than 2 hours?
    /**
     *
     * I would have exposed more config/status change calls suchs as Consent status change, Memory cleanup
     * And more optimization.
     *
     * I would have followed a TDD approach.
     * 
     */ 

    // What do you enjoy the most about being a developer?
    /**
     * Learning new things, discovering new technologies anywhere, anytime.
     * It is amazing how we can write a piece of code that can instantly help someone on the other side of world in their work, or daily life.
     * Sometimes I receive emails for my projects of people telling me that I helped them in some way
     * by making that project. That is the best feeling in the world!
     */ 

    // What do you enjoy the least about being a developer?
    /**
     * We get sucked up on our chairs for too long which cannot be so healthy in the long run. Also, badly written code!
     * 
     */ 

    // Why do you want to work on the VoodooSauce SDK vs. creating games in Unity?
    /**
     * I am more of a utilities guy, that is why I contribute to open source. Because I don't want to just
     * use technologies, I want to also be part of creating things used by other developers, That is self-fulfilling
     * And it gives you the feeling that you are making bigger impact.
     */ 



}
