﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexAI {
	private int player;

	public HexAI(int player){
		this.player = player;
	}

	public HexCell GetNextPlayer(HexCell[] cells){
		foreach(HexCell cell in cells){
			if (cell.GetPlayer() == player && (cell.GetInfo().actions > 0 || cell.GetInfo().attacks > 0)) {
				return cell;
			}
		}
		return null;
	}

	public static HexCell[] aStar(HexCell[] cells, HexCell start){
		Dictionary<HexCell, int> tblStore = new Dictionary<HexCell, int>();
		Queue<HexCell> choices = new Queue<HexCell> ();
		choices.Enqueue (start);
		int turn = 1;
		List<HexCell> path = null;
		while (choices.Count > 0) {
			path = evaluatePaths(tblStore, choices, start, turn++);
			if (path != null) {
				break;
			}
		}
		return path == null ? new HexCell[0] : path.ToArray ();
	}

	private static List<HexCell> evaluatePaths(Dictionary<HexCell, int> tblStore, Queue<HexCell> choices, HexCell start, int turn) {
		Queue<HexCell> toEval = new Queue<HexCell> (choices);
		choices.Clear ();
		while (toEval.Count > 0) {
			HexCell nxt = toEval.Dequeue ();
			HexDirection[] dirs = nxt.dirs;
			HexUtilities.ShuffleArray (dirs);
			foreach(HexDirection dir in dirs) {
				//HexCell neighbor = nxt.GetNeighbor (dir);
				if (nxt.GetNeighbor (dir)) {
					//int nPlayer = nxt.GetNeighbor (dir).GetPlayer ();
					//int sPlayer = start.GetPlayer();
					//bool tCK = tblStore.ContainsKey (nxt.GetNeighbor (dir));

					if (nxt.GetNeighbor (dir).GetPlayer() != start.GetPlayer() && !tblStore.ContainsKey(nxt.GetNeighbor (dir))) {
						tblStore.Add (nxt.GetNeighbor (dir), turn);
						choices.Enqueue (nxt.GetNeighbor (dir));
						if (nxt.GetNeighbor (dir).GetPlayer() != -1) {
							//Destination
							List<HexCell> path = iterateBackFromPoint (nxt.GetNeighbor (dir), tblStore);
							return path;
						}
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