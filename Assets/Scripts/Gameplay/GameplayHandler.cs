using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Game.Core;
using Game.SoundManagement;
using Game.UI;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Game.Gameplay
{
    public class GameplayHandler : MonoBehaviour
    {
        [Header("UI Settings: ")]
        [SerializeField]private UIManager uiManager;
        [SerializeField]private Page gameplayPage;
        [SerializeField]private Page pausePage;
        [SerializeField]private Page gameOverPage;
        [SerializeField]private Page gamePassedPage;
        [SerializeField]private Page loadingPage;
        [SerializeField]private SquashStretchButton resumeButton;
        [SerializeField]private SquashStretchButton[] restartLevelButtons;
        [SerializeField]private SquashStretchButton[] goBackToMainButtons;
        
        [Header("Sound Settings: ")]
        [SerializeField]private SoundData inGameMusicData;
        [SerializeField]private SoundData gameOverMusicData;
        [SerializeField]private SoundData gamePassedMusicData;
        [Header("Logic Settings: ")]
        [SerializeField]private SceneLoader sceneLoader;
        [SerializeField]private BarsUI loadingBar;
        [SerializeField]private TMP_Text gameplayTimerText;
        [SerializeField]private TMP_Text gamePassedTimerText;
        [SerializeField]private TMP_Text gameOverTimerText;
        [SerializeField]private TMP_Text giftGivenText;
        [Min(1)]
        [SerializeField]private int maxGiftsToGive = 10;
        [SerializeField]private GameInput gameInput;
        [SerializeField]private GameObject gamePassedRegion;

        Stopwatch stopwatch;
        private int currentGiftsGivenCount = 0;

        private bool shouldStartTimer = false;

        public void IncreaseGiftGivenCounter()
        {
            currentGiftsGivenCount++;
            giftGivenText.text = currentGiftsGivenCount + "/" + maxGiftsToGive + " Presents Delivered";
            if(!gamePassedRegion.activeInHierarchy && currentGiftsGivenCount >= maxGiftsToGive)
            {
                gamePassedRegion.SetActive(true);
            }
        }
        private string GetTime()
        {
            return stopwatch.Elapsed.Hours + ":" + stopwatch.Elapsed.Minutes + ":" + stopwatch.Elapsed.Seconds;
        }
        

        private void HandleInGameBehaviour(GameState gameState)
        {
            
            switch(gameState)
            {
                case GameState.UnPaused:
                                        if(uiManager.PageCount == 0)
                                        {
                                            SoundManager.Instance.Play(inGameMusicData, Vector3.zero);
                                            uiManager.PushPage(gameplayPage);
                                        }
                                        else if(uiManager.IsPageOnTopOfStack(pausePage))
                                        {
                                            uiManager.PopPage();
                                            stopwatch.Start();
                                        }
                                        
                                        break;
                case GameState.Paused:
                                        uiManager.PushPage(pausePage);
                                        stopwatch.Stop();
                                        break;
                case GameState.GamePassed:
                                        PlayGamePassedSound();
                                        stopwatch.Stop();
                                        gamePassedTimerText.text = GetTime();
                                        uiManager.PushPage(gamePassedPage);
                                        
                                        break;
                case GameState.GameOver:
                                        PlayGameOverSound();
                                        stopwatch.Stop();
                                        gameOverTimerText.text = GetTime();
                                        uiManager.PushPage(gameOverPage);
                                        break;
            }
        }

        private void PlayGameOverSound()
        {
            SoundManager.Instance.Stop(inGameMusicData);
            SoundManager.Instance.Play(gameOverMusicData, Vector3.zero);
        }

        private void PlayGamePassedSound()
        {
            SoundManager.Instance.Stop(inGameMusicData);
            SoundManager.Instance.Play(gamePassedMusicData, Vector3.zero);
        }

        private void RestartLevel()
        {
            loadingPage.OnPostPushAction.RemoveAllListeners();
            loadingPage.OnPostPopAction.AddListener(()=> sceneLoader.LoadScene(Constants.LEVEL_SCENE_NAME));
            uiManager.PushPage(loadingPage);
        }

        private void GoBackToMain()
        {
            loadingPage.OnPostPushAction.RemoveAllListeners();
            loadingPage.OnPostPopAction.AddListener(()=> sceneLoader.LoadScene(Constants.MAIN_MENU_SCENE_NAME));
        }

        private void Setup()
        {
            if(gameplayPage != null)
            {
                gameplayPage.gameObject.SetActive(false);
            }

            if(pausePage != null)
            {
                pausePage.gameObject.SetActive(false);
            }

            if(gameOverPage != null)
            {
                gameOverPage.gameObject.SetActive(false);
            }

            if(gamePassedPage != null)
            {
                gamePassedPage.gameObject.SetActive(false);
            }

            if(loadingPage != null)
            {
                loadingPage.gameObject.SetActive(false);
            }

            sceneLoader.OnSceneLoading.AddListener(loadingBar.SetAmount);

            //Logic for resume button.
            resumeButton.AddListener(GameStateManager.Instance.ResumeGame);

            //Logic for restart level buttons 
            foreach(var button in restartLevelButtons)
            {
                button.AddListener(RestartLevel);
            }

            //Logic for go back to main buttons
            foreach(var button in goBackToMainButtons)
            {
                button.AddListener(GoBackToMain);
            }

            currentGiftsGivenCount = 0;
            giftGivenText.text = currentGiftsGivenCount + "/" + maxGiftsToGive;
            gamePassedRegion.SetActive(false);
            GameStateManager.Instance.OnGameStateChanged.AddListener(HandleInGameBehaviour);
        }

        private void Awake() {
            stopwatch = new Stopwatch();
        }
        // Start is called before the first frame update
        void Start()
        {
            Setup();
        }

        // Update is called once per frame
        void Update()
        {
            if(shouldStartTimer == false && gameInput.GetMovementInput().sqrMagnitude > 0.0f)
            {
                stopwatch.Reset();
                stopwatch.Start();
                shouldStartTimer = true;
            }

            if(gameplayTimerText != null)
            {
                gameplayTimerText.text = GetTime();
            }
        }
    }
}
