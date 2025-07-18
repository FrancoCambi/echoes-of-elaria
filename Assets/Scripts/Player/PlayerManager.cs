using System;
using System.Data;
using UnityEngine;
using UnityEngine.Localization.Settings;
using static UnityEngine.Rendering.DebugUI;
public class PlayerManager : MonoBehaviour
{
    private static PlayerManager instance;

    public static PlayerManager Instance
    {
        get
        {
            instance = instance != null ? instance : FindAnyObjectByType<PlayerManager>();
            return instance;
        }
    }

    public int Level { get; private set; }
    public int ItemLevel { get; private set; }
    public int CurrentXp { get; private set; }
    public int MinDamage { get; private set; }
    public int MaxDamage { get; private set; }
    public int Armor { get; private set; }
    public int Stamina { get; private set; }
    public int Intellect { get; private set; }
    public int ArcanePower { get; private set; }
    public float BasicAttackRange { get; private set; }
    public float MovementSpeed { get; private set; }
    public float DashForce { get; private set; }
    public float DashCD { get; private set; }
    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }
    public int MaxRage { get; private set; }
    public int CurrentRage { get; private set; }
    public int InventorySpace {  get; private set; }    

    public static event Action OnLevelUp;
    public static event Action OnXpGained;
    public static event Action OnCurrentHealthChanged;
    public static event Action OnMaxHealthChanged;
    public static event Action OnCurrentRageChanged;
    public static event Action OnStatsChanged;


    private void Awake()
    {
        LoadStatsFromDatabase();
    }

    private void OnEnable()
    {
        EquipmentManager.OnEquipmentChanged += SaveStatsToDatabase;
    }

    private void OnDisable()
    {
        EquipmentManager.OnEquipmentChanged -= SaveStatsToDatabase;
    }

    #region Updaters

    public void UpdateLevel(int newLevel)
    {
        Level = newLevel;
        SaveStatToDatabase("level", newLevel);
        OnLevelUp?.Invoke();
    }

    public void UpdateCurrentXp(int newXp)
    {
        CurrentXp = newXp;
        SaveStatToDatabase("current_xp", CurrentXp);
        OnXpGained?.Invoke();
    }

    public void UpdateMovementSpeed(float newSpeed)
    {
        MovementSpeed = newSpeed;
        SaveStatToDatabase("movement_speed", newSpeed);
    }

    public void UpdateDashForce(float newDashForce)
    {
        DashForce = newDashForce;
        SaveStatToDatabase("dash_force", newDashForce);
    }

    public void UpdateCurrentHealth(int newHealth)
    {
        CurrentHealth = newHealth;
        SaveStatToDatabase("current_health", CurrentHealth);
        OnCurrentHealthChanged?.Invoke();
    }

    public void UpdateMaxHealth(int newHealth)
    {
        MaxHealth = newHealth;
        SaveStatToDatabase("max_health", MaxHealth);
        OnMaxHealthChanged?.Invoke();

    }

    public void UpdateMinDamage(int newMinDamage)
    {
        MinDamage = newMinDamage;
        SaveStatToDatabase("current_health", MinDamage);
    }

    public void UpdateMaxDamage(int newMaxDamage)
    {
        MaxDamage = newMaxDamage;
        SaveStatToDatabase("current_health", MaxDamage);
    }

    // ------------------STATS---------------------------//

    public void UpdateArmor(int newArmor)
    {
        Armor = newArmor;
        OnStatsChanged?.Invoke();
    }

    public void UpdateStamina(int newStamina)
    {
        Stamina = newStamina;
        OnStatsChanged?.Invoke();
    }

    public void UpdateIntellect(int newIntellect)
    {
        Intellect = newIntellect;
        OnStatsChanged?.Invoke();
    }

    public void UpdateArcanePower(int newArcanePower)
    {
        ArcanePower = newArcanePower;
        OnStatsChanged?.Invoke();
    }

    // ------------------STATS---------------------------//

    public void UpdateMaxRage(int newMaxRage)
    {
        MaxRage = newMaxRage;
        SaveStatToDatabase("max_rage", MaxRage);
    }


    public void UpdateCurrentRage(int newRage)
    {
        CurrentRage = newRage;
        SaveStatToDatabase("current_rage", CurrentRage);
        OnCurrentRageChanged?.Invoke();

    }

    #endregion

    #region Manage
    public void LevelUp()
    {
        UpdateLevel(++Level);

        CurrentXp = 0;

        string alertLocalized = LocalizationSettings.StringDatabase.GetLocalizedString("Ui", "AlertLevelUp");
        AlertManager.Instance.ThrowAlert(alertLocalized);
    }

    public void GainXp(int xp)
    {
        int maxXp = XpManager.Instance.CalculateMaxXp(Level);
        int xpToGain = xp;

        if (CurrentXp + xp >= maxXp)
        {
            xpToGain = xp - (maxXp - CurrentXp);
            LevelUp();
            maxXp = XpManager.Instance.CalculateMaxXp(Level);
        }

        UpdateCurrentXp(Mathf.Clamp(CurrentXp + xpToGain, 0, maxXp));

        FloatingTextManager.Instance.ShowFloatingText(FloatingTextType.Xp, $"+{xp}xp", transform.position, new Vector2(-1, 0.5f));
    }

    // ------------------STATS---------------------------//

    public void GainArmor(int armor)
    {
        UpdateArmor((int)Mathf.Clamp(Armor + armor, 0, Mathf.Infinity));
    }

    public void GainStamina(int stamina)
    {
        UpdateStamina((int)Mathf.Clamp(Stamina + stamina, 0, Mathf.Infinity));
        GainMaxHealth(stamina * GameConstants.StaminaMultiplier);
    }

    public void GainIntellect(int intellect)
    {
        UpdateIntellect((int)Mathf.Clamp(Intellect + intellect, 0, Mathf.Infinity));
    }

    public void GainArcanePower(int arcanePower)
    {
        UpdateArcanePower((int)Mathf.Clamp(ArcanePower + arcanePower, 0, Mathf.Infinity));
    }

    public void LoseArmor(int armor)
    {
        UpdateArmor((int)Mathf.Clamp(Armor - armor, 0, Mathf.Infinity));
    }

    public void LoseStamina(int stamina)
    {
        UpdateStamina((int)Mathf.Clamp(Stamina - stamina, 0, Mathf.Infinity));
        LoseMaxHealth(stamina * GameConstants.StaminaMultiplier);
    }

    public void LoseIntellect(int intellect)
    {
        UpdateIntellect((int)Mathf.Clamp(Intellect - intellect, 0, Mathf.Infinity));
    }

    public void LoseArcanePower(int arcanePower)
    {
        UpdateArcanePower((int)Mathf.Clamp(ArcanePower - arcanePower, 0, Mathf.Infinity));
    }

    // ------------------STATS---------------------------//

    /// <summary>
    /// Increases max health
    /// </summary>
    /// <param name="hp"></param>
    public void GainMaxHealth(int hp)
    {
        UpdateMaxHealth((int)Mathf.Clamp(MaxHealth + hp, 0, Mathf.Infinity));
    }

    /// <summary>
    /// Decreases max health
    /// </summary>
    /// <param name="hp"></param>
    public void LoseMaxHealth(int hp)
    {
        UpdateMaxHealth((int)Mathf.Clamp(MaxHealth - hp, 0, Mathf.Infinity));
    }

    /// <summary>
    /// Heal function
    /// </summary>
    /// <param name="hp"></param>
    public void GainCurrentHealth(int hp)
    {
        UpdateCurrentHealth(Mathf.Clamp(CurrentHealth + hp, 0, MaxHealth));

        AudioClip healSoundClip = Resources.Load<AudioClip>("Audio/Clips/Heal");

        SoundFXManager.Instance.PlaySoundFXClip(healSoundClip, transform);
        FloatingTextManager.Instance.ShowFloatingText(FloatingTextType.Heal, $"+{hp}", transform.position, new Vector2(0, 0));
    }

    /// <summary>
    /// Take damage function
    /// </summary>
    /// <param name="damage"></param>
    public void LoseCurrentHealth(int damage)
    {
        UpdateCurrentHealth(Mathf.Clamp(CurrentHealth - damage, 0, MaxHealth));

        // Gain rage (This formula probably needs to change in the future)
        int rageGained = (int)Mathf.Ceil((damage * 3) / (float)(Level * 8));
        GainRage(rageGained);

        FloatingTextManager.Instance.ShowFloatingText(FloatingTextType.Damage, $"-{damage}", transform.position, new Vector2(0, 0));
    }

    public void GainRage(int amount)
    {
        UpdateCurrentRage(Mathf.Clamp(CurrentRage += amount, 0, MaxRage));

    }


    #endregion

    #region database

    private void LoadStatsFromDatabase()
    {

        string query = $"SELECT * FROM characters WHERE character_id = {GameManager.SelCharID}";
        DataTable table = DBManager.Instance.ExecuteQuery(query);

        if (table.Rows.Count > 0)
        {
            Level = int.Parse(table.Rows[0]["level"].ToString());
            ItemLevel = int.Parse(table.Rows[0]["level"].ToString());
            CurrentXp = int.Parse(table.Rows[0]["current_xp"].ToString());
            MovementSpeed = float.Parse(table.Rows[0]["movement_speed"].ToString());
            DashForce = float.Parse(table.Rows[0]["dash_force"].ToString());
            DashCD = float.Parse(table.Rows[0]["dash_cd"].ToString());
            MinDamage = int.Parse(table.Rows[0]["min_damage"].ToString());
            MaxDamage = int.Parse(table.Rows[0]["max_damage"].ToString());
            Armor = int.Parse(table.Rows[0]["armor"].ToString());
            Stamina = int.Parse(table.Rows[0]["stamina"].ToString());
            Intellect = int.Parse(table.Rows[0]["intellect"].ToString());
            ArcanePower = int.Parse(table.Rows[0]["arcane_power"].ToString());
            BasicAttackRange = float.Parse(table.Rows[0]["basic_attack_range"].ToString());
            MaxHealth = int.Parse(table.Rows[0]["max_health"].ToString());
            CurrentHealth = int.Parse(table.Rows[0]["current_health"].ToString());
            MaxRage = int.Parse(table.Rows[0]["max_rage"].ToString());
            CurrentRage = int.Parse(table.Rows[0]["current_rage"].ToString());
            InventorySpace = int.Parse(table.Rows[0]["inventory_space"].ToString());


        }
    }

    private void SaveStatToDatabase(string statName, float value)
    {
        string query = $"UPDATE characters SET {statName} = {value} WHERE character_id = {GameManager.SelCharID}";
        DBManager.Instance.ExecuteQuery(query);
    }

    private void SaveStatsToDatabase()
    {
        string query = $"UPDATE characters " +
            $"SET armor = {Armor}, " +
                 $"stamina = {Stamina}, " +
                 $"intellect = {Intellect}, " +
                 $"arcane_power = {ArcanePower} " + 
                 $"WHERE character_id = {GameManager.SelCharID}";
        DBManager.Instance.ExecuteQuery(query);
    }

    #endregion

}
