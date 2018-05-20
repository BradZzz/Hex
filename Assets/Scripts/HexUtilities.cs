using UnityEngine;

public static class HexUtilities {
	public static void ShuffleArray<T>(T[] arr) {
		for (int i = arr.Length - 1; i > 0; i--) {
			int r = Random.Range(0, i);
			T tmp = arr[i];
			arr[i] = arr[r];
			arr[r] = tmp;
		}
	}

	public static HexDirection oppositeSide(HexDirection dir){
		switch(dir){
		case HexDirection.E:
			return HexDirection.W;
		case HexDirection.W:
			return HexDirection.E;
		case HexDirection.NE:
			return HexDirection.SW;
		case HexDirection.NW:
			return HexDirection.SE;
		case HexDirection.SW:
			return HexDirection.NE;
		case HexDirection.SE:
			return HexDirection.NW;
		default:
			return HexDirection.None;
		}
	}
}