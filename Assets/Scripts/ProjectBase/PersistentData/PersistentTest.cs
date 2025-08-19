using Sirenix.OdinInspector;
using UnityEngine;
using ProjectBase.PersistentData;

public class PersistentTest : MonoBehaviour
{
    [SerializeField]
    private PersistentDataUtil_Odin.PlayerData testData = new PersistentDataUtil_Odin.PlayerData();

    [Button("测试_写入")] // 编辑器下方便点击
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

    // 🔽 运行时（打包后）测试入口
    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 40), "测试_写入")) Test_Save();
        if (GUI.Button(new Rect(10, 60, 150, 40), "测试_读取")) Test_Load();
    }
}
