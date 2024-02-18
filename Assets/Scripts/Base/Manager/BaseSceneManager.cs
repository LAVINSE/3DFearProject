using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class BaseSceneManager : MonoBehaviour
{
    #region 프로퍼티
    private static Dictionary<string, BaseSceneManager> SceneManagerDict = new Dictionary<string, BaseSceneManager>();
    public abstract string SceneName { get; }

    public GameObject CanvasObj { get; private set; }
    #endregion 프로퍼티

    #region 함수
    /** 초기화 */
    public virtual void Awake()
    {
        BaseSceneManager.SceneManagerDict.TryAdd(this.SceneName, this);

        var RootObjs = this.gameObject.scene.GetRootGameObjects();

        for (int i = 0; i < RootObjs.Length; i++)
        {
            this.CanvasObj = this.CanvasObj ??
                RootObjs[i].transform.Find("Canvas")?.gameObject;
        }
    }

    /** 초기화 => 제거 되었을 경우 */
    public virtual void OnDestroy()
    {
        if (BaseSceneManager.SceneManagerDict.ContainsKey(this.SceneName))
        {
            BaseSceneManager.SceneManagerDict.Remove(this.SceneName);
        }
    }

    /** 씬 관리자를 반환한다 */
    public static T GetSceneManager<T>(string SceneName) where T : BaseSceneManager
    {
        return BaseSceneManager.SceneManagerDict.GetValueOrDefault(SceneName) as T;
    }
    #endregion // 함수
}
