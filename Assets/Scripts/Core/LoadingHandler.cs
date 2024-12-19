using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class LoadingHandler : MonoBehaviour
    {
        
        private SceneLoader sceneLoader;
        private string finalSceneName = "Main Menu Scene";

        public void SetFinalSceneName(string name)
        {
            finalSceneName = name;
        }

        public void LoadFinalScene()
        {
            sceneLoader.LoadScene(finalSceneName);
        }
        private void Awake() 
        {
            sceneLoader = GetComponent<SceneLoader>();
        }
    }
}
