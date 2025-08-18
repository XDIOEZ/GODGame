using Sirenix.OdinInspector;
using UnityEngine;
using ProjectBase.PersistentData;

public class PersistentTest : MonoBehaviour
{
    [SerializeField] // 普通序列化字段
    private PersistentDataUtil_Odin.PlayerData testData = new PersistentDataUtil_Odin.PlayerData();

    [Button("测试_写入")]
    public void Test_Save()
    {
        testData.name = "测试_" + Random.Range(0, 100);
        PersistentDataUtil_Json.Save("PlayerData", testData);
        Debug.Log($"已保存: {testData}");
    }

    [Button("测试_读取")]
    public void Test_Load()
    {
        var defaultData = new PersistentDataUtil_Odin.PlayerData();
        var loadedData = PersistentDataUtil_Json.Load("PlayerData", ref defaultData);
        Debug.Log($"读取结果: {loadedData}");
    }
}
