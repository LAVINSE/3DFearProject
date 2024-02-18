using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class BaseSceneManager : MonoBehaviour
{
    #region ������Ƽ
    private static Dictionary<string, BaseSceneManager> SceneManagerDict = new Dictionary<string, BaseSceneManager>();
    public abstract string SceneName { get; }

    public GameObject CanvasObj { get; private set; }
    #endregion ������Ƽ

    #region �Լ�
    /** �ʱ�ȭ */
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

    /** �ʱ�ȭ => ���� �Ǿ��� ��� */
    public virtual void OnDestroy()
    {
        if (BaseSceneManager.SceneManagerDict.ContainsKey(this.SceneName))
        {
            BaseSceneManager.SceneManagerDict.Remove(this.SceneName);
        }
    }

    /** �� �����ڸ� ��ȯ�Ѵ� */
    public static T GetSceneManager<T>(string SceneName) where T : BaseSceneManager
    {
        return BaseSceneManager.SceneManagerDict.GetValueOrDefault(SceneName) as T;
    }
    #endregion // �Լ�
}
