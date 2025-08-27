using UnityEngine;

public class HorizontalLoopBackground : MonoBehaviour
{
    [Header("��Ҫѭ�����������壨����˳��Start �������źã�")]
    public Transform segmentA;
    public Transform segmentB;
    public Transform segmentC;

    [Header("���������Ŀ��")]
    public float segmentWidth = 20f;

    [Header("�ο�Ŀ�꣨ͨ�����������ң�")]
    public Transform target;

    private Transform[] segments;

    void Start()
    {
        segments = new Transform[] { segmentA, segmentB, segmentC };

        // �� x ����ȷ����ʼ˳����ȷ
        System.Array.Sort(segments, (a, b) => a.position.x.CompareTo(b.position.x));
    }

    void Update()
    {
        if (target == null || segments == null) return;

        Transform left = segments[0];
        Transform center = segments[1];
        Transform right = segments[2];

        float halfWidth = segmentWidth / 2f;

        // ���Ŀ���ܵ� center ���Ұ�ߣ��Ͱ� left �Ƶ����ұ�
        if (target.position.x > center.position.x + halfWidth)
        {
            left.position = new Vector3(right.position.x + segmentWidth, left.position.y, left.position.z);
            ShiftArrayLeft();
        }

        // ���Ŀ���ܵ� center �����ߣ��Ͱ� right �Ƶ������
        else if (target.position.x < center.position.x - halfWidth)
        {
            right.position = new Vector3(left.position.x - segmentWidth, right.position.y, right.position.z);
            ShiftArrayRight();
        }
    }

    void ShiftArrayLeft()
    {
        // ����Ų����center->left, right->center, left->right
        Transform tmp = segments[0];
        segments[0] = segments[1];
        segments[1] = segments[2];
        segments[2] = tmp;
    }

    void ShiftArrayRight()
    {
        // ����Ų����center->right, left->center, right->left
        Transform tmp = segments[2];
        segments[2] = segments[1];
        segments[1] = segments[0];
        segments[0] = tmp;
    }
}
