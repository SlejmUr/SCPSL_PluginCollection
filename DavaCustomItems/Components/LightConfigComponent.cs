using DavaCustomItems.Configs;
using UnityEngine;

namespace DavaCustomItems.Components;

public class LightConfigComponent : MonoBehaviour
{
    public int LightId { get; set; }
    public LightConfig LightConfig { get; set; }
    public bool IsSpawned { get; set; }
}
