using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game.UI
{
    public class GameplayUI : MonoBehaviour
    {
        [SerializeField]private UIManager uiManager;
        [SerializeField]private Page gameplayPage;
        [SerializeField]private Page pausePage;
        [SerializeField]private Page gameOverPage;

        private void LoadPageBasedOnState(GameState gameState)
        {
            //Debug.Log("Loading Page");
            switch(gameState)
            {
                case GameState.UnPaused:
                                        if(uiManager.PageCount == 0)
                                        {
                                            uiManager.PushPage(gameplayPage);
                                        }
                                        else if(uiManager.IsPageOnTopOfStack(pausePage))
                                        {
                                            uiManager.PopPage();
                                        }
                                        break;
                case GameState.Paused:
                                        if(uiManager.IsPageInStack(gameplayPage))
                                        {
                                            uiManager.PushPage(pausePage);
                                        }
                                        
                                        break;
                case GameState.GameOver:
                                        uiManager.PushPage(gameOverPage);
                                        break;
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            if(GameStateManager.Instance != null)
            {
                GameStateManager.Instance.OnGameStateChanged.AddListener(LoadPageBasedOnState);
            }
            else
            {
                //Debug.LogError("Game State Manager is null!");
            }                        
        }
    }
}
