using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

\
#region *** GameManager 하는일 정리 ***
//싱글톤
//GameFlow 코루틴
//GameState enum
//웨이브 및 몬스터 관리
//Damagable 및 날씨관리
//UI관리 (재시작 등등)
//디버그용 'T키'
//exp 경험치 관련
#endregion

public class GameManager : Singleton<GameManager>
{
    [Header("스테이지 설정")]
    public List<WaveData> waves;
    public GameObject bossPrefab;

    [Header("플레이어 참조")]
    public Transform playerTransform;

    [Header("게임플레이 설정")]
    public float spawnInterval = 1.5f;
    public int maxMonstersOnField = 20;
    public float damageZoneSpawnRadius = 15f;
    public float damageZoneSpawnDelay = 3f;

    [Header("날씨 설정")]
    public Light sunLight;
    public Material wave1Skybox, wave2Skybox, wave3Skybox;
    public GameObject wave2WeatherVFX, wave3WeatherVFX;
    private GameObject currentWeatherVFXInstance;

    // --- 내부 상태 변수들 ---
    private int currentWaveIndex = 0;
    private int monstersSpawnedThisWave = 0;
    private int monstersKilledThisWave = 0;
    private List<GameObject> activeMonsters = new List<GameObject>();

    private List<GameObject> activeDamageZones = new List<GameObject>();


    public GameObject _gameOverCanvas;

    [Header("UI 설정")]
    public TextMeshProUGUI _countdownText;
    public GameObject waveInfoPanel; 
    public TextMeshProUGUI waveTitleText;

    [Header("날씨 설정")]
    public Light sunLight;
    public Material wave1Skybox; 
    public Material wave2Skybox; 
    public Material wave3Skybox; 
    public GameObject wave2WeatherVFX;
    public GameObject wave3WeatherVFX; 
    private GameObject currentWeatherVFXInstance;

    [Header("플로팅 텍스트 설정")]
    public GameObject floatingTextPrefab; 
    public Canvas mainCanvas; 

    public enum GameState { InitialWait, WaveInProgress, WaveComplete, BossFight, GameWon }
    public GameState currentState { get; private set; }

    #region Unity Lifecycle & Game Flow
    void Start()
    {
        SetWeatherForWave(0);
        UIManager.Instance.SetActiveCountdown(false);
        StartCoroutine(GameFlow());
    }

    void Update()
    {
        // 디버그용 'T'키
        if (Input.GetKeyDown(KeyCode.T) && activeMonsters.Count > 0)
        {
            activeMonsters[0].GetComponent<EnemyController>()?.TakeDamage(1000);
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
        SetWeatherForWave(0); 

        if (_countdownText != null)
        {
            _countdownText.gameObject.SetActive(false);
        }
        StartCoroutine(GameFlow());
    }


    private IEnumerator GameFlow()
    {
        currentState = GameState.InitialWait;

        UIManager.Instance.SetActiveCountdown(true);
        for (int i = 3; i > 0; i--)
        {
            UIManager.Instance.SetCountdownText(i.ToString());
            yield return new WaitForSeconds(1f);
        }
        UIManager.Instance.SetActiveCountdown(false);

        while (currentWaveIndex < waves.Count)
        {
            yield return StartCoroutine(WaveCoroutine());
            currentWaveIndex++;
        }

        yield return StartCoroutine(BossFightCoroutine());
        currentState = GameState.GameWon;
    }
    #endregion

    #region Wave & Spawn Logic
    private IEnumerator WaveCoroutine()
    {
        SetWeatherForWave(currentWaveIndex);
        WaveData currentWave = waves[currentWaveIndex];
        monstersSpawnedThisWave = 0;
        monstersKilledThisWave = 0;
        activeMonsters.Clear();

        UIManager.Instance.ShowWaveInfo(currentWaveIndex);
        yield return new WaitForSeconds(3f);
        UIManager.Instance.HideWaveInfo();

        currentState = GameState.WaveInProgress;
        StartCoroutine(SpawnMonsters(currentWave));
        StartCoroutine(SpawnDamageZoneCoroutine());

        while (monstersKilledThisWave < currentWave.totalMonstersToSpawn)
        {
            yield return null;
        }

        currentState = GameState.WaveComplete;
        ClearAllDamageZones();
    }

    private IEnumerator SpawnMonsters(WaveData wave)
    {
        while (monstersSpawnedThisWave < wave.totalMonstersToSpawn)
        {
            if (activeMonsters.Count < maxMonstersOnField)
            {
                GameObject monsterPrefab = wave.monsterPrefabs[Random.Range(0, wave.monsterPrefabs.Count)];
                Vector2 randomPoint = Random.insideUnitCircle.normalized * Random.Range(5f, 15f); // min/max 스폰 거리 사용
                Vector3 spawnPosition = playerTransform.position + new Vector3(randomPoint.x, 0, randomPoint.y);

                GameObject newMonster = SpawnManager.Instance.SpawnFromPool(monsterPrefab.name, spawnPosition, Quaternion.identity);
                if (newMonster != null)
                {
                    activeMonsters.Add(newMonster);
                    monstersSpawnedThisWave++;
                }
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private IEnumerator SpawnDamageZoneCoroutine()
    {
        yield return new WaitForSeconds(damageZoneSpawnDelay);
        if (playerTransform != null)
        {
            Vector2 randomCircle = Random.insideUnitCircle.normalized * damageZoneSpawnRadius;
            Vector3 spawnPosition = playerTransform.position + new Vector3(randomCircle.x, 0, randomCircle.y);
            GameObject newZone = SpawnManager.Instance.SpawnFromPool("DamageZone", spawnPosition, Quaternion.identity);
            if (newZone != null) activeDamageZones.Add(newZone);
        }

        yield return new WaitForSeconds(5f); // 두 번째 스폰
        if (playerTransform != null)
        {
            Vector2 randomCircle = Random.insideUnitCircle.normalized * damageZoneSpawnRadius;
            Vector3 spawnPosition = playerTransform.position + new Vector3(randomCircle.x, 0, randomCircle.y);
            GameObject newZone = SpawnManager.Instance.SpawnFromPool("DamageZone", spawnPosition, Quaternion.identity);
            if (newZone != null) activeDamageZones.Add(newZone);
        }
    }

    public void OnMonsterKilled(GameObject monster)
    {
        monstersKilledThisWave++;
        activeMonsters.Remove(monster);
    }
    #endregion

    #region Other Management
    void ClearAllDamageZones()
    {
        foreach (GameObject zone in activeDamageZones)
        {
            SpawnManager.Instance.ReturnToPool("DamageZone", zone);
        }
        activeDamageZones.Clear();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void SetWeatherForWave(int waveIndex) { /* ... 날씨 로직은 여기에 유지 ... */ }
    private IEnumerator BossFightCoroutine() { /* ... 보스전 로직은 여기에 유지 ... */ }
    #endregion
}