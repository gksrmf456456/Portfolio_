using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderPositionAutoSetter : MonoBehaviour
{
    [SerializeField]
    private Vector3 distance = Vector3.down * 20.0f;
    private Transform targetTransform;
    private RectTransform rectTransform;

    public void Setup(Transform target)
    {
        // Slider UI �� �Ѿ� �ٴ� target ����
        targetTransform = target;

        rectTransform = GetComponent<RectTransform>();

    }

    private void Start()
    {
        rectTransform.SetSiblingIndex(0);
        
    }

    private void LateUpdate()
    {
        if(targetTransform == null)
        {
            Destroy(this.gameObject);
            return;
        }

        // ������Ʈ�� ���� ��ǥ�� �������� ȭ�鿡���� ��ǥ ���� ����
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetTransform.position);
        // ȭ�鳻���� ��ǥ + distance ��ŭ ������ ��ġ�� Slider UI �� ��ġ�� ����
        rectTransform.position = screenPosition + distance;
    }
}
