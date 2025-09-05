using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

\
#region *** GameManager �ϴ��� ���� ***
//�̱���
//GameFlow �ڷ�ƾ
//GameState enum
//���̺� �� ���� ����
//Damagable �� ��������
//UI���� (����� ���)
//����׿� 'TŰ'
//exp ����ġ ����
#endregion

public class GameManager : Singleton<GameManager>
{
    [Header("�������� ����")]
    public List<WaveData> waves;
    public GameObject bossPrefab;

    [Header("�÷��̾� ����")]
    public Transform playerTransform;

    [Header("�����÷��� ����")]
    public float spawnInterval = 1.5f;
    public int maxMonstersOnField = 20;
    public float damageZoneSpawnRadius = 15f;
    public float damageZoneSpawnDelay = 3f;

    [Header("���� ����")]
    public Light sunLight;
    public Material wave1Skybox, wave2Skybox, wave3Skybox;
    public GameObject wave2WeatherVFX, wave3WeatherVFX;
    private GameObject currentWeatherVFXInstance;

    // --- ���� ���� ������ ---
    private int currentWaveIndex = 0;
    private int monstersSpawnedThisWave = 0;
    private int monstersKilledThisWave = 0;
    private List<GameObject> activeMonsters = new List<GameObject>();

    private List<GameObject> activeDamageZones = new List<GameObject>();


    public GameObject _gameOverCanvas;

    [Header("UI ����")]
    public TextMeshProUGUI _countdownText;
    public GameObject waveInfoPanel; 
    public TextMeshProUGUI waveTitleText;

    [Header("���� ����")]
    public Light sunLight;
    public Material wave1Skybox; 
    public Material wave2Skybox; 
    public Material wave3Skybox; 
    public GameObject wave2WeatherVFX;
    public GameObject wave3WeatherVFX; 
    private GameObject currentWeatherVFXInstance;

    [Header("�÷��� �ؽ�Ʈ ����")]
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
        // ����׿� 'T'Ű
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
                Vector2 randomPoint = Random.insideUnitCircle.normalized * Random.Range(5f, 15f); // min/max ���� �Ÿ� ���
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

        yield return new WaitForSeconds(5f); // �� ��° ����
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

    void SetWeatherForWave(int waveIndex) { /* ... ���� ������ ���⿡ ���� ... */ }
    private IEnumerator BossFightCoroutine() { /* ... ������ ������ ���⿡ ���� ... */ }
    #endregion
}