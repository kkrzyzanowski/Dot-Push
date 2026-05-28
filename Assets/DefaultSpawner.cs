using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DefaultSpawner : Spawner
{
    protected override void ApplyEffect()
    {
        PlayerMove.playerMoveInstance.ActiveTouch(transform.position);
        ConfigurationGame.ConfigurationGameInstance.AddPoints();
    }
}
