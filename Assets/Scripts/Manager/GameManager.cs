using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("스테이지 설정")]
    public List<WaveData> waves;
    public GameObject bossPrefab;

    [Header("플레이어 및 스폰 설정")]
    [Tooltip("플레이어의 Transform을 연결하여 스폰 위치를 계산합니다.")]
    public Transform playerTransform;
    public float minSpawnDistance = 5f;
    public float maxSpawnDistance = 15f;

    [Header("스폰 타이밍 설정")]
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
        // 1. 초기 준비 시간
        currentState = GameState.InitialWait;
        Debug.Log("스테이지 시작! 3초 대기합니다.");
        yield return new WaitForSeconds(initialWaitTime);

        // 웨이브 진행
        while (currentWaveIndex < waves.Count)
        {
            yield return StartCoroutine(WaveCoroutine());
            currentWaveIndex++;
        }

        // 3. 보스전
        yield return StartCoroutine(BossFightCoroutine());

        currentState = GameState.GameWon;
        Debug.Log("게임 클리어!");
    }

    private IEnumerator WaveCoroutine()
    {
        WaveData currentWave = waves[currentWaveIndex];
        monstersSpawnedThisWave = 0;
        monstersKilledThisWave = 0;
        activeMonsters.Clear();
        currentState = GameState.WaveInProgress;

        Debug.Log($"웨이브 {currentWaveIndex + 1} 시작! 목표: {currentWave.totalMonstersToSpawn}마리");

        // 몬스터 스폰 시작
        StartCoroutine(SpawnMonsters(currentWave));

        // 해당 웨이브의 모든 몬스터를 처치할 때까지 대기
        while (monstersKilledThisWave < currentWave.totalMonstersToSpawn)
        {
            yield return null;
        }

        currentState = GameState.WaveComplete;
        Debug.Log($"웨이브 {currentWaveIndex + 1} 클리어!");
    }

    private IEnumerator SpawnMonsters(WaveData wave)
    {
        while (monstersSpawnedThisWave < wave.totalMonstersToSpawn)
        {
            if (activeMonsters.Count < maxMonstersOnField)
            {
                // 스폰할 몬스터 랜덤 선택
                GameObject monsterToSpawn = wave.monsterPrefabs[Random.Range(0, wave.monsterPrefabs.Count)];

                // 플레이어 주변 랜덤 위치 계산
                Vector2 randomPoint = Random.insideUnitCircle.normalized * Random.Range(minSpawnDistance, maxSpawnDistance);
                Vector3 spawnPosition = playerTransform.position + new Vector3(randomPoint.x, 0, randomPoint.y);

                // TODO: NavMesh 위 샘플링으로 스폰 가능한 위치인지 확인하는 로직 추가하면 더 좋음

                GameObject newMonster = Instantiate(monsterToSpawn, spawnPosition, Quaternion.identity);
                activeMonsters.Add(newMonster);
                monstersSpawnedThisWave++;
            }
            yield return new WaitForSeconds(spawnInterval);
        }
        Debug.Log("이 웨이브의 모든 몬스터가 스폰되었습니다.");
    }

    private IEnumerator BossFightCoroutine()
    {
        currentState = GameState.BossFight;
        Debug.Log("보스전 시작!");

        Vector3 bossSpawnPosition = playerTransform.position + (playerTransform.forward * 10f);
        GameObject boss = Instantiate(bossPrefab, bossSpawnPosition, Quaternion.identity);
        activeMonsters.Add(boss);

        while (activeMonsters.Count > 0)
        {
            yield return null;
        }

        Debug.Log("보스 처치!");
    }

    public void OnMonsterKilled(GameObject monster)
    {
        if (currentState == GameState.WaveInProgress)
        {
            monstersKilledThisWave++;
        }
        activeMonsters.Remove(monster);
        Debug.Log($"몬스터 처치! 남은 목표: {waves[currentWaveIndex].totalMonstersToSpawn - monstersKilledThisWave}");
    }
}

