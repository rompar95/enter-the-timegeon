﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTimestop : ChronosMonoBehaviour
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private GameObject _timestop;
    private float _nextTimestopTime;
    [SerializeField] private float _timestopCooldown = 5f;

    private void Update()
    {
        if (!_player.Stats.IsAlive() || _player.State != PlayerState.UNDER_CONTROL)
        {
            return;
        }

        CreateTimestop();
    }

    private void CreateTimestop()
    {
        if (Input.GetKeyDown(KeyCode.F) && ChronosTime.unscaledTime >= _nextTimestopTime)
        {
            Vector2 pos = new Vector2(_player.MousePos.x, _player.MousePos.y);
            GameObject timestopObj = MF_AutoPool.Spawn(_timestop, pos, Quaternion.identity);
            timestopObj.GetComponent<Timestop>().OnSpawned();
            _nextTimestopTime = ChronosTime.unscaledTime + _timestopCooldown;
        }
    }
}