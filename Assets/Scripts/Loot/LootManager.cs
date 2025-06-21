using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class LootManager : Panel
{
    private static LootManager instance;
    
    public static LootManager Instance
    {
        get
        {
            instance = instance != null ? instance : FindAnyObjectByType<LootManager>();
            return instance;
        }
    }

    private GameObject lootPrefab;
    private List<Loot> currentLoot;

    public List<Loot> CurrentLoot
    {
        get
        {
            return currentLoot;
        }
        set
        {
            currentLoot = value;
        }
    }
 

    private void Start()
    {
        lootPrefab = Resources.Load<GameObject>("Prefabs/Loot");
        currentLoot = new List<Loot>();
    }

    public List<LootEntry> GetLootByMobID(int id)
    {
        string query = $"SELECT * FROM mobs_loot WHERE mob_id = {id}";
        DataTable table = DBManager.Instance.ExecuteQuery(query);
        List<LootEntry> lootList = new();

        foreach (DataRow row in table.Rows)
        {
            int _itemId = int.Parse(row["item_id"].ToString());
            float _dropChance = float.Parse(row["drop_chance"].ToString());
            int _min = int.Parse(row["min"].ToString());
            int _max = int.Parse(row["max"].ToString());
            LootEntry loot = new(_itemId, _dropChance, _min, _max);
            lootList.Add(loot);
        }

        return lootList;
    }

    public List<DroppedLoot> CreateLootTableByMobID(int id)
    {
        List<LootEntry> potLootList = GetLootByMobID(id);
        List<DroppedLoot> lootList = new();
        foreach (LootEntry potLoot in potLootList)
        {
            float randomVal = Random.value;
            if (randomVal < potLoot.DropChance)
            {
                int amount = potLoot.Min;

                for (int i = potLoot.Min; i < potLoot.Max; i++)
                {
                    float chanceForMore = Mathf.Pow(potLoot.DropChance, i);

                    if (Random.value < chanceForMore)
                    {
                        amount++;
                    }
                }

                DroppedLoot loot = new(potLoot.ItemID, amount);
                lootList.Add(loot);
            }
        }

        return lootList;

    }

    public void ShowLootTable(List<DroppedLoot> lootList)
    {
        Close();

        foreach (DroppedLoot dLoot in lootList)
        {
            Loot loot = Instantiate(lootPrefab, transform).GetComponent<Loot>();
            currentLoot.Add(loot);
            loot.SetUpLoot(ItemsManager.Instance.GetItemByID(dLoot.Id), dLoot.Amount);
        }

        Open();
    }

    public override void Close()
    {
        base.Close();

        foreach (Loot loot in currentLoot)
        {
            Destroy(loot.gameObject);
        }

        currentLoot = new();
    }

}
