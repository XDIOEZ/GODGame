using UnityEngine;

public class HorizontalLoopBackground : MonoBehaviour
{
    [Header("需要循环的三个物体（任意顺序，Start 会重新排好）")]
    public Transform segmentA;
    public Transform segmentB;
    public Transform segmentC;

    [Header("单个背景的宽度")]
    public float segmentWidth = 20f;

    [Header("参考目标（通常是相机或玩家）")]
    public Transform target;

    private Transform[] segments;

    void Start()
    {
        segments = new Transform[] { segmentA, segmentB, segmentC };

        // 按 x 排序，确保初始顺序正确
        System.Array.Sort(segments, (a, b) => a.position.x.CompareTo(b.position.x));
    }

    void Update()
    {
        if (target == null || segments == null) return;

        Transform left = segments[0];
        Transform center = segments[1];
        Transform right = segments[2];

        float halfWidth = segmentWidth / 2f;

        // 如果目标跑到 center 的右半边，就把 left 移到最右边
        if (target.position.x > center.position.x + halfWidth)
        {
            left.position = new Vector3(right.position.x + segmentWidth, left.position.y, left.position.z);
            ShiftArrayLeft();
        }

        // 如果目标跑到 center 的左半边，就把 right 移到最左边
        else if (target.position.x < center.position.x - halfWidth)
        {
            right.position = new Vector3(left.position.x - segmentWidth, right.position.y, right.position.z);
            ShiftArrayRight();
        }
    }

    void ShiftArrayLeft()
    {
        // 往左挪动：center->left, right->center, left->right
        Transform tmp = segments[0];
        segments[0] = segments[1];
        segments[1] = segments[2];
        segments[2] = tmp;
    }

    void ShiftArrayRight()
    {
        // 往右挪动：center->right, left->center, right->left
        Transform tmp = segments[2];
        segments[2] = segments[1];
        segments[1] = segments[0];
        segments[0] = tmp;
    }
}
