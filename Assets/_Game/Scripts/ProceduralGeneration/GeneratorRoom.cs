using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GeneratorRoom
{
    public Vector2i location;
    public Vector2i size;
    public bool isSafe = false;
    public List<GeneratorChunk> chunks = new List<GeneratorChunk>();
}
