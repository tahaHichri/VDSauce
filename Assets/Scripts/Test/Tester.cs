using System;
using UnityEngine;
using UnityEngine.UI;


namespace Test
{
    public class Tester : MonoBehaviour
    {
        public Button showAdButton;

        public Button startGameButton;

        public Button endGameButton; 

        private void Awake()
        {
            showAdButton.onClick.AddListener(ShowAdClick);
            startGameButton.onClick.AddListener(StartGameClick);
            endGameButton.onClick.AddListener(EndGameClick);
            
            VoodooSauce.SetAdDisplayConditions(60, 3); 
        }

        private void ShowAdClick()
        {
            VoodooSauce.ShowAd(); 
        }

        private void StartGameClick()
        {
            VoodooSauce.StartGame(); 
        }

        private void EndGameClick()
        {
            VoodooSauce.EndGame(); 
        }
    }
}