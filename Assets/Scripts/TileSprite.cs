﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileSprite : MonoBehaviour {

  public Sprite Knight;
  public Sprite Lancer;
  public Sprite Swordsman;

  public Sprite GetCharacterImage(UnitInfo unit) { 
    switch (unit.type) {
    case UnitInfo.unitType.Knight:
      return Knight;
    case UnitInfo.unitType.Lancer:
      return Lancer;
    case UnitInfo.unitType.Swordsman:
      return Swordsman;
    default:
      return Knight;
    }
  }

  public Sprite[] Castle;
  public Sprite[] City;
  public Sprite[] Forest;
  public Sprite[] Grass;
  public Sprite[] Road;
  public Sprite[] Mountain;
  public Sprite[] Treasure;
  public Sprite[] Water;

  public Sprite eSpawn;
  public Sprite pSpawn;

  public void setUnit (UnitInfo unit) {
    SpriteRenderer spRend = gameObject.transform.Find("UnitObject").GetComponent<SpriteRenderer> ();

    spRend.enabled = false;

    if (unit.playerNo > -1) {
      spRend.enabled = true;
      switch(unit.type) {
//      case UnitInfo.unitType.Knight:
//        spRend.sprite = Knight;
//        break;
      case UnitInfo.unitType.Lancer:
        spRend.sprite = Lancer;
        spRend.transform.localPosition = new Vector3(1.5f, 7, -1);
        //spRend.gameObject.transform.position = new Vector3 (transform.position.x,55,-70);
        break;
      case UnitInfo.unitType.Swordsman:
        spRend.sprite = Swordsman;
        spRend.transform.localPosition = new Vector3(0, 10, -1);
        break;
      default:
        spRend.sprite = Knight;
        spRend.transform.localPosition = new Vector3(-2, 7, -1);
        //spRend.gameObject.transform.position = new Vector3 (transform.position.x,55,-70);
        break;
      }
    } else if (unit.playerNo > -1) {
      spRend.enabled = true;
    }
  }

  public Sprite getTile(TileInfo.tileType type) {
    switch(type){
    case TileInfo.tileType.Castle:
      return Castle [Random.Range (0, Castle.Length)];
    case TileInfo.tileType.City:
      return City [Random.Range (0, City.Length)];
    case TileInfo.tileType.Forest:
      return Forest [Random.Range (0, Forest.Length)];
    case TileInfo.tileType.Grass:
      return Grass [Random.Range (0, Grass.Length)];
    case TileInfo.tileType.Mountain:
      return Mountain [Random.Range (0, Mountain.Length)];
    case TileInfo.tileType.Road:
      return Road [Random.Range (0, Road.Length)];
    case TileInfo.tileType.Treasure:
      return Treasure [Random.Range (0, Treasure.Length)];
    case TileInfo.tileType.Water:
      return Water [Random.Range (0, Water.Length)];
    case TileInfo.tileType.eSpawn:
      return eSpawn;
    case TileInfo.tileType.pSpawn:
      return pSpawn;
    default:
      return Grass [Random.Range (0, Grass.Length)];
    }
  }

  public void setTile (TileInfo.tileType type) {
    SpriteRenderer spRend = gameObject.transform.Find("TileObject").GetComponent<SpriteRenderer> ();
    spRend.enabled = true;

    switch(type){
    case TileInfo.tileType.Castle:
      spRend.sprite = Castle [Random.Range (0, Castle.Length)];
      break;
    case TileInfo.tileType.City:
      spRend.sprite = City [Random.Range (0, City.Length)];
      break;
    case TileInfo.tileType.Forest:
      spRend.sprite = Forest [Random.Range (0, Forest.Length)];
      break;
    case TileInfo.tileType.Grass:
      spRend.sprite = Grass [Random.Range (0, Grass.Length)];
      break;
    case TileInfo.tileType.Mountain:
      spRend.sprite = Mountain [Random.Range (0, Mountain.Length)];
      break;
    case TileInfo.tileType.Road:
      spRend.sprite = Road [Random.Range (0, Road.Length)];
      break;
    case TileInfo.tileType.Treasure:
      spRend.sprite = Treasure [Random.Range (0, Treasure.Length)];
      break;
    case TileInfo.tileType.Water:
      spRend.sprite = Water [Random.Range (0, Water.Length)];
      break;
    case TileInfo.tileType.eSpawn:
      spRend.sprite = eSpawn;
      break;
    case TileInfo.tileType.pSpawn:
      spRend.sprite = pSpawn;
      break;
    default:
      Debug.Log ("default");
      Debug.Log (type);
      spRend.enabled = false;
      break;
    }
  }
}
