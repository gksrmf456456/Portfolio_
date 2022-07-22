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
        // Slider UI 가 쫓아 다닐 target 설정
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

        // 오브젝트의 월드 좌표를 기준으로 화면에서의 좌표 값을 구함
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetTransform.position);
        // 화면내에서 좌표 + distance 만큼 떨어진 위치를 Slider UI 의 위치로 설정
        rectTransform.position = screenPosition + distance;
    }
}
