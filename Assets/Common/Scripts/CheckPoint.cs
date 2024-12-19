using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICheckpoint
{
    void SetCheckpoint(Vector3 position);
    void Respawn();
}
