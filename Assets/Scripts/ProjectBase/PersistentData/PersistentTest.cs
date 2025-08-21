using UnityEngine;
using UnityEngine.InputSystem; // 新输入系统
using ProjectBase.PersistentData;

public class PersistentTest : MonoBehaviour
{
    [SerializeField] private PersistentDataUtil_Odin.PlayerData testData = new();

    // 定义屏幕按钮区域
    private Rect saveRect = new Rect(10, 10, 150, 40);
    private Rect loadRect = new Rect(10, 60, 150, 40);

    private void Update()
    {
        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
        {
            Vector2 pos = Pointer.current.position.ReadValue();
            pos.y = Screen.height - pos.y; // GUI坐标系Y轴翻转

            if (saveRect.Contains(pos)) Test_Save();
            if (loadRect.Contains(pos)) Test_Load();
        }
    }

    public void Test_Save()
    {
        testData.name = "测试_" + Random.Range(0, 100);
        PersistentDataUtil_Json.Save("PlayerData", testData);
        Debug.Log($"已保存: {testData}");
    }

    public void Test_Load()
    {
        var defaultData = new PersistentDataUtil_Odin.PlayerData();
        var loadedData = PersistentDataUtil_Json.Load("PlayerData", ref defaultData);
        Debug.Log($"读取结果: {loadedData}");
    }

    private void OnGUI()
    {
        GUI.Box(saveRect, "测试_写入");
        GUI.Box(loadRect, "测试_读取");
    }
}
