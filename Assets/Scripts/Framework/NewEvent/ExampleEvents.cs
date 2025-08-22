using UnityEngine;


/// <summary>
/// 玩家死亡事件
/// </summary>
public class PlayerDeathEvent : IEvent
{
    public int PlayerId; // 玩家ID
    public string DeathReason; // 死亡原因
    public float DeathTime; // 死亡时间（秒）

    public PlayerDeathEvent(int playerId, string deathReason, float deathTime)
    {
        PlayerId = playerId;
        DeathReason = deathReason;
        DeathTime = deathTime;
    }
}

/// <summary>
/// 道具拾取事件
/// </summary>
public class ItemPickupEvent : IEvent
{
    public string ItemId; // 道具ID
    public int Count; // 拾取数量
    public Vector3 PickupPosition; // 拾取位置

    public ItemPickupEvent(string itemId, int count, Vector3 position)
    {
        ItemId = itemId;
        Count = count;
        PickupPosition = position;
    }
}
