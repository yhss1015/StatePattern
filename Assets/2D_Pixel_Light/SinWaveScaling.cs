using UnityEngine;

public class SinWaveScaling : MonoBehaviour
{
    public float speed = 1.0f;  // �ӵ� ����
    public float scaleFactor = 0.02f;  // ũ�� ��ȭ��

    private Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        float time = Time.time * speed; // �ð��� �ӵ��� ����
        float sinValue = Mathf.Sin(time); // ���� �Լ� ����
        float scaleChange = sinValue * scaleFactor; // ������ ��ȭ�� ���

        transform.localScale = initialScale + new Vector3(scaleChange, scaleChange, scaleChange); // ��� �࿡ ��ȭ ����
    }
}
