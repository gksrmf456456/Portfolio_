using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField]
    private TowerSpawner towerSpawner;
    [SerializeField]
    private TowerDataViewer towerDataViewer;

    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;
    private Transform hitTransform = null;      // 마우스 픽킹으로 선택한 오브젝트 임시 저장

    private void Awake()
    {
        // MainCamera 태그를 갖고 있는 오브젝트 탐색 후 Camera 컴포넌트 정보 전달
        // mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); 와 동일
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject() == true)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);     // 카메라 위치에서 화면의 마우스 위치를 관통하는 광선 생성

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))          // 2D 모니터를 통해 3D 월드의 오브젝트를 마우스로 선택하는 방법, 광선에 부딪히는 오브젝드를 검출해서 hit 에 저장
            {
                hitTransform = hit.transform;

                if (hit.transform.CompareTag("Tile"))                   // 광선에 부딪힌 오브젝트의 태그가 Tile 이면
                {
                    towerSpawner.SpawnTower(hit.transform);             // 타워를 생성하는 SpawnTower() 호출
                }
                else if (hit.transform.CompareTag("Tower"))
                {
                    towerDataViewer.OnPanel(hit.transform);
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (hitTransform == null || hitTransform.CompareTag("Tower") == false)   // 마우스를 눌렀을때 선택한 오브젝트가 없거나 선택한 오브젝트가 타워가 아니면
            {
                towerDataViewer.OffPanel();     // 타워 정보 패널을 비활성화
            }

            hitTransform = null;
        }
    }
}
