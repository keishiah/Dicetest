using System;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace _Scripts.Infrastructure
{
    public class SceneLoader 
    {
        public async void LoadScene(string nextScene, Action onLoaded = null)
        {
            await SceneManager.LoadSceneAsync(nextScene).ToUniTask();

            await UniTask.DelayFrame(1);
            onLoaded?.Invoke();
        }
    }
}