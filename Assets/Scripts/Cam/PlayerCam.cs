using UnityEngine;
using Cinemachine;  // 别忘了引入 Cinemachine 命名空间

public class PlayerCam : MonoBehaviour
{
    private CinemachineVirtualCamera vcam;

    public void Start()
    {
        // 获取子物体上的虚拟相机
        vcam = GetComponentInChildren<CinemachineVirtualCamera>();

        if (vcam != null && GameManager.Instance.Player != null)
        {
            // 设置相机跟随和注视目标
            vcam.Follow = GameManager.Instance.Player.transform;
            vcam.LookAt = GameManager.Instance.Player.transform;
        }
        else
        {
            Debug.LogWarning("未找到虚拟相机或玩家对象！");
        }

        // 脱离父对象
        transform.SetParent(null);
    }
}
