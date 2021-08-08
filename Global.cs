using Godot;
using System;
using System.Collections.Generic;

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
    };
    public static readonly String[] MEDIUM_CHARGERS = {
        "res://scenes/enemies/charger/BlobCharger.tscn",
        "res://scenes/enemies/charger/TeleportingCharger.tscn",
        "res://scenes/enemies/charger/RigidCharger.tscn",
    };
    public static readonly String[] HARD_CHARGERS = {
        "res://scenes/enemies/spawner/BlobChargerSpawner.tscn",
        "res://scenes/enemies/spawner/ChargerSpawner.tscn",
        "res://scenes/enemies/spawner/TeleportingChargerSpawner.tscn",
    };
    public static readonly String[] EASY_SHOOTERS = {
        "res://scenes/enemies/shooter/Shooter.tscn",
        "res://scenes/enemies/shooter/LongRangeShooter.tscn",
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
        "res://scenes/enemies/shooter/Shooter.tscn",
        "res://scenes/enemies/shooter/LongRangeShooter.tscn",
        "res://scenes/enemies/shooter/SprayerSticky.tscn",
    };
    public static readonly String[] MEDIUM_RANDOM = {
        "res://scenes/enemies/all rounder/Ranged.tscn",
        "res://scenes/enemies/all rounder/RangedBlobnadier.tscn",
        "res://scenes/enemies/all rounder/ParaBlobCarrier.tscn",
        "res://scenes/enemies/all rounder/SlowBlobCarrier.tscn",
        "res://scenes/enemies/all rounder/TeleBlobCarrier.tscn",
        "res://scenes/enemies/charger/BlobCharger.tscn",
        "res://scenes/enemies/charger/TeleportingCharger.tscn",
        "res://scenes/enemies/charger/RigidCharger.tscn",
        "res://scenes/enemies/shooter/LaserShooter.tscn",
        "res://scenes/enemies/shooter/LongRangeShooter.tscn",
        "res://scenes/enemies/shooter/SprayerSticky.tscn",
        "res://scenes/enemies/shooter/SprayerAcid.tscn",
    };
    public static readonly String[] HARD_RANDOM = {
        "res://scenes/enemies/all rounder/DualRanged.tscn",
        "res://scenes/enemies/all rounder/RangedShielded.tscn",
        "res://scenes/enemies/all rounder/MeleeRanged.tscn",
        "res://scenes/enemies/spawner/BlobChargerSpawner.tscn",
        "res://scenes/enemies/spawner/ChargerSpawner.tscn",
        "res://scenes/enemies/spawner/TeleportingChargerSpawner.tscn",
        "res://scenes/enemies/shooter/LongRangeCannoneer.tscn",
        "res://scenes/enemies/shooter/ScatterShooter.tscn",
        "res://scenes/enemies/shooter/SprayerAcid.tscn",
    };

    //lvl sets
    //NONE
    public static readonly String[] ID00_SET1 = {

    };
    public static readonly String[] ID01_SET1 = {

    };
    public static readonly String[] ID02_SET1 = {

    };
    public static readonly String[] ID03_SET1 = {

    };
    public static readonly String[] ID04_SET1 = {

    };
    public static readonly String[] ID05_SET1 = {
        "res://levels/051None1.tscn"
    };
    public static readonly String[] ID06_SET1 = {

    };
    public static readonly String[] ID07_SET1 = {

    };
    public static readonly String[] ID08_SET1 = {

    };
    public static readonly String[] ID09_SET1 = {

    };
    public static readonly String[] ID10_SET1 = {

    };
    public static readonly String[] ID11_SET1 = {
        "res://levels/111None1.tscn"
    };
    public static readonly String[] ID12_SET1 = {
        "res://levels/121None1.tscn"
    };
    public static readonly String[] ID13_SET1 = {

    };
    public static readonly String[] ID14_SET1 = {

    };
    public static readonly String[] ID00_SET2 = {

    };
    public static readonly String[] ID01_SET2 = {

    };
    public static readonly String[] ID02_SET2 = {

    };
    public static readonly String[] ID03_SET2 = {

    };
    public static readonly String[] ID04_SET2 = {

    };
    public static readonly String[] ID05_SET2 = {
        "res://levels/052None1.tscn"
    };
    public static readonly String[] ID06_SET2 = {

    };
    public static readonly String[] ID07_SET2 = {

    };
    public static readonly String[] ID08_SET2 = {

    };
    public static readonly String[] ID09_SET2 = {

    };
    public static readonly String[] ID10_SET2 = {

    };
    public static readonly String[] ID11_SET2 = {

    };
    public static readonly String[] ID12_SET2 = {
        "res://levels/122None1.tscn"
    };
    public static readonly String[] ID13_SET2 = {

    };
    public static readonly String[] ID14_SET2 = {

    };
    public static readonly String[] ID00_SET3 = {

    };
    public static readonly String[] ID01_SET3 = {

    };
    public static readonly String[] ID02_SET3 = {

    };
    public static readonly String[] ID03_SET3 = {

    };
    public static readonly String[] ID04_SET3 = {

    };
    public static readonly String[] ID05_SET3 = {

    };
    public static readonly String[] ID06_SET3 = {

    };
    public static readonly String[] ID07_SET3 = {

    };
    public static readonly String[] ID08_SET3 = {

    };
    public static readonly String[] ID09_SET3 = {

    };
    public static readonly String[] ID10_SET3 = {

    };
    public static readonly String[] ID11_SET3 = {

    };
    public static readonly String[] ID12_SET3 = {

    };
    public static readonly String[] ID13_SET3 = {

    };
    public static readonly String[] ID14_SET3 = {

    };


    //CHECKPT
    public static readonly String[] ID01_SET1_CHECKPT = {

    };
    public static readonly String[] ID02_SET1_CHECKPT = {

    };
    public static readonly String[] ID03_SET1_CHECKPT = {

    };
    public static readonly String[] ID04_SET1_CHECKPT = {

    };
    public static readonly String[] ID05_SET1_CHECKPT = {

    };
    public static readonly String[] ID06_SET1_CHECKPT = {

    };
    public static readonly String[] ID07_SET1_CHECKPT = {

    };
    public static readonly String[] ID08_SET1_CHECKPT = {

    };
    public static readonly String[] ID09_SET1_CHECKPT = {

    };
    public static readonly String[] ID10_SET1_CHECKPT = {

    };
    public static readonly String[] ID11_SET1_CHECKPT = {
        "res://levels/111Checkpt1.tscn"
    };
    public static readonly String[] ID12_SET1_CHECKPT = {

    };
    public static readonly String[] ID13_SET1_CHECKPT = {

    };
    public static readonly String[] ID14_SET1_CHECKPT = {

    };
    public static readonly String[] ID01_SET2_CHECKPT = {

    };
    public static readonly String[] ID02_SET2_CHECKPT = {

    };
    public static readonly String[] ID03_SET2_CHECKPT = {

    };
    public static readonly String[] ID04_SET2_CHECKPT = {

    };
    public static readonly String[] ID05_SET2_CHECKPT = {

    };
    public static readonly String[] ID06_SET2_CHECKPT = {

    };
    public static readonly String[] ID07_SET2_CHECKPT = {

    };
    public static readonly String[] ID08_SET2_CHECKPT = {

    };
    public static readonly String[] ID09_SET2_CHECKPT = {

    };
    public static readonly String[] ID10_SET2_CHECKPT = {

    };
    public static readonly String[] ID11_SET2_CHECKPT = {

    };
    public static readonly String[] ID12_SET2_CHECKPT = {

    };
    public static readonly String[] ID13_SET2_CHECKPT = {

    };
    public static readonly String[] ID14_SET2_CHECKPT = {

    };
    public static readonly String[] ID01_SET3_CHECKPT = {

    };
    public static readonly String[] ID02_SET3_CHECKPT = {

    };
    public static readonly String[] ID03_SET3_CHECKPT = {

    };
    public static readonly String[] ID04_SET3_CHECKPT = {

    };
    public static readonly String[] ID05_SET3_CHECKPT = {

    };
    public static readonly String[] ID06_SET3_CHECKPT = {

    };
    public static readonly String[] ID07_SET3_CHECKPT = {

    };
    public static readonly String[] ID08_SET3_CHECKPT = {

    };
    public static readonly String[] ID09_SET3_CHECKPT = {

    };
    public static readonly String[] ID10_SET3_CHECKPT = {

    };
    public static readonly String[] ID11_SET3_CHECKPT = {

    };
    public static readonly String[] ID12_SET3_CHECKPT = {

    };
    public static readonly String[] ID13_SET3_CHECKPT = {

    };
    public static readonly String[] ID14_SET3_CHECKPT = {

    };


    //SECRET
    public static readonly String[] ID07_SET1_SECRET = {

    };
    public static readonly String[] ID11_SET1_SECRET = {

    };
    public static readonly String[] ID13_SET1_SECRET = {

    };
    public static readonly String[] ID14_SET1_SECRET = {

    };
    public static readonly String[] ID07_SET2_SECRET = {

    };
    public static readonly String[] ID11_SET2_SECRET = {

    };
    public static readonly String[] ID13_SET2_SECRET = {

    };
    public static readonly String[] ID14_SET2_SECRET = {

    };
    public static readonly String[] ID07_SET3_SECRET = {

    };
    public static readonly String[] ID11_SET3_SECRET = {

    };
    public static readonly String[] ID13_SET3_SECRET = {

    };
    public static readonly String[] ID14_SET3_SECRET = {

    };


    //SECRET N
    public static readonly String[] ID01_SET1_SECRET_N = {

    };
    public static readonly String[] ID02_SET1_SECRET_N = {

    };
    public static readonly String[] ID04_SET1_SECRET_N = {

    };
    public static readonly String[] ID08_SET1_SECRET_N = {

    };
    public static readonly String[] ID01_SET2_SECRET_N = {

    };
    public static readonly String[] ID02_SET2_SECRET_N = {

    };
    public static readonly String[] ID04_SET2_SECRET_N = {

    };
    public static readonly String[] ID08_SET2_SECRET_N = {

    };
    public static readonly String[] ID01_SET3_SECRET_N = {

    };
    public static readonly String[] ID02_SET3_SECRET_N = {

    };
    public static readonly String[] ID04_SET3_SECRET_N = {

    };
    public static readonly String[] ID08_SET3_SECRET_N = {

    };


    //SECRET E
    public static readonly String[] ID01_SET1_SECRET_E = {

    };
    public static readonly String[] ID02_SET1_SECRET_E = {

    };
    public static readonly String[] ID04_SET1_SECRET_E = {

    };
    public static readonly String[] ID08_SET1_SECRET_E = {

    };
    public static readonly String[] ID01_SET2_SECRET_E = {

    };
    public static readonly String[] ID02_SET2_SECRET_E = {

    };
    public static readonly String[] ID04_SET2_SECRET_E = {

    };
    public static readonly String[] ID08_SET2_SECRET_E = {

    };
    public static readonly String[] ID01_SET3_SECRET_E = {

    };
    public static readonly String[] ID02_SET3_SECRET_E = {

    };
    public static readonly String[] ID04_SET3_SECRET_E = {

    };
    public static readonly String[] ID08_SET3_SECRET_E = {

    };


    //SECRET W
    public static readonly String[] ID01_SET1_SECRET_W = {

    };
    public static readonly String[] ID02_SET1_SECRET_W = {

    };
    public static readonly String[] ID04_SET1_SECRET_W = {

    };
    public static readonly String[] ID08_SET1_SECRET_W = {

    };
    public static readonly String[] ID01_SET2_SECRET_W = {

    };
    public static readonly String[] ID02_SET2_SECRET_W = {

    };
    public static readonly String[] ID04_SET2_SECRET_W = {

    };
    public static readonly String[] ID08_SET2_SECRET_W = {

    };
    public static readonly String[] ID01_SET3_SECRET_W = {

    };
    public static readonly String[] ID02_SET3_SECRET_W = {

    };
    public static readonly String[] ID04_SET3_SECRET_W = {

    };
    public static readonly String[] ID08_SET3_SECRET_W = {

    };

    
    //SECRET S
    public static readonly String[] ID01_SET1_SECRET_S = {

    };
    public static readonly String[] ID02_SET1_SECRET_S = {

    };
    public static readonly String[] ID04_SET1_SECRET_S = {

    };
    public static readonly String[] ID08_SET1_SECRET_S = {

    };
    public static readonly String[] ID01_SET2_SECRET_S = {

    };
    public static readonly String[] ID02_SET2_SECRET_S = {

    };
    public static readonly String[] ID04_SET2_SECRET_S = {

    };
    public static readonly String[] ID08_SET2_SECRET_S = {

    };
    public static readonly String[] ID01_SET3_SECRET_S = {

    };
    public static readonly String[] ID02_SET3_SECRET_S = {

    };
    public static readonly String[] ID04_SET3_SECRET_S = {

    };
    public static readonly String[] ID08_SET3_SECRET_S = {

    };


}
