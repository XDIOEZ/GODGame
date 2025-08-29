using UnityEngine;
using Cinemachine;  // ���������� Cinemachine �����ռ�

public class PlayerCam : MonoBehaviour
{
    private CinemachineVirtualCamera vcam;

    public void Start()
    {
        // ��ȡ�������ϵ��������
        vcam = GetComponentInChildren<CinemachineVirtualCamera>();

        if (vcam != null && GameManager.Instance.Player != null)
        {
            // ������������ע��Ŀ��
            vcam.Follow = GameManager.Instance.Player.transform;
            vcam.LookAt = GameManager.Instance.Player.transform;
        }
        else
        {
            Debug.LogWarning("δ�ҵ������������Ҷ���");
        }

        // ���븸����
        transform.SetParent(null);
    }
}
