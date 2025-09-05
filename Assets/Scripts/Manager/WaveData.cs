using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Wave Data", menuName = "Stage/Create Wave Data")]
public class WaveData : ScriptableObject
{
    [Header("웨이브 구성")]
    public List<GameObject> monsterPrefabs;

    [Header("처치 마릿수")]
    public int totalMonstersToSpawn;
}
