using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WelcomeBack : MonoBehaviour
{
    // 发生碰撞时触发 获取RB2D组件 将RB2D挂接的GameObject 传送到 记录点
    void OnCollisionEnter2D(Collision2D collision)
    {
        // 检查碰撞物体是否为Player图层
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // 获取RB2D组件
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

            // 检查是否存在RB2D组件
            if (rb != null)
            {
                // 将RB2D挂接的GameObject传送到记录点
                TransportToRecordPoint(collision.gameObject);
            }
        }
    }

    // 调用您的传送接口方法
    private void TransportToRecordPoint(GameObject obj)
    {
        Checkpoint.BackToCurrentActiveCheckpoint(obj);
    }
}