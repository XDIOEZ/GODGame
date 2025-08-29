using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSaveManager : SingletonAutoMono<GameSaveManager>
{
    [Header("当前存档名称(默认是玩家的名字)")]
    public string SaveName;

    public void Start()
    {
        Checkpoint.CurrentActiveCheckpointName = "默认检查点";
    }

    #region 存档点数据

    [Button("加载存档点数据")]
    public void Load_ChcekPoint()
    {
        PersistentDataUtil_Json.Load("新增记录点数据", ref Checkpoint.NewCheckpointData);
        PersistentDataUtil_Json.Load("当前激活的Checkpoint的名字", ref Checkpoint.CurrentActiveCheckpointName);
        Checkpoint.IsRun_Checkpoints_Dictionary.ForEach(x => x.Value.Load());
        Checkpoint.LoadNewAddedCheckpointData();
    }
    [Button("保存存档点数据")]
    public void Save_ChcekPoint()
    {
        PersistentDataUtil_Json.Save("新增记录点数据", Checkpoint.NewCheckpointData);
        PersistentDataUtil_Json.Save("当前激活的Checkpoint的名字", Checkpoint.CurrentActiveCheckpointName);
        Checkpoint.IsRun_Checkpoints_Dictionary.ForEach(x => x.Value.Save());
    }
    [Button("应用存档点数据")]
    public void Aply_ChcekPoint()
    {
        Checkpoint.BackToCheckPoint(Checkpoint.CurrentActiveCheckpointName, GameManager.Instance.Player.gameObject);
    }
    [Button("删除存档点数据")]
    public void Delete_ChcekPoint()
    {
        PersistentDataUtil_Json.Delete("新增记录点数据");
        PersistentDataUtil_Json.Delete("当前激活的Checkpoint的名字");
        Checkpoint.IsRun_Checkpoints_Dictionary.ForEach(x => PersistentDataUtil_Json.Delete(x.Key));
    }
    #endregion
}
