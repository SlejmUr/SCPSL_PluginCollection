using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Random = UnityEngine.Random;
using UnityEngine;
using Exiled.API.Enums;

namespace ItemSpawner;

internal class LootSpawner
{
    public static bool Spawn(ItemType Item, SpawnType spawnType, ArraySegment<string> arguments, out string response)
    {
        ushort ammo = 0;
        Vector3 offset = Vector3.zero;
        bool RandomVelocity = false;
        response = "Something not good!";
        switch (spawnType)
        {
            case SpawnType.Location:
                if (arguments.Count < 5)
                {
                    response = "Settings require X Y Z, you didnt specified that!";
                    return false;
                }
                if (!float.TryParse(arguments.At(2), out float loc_x))
                {
                    response = "X cannot be parsed as float!";
                    return false;
                }
                if (!float.TryParse(arguments.At(3), out float loc_y))
                {
                    response = "Y cannot be parsed as float!";
                    return false;
                }
                if (!float.TryParse(arguments.At(4), out float loc_z))
                {
                    response = "Z cannot be parsed as float!";
                    return false;
                }
                if (!GetAdvancedSettings(arguments, 5, out response, out ammo, out offset, out RandomVelocity))
                {
                    return false;
                }
                return SpawnLocation(Item, new Vector3(loc_x, loc_y, loc_z), ammo, offset, RandomVelocity, out response);
            case SpawnType.Room:
                if (arguments.Count < 2)
                {
                    response = "Settings require RoomId, you didnt specified that!";
                    return false;
                }
                if (!Enum.TryParse(arguments.At(2), out RoomType room))
                {
                    response = "RoomType cannot be parsed!";
                    return false;
                }
                if (!GetAdvancedSettings(arguments, 3, out response, out ammo, out offset, out RandomVelocity))
                {
                    return false;
                }
                return SpawnLocation(Item, Room.Get(room).Position + Vector3.up, ammo, offset, RandomVelocity, out response);
            case SpawnType.Player:
                if (arguments.Count < 2)
                {
                    response = "Settings require PlayerId, you didnt specified that!";
                    return false;
                }
                if (!int.TryParse(arguments.At(2), out int playerId))
                {
                    response = "PlayerId cannot be parsed!";
                    return false;
                }
                if (!GetAdvancedSettings(arguments, 3, out response, out ammo, out offset, out RandomVelocity))
                {
                    return false;
                }
                return SpawnLocation(Item, Player.Get(playerId).Position, ammo, offset, RandomVelocity, out response);
            default:
                break;
        }
        return false;
    }

    public static bool GetAdvancedSettings(ArraySegment<string> arguments, int parameter_end, out string response, out ushort ammo, out Vector3 offset, out bool RandomVelocity)
    {
        ammo = 0;
        offset = Vector3.zero;
        RandomVelocity = false;
        response = string.Empty;
        if (arguments.Count >= (parameter_end + 1))
        {
            if (!ushort.TryParse(arguments.At(parameter_end), out ammo))
            {
                response = "Ammo cannot be parsed!";
                return false;
            }
        }
        if (arguments.Count >= (parameter_end + 4))
        {
            if (!float.TryParse(arguments.At(parameter_end + 1), out float loc_x))
            {
                response = "X cannot be parsed as float!";
                return false;
            }
            if (!float.TryParse(arguments.At(parameter_end + 2), out float loc_y))
            {
                response = "Y cannot be parsed as float!";
                return false;
            }
            if (!float.TryParse(arguments.At(parameter_end + 3), out float loc_z))
            {
                response = "Z cannot be parsed as float!";
                return false;
            }
            offset = new Vector3(loc_x, loc_y, loc_z);
        }
        if (arguments.Count >= (parameter_end + 5))
        {
            if (!bool.TryParse(arguments.At(parameter_end + 4), out RandomVelocity))
            {
                response = "UseRandomVelocity cannot be parsed!";
                return false;
            }
        }
        return true;
    }

    public static bool SpawnLocation(ItemType Item, Vector3 location, ushort ammo, Vector3 offset, bool RandomVelocity, out string response)
    {
        Log.Debug($"SpawnLocation {Item} {location} {ammo} {offset} {RandomVelocity}");
        Pickup pickup = Pickup.CreateAndSpawn(Item, location + offset);
        Log.Debug("Pickup spawned");
        if (pickup is AmmoPickup ammopickup)
        {
            ammopickup.Ammo = ammo;
        }
        if (RandomVelocity)
        {
            if (pickup.GameObject != null && pickup.GameObject.TryGetComponent(out Rigidbody rigidbody))
                rigidbody.velocity = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), Random.Range(-5, 5));
        }
        response = "Pickup spawned!";
        return true;
    }
}
