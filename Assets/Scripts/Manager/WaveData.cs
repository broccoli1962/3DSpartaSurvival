using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Wave Data", menuName = "Stage/Create Wave Data")]
public class WaveData : ScriptableObject
{
    [Header("���̺� ����")]
    public List<GameObject> monsterPrefabs;

    [Header("óġ ������")]
    public int totalMonstersToSpawn;
}
