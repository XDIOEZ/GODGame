//���в���������ֱ�Ӵ��������������ڴ˴����Ƽ��¼��÷���


/// <summary>
/// �޲����¼�����
/// ���ڴ�������Ҫ�������ݵķ�������ˢ��ҳ�桢������Ч�ȣ�
/// </summary>
public class EmptyEvent : IEvent
{
    // ���趨���κ��ֶΣ�����Ϊ�¼���ʶ
}

/// <summary>
/// �����޲����¼�ʾ����ˢ��ҳ���¼�
/// ��������Ҫˢ��ĳ��ҳ�棬��Ȼ�ǲ���������
/// </summary>
public class RefreshPageEvent : EmptyEvent
{
    // �̳���EmptyEvent������������
}

public class EmptyEventOne : EmptyEvent { }
public class EmptyEventTwo : EmptyEvent { }
public class EmptyEventThree : EmptyEvent { }
public class EmptyEventFour : EmptyEvent { }