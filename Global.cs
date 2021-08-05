using Godot;
using System;

public static class Global
{
    public const short BIT_MASK_ENEMY = 0;
    public const short BIT_MASK_WALL = 1;
    public const short BIT_MASK_SEETHROUGH_WALL = 5;
    public const short BIT_MASK_PLAYER = 2;
    public const short BIT_MASK_WEAPON = 3;
    public const short BIT_MASK_WEAPON_LARGE = 4;
    public const int CCD_MAX = 2500000;
    public const String WEAP_LARGE_SCN = "res://scenes/player/WeaponLarge.tscn";
    public const String PLAYER_SCN = "res://scenes/player/Player.tscn";
    public const String SAVE_DIR = "user://save/";
    public static readonly String[] RAND_ENEMY_EASY_SET_1 = {
        "res://scenes/enemies/all rounder/Melee.tscn",
        "res://scenes/enemies/all rounder/MeleeShielded.tscn",
        "res://scenes/enemies/all rounder/MeleeBlobnadier.tscn",
        "res://scenes/enemies/all rounder/BlobnadierShielded.tscn",
        "res://scenes/enemies/charger/Charger.tscn",
        "res://scenes/enemies/shooter/Shooter.tscn"
    };
    public static readonly String[] RAND_ENEMY_MEDIUM_SET_1 = {
        "res://scenes/enemies/all rounder/Ranged.tscn",
        "res://scenes/enemies/all rounder/RangedBlobnadier.tscn",
        "res://scenes/enemies/all rounder/ParaBlobCarrier.tscn",
        "res://scenes/enemies/all rounder/SlowBlobCarrier.tscn",
        "res://scenes/enemies/all rounder/TeleBlobCarrier.tscn",
        "res://scenes/enemies/shooter/Shooter.tscn",
        "res://scenes/enemies/shooter/LongRangeShooter.tscn"
    };
    public static readonly String[] RAND_ENEMY_HARD_SET_1 = {
        "res://scenes/enemies/all rounder/DualRanged.tscn",
        "res://scenes/enemies/all rounder/RangedShielded.tscn",
        "res://scenes/enemies/all rounder/MeleeRanged.tscn",
        "res://scenes/enemies/charger/TeleportingCharger.tscn",
        "res://scenes/enemies/shooter/LongRangeShooter.tscn"
    };
    public static readonly String[] RAND_ENEMY_EASY_SET_2 = {
        "res://scenes/enemies/blob/HostileBlob.tscn",
        "res://scenes/enemies/charger/Charger.tscn",
        "res://scenes/enemies/shooter/SprayerSticky.tscn",
        "res://scenes/enemies/all rounder/MeleeShielded.tscn",
        "res://scenes/enemies/all rounder/BlobnadierShielded.tscn"
    };
    public static readonly String[] RAND_ENEMY_MEDIUM_SET_2 = {
        "res://scenes/enemies/charger/RigidCharger.tscn",
        "res://scenes/enemies/shooter/SprayerSticky.tscn",
        "res://scenes/enemies/all rounder/MeleeShielded.tscn",
        "res://scenes/enemies/all rounder/BlobnadierShielded.tscn"
    };
    public static readonly String[] RAND_ENEMY_HARD_SET_2 = {
        "res://scenes/enemies/charger/TeleportingCharger.tscn",
        "res://scenes/enemies/spawner/BlobChargerSpawner.tscn",
        "res://scenes/enemies/shooter/SprayerAcid.tscn"
    };
    public static readonly String[] RAND_ENEMY_EASY_SET_3 = {
        "res://scenes/enemies/shooter/Shooter.tscn",
        "res://scenes/enemies/shooter/LongRangeShooter.tscn",
        "res://scenes/enemies/shooter/SprayerSticky.tscn",
        "res://scenes/enemies/all rounder/Ranged.tscn",
        "res://scenes/enemies/all rounder/ParaBlobCarrier.tscn",
        "res://scenes/enemies/all rounder/SlowBlobCarrier.tscn",
        "res://scenes/enemies/all rounder/TeleBlobCarrier.tscn"
    };
    public static readonly String[] RAND_ENEMY_MEDIUM_SET_3 = {
        "res://scenes/enemies/shooter/LaserShooter.tscn",
        "res://scenes/enemies/shooter/LongRangeShooter.tscn",
        "res://scenes/enemies/shooter/SprayerSticky.tscn",
        "res://scenes/enemies/shooter/SprayerAcid.tscn",
        "res://scenes/enemies/all rounder/RangedShielded.tscn",
        "res://scenes/enemies/all rounder/ParaBlobCarrier.tscn",
        "res://scenes/enemies/all rounder/SlowBlobCarrier.tscn",
        "res://scenes/enemies/all rounder/TeleBlobCarrier.tscn"
    };
    public static readonly String[] RAND_ENEMY_HARD_SET_3 = {
        "res://scenes/enemies/shooter/LongRangeCannoneer.tscn",
        "res://scenes/enemies/shooter/ScatterShooter.tscn",
        "res://scenes/enemies/shooter/SprayerAcid.tscn",
        "res://scenes/enemies/all rounder/DualRanged.tscn"
    };


}
