using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyHPSliderPrefab; // �� ü���� ��Ÿ���� Slider UI ������
    [SerializeField]
    private Transform canvasTransform;  // UI�� ǥ���ϴ� Canvas ������Ʈ�� Transform
    //[SerializeField]
    //private float spawnTime;            // �� ���� �ֱ� 
    [SerializeField]
    private Transform[] wayPoints;      // ���� ���������� �̵� ���
    [SerializeField]
    private PlayerHP playerHP;
    [SerializeField]
    private PlayerGold playerGold;
    private Wave currentWave;           // ���� ���̺� ����
    private int currentEnemyCount;      // ���� ���̺꿡 �����ִ� �� ����

    // ������ �������� ������ �������� �� List ���
    private List<Enemy> enemyList;      // ���� �ʿ� �����ϴ� ��� ���� ����

    // ���� ������ ������ EnemySpawner ���� �ϱ� ������ Set ���ʿ�
    public List<Enemy> EnemyList => enemyList;

    // ���� ���̺��� �����ִ� ��, �ִ� �� ����
    public int CurrentEnemyCount => currentEnemyCount;
    public int MaxEnemyCount => currentWave.maxEnemyCount;

    private void Awake()
    {
        enemyList = new List<Enemy>();  // �� ����Ʈ �޸� �Ҵ�
        //StartCoroutine(SpawnEnemy());
    }

    public void StartWave(Wave wave)
    {
        currentWave = wave;
        // ���� ���̺��� �ִ� �� ���ڸ� ����
        currentEnemyCount = currentWave.maxEnemyCount;
        // ���� ���̺� ����
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        // ���� ���̺꿡�� ������ �� ����
        int spawnEnemyCount = 0;

        //while (true)
        // ���� ���̺꿡�� �����Ǿ�� �ϴ� ���ڸ�ŭ ���� �����ϰ� �ڷ�ƾ ����
        while(spawnEnemyCount < currentWave.maxEnemyCount)
        {
            //GameObject clone = Instantiate(enemyPrefab);    // �� ������Ʈ ����
            // ���̺꿡 �����ϴ� ���� ������ ���� ������ �� ������ ���� �����ϵ��� �����ϰ� �� ������Ʈ ����
            int enemyIndex = Random.Range(0, currentWave.enemyPrefabs.Length);
            GameObject clone = Instantiate(currentWave.enemyPrefabs[enemyIndex]);
            Enemy enemy = clone.GetComponent<Enemy>();      // ��� ������ ���� Enemy ������Ʈ

            enemy.Setup(this, wayPoints);                   // wayPoint ������ �Ű������� Setup() ȣ��
            enemyList.Add(enemy);                           // ����Ʈ�� ��� ������ �� ���� ����

            SpawnEnemyHPSlider(clone);

            spawnEnemyCount ++;

            // �� ���̺긶�� spawnTime �� �ٸ� �� �ֱ� ������ ���� ���̺�(currnetWave)�� spawnTime ���
            yield return new WaitForSeconds(currentWave.spawnTime);     // spawnTime �ð� ���� ���
        }
    }

    public void DestroyEnemy(EnemyDestroyType type, Enemy enemy, int gold)
    {
        // ���� ��ǥ�������� �������� ��
        if(type == EnemyDestroyType.Arrive)
        {
            // �÷��̾� ü�� 1 ����
            playerHP.TakeDamage(1);
        }
        // ���� �÷��̾�� ������� ��
        else if( type == EnemyDestroyType.Kill)
        {
            // ���� ������ ���� ����� �÷��̾� ��� ȹ��
            playerGold.CurrentGold += gold;
        }

        currentEnemyCount--;        // ���� ����Ҷ� ���� ���̺꿡 �����ϴ� �� ���� ����
        enemyList.Remove(enemy);    // ����Ʈ���� ����ϴ� �� ���� ����
        Destroy(enemy.gameObject);  // �� ������Ʈ ����
    }

    private void SpawnEnemyHPSlider(GameObject enemy)
    {
        // ��ü���� ��Ÿ���� Slider UI Instantiateȭ �Ͽ� ����
        GameObject sliderClone = Instantiate(enemyHPSliderPrefab);
        // Slider UI ������Ʈ�� parent(Canvas)�� �ڽ����� ����
        sliderClone.transform.SetParent(canvasTransform);
        // ũ�⸦ �ٽ� (1, 1, 1)�� ����
        sliderClone.transform.localScale = Vector3.one;

        // Slider UI �� �Ѿƴٴ� ����� �������� ����
        sliderClone.GetComponent<SliderPositionAutoSetter>().Setup(enemy.transform);
        // Slider UI �� �ڽ��� ü�� ������ ǥ���ϵ��� ����
        sliderClone.GetComponent<EnemyHPViewer>().Setup(enemy.GetComponent<EnemyHP>());
    }
}
