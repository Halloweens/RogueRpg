using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GeneratorCorridor {

    public Vector2i location;
    public Vector2i size;
    public List<GeneratorChunk> chunks = new List<GeneratorChunk>();
}
