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


/// <summary>
/// taptap��¼ר��
/// </summary>
public class EmptyEventTaptapLogin : EmptyEvent { }
public class EmptyEventTwo : EmptyEvent { }
public class EmptyEventThree : EmptyEvent { }


public class ParameterPartsTouchEvent : EmptyEvent { }