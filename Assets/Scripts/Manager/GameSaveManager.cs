using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSaveManager : SingletonAutoMono<GameSaveManager>
{
    [Header("��ǰ�浵����(Ĭ������ҵ�����)")]
    public string SaveName;

    public void Start()
    {
        Checkpoint.CurrentActiveCheckpointName = "Ĭ�ϼ���";
    }

    #region �浵������

    [Button("���ش浵������")]
    public void Load_ChcekPoint()
    {
        PersistentDataUtil_Json.Load("������¼������", ref Checkpoint.NewCheckpointData);
        PersistentDataUtil_Json.Load("��ǰ�����Checkpoint������", ref Checkpoint.CurrentActiveCheckpointName);
        Checkpoint.IsRun_Checkpoints_Dictionary.ForEach(x => x.Value.Load());
        Checkpoint.LoadNewAddedCheckpointData();
    }
    [Button("����浵������")]
    public void Save_ChcekPoint()
    {
        PersistentDataUtil_Json.Save("������¼������", Checkpoint.NewCheckpointData);
        PersistentDataUtil_Json.Save("��ǰ�����Checkpoint������", Checkpoint.CurrentActiveCheckpointName);
        Checkpoint.IsRun_Checkpoints_Dictionary.ForEach(x => x.Value.Save());
    }
    [Button("Ӧ�ô浵������")]
    public void Aply_ChcekPoint()
    {
        Checkpoint.BackToCheckPoint(Checkpoint.CurrentActiveCheckpointName, GameManager.Instance.Player.gameObject);
    }
    [Button("ɾ���浵������")]
    public void Delete_ChcekPoint()
    {
        PersistentDataUtil_Json.Delete("������¼������");
        PersistentDataUtil_Json.Delete("��ǰ�����Checkpoint������");
        Checkpoint.IsRun_Checkpoints_Dictionary.ForEach(x => PersistentDataUtil_Json.Delete(x.Key));
    }
    #endregion
}
