using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Core : MonoBehaviour
{
    [SerializeField] GameObject evSysObj;

    public int stageIndex { get; set; } = 0;

    public static Core I = null;

    Stack<Scene> sceneStk = new Stack<Scene>();

    #region Singleton
    private void Awake()
    {
        if(I == null)
        {
            I = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        if(_mode == LoadSceneMode.Additive)
        {
            sceneStk.Push(_scene);
        }
    }

    public void LoadScene(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName);
    }

    public void LoadScene(string _sceneName, LoadSceneMode _loadSceneMode)
    {
        SceneManager.LoadScene(_sceneName, _loadSceneMode);
    }

    public void UnloadLastAdditiveScene()
    {
        if(sceneStk.Count > 0)
        {
            SceneManager.UnloadScene(sceneStk.Pop());
        }
    }

    public void OnUIInput()
    {
        evSysObj.SetActive(true);
    }

    public void OffUIInput()
    {
        evSysObj.SetActive(false);
    }
}
