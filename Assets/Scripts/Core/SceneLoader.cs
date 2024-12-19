using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class SceneLoader : MonoBehaviour
{
    public UnityEvent<float> OnSceneLoading;
    private Coroutine _sceneCoroutine;

    public void LoadScene(string sceneName)
    {
        if(_sceneCoroutine == null)
        {
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
