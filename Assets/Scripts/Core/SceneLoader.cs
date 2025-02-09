using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Game.Core
{
    public class SceneLoader : MonoBehaviour
    {
        public UnityEvent<float> OnSceneLoading;
        private Coroutine _sceneCoroutine;

        public void LoadScene(string sceneName)
        {
            if(_sceneCoroutine == null)
            {
                //Debug.Log("Loading scene " + sceneName);
                _sceneCoroutine = StartCoroutine(LoadSceneAsync(sceneName));
            }
        }


        private IEnumerator LoadSceneAsync(string sceneName)
        {

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

            while(!asyncLoad.isDone)
            {
                OnSceneLoading?.Invoke(asyncLoad.progress);
                yield return null;
            }
            
            _sceneCoroutine = null;
        }
    }
}

