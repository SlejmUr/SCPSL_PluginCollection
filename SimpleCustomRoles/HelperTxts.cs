using Exiled.API.Features;
using System.Collections.Generic;
using System.IO;

namespace SimpleCustomRoles
{
    internal class HelperTxts
    {
        static string Dir = Path.Combine(Path.Combine(Paths.Configs, "SimpleCustomRoles"), "HelpTXT");
        static Dictionary<string, string> TxtPair = new Dictionary<string, string>()
        {
            { "EffectTypes", "AmnesiaItems,\r\nAmnesiaVision,\r\nAsphyxiated,\r\nBleeding,\r\nBlinded,\r\nBurned,\r\nConcussed,\r\nCorroding,\r\nDeafened,\r\nDecontaminating,\r\nDisabled,\r\nEnsnared,\r\nExhausted,\r\nFlashed,\r\nHemorrhage,\r\nInvigorated,\r\nBodyshotReduction,\r\nPoisoned,\r\nScp207,\r\nInvisible,\r\nSinkHole,\r\nDamageReduction,\r\nMovementBoost,\r\nRainbowTaste,\r\nSeveredHands,\r\nStained,\r\nVitality,\r\nHypothermia,\r\nScp1853,\r\nCardiacArrest,\r\nInsufficientLighting,\r\nSoundtrackMute,\r\nSpawnProtected,\r\nTraumatized,\r\nAntiScp207,\r\nScanned,\r\nPocketCorroding,\r\nSilentWalk,\r\nMarshmallow,\r\nStrangled,\r\nGhostly" },
            { "ItemTypes", "None,\r\nKeycardJanitor,\r\nKeycardScientist,\r\nKeycardResearchCoordinator,\r\nKeycardZoneManager,\r\nKeycardGuard,\r\nKeycardMTFPrivate,\r\nKeycardContainmentEngineer,\r\nKeycardMTFOperative,\r\nKeycardMTFCaptain,\r\nKeycardFacilityManager,\r\nKeycardChaosInsurgency,\r\nKeycardO5,\r\nRadio,\r\nGunCOM15,\r\nMedkit,\r\nFlashlight,\r\nMicroHID,\r\nSCP500,\r\nSCP207,\r\nAmmo12gauge,\r\nGunE11SR,\r\nGunCrossvec,\r\nAmmo556x45,\r\nGunFSP9,\r\nGunLogicer,\r\nGrenadeHE,\r\nGrenadeFlash,\r\nAmmo44cal,\r\nAmmo762x39,\r\nAmmo9x19,\r\nGunCOM18,\r\nSCP018,\r\nSCP268,\r\nAdrenaline,\r\nPainkillers,\r\nCoin,\r\nArmorLight,\r\nArmorCombat,\r\nArmorHeavy,\r\nGunRevolver,\r\nGunAK,\r\nGunShotgun,\r\nSCP330,\r\nSCP2176,\r\nSCP244a,\r\nSCP244b,\r\nSCP1853,\r\nParticleDisruptor,\r\nGunCom45,\r\nSCP1576,\r\nJailbird,\r\nAntiSCP207,\r\nGunFRMG0,\r\nGunA7,\r\nLantern" },
            { "LocationSpawnPrioritys", "None,\r\nSpawnZone,\r\nSpawnRoom,\r\nExactPosition,\r\nFullRandom" },
            { "RoleTypeIds", "None,\r\nScp173,\r\nClassD,\r\nSpectator,\r\nScp106,\r\nNtfSpecialist,\r\nScp049,\r\nScientist,\r\nScp079,\r\nChaosConscript,\r\nScp096,\r\nScp0492,\r\nNtfSergeant,\r\nNtfCaptain,\r\nNtfPrivate,\r\nTutorial,\r\nFacilityGuard,\r\nScp939,\r\nCustomRole,\r\nChaosRifleman,\r\nChaosMarauder,\r\nChaosRepressor,\r\nOverwatch,\r\nFilmmaker,\r\nScp3114" },
            { "RoomTypes", "Unknown,\r\nLczArmory,\r\nLczCurve,\r\nLczStraight,\r\nLcz914,\r\nLczCrossing,\r\nLczTCross,\r\nLczCafe,\r\nLczPlants,\r\nLczToilets,\r\nLczAirlock,\r\nLcz173,\r\nLczClassDSpawn,\r\nLczCheckpointB,\r\nLczGlassBox,\r\nLczCheckpointA,\r\nHcz079,\r\nHczEzCheckpointA,\r\nHczEzCheckpointB,\r\nHczArmory,\r\nHcz939,\r\nHczHid,\r\nHcz049,\r\nHczCrossing,\r\nHcz106,\r\nHczNuke,\r\nHczTesla,\r\nHczServers,\r\nHczTCross,\r\nHczCurve,\r\nHcz096,\r\nEzVent,\r\nEzIntercom,\r\nEzGateA,\r\nEzDownstairsPcs,\r\nEzCurve,\r\nEzPcs,\r\nEzCrossing,\r\nEzCollapsedTunnel,\r\nEzConference,\r\nEzStraight,\r\nEzCafeteria,\r\nEzUpstairsPcs,\r\nEzGateB,\r\nEzShelter,\r\nPocket,\r\nSurface,\r\nHczStraight,\r\nEzTCross,\r\nLcz330,\r\nEzCheckpointHallway,\r\nHczTestRoom,\r\nHczElevatorA,\r\nHczElevatorB" },
            { "ZoneTypes", "Unspecified,\r\nLightContainment,\r\nHeavyContainment,\r\nEntrance,\r\nSurface,\r\nOther" },
            { "AmmoTypes", "None,\r\nNato556,\r\nNato762,\r\nNato9,\r\nAmmo12Gauge,\r\nAmmo44Cal" },
            { "SpawnableTeamTypes", "None,\r\nChaosInsurgency,\r\nNineTailedFox" },
            { "Teams", "SCPs,\r\nFoundationForces,\r\nChaosInsurgency,\r\nScientists,\r\nClassD,\r\nDead,\r\nOtherAlive" },
            { "CandyKindIds", "None,\r\nRainbow,\r\nYellow,\r\nPurple,\r\nRed,\r\nGreen,\r\nBlue,\r\nPink" },
            { "DamageType", "Unknown,\r\nFalldown,\r\nWarhead,\r\nDecontamination,\r\nAsphyxiation,\r\nPoison,\r\nBleeding,\r\nFirearm,\r\nMicroHid,\r\nTesla,\r\nScp,\r\nExplosion,\r\nScp018,\r\nScp207,\r\nRecontainment,\r\nCrushed,\r\nFemurBreaker,\r\nPocketDimension,\r\nFriendlyFireDetector,\r\nSeveredHands,\r\nCustom,\r\nScp049,\r\nScp096,\r\nScp173,\r\nScp939,\r\nScp0492,\r\nScp106,\r\nCrossvec,\r\nLogicer,\r\nRevolver,\r\nShotgun,\r\nAK,\r\nCom15,\r\nCom18,\r\nFsp9,\r\nE11Sr,\r\nHypothermia,\r\nParticleDisruptor,\r\nCardiacArrest,\r\nCom45,\r\nJailbird,\r\nFrmg0,\r\nA7,\r\nScp3114,\r\nStrangled,\r\nMarshmallow" }
            //{ "", "" },
        };
        public static void WriteAll()
        {
            if (!Directory.Exists(Dir))
                Directory.CreateDirectory(Dir);
            foreach (var item in TxtPair)
            {
                var txt = Path.Combine(Dir, item.Key);
                if (!File.Exists(txt + ".txt"))
                {
                    File.WriteAllText(txt + ".txt", item.Value);
                }
            }
            

        }
    }
}
