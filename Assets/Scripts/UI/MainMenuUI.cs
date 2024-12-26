using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Core;
using Game.SoundManagement;
using UnityEngine.Audio;
namespace Game.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField]private UIManager uiManager;
        [SerializeField]private Page firstPage;
        [SerializeField]private Page optionsPage;
        [SerializeField]private Page creditsPage;
        [SerializeField]private Page loadingPage;
        [SerializeField]private BarsUI loadingBar;
        [SerializeField]private SquashStretchButton playButton;
        [SerializeField]private SquashStretchButton optionsButton;
        [SerializeField]private SquashStretchButton creditsButton;
        [SerializeField]private SquashStretchButton[] goBackToFirstPageButtons;
        [SerializeField]private SquashStretchButton quitButton;
        [SerializeField]private Slider musicVolSlider;
        [SerializeField]private Slider sfxVolSlider;
        [SerializeField]private SoundData sfxTestSoundData;
        [SerializeField]private SoundData mainMenuMusic;

        private ApplicationQuit applicationQuit;
        private SceneLoader sceneLoader;

        private void GoBackToMain()
        {
            uiManager.PopPage();
        }
        private void Setup()
        {
            applicationQuit = FindObjectOfType<ApplicationQuit>();
            sceneLoader = GetComponent<SceneLoader>();
            if(firstPage != null)
            {
                firstPage.gameObject.SetActive(false);
            }
            if(optionsPage != null)
            {
                optionsPage.gameObject.SetActive(false);
            }
            if(creditsPage != null)
            {
                creditsPage.gameObject.SetActive(false);
            }
            if(loadingPage != null)
            {
                loadingBar.gameObject.SetActive(false);
            }
            

            //Logic for play button, first page, loading page and loading bar.
            sceneLoader.OnSceneLoading.AddListener(loadingBar.SetAmount);
            playButton.AddListener(()=> uiManager.PushPage(loadingPage));
            loadingPage.OnPostPushAction.AddListener(()=> sceneLoader.LoadScene(Constants.LEVEL_SCENE_NAME));

            //Logic for options button
            optionsButton.AddListener(()=> uiManager.PushPage(optionsPage));

            //Logic for credits button
            creditsButton.AddListener(()=> uiManager.PushPage(creditsPage));

            //Logic for go back to first page buttons
            foreach(var button in goBackToFirstPageButtons)
            {
                button.AddListener(GoBackToMain);
            }
            
            //Logic for Quit Button
            quitButton.AddListener(()=> applicationQuit.QuitApplication());
            //Logic for setting up music and sfx volume sliders
            
            musicVolSlider.value = SoundManager.Instance.GetMusicVolume();
            sfxVolSlider.value = SoundManager.Instance.GetSFXVolume();

            musicVolSlider.onValueChanged.AddListener(SoundManager.Instance.SetMusicVolume);
            sfxVolSlider.onValueChanged.AddListener(SoundManager.Instance.SetSFXVolume);
            sfxVolSlider.onValueChanged.AddListener((value) => SoundManager.Instance.Play(sfxTestSoundData, Vector3.zero, true));

            uiManager.PushPage(firstPage);
            SoundManager.Instance.Play(mainMenuMusic, Vector3.zero);
        }
        // Start is called before the first frame update
        void Start()
        {
            Setup();
        }
    }
}
