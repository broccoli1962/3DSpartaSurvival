using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("�������� ����")]
    public List<WaveData> waves;
    public GameObject bossPrefab;

    [Header("�÷��̾� �� ���� ����")]
    [Tooltip("�÷��̾��� Transform�� �����Ͽ� ���� ��ġ�� ����մϴ�.")]
    public Transform playerTransform;
    public float minSpawnDistance = 5f;
    public float maxSpawnDistance = 15f;

    [Header("���� Ÿ�̹� ����")]
    public float initialWaitTime = 3f; 
    public float spawnInterval = 1.5f; 
    public int maxMonstersOnField = 20; 

    private int currentWaveIndex = 0;
    private int monstersSpawnedThisWave = 0;
    private int monstersKilledThisWave = 0;
    private List<GameObject> activeMonsters = new List<GameObject>();

    public enum GameState { InitialWait, WaveInProgress, WaveComplete, BossFight, GameWon }
    public GameState currentState { get; private set; }


    void Start()
    {
        StartCoroutine(GameFlow());
    }

    private IEnumerator GameFlow()
    {
        // 1. �ʱ� �غ� �ð�
        currentState = GameState.InitialWait;
        Debug.Log("�������� ����! 3�� ����մϴ�.");
        yield return new WaitForSeconds(initialWaitTime);

        // ���̺� ����
        while (currentWaveIndex < waves.Count)
        {
            yield return StartCoroutine(WaveCoroutine());
            currentWaveIndex++;
        }

        // 3. ������
        yield return StartCoroutine(BossFightCoroutine());

        currentState = GameState.GameWon;
        Debug.Log("���� Ŭ����!");
    }

    private IEnumerator WaveCoroutine()
    {
        WaveData currentWave = waves[currentWaveIndex];
        monstersSpawnedThisWave = 0;
        monstersKilledThisWave = 0;
        activeMonsters.Clear();
        currentState = GameState.WaveInProgress;

        Debug.Log($"���̺� {currentWaveIndex + 1} ����! ��ǥ: {currentWave.totalMonstersToSpawn}����");

        // ���� ���� ����
        StartCoroutine(SpawnMonsters(currentWave));

        // �ش� ���̺��� ��� ���͸� óġ�� ������ ���
        while (monstersKilledThisWave < currentWave.totalMonstersToSpawn)
        {
            yield return null;
        }

        currentState = GameState.WaveComplete;
        Debug.Log($"���̺� {currentWaveIndex + 1} Ŭ����!");
    }

    private IEnumerator SpawnMonsters(WaveData wave)
    {
        while (monstersSpawnedThisWave < wave.totalMonstersToSpawn)
        {
            if (activeMonsters.Count < maxMonstersOnField)
            {
                // ������ ���� ���� ����
                GameObject monsterToSpawn = wave.monsterPrefabs[Random.Range(0, wave.monsterPrefabs.Count)];

                // �÷��̾� �ֺ� ���� ��ġ ���
                Vector2 randomPoint = Random.insideUnitCircle.normalized * Random.Range(minSpawnDistance, maxSpawnDistance);
                Vector3 spawnPosition = playerTransform.position + new Vector3(randomPoint.x, 0, randomPoint.y);

                // TODO: NavMesh �� ���ø����� ���� ������ ��ġ���� Ȯ���ϴ� ���� �߰��ϸ� �� ����

                GameObject newMonster = Instantiate(monsterToSpawn, spawnPosition, Quaternion.identity);
                activeMonsters.Add(newMonster);
                monstersSpawnedThisWave++;
            }
            yield return new WaitForSeconds(spawnInterval);
        }
        Debug.Log("�� ���̺��� ��� ���Ͱ� �����Ǿ����ϴ�.");
    }

    private IEnumerator BossFightCoroutine()
    {
        currentState = GameState.BossFight;
        Debug.Log("������ ����!");

        Vector3 bossSpawnPosition = playerTransform.position + (playerTransform.forward * 10f);
        GameObject boss = Instantiate(bossPrefab, bossSpawnPosition, Quaternion.identity);
        activeMonsters.Add(boss);

        while (activeMonsters.Count > 0)
        {
            yield return null;
        }

        Debug.Log("���� óġ!");
    }

    public void OnMonsterKilled(GameObject monster)
    {
        if (currentState == GameState.WaveInProgress)
        {
            monstersKilledThisWave++;
        }
        activeMonsters.Remove(monster);
        Debug.Log($"���� óġ! ���� ��ǥ: {waves[currentWaveIndex].totalMonstersToSpawn - monstersKilledThisWave}");
    }
}

