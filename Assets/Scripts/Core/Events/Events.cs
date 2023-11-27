
using System;
using UnityEngine;

public static class Events 
{
    //Level Manager
    public static  Action levelLoadedEvent;
    public static  Action levelSuccesedEvent;
    public static  Action levelFailedEvent;

    //Cube Block
    public static  Action<int, int> cubeLeavedGridEvent;
    public static  Action bombSpawningEvent;
    public static  Action bombSpawnDoneEvent;

    //Duck Block
    public static Action<int, int> batLeavedGridEvent;

    //Rocket Block
    public static Action bombStartedEvent;
    public static Action bombEndedEvent;

    //Goal Panel
    public static Action allGoalsEndedEvent;
    public static Action goalsFailedEvent;

    //Moves Panel
    public static Action movesFinishedEvent;

    //Audio Manager
    public static Action bottlePopAudioPlayEvent;
    public static Action batExplodeAudioPlayEvent;
    public static Action cubeExplosionAudioPlayEvent;
    public static Action cubeCollectAudioPlayEvent;

}
