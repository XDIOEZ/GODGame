using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : InteractBase
{
    #region ������̬�ֶ�
    [ShowInInspector]
    [Header("�洢����Checkpoint���ֵ�")]
    public static Dictionary<string, Checkpoint> IsRun_Checkpoints_Dictionary=new Dictionary<string, Checkpoint>();
    [Header("������Checkpoint����")]
    public static Dictionary<string, CheckpointData> NewCheckpointData = new Dictionary<string, CheckpointData>();
  
    [Header("��ǰ�����Checkpoint������,Ҳ������ҵĸ����")]
    [ShowInInspector]
    public static string CurrentActiveCheckpointName = "Ĭ�ϼ���";
    #endregion
    #region �������
    [Header("��ǰCheckpoint����")]
    public CheckpointData Data = new();
    #endregion
    #region �������
    public SpriteRenderer spriteRenderer;
    #endregion

    public new void Start()
    {
        GetComponentInChildren<InteractReciver>().onInteractEvent_Start += Action;
        IsRun_Checkpoints_Dictionary.TryGetValue(Data.CheckpointName, out Checkpoint checkpoint);
        if (checkpoint == null)
        {
            IsRun_Checkpoints_Dictionary[gameObject.name] = this;
            Data.CheckpointName = gameObject.name;
        }
        gameObject.name = Data.CheckpointName;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Load()
    {
        PersistentDataUtil_Json.Load(gameObject.name, ref Data);
        transform.position = Data.CheckpointPosition;
    }

    public void Save()
    {
        Data.CheckpointPosition = transform.position;
        PersistentDataUtil_Json.Save(Data.CheckpointName,Data);
    }

    public override void Action(Interacter interacter)
    {
        Debug.Log("��Ҽ�����Checkpoint:" + Data.CheckpointName);
        SetThisCheckpointActive();
    }

    [Tooltip("�ص�ָ����Checkpoint")]
    public static void BackToCheckPoint(string checkpointName, GameObject player)
    {
        player.transform.position = IsRun_Checkpoints_Dictionary[checkpointName].transform.position;
        player.transform.position += new Vector3(1, 0, 0);
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.GetComponent<Rigidbody2D>().angularVelocity = 0;
        player.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    [Tooltip("�ص���ǰ�����Checkpoint")]
    public static void BackToCurrentActiveCheckpoint(GameObject player)
    {
        BackToCheckPoint(CurrentActiveCheckpointName, player);
    }

    [Tooltip("���õ�ǰCheckpointΪ����״̬")]
    [Button("���õ�ǰCheckpointΪ����״̬")]
    public void SetThisCheckpointActive()
    {
        CurrentActiveCheckpointName = Data.CheckpointName;
        if (Data.activatedState == false)
        {
            Checkpoint.IsRun_Checkpoints_Dictionary.ForEach(item =>
            {
                if (item.Value.Data.activatedState == true)
                {
                    item.Value.Data.activatedState = false;
                }
            });
            Data.activatedState = true;
        }
    }

    public void Update()
    {
        if(Data.activatedState == true)
        {
            spriteRenderer.color = Color.green;
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }

    public static void LoadNewAddedCheckpointData()
    {
        if (Checkpoint.NewCheckpointData.Count > 0)
        {
            foreach (var item in NewCheckpointData)
            {
               GameObject checkpoint = Instantiate(ResManager.Instance.GetPrefab("Checkpoint"));
               checkpoint.name = item.Key;
               checkpoint.GetComponent<Checkpoint>().Load();
            }
        }
    }

    [Button("�����µ�Checkpoint")]
    public static void CreateNewCheckpoint(Vector3 position,string name = "")
    {
        GameObject checkpoint = Instantiate(ResManager.Instance.GetPrefab("Checkpoint"));

        checkpoint.transform.position = position;
        if (name == "")
        {
            checkpoint.GetComponent<Checkpoint>().Data.CheckpointName = "NewCheckpoint_" + Random.Range(10000, 99999);
            checkpoint.name = checkpoint.GetComponent<Checkpoint>().Data.CheckpointName;
        }
        else
        {
            checkpoint.GetComponent<Checkpoint>().Data.CheckpointName = name;
            checkpoint.name = name;
        }
        NewCheckpointData[checkpoint.name] = checkpoint.GetComponent<Checkpoint>().Data;
    }
}

[System.Serializable]
public class CheckpointData
{
    [ShowInInspector]
    [Header("��¼������")]
    public string CheckpointName = "";
    [Header("��¼��λ��")]
    public Vector3 CheckpointPosition = Vector3.zero;
    [Header("�Ƿ��Ѿ���������")]
    public bool activatedState = false;
}