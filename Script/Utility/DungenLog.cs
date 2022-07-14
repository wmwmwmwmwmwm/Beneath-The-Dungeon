using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DunGen;

public class DungenLog : MonoBehaviour
{
	public RuntimeDungeon DungenComponent;

	void Start()
	{
		DungenComponent.Generator.OnGenerationStatusChanged += Generator_OnGenerationStatusChanged;
	}

	void Generator_OnGenerationStatusChanged(DungeonGenerator generator, GenerationStatus status)
	{
		switch (status)
		{
			case GenerationStatus.Complete:
				print($"============generator.CurrentDungeon.AllTiles================");
				foreach (Tile tile in generator.CurrentDungeon.AllTiles) print($"{tile.name}");
				print($"============generator.CurrentDungeon.MainPathTiles================");
				foreach (Tile tile in generator.CurrentDungeon.MainPathTiles) print($"{tile.name}");
				print($"============generator.CurrentDungeon.BranchPathTiles================");
				foreach (Tile tile in generator.CurrentDungeon.BranchPathTiles) print($"{tile.name}");
				print($"============generator.CurrentDungeon.ConnectionGraph================");
				foreach (DungeonGraphNode node in generator.CurrentDungeon.ConnectionGraph.Nodes)
				{
					foreach (DungeonGraphConnection conn in node.Connections)
					{
						print($"{node.Tile.name}의 connection : {conn.A.Tile.name} {conn.B.Tile.name}   {conn.DoorwayA.name} {conn.DoorwayB.name}");
					}
				}
				break;
		}
	}
}
