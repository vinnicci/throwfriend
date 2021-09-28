using Godot;
using System;

public static class Global
{
    public const short BIT_MASK_ENEMY = 0;
    public const short BIT_MASK_WALL = 1;
    public const short BIT_MASK_PLAYER = 2;
    public const short BIT_MASK_WEAPON = 3;
    public const short BIT_MASK_SEETHROUGH_WALL = 4;
    public const int CCD_MAX = 1000000;
    public const String PLAYER_SCN = "res://scenes/player/Player.tscn";
    public const String SAVE_DIR = "user://save/";


    //enemy sets for random enemy spawner
    public static readonly String[] EASY_ALL_ROUNDERS = {
        "res://scenes/enemies/all rounder/Melee.tscn",
        "res://scenes/enemies/all rounder/MeleeShielded.tscn",
        "res://scenes/enemies/all rounder/MeleeBlobnadier.tscn",
        "res://scenes/enemies/all rounder/BlobnadierShielded.tscn",
    };
    public static readonly String[] MEDIUM_ALL_ROUNDERS = {
        "res://scenes/enemies/all rounder/Ranged.tscn",
        "res://scenes/enemies/all rounder/RangedBlobnadier.tscn",
        "res://scenes/enemies/all rounder/ParaBlobCarrier.tscn",
        "res://scenes/enemies/all rounder/SlowBlobCarrier.tscn",
        "res://scenes/enemies/all rounder/TeleBlobCarrier.tscn",
    };
    public static readonly String[] HARD_ALL_ROUNDERS = {
        "res://scenes/enemies/all rounder/DualRanged.tscn",
        "res://scenes/enemies/all rounder/RangedShielded.tscn",
        "res://scenes/enemies/all rounder/MeleeRanged.tscn",
    };
    public static readonly String[] EASY_CHARGERS = {
        "res://scenes/enemies/blob/HostileBlob.tscn",
        "res://scenes/enemies/charger/Charger.tscn",
        "res://scenes/enemies/blob/TeleportingBlob.tscn",
        "res://scenes/enemies/blob/NukeBlob.tscn",
    };
    public static readonly String[] MEDIUM_CHARGERS = {
        "res://scenes/enemies/charger/RigidCharger.tscn",
        "res://scenes/enemies/charger/TeleportingCharger.tscn",
        "res://scenes/enemies/charger/BlobCharger.tscn",
    };
    public static readonly String[] HARD_CHARGERS = {
        "res://scenes/enemies/spawner/ChargerSpawner.tscn",
        "res://scenes/enemies/spawner/BlobChargerSpawner.tscn",
        "res://scenes/enemies/spawner/TeleportingChargerSpawner.tscn",
    };
    public static readonly String[] EASY_SHOOTERS = {
        "res://scenes/enemies/shooter/Shooter.tscn",
        "res://scenes/enemies/shooter/SprayerSticky.tscn",
    };
    public static readonly String[] MEDIUM_SHOOTERS = {
        "res://scenes/enemies/shooter/LaserShooter.tscn",
        "res://scenes/enemies/shooter/LongRangeShooter.tscn",
        "res://scenes/enemies/shooter/SprayerSticky.tscn",
        "res://scenes/enemies/shooter/SprayerAcid.tscn",
    };
    public static readonly String[] HARD_SHOOTERS = {
        "res://scenes/enemies/shooter/LongRangeCannoneer.tscn",
        "res://scenes/enemies/shooter/ScatterShooter.tscn",
        "res://scenes/enemies/shooter/SprayerAcid.tscn",
    };
    public static readonly String[] EASY_RANDOM = {
        "res://scenes/enemies/all rounder/Melee.tscn",
        "res://scenes/enemies/all rounder/MeleeShielded.tscn",
        "res://scenes/enemies/all rounder/MeleeBlobnadier.tscn",
        "res://scenes/enemies/all rounder/BlobnadierShielded.tscn",
        "res://scenes/enemies/blob/HostileBlob.tscn",
        "res://scenes/enemies/charger/Charger.tscn",
        "res://scenes/enemies/blob/TeleportingBlob.tscn",
        "res://scenes/enemies/blob/NukeBlob.tscn",
        "res://scenes/enemies/shooter/Shooter.tscn",
        "res://scenes/enemies/shooter/SprayerSticky.tscn",
    };
    public static readonly String[] MEDIUM_RANDOM = {
        "res://scenes/enemies/all rounder/Ranged.tscn",
        "res://scenes/enemies/all rounder/RangedBlobnadier.tscn",
        "res://scenes/enemies/all rounder/ParaBlobCarrier.tscn",
        "res://scenes/enemies/all rounder/SlowBlobCarrier.tscn",
        "res://scenes/enemies/all rounder/TeleBlobCarrier.tscn",
        "res://scenes/enemies/charger/RigidCharger.tscn",
        "res://scenes/enemies/charger/TeleportingCharger.tscn",
        "res://scenes/enemies/charger/BlobCharger.tscn",
        "res://scenes/enemies/shooter/LaserShooter.tscn",
        "res://scenes/enemies/shooter/LongRangeShooter.tscn",
        "res://scenes/enemies/shooter/SprayerSticky.tscn",
        "res://scenes/enemies/shooter/SprayerAcid.tscn",
    };
    public static readonly String[] HARD_RANDOM = {
        "res://scenes/enemies/all rounder/DualRanged.tscn",
        "res://scenes/enemies/all rounder/RangedShielded.tscn",
        "res://scenes/enemies/all rounder/MeleeRanged.tscn",
        "res://scenes/enemies/spawner/ChargerSpawner.tscn",
        "res://scenes/enemies/spawner/BlobChargerSpawner.tscn",
        "res://scenes/enemies/spawner/TeleportingChargerSpawner.tscn",
        "res://scenes/enemies/shooter/LongRangeCannoneer.tscn",
        "res://scenes/enemies/shooter/ScatterShooter.tscn",
        "res://scenes/enemies/shooter/SprayerAcid.tscn",
    };
    public static readonly String[] ELITE_ALL_ROUNDERS = {
        "res://scenes/enemies/all rounder/EliteMeleeShielded.tscn",
        "res://scenes/enemies/all rounder/EliteRangedShielded.tscn",
    };
    public static readonly String[] ELITE_CHARGERS = {
        "res://scenes/enemies/charger/EliteBlobCharger.tscn",
        "res://scenes/enemies/spawner/EliteChargerSpawner.tscn",
    };
    public static readonly String[] ELITE_SHOOTERS = {

    };

    
    public static void PrintErrNotImplemented(String className, String funcName) {
        GD.PrintErr(className + ", " + funcName + ": Function not implemented.");
    }


}
