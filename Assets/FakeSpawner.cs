using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FakeSpawner : Spawner
{
    protected override void ApplyEffect()
    {
        PlayerMove.playerMoveInstance.ActiveTouch(transform.position);
        ConfigurationGame.ConfigurationGameInstance.SubtractPoints();
    }
}
