using Sirenix.OdinInspector;
using UnityEngine;
using ProjectBase.PersistentData;

public class PersistentTest : MonoBehaviour
{
    [SerializeField] // ��ͨ���л��ֶ�
    private PersistentDataUtil_Odin.PlayerData testData = new PersistentDataUtil_Odin.PlayerData();

    [Button("����_д��")]
    public void Test_Save()
    {
        testData.name = "����_" + Random.Range(0, 100);
        PersistentDataUtil_Json.Save("PlayerData", testData);
        Debug.Log($"�ѱ���: {testData}");
    }

    [Button("����_��ȡ")]
    public void Test_Load()
    {
        var defaultData = new PersistentDataUtil_Odin.PlayerData();
        var loadedData = PersistentDataUtil_Json.Load("PlayerData", ref defaultData);
        Debug.Log($"��ȡ���: {loadedData}");
    }
}
