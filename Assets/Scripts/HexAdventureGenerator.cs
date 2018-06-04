using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexAdventureGenerator {
  public static void generateMap(HexCell[] cells, int h, int w){
    List<HexCell> unusedCells = new List<HexCell> (cells);

    foreach (HexCell cell in cells) {
      cell.setType(TileInfo.tileType.Grass);
    }
    addRoad (cells, cells [0], cells [cells.Length - 1]);

    addHovel (cells, h, w, cells.Length - (2 * w) + 1);

    addCastle (cells, h, w, cells.Length - (5 * w) + 1);

    addTreasure(cells, h, w, cells.Length - (8 * w) - 3);

    addWater(cells, h, w, cells.Length - (6 * w) - 4);
  }

  private static void addHovel(HexCell[] cells, int h, int w, int pos){
    HexCell landing = cells [pos];
    landing.setType(TileInfo.tileType.City);
    foreach(HexDirection dir in landing.dirs) {
      if (landing.GetNeighbor (dir)) {
        landing.GetNeighbor (dir).setType (TileInfo.tileType.Forest);
      }
    }
  }

  private static void addCastle(HexCell[] cells, int h, int w, int pos){
    HexCell landing = cells [pos];
    landing.setType(TileInfo.tileType.Castle);
    foreach(HexDirection dir in landing.dirs) {
      if (landing.GetNeighbor (dir)) {
        landing.GetNeighbor (dir).setType (TileInfo.tileType.Forest);
      }
    }
  }

  private static void addTreasure(HexCell[] cells, int h, int w, int pos){
    HexCell landing = cells [pos];
    landing.setType(TileInfo.tileType.Treasure);
    foreach(HexDirection dir in landing.dirs) {
      if (landing.GetNeighbor (dir)) {
        landing.GetNeighbor (dir).setType (TileInfo.tileType.Treasure);
      }
    }
  }

  private static void addWater(HexCell[] cells, int h, int w, int pos){
    HexCell landing = cells [pos];
    landing.setType(TileInfo.tileType.Water);
    foreach(HexDirection dir in landing.dirs) {
      if (landing.GetNeighbor (dir)) {
        landing.GetNeighbor (dir).setType (TileInfo.tileType.Water);
      }
    }
  }

  private static HexCell[] addRoad(HexCell[] cells, HexCell start, HexCell end){
    HexCell[] road = HexAI.aStar (cells, start, end);
    start.setType (TileInfo.tileType.Road);
    foreach (HexCell cell in road) {
      cell.setType(TileInfo.tileType.Road);
    }
    return road;
  }

  /*
   * Need the distance in numbers between two points here
   */

  private static void addCastle(HexCell[] cells, HexCell start, HexCell end){
    HexCell[] road = HexAI.aStar (cells, start, end);
    start.setType (TileInfo.tileType.Road);
    foreach (HexCell cell in road) {
      cell.setType(TileInfo.tileType.Road);
    }
  }
}
