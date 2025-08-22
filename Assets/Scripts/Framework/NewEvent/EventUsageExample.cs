using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// 事件系统使用示例
/// 展示如何注册、发布事件，以及在对象销毁时取消注册
/// </summary>
public class EventUsageExample : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput; // 同对象上的 PlayerInput
    [SerializeField] private string actionNameOne; // InputAction 名称
    [SerializeField] private string actionNameTwo; // InputAction 名称
    [SerializeField] private string actionNameThree; // InputAction 名称
    private InputAction EventOneactive;
    private InputAction EventTwoactive;
    private InputAction EventThreeactive;


    private int pageRefreshcount;

    private void OnEnable()
    {

        // 注册事件监听（玩家死亡事件）
        EventManager.Instance.On<PlayerDeathEvent>(OnPlayerDeath);
        // 注册道具拾取事件
        EventManager.Instance.On<ItemPickupEvent>(OnItemPickup);
        EventManager.Instance.On<RefreshPageEvent>(Refresh);
        EventManager.Instance.On<RefreshPageEvent>(RefreshTwo);
        EventOneactive = playerInput.actions[actionNameOne];
        if (EventOneactive != null)
        {
            EventOneactive.performed += GetKeyOne;
            EventOneactive.Enable();
        }
        else
        {
            Debug.LogWarning($"PlayerInput 上未找到 InputAction: {actionNameOne}");
        }
        EventTwoactive = playerInput.actions[actionNameTwo];
        if (EventTwoactive != null)
        {
            EventTwoactive.performed += GetKeyTwo;
            EventTwoactive.Enable();
        }
        else
        {
            Debug.LogWarning($"PlayerInput 上未找到 InputAction: {actionNameTwo}");
        }
        EventThreeactive = playerInput.actions[actionNameThree];
        if (EventThreeactive != null)
        {
            EventThreeactive.performed += GetKeyThree;
            EventThreeactive.Enable();
        }
        else
        {
            Debug.LogWarning($"PlayerInput 上未找到 InputAction: {actionNameThree}");
        }

    }

    private void OnDisable()
    {
        // 取消事件监听（防止对象销毁后仍收到事件，导致空引用错误）
        EventManager.Instance.Off<PlayerDeathEvent>(OnPlayerDeath);
        EventManager.Instance.Off<ItemPickupEvent>(OnItemPickup);
        EventManager.Instance.Off<RefreshPageEvent>(Refresh);
        EventManager.Instance.Off<RefreshPageEvent>(RefreshTwo);
    }

    private void Update()
    {

    }

    private void GetKeyOne(InputAction.CallbackContext context)
    {
        var deathEvent = new PlayerDeathEvent(
               playerId: 1001,
               deathReason: "被怪物击杀",
               deathTime: Time.time
           );
        EventManager.Instance.Emit(deathEvent);
    }

    private void GetKeyTwo(InputAction.CallbackContext context)
    {
            var pickupEvent = new ItemPickupEvent(
                itemId: "hp_potion_001",
                count: 3,
                position: transform.position
            );
            EventManager.Instance.Emit(pickupEvent);
    }

    private void GetKeyThree(InputAction.CallbackContext context)
    {
        EventManager.Instance.Emit(new RefreshPageEvent());
    }

    private void Refresh(RefreshPageEvent evt)
    {
        pageRefreshcount++;
        Debug.Log($"我是第一个方法:当前刷新了页面{pageRefreshcount}");
    }
    private void RefreshTwo(RefreshPageEvent evt)
    {
        Debug.Log($"我是第二个方法:当前刷新了页面{pageRefreshcount}");
    }

    // 玩家死亡事件回调
    private void OnPlayerDeath(PlayerDeathEvent evt)
    {
        Debug.Log($"玩家{evt.PlayerId}在{evt.DeathTime:F2}秒死亡，原因：{evt.DeathReason}");
        // 实际项目中可在这里处理UI提示、音效播放、任务更新等逻辑
    }

    // 道具拾取事件回调
    private void OnItemPickup(ItemPickupEvent evt)
    {
        Debug.Log($"在位置{evt.PickupPosition}拾取了{evt.Count}个{evt.ItemId}");
        // 实际项目中可在这里处理背包更新、UI提示等逻辑
    }
}
