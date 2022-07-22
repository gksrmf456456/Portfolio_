using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyHPSliderPrefab; // 적 체력을 나타내는 Slider UI 프리팹
    [SerializeField]
    private Transform canvasTransform;  // UI를 표현하는 Canvas 오브젝트의 Transform
    //[SerializeField]
    //private float spawnTime;            // 적 생성 주기 
    [SerializeField]
    private Transform[] wayPoints;      // 현재 스테이지의 이동 경로
    [SerializeField]
    private PlayerHP playerHP;
    [SerializeField]
    private PlayerGold playerGold;
    private Wave currentWave;           // 현재 웨이브 정보
    private int currentEnemyCount;      // 현재 웨이브에 남아있는 적 숫자

    // 저장할 데이터의 개수가 가변적일 때 List 사용
    private List<Enemy> enemyList;      // 현재 맵에 존재하는 모든 적의 정보

    // 적의 생성과 삭제는 EnemySpawner 에서 하기 때문에 Set 불필요
    public List<Enemy> EnemyList => enemyList;

    // 현재 웨이브의 남아있는 적, 최대 적 숫자
    public int CurrentEnemyCount => currentEnemyCount;
    public int MaxEnemyCount => currentWave.maxEnemyCount;

    private void Awake()
    {
        enemyList = new List<Enemy>();  // 적 리스트 메모리 할당
        //StartCoroutine(SpawnEnemy());
    }

    public void StartWave(Wave wave)
    {
        currentWave = wave;
        // 현재 웨이브의 최대 적 숫자를 저장
        currentEnemyCount = currentWave.maxEnemyCount;
        // 현재 웨이브 시작
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        // 현재 웨이브에서 생성한 적 숫자
        int spawnEnemyCount = 0;

        //while (true)
        // 현재 웨이브에서 생성되어야 하는 숫자만큼 정을 생성하고 코루틴 종료
        while(spawnEnemyCount < currentWave.maxEnemyCount)
        {
            //GameObject clone = Instantiate(enemyPrefab);    // 적 오프젝트 생성
            // 웨이브에 등장하는 적의 종류가 여러 종류일 때 임의의 적이 등장하도록 설정하고 적 오브젝트 생성
            int enemyIndex = Random.Range(0, currentWave.enemyPrefabs.Length);
            GameObject clone = Instantiate(currentWave.enemyPrefabs[enemyIndex]);
            Enemy enemy = clone.GetComponent<Enemy>();      // 방금 생성된 적의 Enemy 컴포넌트

            enemy.Setup(this, wayPoints);                   // wayPoint 정보를 매개변수로 Setup() 호출
            enemyList.Add(enemy);                           // 리스트에 방금 생성된 적 정보 저장

            SpawnEnemyHPSlider(clone);

            spawnEnemyCount ++;

            // 각 웨이브마다 spawnTime 이 다를 수 있기 때문에 현재 웨이브(currnetWave)의 spawnTime 사용
            yield return new WaitForSeconds(currentWave.spawnTime);     // spawnTime 시간 동안 대기
        }
    }

    public void DestroyEnemy(EnemyDestroyType type, Enemy enemy, int gold)
    {
        // 적이 목표지점까지 도착했을 때
        if(type == EnemyDestroyType.Arrive)
        {
            // 플레이어 체력 1 감소
            playerHP.TakeDamage(1);
        }
        // 적이 플레이어에게 사망했을 때
        else if( type == EnemyDestroyType.Kill)
        {
            // 적의 종류에 따라 사망시 플레이어 골드 획득
            playerGold.CurrentGold += gold;
        }

        currentEnemyCount--;        // 적이 사망할때 마다 웨이브에 생존하는 적 숫자 감소
        enemyList.Remove(enemy);    // 리스트에서 사망하는 적 정보 삭제
        Destroy(enemy.gameObject);  // 적 오브젝트 삭제
    }

    private void SpawnEnemyHPSlider(GameObject enemy)
    {
        // 적체력을 나타내는 Slider UI Instantiate화 하여 생성
        GameObject sliderClone = Instantiate(enemyHPSliderPrefab);
        // Slider UI 오브젝트를 parent(Canvas)의 자식으로 설정
        sliderClone.transform.SetParent(canvasTransform);
        // 크기를 다시 (1, 1, 1)로 설정
        sliderClone.transform.localScale = Vector3.one;

        // Slider UI 가 쫓아다닐 대상을 본인으로 설정
        sliderClone.GetComponent<SliderPositionAutoSetter>().Setup(enemy.transform);
        // Slider UI 에 자신의 체력 정보를 표시하도록 설정
        sliderClone.GetComponent<EnemyHPViewer>().Setup(enemy.GetComponent<EnemyHP>());
    }
}
