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
    private Transform hitTransform = null;      // ���콺 ��ŷ���� ������ ������Ʈ �ӽ� ����

    private void Awake()
    {
        // MainCamera �±׸� ���� �ִ� ������Ʈ Ž�� �� Camera ������Ʈ ���� ����
        // mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); �� ����
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
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);     // ī�޶� ��ġ���� ȭ���� ���콺 ��ġ�� �����ϴ� ���� ����

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))          // 2D ����͸� ���� 3D ������ ������Ʈ�� ���콺�� �����ϴ� ���, ������ �ε����� �������带 �����ؼ� hit �� ����
            {
                hitTransform = hit.transform;

                if (hit.transform.CompareTag("Tile"))                   // ������ �ε��� ������Ʈ�� �±װ� Tile �̸�
                {
                    towerSpawner.SpawnTower(hit.transform);             // Ÿ���� �����ϴ� SpawnTower() ȣ��
                }
                else if (hit.transform.CompareTag("Tower"))
                {
                    towerDataViewer.OnPanel(hit.transform);
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (hitTransform == null || hitTransform.CompareTag("Tower") == false)   // ���콺�� �������� ������ ������Ʈ�� ���ų� ������ ������Ʈ�� Ÿ���� �ƴϸ�
            {
                towerDataViewer.OffPanel();     // Ÿ�� ���� �г��� ��Ȱ��ȭ
            }

            hitTransform = null;
        }
    }
}
