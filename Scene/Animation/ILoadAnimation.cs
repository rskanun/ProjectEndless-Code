using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public interface ILoadAnimation
{
    public void OnPlayAnimation(List<string> loadScenes, List<string> unloadScenes, UnloadSceneOptions unloadOptions, Action completeAction);
}