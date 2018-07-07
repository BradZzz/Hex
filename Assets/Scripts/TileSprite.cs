using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TileSprite : MonoBehaviour {

  public Sprite Knight;
  public Sprite Lancer;
  public Sprite Swordsman;

  public Sprite Monster;

  public Sprite GetCharacterImage(UnitInfo unit) { 
    switch (unit.type) {
    case UnitInfo.unitType.Knight:
      return Knight;
    case UnitInfo.unitType.Lancer:
      return Lancer;
    case UnitInfo.unitType.Swordsman:
      return Swordsman;
    case UnitInfo.unitType.Monster:
      return Monster;
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
  public Sprite[] Sand;
  public Sprite[] Treasure;
  public Sprite[] Water;

  public Sprite eSpawn;
  public Sprite pSpawn;

  public void unitHit(){
    gameObject.transform.Find("UnitObject").GetComponent<BattleGui> ().isHit();
  }

  public void setUnit (UnitInfo unit) {
    SpriteRenderer spRend = gameObject.transform.Find("UnitObject").GetComponent<SpriteRenderer> ();

    spRend.enabled = false;

    if (unit.playerNo > -1) {
      spRend.enabled = true;
      switch(unit.type) {
      case UnitInfo.unitType.Knight:
        spRend.sprite = Knight;
        spRend.transform.localPosition = new Vector3(-2, 7, -1);
        break;
      case UnitInfo.unitType.Lancer:
        spRend.sprite = Lancer;
        spRend.transform.localPosition = new Vector3(1.5f, 7, -1);
        //spRend.gameObject.transform.position = new Vector3 (transform.position.x,55,-70);
        break;
      case UnitInfo.unitType.Swordsman:
        spRend.sprite = Swordsman;
        spRend.transform.localPosition = new Vector3(0, 10, -1);
        break;
      case UnitInfo.unitType.Monster:
        spRend.sprite = Monster;
        spRend.transform.localPosition = new Vector3(0, 10, -1);
        break;
      case UnitInfo.unitType.Adventure:
        if (unit.human) {
          spRend.sprite = Knight;
          spRend.transform.localPosition = new Vector3(-2, 7, -1);
        } else {
          spRend.sprite = Monster;
          spRend.transform.localPosition = new Vector3(0, 10, -1);
        }
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
    case TileInfo.tileType.Sand:
      return Sand [Random.Range (0, Sand.Length)];
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

  public void setTile (UnitInfo unit, TileInfo.tileType type, bool interaction) {
    SpriteRenderer spRend = gameObject.transform.Find("TileObject").GetComponent<SpriteRenderer> ();
    spRend.enabled = true;

    switch(type){
    case TileInfo.tileType.Castle:
      if (!Castle.Contains(spRend.sprite)) {
        spRend.sprite = Castle [Random.Range (0, Castle.Length)];
      }
      break;
    case TileInfo.tileType.City:
      if (!City.Contains(spRend.sprite)) {
        spRend.sprite = City [Random.Range (0, City.Length)];
      }
      break;
    case TileInfo.tileType.Forest:
      if (!Forest.Contains(spRend.sprite)) {
        spRend.sprite = Forest [Random.Range (0, Forest.Length)];
      }
      break;
    case TileInfo.tileType.Grass:
      if (!Grass.Contains(spRend.sprite)) {
        spRend.sprite = Grass [Random.Range (0, Grass.Length)];
      }
      break;
    case TileInfo.tileType.Mountain:
      if (!Mountain.Contains(spRend.sprite)) {
        spRend.sprite = Mountain [Random.Range (0, Mountain.Length)];
      }
      break;
    case TileInfo.tileType.Road:
      if (!Road.Contains(spRend.sprite)) {
        spRend.sprite = Road [Random.Range (0, Road.Length)];
      }
      break;
    case TileInfo.tileType.Sand:
      if (!Sand.Contains(spRend.sprite)) {
        spRend.sprite = Sand [Random.Range (0, Sand.Length)];
      }
      break;
    case TileInfo.tileType.Treasure:
      if (!Treasure.Contains(spRend.sprite)) {
        spRend.sprite = Treasure [Random.Range (0, Treasure.Length)];
      }
      break;
    case TileInfo.tileType.Water:
      if (!Water.Contains(spRend.sprite)) {
        spRend.sprite = Water [Random.Range (0, Water.Length)];
      }
      break;
    case TileInfo.tileType.eSpawn:
      spRend.sprite = eSpawn;
      break;
    case TileInfo.tileType.pSpawn:
      spRend.sprite = pSpawn;
      break;
    default:
      Debug.Log ("default");
      Debug.Log (type.ToString());
      spRend.enabled = false;
      break;
    }
    if (unit.human || unit.playerNo == -1) {
      spRend.color = new Color (1, 1, 1, 1);
    } else {
      spRend.color = new Color (1, .8f, .8f, .9f);
    }
  }
}
