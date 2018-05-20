using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexAI {
	private int player;

	public HexAI(int player){
		this.player = player;
	}

	public HexCell GetNextPlayer(HexCell[] cells){
		List<HexCell> playerCells = new List<HexCell> ();
		foreach (HexCell cell in cells) {
			if (cell.GetPlayer() == player && (cell.GetInfo().actions > 0 || cell.GetInfo().attacks > 0)) {
				playerCells.Add (cell);
			}
		}
		for(int i = 0; i < playerCells.Count; i++) {
			HexCell[] path = HexAI.aStar(cells, playerCells[i]);
			if (path.Length > 0) {
				return playerCells [i];
			} else if (i == playerCells.Count - 1) {
				return playerCells [i];
			}
		}
//
//
//		foreach(HexCell cell in cells){
//			if (cell.GetPlayer() == player && (cell.GetInfo().actions > 0 || cell.GetInfo().attacks > 0)) {
//				return cell;
//			}
//		}
		return null;
	}

	public static HexCell[] aStar(HexCell[] cells, HexCell start){
		Dictionary<HexCell, int> tblStore = new Dictionary<HexCell, int>();
		Queue<HexCell> choices = new Queue<HexCell> ();
		choices.Enqueue (start);
		int turn = 1;
		List<HexCell> path = null;
		List<HexCell> finalDest = null;
		while (choices.Count > 0) {
			path = evaluatePaths(tblStore, choices, start, finalDest, turn++);
			if (path != null) {
				break;
			}
		}
		return path == null ? new HexCell[0] : path.ToArray ();
	}

	private static List<HexCell> evaluatePaths(Dictionary<HexCell, int> tblStore, Queue<HexCell> choices, HexCell start, List<HexCell> finalDest, int turn) {
		Queue<HexCell> toEval = new Queue<HexCell> (choices);
		choices.Clear ();
		while (toEval.Count > 0) {
			HexCell nxt = toEval.Dequeue ();
			HexDirection[] dirs = nxt.dirs;
			HexUtilities.ShuffleArray (dirs);
			foreach(HexDirection dir in dirs) {
				//HexCell neighbor = nxt.GetNeighbor (dir);
				if (nxt.GetNeighbor (dir)) {
					if (nxt.GetNeighbor (dir).GetPlayer() != start.GetPlayer() && !tblStore.ContainsKey(nxt.GetNeighbor (dir))) {
						//The destination here should be different for the swordsman and lancer
						if (finalDest == null && nxt.GetNeighbor (dir).GetPlayer () != -1) {
							finalDest = new List<HexCell> ();
							switch (start.GetInfo ().type) {
								case UnitInfo.unitType.Lancer:
									//Look at each side around this cell and check to see if there any other sides with an enemy attached
									HexDirection[] goodLancer = nxt.GetNeighbor (dir).GetLancerDirs (start.GetPlayer ());
									if (goodLancer.Length > 0) {
										foreach (HexDirection lDir in goodLancer) {
											finalDest.Add (nxt.GetNeighbor (dir).GetNeighbor (lDir));
										}
									} else {
										finalDest.Add (nxt.GetNeighbor (dir));
										tblStore.Add (nxt.GetNeighbor (dir), turn);
										choices.Enqueue (nxt.GetNeighbor (dir));
									}
									break;
								case UnitInfo.unitType.Swordsman:
										//Look at each cell around this cell and see if another is occupied. Move to 
									HexDirection[] goodSwordsman = nxt.GetNeighbor (dir).GetSwordDirs (start.GetPlayer ());
									if (goodSwordsman.Length > 0) {
										foreach (HexDirection lDir in goodSwordsman){
											finalDest.Add (nxt.GetNeighbor (dir).GetNeighbor(lDir));
										}
									} else {
										finalDest.Add (nxt.GetNeighbor (dir));
										tblStore.Add (nxt.GetNeighbor (dir), turn);
										choices.Enqueue (nxt.GetNeighbor (dir));
									}
									break;
								default:
									finalDest.Add (nxt.GetNeighbor (dir));
									tblStore.Add (nxt.GetNeighbor (dir), turn);
									choices.Enqueue (nxt.GetNeighbor (dir));
									break;
							}
						} else {
							tblStore.Add (nxt.GetNeighbor (dir), turn);
							choices.Enqueue (nxt.GetNeighbor (dir));
						}
						if (finalDest != null) {
							foreach (HexCell dest in finalDest) {
								HexCell endPoint = finalDest.Contains (nxt.GetNeighbor (dir)) ? nxt.GetNeighbor (dir) 
									: (tblStore.ContainsKey (dest) ? dest : null);
								if (endPoint != null) {
									List<HexCell> path = iterateBackFromPoint (endPoint, tblStore);
									return path;
								}
							}

						}
//						if (finalDest != null && finalDest.Contains(nxt.GetNeighbor (dir))) {
//							List<HexCell> path = iterateBackFromPoint (nxt.GetNeighbor (dir), tblStore);
//							return path;
//						}
					}
				}
			}
		}
		return null;
	}

	private static List<HexCell> iterateBackFromPoint(HexCell end, Dictionary<HexCell, int> tblStore){
		List<HexCell> path = new List<HexCell>();
		HexCell current = end;
		int turn = tblStore [end] - 1;
		path.Add (end);

		while (turn > 0) {
			HexDirection[] dirs = current.dirs;
			HexUtilities.ShuffleArray (dirs);
			foreach (HexDirection dir in dirs) {
				if (current.GetNeighbor(dir) && tblStore.ContainsKey(current.GetNeighbor(dir)) && tblStore [current.GetNeighbor(dir)] == turn) {
					path.Add (current.GetNeighbor(dir));
					current = current.GetNeighbor (dir);
					turn--;
					break;
				}
			}
		}
		return path;
	}
}