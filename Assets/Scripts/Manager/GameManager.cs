using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [Header("스테이지 설정")]
    public List<WaveData> waves;
    public GameObject bossPrefab;

    [Header("플레이어 및 스폰 설정")]
    public Transform playerTransform;
    public float minSpawnDistance = 5f;
    public float maxSpawnDistance = 15f;

    [Header("스폰 타이밍 설정")]
    public float initialWaitTime = 3f;
    public float spawnInterval = 1.5f;
    public int maxMonstersOnField = 20;

    [Header("데미지 존 설정")]
    public GameObject damageZonePrefab;
    public float damageZoneSpawnRadius = 15f;
    public float damageZoneSpawnDelay = 3f;
    private List<GameObject> activeDamageZones = new List<GameObject>();

    private int currentWaveIndex = 0;
    private int monstersSpawnedThisWave = 0;
    private int monstersKilledThisWave = 0;
    private List<GameObject> activeMonsters = new List<GameObject>();

    public GameObject _gameOverCanvas;

    [Header("UI 설정")]
    public TextMeshProUGUI _countdownText;
    public GameObject waveInfoPanel; 
    public TextMeshProUGUI waveTitleText;

    public enum GameState { InitialWait, WaveInProgress, WaveComplete, BossFight, GameWon }
    public GameState currentState { get; private set; }

    #region 게임오버 함수
    public void ShowGameOverScreen()
    {
        if (_gameOverCanvas != null)
        {
            _gameOverCanvas.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (activeMonsters.Count > 0)
            {
                GameObject targetMonster = activeMonsters[0];

                EnemyController enemyController = targetMonster.GetComponent<EnemyController>();

                if (enemyController != null)
                {
                    enemyController.TakeDamage(1000);
                    Debug.Log(targetMonster.name + "에게 1000의 디버그 데미지를 입혔습니다!");
                }
            }
            else
            {
                Debug.Log("공격할 몬스터가 없습니다.");
            }
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    #endregion

    void Start()
    {
        if (_countdownText != null)
        {
            _countdownText.gameObject.SetActive(false);
        }
        StartCoroutine(GameFlow());
    }

    private IEnumerator GameFlow()
    {
        currentState = GameState.InitialWait;

        if (_countdownText != null)
        {
            _countdownText.gameObject.SetActive(true);
            for (int i = 3; i > 0; i--)
            {
                _countdownText.text = i.ToString();
                yield return new WaitForSeconds(1f);
            }
            _countdownText.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Countdown Text가 할당되지 않아 3초 대기합니다.");
            yield return new WaitForSeconds(initialWaitTime);
        }

        // 웨이브 진행
        while (currentWaveIndex < waves.Count)
        {
            yield return StartCoroutine(WaveCoroutine());
            currentWaveIndex++;
        }

        yield return StartCoroutine(BossFightCoroutine());

        currentState = GameState.GameWon;
        Debug.Log("게임 클리어!");
    }

    #region Wave 관련 내용(UI 패널/카운트 다운)
    private IEnumerator WaveCoroutine()
    {
        WaveData currentWave = waves[currentWaveIndex];
        monstersSpawnedThisWave = 0;
        monstersKilledThisWave = 0;
        activeMonsters.Clear();

        ShowWaveInfo(currentWave);

        yield return new WaitForSeconds(3f);

        HideWaveInfo();

        currentState = GameState.WaveInProgress;
        Debug.Log($"웨이브 {currentWaveIndex + 1} 시작! 목표: {currentWave.totalMonstersToSpawn}마리");

        StartCoroutine(SpawnMonsters(currentWave));
        StartCoroutine(SpawnDamageZoneCoroutine());

        while (monstersKilledThisWave < currentWave.totalMonstersToSpawn)
        {
            yield return null;
        }

        currentState = GameState.WaveComplete;
        Debug.Log($"웨이브 {currentWaveIndex + 1} 클리어!");

        ClearAllDamageZones();
    }
    void ShowWaveInfo(WaveData wave)
    {
        if (waveInfoPanel != null)
        {
            waveTitleText.text = $"Wave {currentWaveIndex + 1}";

            waveInfoPanel.SetActive(true);
        }
    }

    void HideWaveInfo()
    {
        if (waveInfoPanel != null)
        {
            waveInfoPanel.SetActive(false);
        }
    }
    #endregion

    private IEnumerator SpawnMonsters(WaveData wave)
    {
        while (monstersSpawnedThisWave < wave.totalMonstersToSpawn)
        {
            if (activeMonsters.Count < maxMonstersOnField)
            {
                GameObject monsterToSpawn = wave.monsterPrefabs[Random.Range(0, wave.monsterPrefabs.Count)];

                Vector2 randomPoint = Random.insideUnitCircle.normalized * Random.Range(minSpawnDistance, maxSpawnDistance);
                Vector3 spawnPosition = playerTransform.position + new Vector3(randomPoint.x, 0, randomPoint.y);

                GameObject newMonster = Instantiate(monsterToSpawn, spawnPosition, Quaternion.identity);
                activeMonsters.Add(newMonster);
                monstersSpawnedThisWave++;

                int remainingSpawns = wave.totalMonstersToSpawn - monstersSpawnedThisWave;
                Debug.Log($"{monsterToSpawn.name} 1마리 스폰 / 남은 스폰 마릿수: {remainingSpawns}");
            }
            yield return new WaitForSeconds(spawnInterval);
        }
        Debug.Log("이 웨이브의 모든 몬스터가 스폰되었습니다.");
    }

    private IEnumerator SpawnDamageZoneCoroutine()
    {
        yield return new WaitForSeconds(damageZoneSpawnDelay); 

        if (playerTransform != null) 
        {
            Vector2 randomCircle1 = Random.insideUnitCircle.normalized * damageZoneSpawnRadius;
            Vector3 spawnPosition1 = playerTransform.position + new Vector3(randomCircle1.x, 0, randomCircle1.y);

            GameObject newZone1 = Instantiate(damageZonePrefab, spawnPosition1, Quaternion.identity);
            activeDamageZones.Add(newZone1);
        }

        yield return new WaitForSeconds(5f); 

        if (playerTransform != null) 
        {
            Vector2 randomCircle2 = Random.insideUnitCircle.normalized * damageZoneSpawnRadius;
            Vector3 spawnPosition2 = playerTransform.position + new Vector3(randomCircle2.x, 0, randomCircle2.y);

            GameObject newZone2 = Instantiate(damageZonePrefab, spawnPosition2, Quaternion.identity);
            activeDamageZones.Add(newZone2);
        }
    }

    void ClearAllDamageZones()
    {
        foreach (GameObject zone in activeDamageZones)
        {
            Destroy(zone);
        }
        activeDamageZones.Clear();
        Debug.Log("모든 Damage Zone 제거 완료!");
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