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
 * Please do not spend more than 2 hours on the code implementation portion of this exercise
 * Please do not modify the code in the 3rdParty folder
 * Make sure to read this entire file before starting to code.  We include important instructions on how to use the TopAds and TopAnalytics SDKs
 * 
 */

 // Bonus Question : Show an android Toast when you launch the app.


public static class VoodooSauce{
	
	// Before calling methods in TopAds and TopAnalytics you must call their init methods 
	// TopAds requires the TopAds prefab to be created in the scene
	// You also need to collect user GDPR consent and pass that boolean value to TopAds and TopAnalytics 
	// You can collect this consent by displaying a popup to the user at the start of the game and then storing that value for future use 
	
	public static void StartGame()
	{
		// Track in TopAnalytics that a game has started 
	}

	public static void EndGame()
	{
		// Track in TopAnalytics that a game has ended 
	}
	
	public static void ShowAd()
	{
		// TopAds methods must be called with a unique "string" ad unit id 
		// For your test app that id is "f4280fh0318rf0h2" 
		// However, when releasing the SDK to other studios, their ad unit id will be different 
		// Please find a non code way to allow studios to provide their ad unit id to your VoodooSauce SDK 
		
		
		// Before an ad is available to display, you must call TopAds.RequestAd 
		

		// Track in TopAnalytics when an ad is displayed 
	}

	public static void SetAdDisplayConditions(int secondsBetweenAds, int gamesBetweenAds)
	{
		// Add a system that shows ads based on satisfying EITHER conditions provided 
		// secondsBetweenAds: only show an ad if the previous ad was shown more than "secondBetweenAds" ago 
		// gamesBetweenAds: only show an ad if "gamesBetweenAds" amount of games was played since the previous ad 
	}
	
	
	// === Please answer these quick questions within the code file ===  
	
	// In the VoodooSauce we integrate many 3rd party SDKs of varying reliability that display Ads, Analytics, etc.
	// What processes would you suggest to ensure that the VoodooSauce SDK is minimally affected by crashes 
	// in another SDK?
	
	// What are some pitfalls/shortcomings in your above implementation?
	
	// How would you improve your implementation if you had more than 2 hours? 
	
	// What do you enjoy the most about being a developer? 
	
	// What do you enjoy the least about being a developer? 
	
	// Why do you want to work on the VoodooSauce SDK vs. creating games in Unity?
	
	
	
}
