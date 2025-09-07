using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    private Player _player;
    public Player Player
    {
        get
        {
            if (_player == null)
                _player = ResourceManager.Instance.CreateCharacter<Player>(Prefab.Player);
            return _player;
        }
    }
}
