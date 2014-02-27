﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Creates a grid of specified dimensions and generates a procedural maze using a
 * modified form of Prim's Algorithm.
 * @author Timothy Sesler
 * @author tds45
 * @date 4 February 2014
 *
 * Adapted from work provided online by Austin Takechi
 * Contact: MinoruTono@Gmail.com
 */
public class GridCreator : MonoBehaviour {

	public Material floorTexture;
	public Material wallTexture;
	public Material endTexture;
	public Material startTexture;

	public Transform CellPrefab;
	public Transform WallPrefab;
	public Vector3 Size;
	public Transform[,] Grid;
	public Transform end;
	public Transform player;

	public Transform AmmoPickup;
	public Transform BatteryPickup;
	public Transform HPPickup;
	public Transform OxyPickup;

	public static float dimensions;

	public Transform MonsterPrefab;
	public int Monsters = 0;
	public int MonstersSpawned = 0;

	private Transform wall1;
	private Transform wall2;
	private Transform wall3;
	private Transform wall4;

	// Use this for initialization
	void Start () {
		if (Monsters < 1) {
			Monsters = 1;
		}
		else {
			Monsters = Monsters * 4;
		}
		MonstersSpawned = 0;
		CreateGrid();
		SetRandomNumbers();
		SetAdjacents();
		SetStart();
		FindNext();
		BuildWalls();
	}

	// Creates the grid by instantiating provided cell prefabs.
	void CreateGrid () {
		Grid = new Transform[(int)Size.x,(int)Size.z];
		dimensions = (Size.x - 1) * 6;

		// Places the cells and names them according to their coordinates in the grid.
		for (int x = 0; x < Size.x; x++) {
			for (int z = 0; z < Size.z; z++) {
				Transform newCell;
				newCell = (Transform)Instantiate(CellPrefab, new Vector3(6 * x, 0, 6 * z), Quaternion.identity);
				newCell.name = string.Format("({0},0,{1})", x, z);
				newCell.parent = transform;
				newCell.GetComponent<CellScript>().Position = new Vector3(6 * x, 0, 6 * z);
				Grid[x,z] = newCell;
			}
		}
		// Centers the camera on the maze.
		// Feel free to adjust this as needed.
		//Camera.main.transform.position = Grid[(int)(Size.x / 2f),(int)(Size.z / 2f)].position + Vector3.up * 100f;
		//Camera.main.orthographicSize = Mathf.Max(Size.x * 0.55f, Size.z * 0.5f);
	}

	// Sets a random weight to each cell.
	void SetRandomNumbers () {
		foreach (Transform child in transform) {
			int weight = Random.Range(0,10);
			child.GetComponentInChildren<TextMesh>().text = weight.ToString();
			child.GetComponent<CellScript>().Weight = weight;
		}
	}

	// Determines the adjacent cells of each cell in the grid.
	void SetAdjacents () {
		for(int x = 0; x < Size.x; x++){
			for (int z = 0; z < Size.z; z++) {
				Transform cell;
				cell = Grid[x,z];
				CellScript cScript = cell.GetComponent<CellScript>();

				if (x - 1 >= 0) {
					cScript.Adjacents.Add(Grid[x - 1, z]);
				}
				if (x + 1 < Size.x) {
					cScript.Adjacents.Add(Grid[x + 1, z]);
				}
				if (z - 1 >= 0) {
					cScript.Adjacents.Add(Grid[x, z - 1]);
				}
				if (z + 1 < Size.z) {
					cScript.Adjacents.Add(Grid[x, z + 1]);
				}

				cScript.Adjacents.Sort(SortByLowestWeight);
			}
		}
	}

	// Sorts the weights of adjacent cells.
	// Check the link for more info on custom comparators and sorting.
	// http://msdn.microsoft.com/en-us/library/0e743hdt.aspx
	int SortByLowestWeight (Transform inputA, Transform inputB) {
		int a = inputA.GetComponent<CellScript>().Weight;
		int b = inputB.GetComponent<CellScript>().Weight;
		return a.CompareTo(b);
	}

	/*********************************************************************
	 * Everything after this point pertains to generating the actual maze.
	 * Look at the Wikipedia page for more info on Prim's Algorithm.
	 * http://en.wikipedia.org/wiki/Prim%27s_algorithm
	 ********************************************************************/
	public List<Transform> PathCells;			// The cells in the path through the grid.
	public List<List<Transform>> AdjSet;		// A list of lists representing available adjacent cells.
	/** Here is the structure:
	 *  AdjSet{
	 * 		[ 0 ] is a list of all the cells
	 *      that have a weight of 0, and are
	 *      adjacent to the cells in the path
	 *      [ 1 ] is a list of all the cells
	 *      that have a weight of 1, and are
	 * 		adjacent to the cells in the path
	 *      ...
	 *      [ 9 ] is a list of all the cells
	 *      that have a weight of 9, and are
	 *      adjacent to the cells in the path
	 * 	}
	 *
	 * Note: Multiple entries of the same cell
	 * will not appear as duplicates.
	 * (Some adjacent cells will be next to
	 * two or three or four other path cells).
	 * They are only recorded in the AdjSet once.
	 */

	// Initializes the sets and the starting cell.
	void SetStart () {
		PathCells = new List<Transform>();
		AdjSet = new List<List<Transform>>();

		for (int i = 0; i < 10; i++) {
			AdjSet.Add(new List<Transform>());
		}
		AddToSet(Grid[0, 0]);
	}

	// Adds a cell to the set of visited cells.
	void AddToSet (Transform cellToAdd) {
		PathCells.Add(cellToAdd);

		foreach (Transform adj in cellToAdd.GetComponent<CellScript>().Adjacents) {
			adj.GetComponent<CellScript>().AdjacentsOpened++;

			if (!PathCells.Contains(adj) && !(AdjSet[adj.GetComponent<CellScript>().Weight].Contains(adj))) {
				AdjSet[adj.GetComponent<CellScript>().Weight].Add(adj);
			}
		}
	}

	// Determines the next cell to be visited.
	void FindNext () {
		Transform next;

		do {
			bool isEmpty = true;
			int lowestList = 0;

			// We loop through each sub-list in the AdjSet list of lists, until we find one with a count of more than 0.
			// If there are more than 0 items in the sub-list, it is not empty.
			// We've found the lowest sub-list, so there is no need to continue searching.
			for (int i = 0; i < 10; i++) {
				lowestList = i;

				if (AdjSet[i].Count > 0) {
					isEmpty = false;
					break;
				}
			}

			// The maze is complete.
			if (isEmpty) {
				//Debug.Log("Generation completed in " + Time.timeSinceLevelLoad + " seconds.");
				CancelInvoke("FindNext");
				end = PathCells[PathCells.Count - 1];

				foreach (Transform cell in Grid) {
					// Removes displayed weight
					cell.GetComponentInChildren<TextMesh>().renderer.enabled = false;

					if (!PathCells.Contains(cell)) {
						// Make the maze 3D
						cell.localScale += new Vector3(0f, 5f, 0f);
						cell.localPosition += new Vector3(0f, 3.5f, 0f);
						cell.renderer.material = wallTexture;
					}
					else {
						if (cell != Grid[0,0]){
							SpawnPickups(cell);
						}
						cell.renderer.material = floorTexture;
						if(MonstersSpawned < Monsters){
							Instantiate(MonsterPrefab, new Vector3(Random.Range (0, dimensions), 7f, Random.Range (0, dimensions)), Quaternion.identity);
							MonstersSpawned++;
						}
					}
				}
				// Give the start and exit some special textures
				Grid[0, 0].renderer.material = startTexture;
				end.renderer.material = endTexture;
				return;
			}
			// If we did not finish, then:
			// 1. Use the smallest sub-list in AdjSet as found earlier with the lowestList variable.
			// 2. With that smallest sub-list, take the first element in that list, and use it as the 'next'.
			next = AdjSet[lowestList][0];
			// Since we do not want the same cell in both AdjSet and Set, remove this 'next' variable from AdjSet.
			AdjSet[lowestList].Remove(next);
		} while (next.GetComponent<CellScript>().AdjacentsOpened >= 2);	// This keeps the walls in the grid, otherwise Prim's Algorithm would just visit every cell

		// The 'next' transform's material color becomes white.
		next.renderer.material.color = Color.white;
		// We add this 'next' transform to the Set our function.
		AddToSet(next);
		// Recursively call this function as soon as it finishes.
		Invoke("FindNext", 0);
	}

	void SpawnPickups(Transform cell){
		if (Random.Range(0, 100) <= 20){
			switch(Random.Range(0, 4) % 4){
				case 0:
					Instantiate(AmmoPickup, cell.localPosition + Vector3.up, Quaternion.identity);
					break;
				case 1:
					Instantiate(HPPickup, cell.localPosition + Vector3.up, Quaternion.identity);
					break;
				case 2:
					Instantiate(BatteryPickup, cell.localPosition + Vector3.up, Quaternion.identity);
					break;
				case 3:
					Instantiate(OxyPickup, cell.localPosition + Vector3.up, Quaternion.identity);
					break;
				default:
					Debug.Log("Bad spawn attempt");
					break;
			}
		}
	}

	void BuildWalls() {
		//Wall 1
		wall1 = (Transform)Instantiate(WallPrefab, new Vector3(-4.5f, 3.5f, (Size.z / 2f) * 6f - 3f), Quaternion.identity);
		wall1.name = string.Format("Wall");
		wall1.localScale += new Vector3(2f, 5f, Size.z * 6f);
		wall1.renderer.material.mainTextureScale = new Vector2(Size.z,1);
		//Wall 2
		wall2 = (Transform)Instantiate(WallPrefab, new Vector3(6f * Size.x - 1.5f, 3.5f, (Size.z / 2f) * 6f - 3f), Quaternion.identity);
		wall2.name = string.Format("Wall");
		wall2.localScale += new Vector3(2f, 5f, Size.z * 6f);
		wall2.renderer.material.mainTextureScale = new Vector2(Size.z,1);
		//Wall 3
		wall3 = (Transform)Instantiate(WallPrefab, new Vector3((Size.x / 2f) * 6f - 3f, 3.5f, -4.5f), Quaternion.identity);
		wall3.name = string.Format("Wall");
		wall3.localScale += new Vector3(Size.x * 6f, 5f, 2f);
		wall3.renderer.material.mainTextureScale = new Vector2(Size.z,1);
		//Wall 4
		wall4 = (Transform)Instantiate(WallPrefab, new Vector3((Size.x / 2f) * 6f - 3f, 3.5f, 6f * Size.z - 1.5f), Quaternion.identity);
		wall4.name = string.Format("Wall");
		wall4.localScale += new Vector3(Size.x * 6f, 5f, 2f);
		wall4.renderer.material.mainTextureScale = new Vector2(Size.z,1);
	}

	// Called once per frame.
	void Update() {
		// Check if the player is at the end
		if (end != null) {
			if ((player.localPosition.x >= end.localPosition.x - 2f)
			 && (player.localPosition.x <= end.localPosition.x + 2f)
			 && (player.localPosition.z >= end.localPosition.z - 2f)
			 && (player.localPosition.z <= end.localPosition.z + 2f)) {
				// If so, destroy the old maze...
				Destroy(wall1.transform.gameObject);
				Destroy(wall2.transform.gameObject);
				Destroy(wall3.transform.gameObject);
				Destroy(wall4.transform.gameObject);
				for (int i = 0; i < transform.childCount; i++) {
					Destroy(transform.GetChild(i).gameObject);
				}
				// And restart the maze
				player.localPosition = new Vector3(0f, 5f, 0f);
				Size.Set(Size.x + 5f, Size.y, Size.z + 5f);
				Start();
				// TODO: Increment the level counter
			}
		}
	}
}