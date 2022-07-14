#if UNITY_EDITOR
using DunGen.Graph;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static SingletonLoader;

public class BattleTestManager : MonoBehaviour
{
	[Title("인게임")]
	public RaceData TestRaceData;

	void Start()
	{
		StartCoroutine(InitCoroutine());
		IEnumerator InitCoroutine()
		{
			yield return new WaitForSeconds(0.1f);
			gm.Phase = GameManager.PhaseEnum.Dungeon;
			p.Name = "테스트맨";
			p.ThisRaceData = TestRaceData;
			p.Status.SetStartPlayerStatus(p.ThisRaceData);
			p.Status.HealAll();
			dc.CurrentDungeonData = StartDungeonData;
		}
	}

	public DungeonData StartDungeonData;
	[Button("테스트 던전 생성", ButtonHeight = 80)]
	public void CreateTestDungeon()
	{
		StartCoroutine(CreateTestDungeonCoroutine());
		IEnumerator CreateTestDungeonCoroutine()
		{
			dc.DungenComponent.Generator.DungeonFlow = db.LoadAssetReference<DungeonFlow>(StartDungeonData.DungenFlow);
			dc.CurrentFloor = 1;
			dc.NewGameSetup();
			dc.LastUsedPortalIndex = 4;
			dc.LastUsedPortalType = PortalTypeEnum.Start;
			dc.LastUsedPortalType = StartDungeonData.DungeonArea switch
			{
				DungeonAreaEnum.MainDungeon13 => PortalTypeEnum.Start,
				DungeonAreaEnum.MainDungeon47 => PortalTypeEnum.Upstair,
				DungeonAreaEnum.MainDungeon812 => PortalTypeEnum.Upstair,
				DungeonAreaEnum.MainDungeon1315 => PortalTypeEnum.Upstair,
				DungeonAreaEnum.LostTemple => PortalTypeEnum.Entrance,
				DungeonAreaEnum.ShadowTunnel => PortalTypeEnum.Entrance,
				DungeonAreaEnum.OceanGarden => PortalTypeEnum.Entrance,
				DungeonAreaEnum.Pandemonium => PortalTypeEnum.Entrance,
				DungeonAreaEnum.IronCanyon => PortalTypeEnum.Entrance,
				DungeonAreaEnum.AncientForest => PortalTypeEnum.Entrance,
				DungeonAreaEnum.Necropolis => PortalTypeEnum.Entrance,
				DungeonAreaEnum.SpiritGrave => PortalTypeEnum.Entrance,
				DungeonAreaEnum.Inferno => PortalTypeEnum.Entrance,
				DungeonAreaEnum.Monolith => PortalTypeEnum.Entrance,
				_ => PortalTypeEnum.Entrance,
			};

			yield return StartCoroutine(DungeonController.Instance.LoadFloor());
			if (StartDungeonData == TestDungeonData)
			{
				List<RoomInformation>.Enumerator AllRoomEnumerator = DungeonController.Instance.Map.Values.ToList().GetEnumerator();
				foreach (DungeonData.MonsterSpawnInfo OneMonsterInfo in StartDungeonData.MonsterSpawnInfoList)
				{
					AllRoomEnumerator.MoveNext();
					DungeonController.Instance.CreateEventObject(AllRoomEnumerator.Current, OneMonsterInfo.MonsterPrefab.gameObject);
				}
				foreach (Monster OneMonster in StartDungeonData.NamedMonsterPrefabs)
				{
					AllRoomEnumerator.MoveNext();
					DungeonController.Instance.CreateEventObject(AllRoomEnumerator.Current, OneMonster.gameObject);
				}
				//for (int i = 0; i < 10; i++)
				//{
				//	AllRoomEnumerator.MoveNext();
				//	GameObject NewSkillChest = DungeonController.Instance.CreateEventObject(AllRoomEnumerator.Current, DBManager.Instance.SkillChestSteelPrefab);
				//	NewSkillChest.GetComponent<EncounterEventSkillChest>().SkillID = DungeonController.Instance.GetSpawnSkillID();
				//}
				for (int i = 0; i < 10; i++)
				{
					AllRoomEnumerator.MoveNext();
					GameObject NewEquipmentChest = DungeonController.Instance.CreateEventObject(AllRoomEnumerator.Current, DBManager.Instance.EquipmentChestSteelPrefab);
					NewEquipmentChest.GetComponent<EncounterEventEquipmentChest>().ContainEquipment = DBManager.Instance.GetRandomEquipment();
				}
			}
		}
	}

	public Monster BattleMonster;
	[Button("전투 시작", ButtonHeight = 80)]
	public void StartBattle()
	{
		StartCoroutine(em.StartBattle(BattleMonster));
	}

	[Title("에디터")]
	public DungeonData TestDungeonData;
	public List<DungeonData> MonsterListOfTestDungeon;
	[Button("TestDungeon에 몬스터들 할당", ButtonHeight = 80)]
	void AssignMonsters()
	{
		TestDungeonData.MonsterSpawnInfoList = new List<DungeonData.MonsterSpawnInfo>();
		TestDungeonData.NamedMonsterPrefabs = new List<Monster>();
		foreach (DungeonData OneDungeon in MonsterListOfTestDungeon)
		{
			foreach (DungeonData.MonsterSpawnInfo OneMonsterInfo in OneDungeon.MonsterSpawnInfoList)
			{
				TestDungeonData.MonsterSpawnInfoList.Add(new DungeonData.MonsterSpawnInfo()
				{
					MonsterPrefab = OneMonsterInfo.MonsterPrefab,
				});
			}
			foreach (Monster OneMonster in OneDungeon.NamedMonsterPrefabs)
			{
				TestDungeonData.NamedMonsterPrefabs.Add(OneMonster);
			}
		}
		EditorUtility.SetDirty(TestDungeonData);
		AssetDatabase.SaveAssets();
	}
}
#endif