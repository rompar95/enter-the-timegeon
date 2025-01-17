﻿using UnityEngine;

public class PlayerTimestop : ChronosMonoBehaviour
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private AbilityUI _abilityUI;
    [SerializeField] private GameObject _timestop;   
    [SerializeField] private float _timestopCooldown = 5f;
    private float _timestopCooldownTimer;

    [SerializeField] private Sprite[] _icons;
    [SerializeField] private UnityEngine.UI.Image _image;

    private void Update()
    {
        if (!_player.Stats.IsAlive() || _player.State != PlayerState.UNDER_CONTROL || _player.IsInDialog || _player.IsInFinalDialog || _player.IsInPortal)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            _image.sprite = _icons[1];
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            _image.sprite = _icons[0];
        }

        if (_timestopCooldownTimer > 0)
        {
            _timestopCooldownTimer -= ChronosTime.deltaTime;
            _abilityUI.CooldownTimer.text = Mathf.Floor(_timestopCooldownTimer).ToString();
        }
        else
        {
            _timestopCooldownTimer = 0;
            _abilityUI.Cooldown.gameObject.SetActive(false);
        }

        CreateTimestop();
    }

    private void CreateTimestop()
    {
        if (!_player.Stats.HasShards())
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && _timestopCooldownTimer == 0 && _player.State == PlayerState.UNDER_CONTROL)
        {
            _player.Stats.TakeShard();
            Vector2 pos = new Vector2(_player.MousePos.x, _player.MousePos.y);
            GameObject timestopObj = MF_AutoPool.Spawn(_timestop, pos, Quaternion.identity);
            timestopObj.GetComponent<Timestop>().OnSpawned();
            _timestopCooldownTimer = _timestopCooldown;
            ChronosTime.Plan(0.1f, delegate() { _abilityUI.Cooldown.gameObject.SetActive(true); });
        }
    }
}
