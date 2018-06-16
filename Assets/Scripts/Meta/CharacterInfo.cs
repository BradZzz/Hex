using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterInfo {
  //Things that affect the adventure
  public bool canTraverseMountains;
  public bool canTraverseWater;
  public int playerMovementMod;
  public int monsterMovementMod;
  public float avoidEncountersMod;
  public int viewAreaMod;
  public float goldEarnedMod;

  //Things that affect the battle
  public int battleHealthMod;
  public int battleMovementMod;

  //Things that affect the minigames
  public float minigameTimeMod;
  public float minigameScoreMod;

  //Things that affect locations
  public float goodEventsChance;
  public float reputationGainedMod;
  public float discountMod;

  //Things that affect characters
  public float badOptionsTurningRedChance;

  private void resetAttributes(){
    canTraverseMountains = false;
    canTraverseWater = false;
    playerMovementMod = 0;
    monsterMovementMod = 0;
    avoidEncountersMod = 0;
    viewAreaMod = 0;
    goldEarnedMod = 0;
    battleHealthMod = 0;
    battleMovementMod = 0;
    minigameTimeMod = 0;
    minigameScoreMod = 0;
    goodEventsChance = 0;
    reputationGainedMod = 0;
    discountMod = 0;
    badOptionsTurningRedChance = 0;
  }

  public void processAttributes(CharacterInfo.attributeType[] attributes){
    resetAttributes ();
    foreach(CharacterInfo.attributeType attribute in attributes){
      switch(attribute){
      case CharacterInfo.attributeType.Sturdy:
        canTraverseMountains = true;
        break;
      case CharacterInfo.attributeType.Slippery:
        canTraverseWater = true;
        break;
      case CharacterInfo.attributeType.Spry:
        playerMovementMod = 1;
        break;
      case CharacterInfo.attributeType.Cursed:
        monsterMovementMod = 2;
        break;
      case CharacterInfo.attributeType.Intimidating:
        avoidEncountersMod = .5f;
        break;
      case CharacterInfo.attributeType.Keen:
        viewAreaMod = 1;
        break;
      case CharacterInfo.attributeType.Miserly:
        goldEarnedMod = 1.5f;
        break;
      case CharacterInfo.attributeType.Brawny:
        battleHealthMod = 1;
        break;
      case CharacterInfo.attributeType.Inspiring:
        battleMovementMod = 1;
        break;
      case CharacterInfo.attributeType.Punctual:
        minigameTimeMod = 1.5f;
        break;
      case CharacterInfo.attributeType.Tenacious:
        minigameScoreMod = 1.5f;
        break;
      case CharacterInfo.attributeType.Lucky:
        goodEventsChance = 1.25f;
        break;
      case CharacterInfo.attributeType.Known:
        reputationGainedMod = 1.25f;
        break;
      case CharacterInfo.attributeType.Shrewd:
        discountMod = 1.25f;
        break;
      case CharacterInfo.attributeType.Cunning:
        badOptionsTurningRedChance = 1.25f;
        break;
      }
    }
  }

  public enum attributeType {
    Sturdy,
    Slippery,
    Spry,
    Cursed,
    Intimidating,
    Keen,
    Miserly,
    Brawny,
    Inspiring,
    Punctual,
    Tenacious,
    Lucky,
    Known,
    Shrewd,
    Cunning,
    None
  }
}