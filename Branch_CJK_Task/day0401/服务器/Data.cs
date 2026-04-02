using System.Net.Sockets;
using Message;
using Newtonsoft.Json;

/// <summary>
/// 道具静态配置数据（对应 Item 表中的一条记录）
/// </summary>
public class ItemData
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("type")]
    public int Type { get; set; }

    [JsonProperty("subType")]
    public int SubType { get; set; }

    [JsonProperty("quality")]
    public int Quality { get; set; }

    [JsonProperty("packTab")]
    public int PackTab { get; set; }

    [JsonProperty("priority")]
    public int Priority { get; set; }

    [JsonProperty("maxPile")]
    public int MaxPile { get; set; }

    [JsonProperty("sellPrice")]
    public int SellPrice { get; set; }

    [JsonProperty("minLevel")]
    public int MinLevel { get; set; }

    [JsonProperty("obtainId")]
    public int ObtainId { get; set; }

    [JsonProperty("msg")]
    public string Msg { get; set; }

    [JsonProperty("icon")]
    public string Icon { get; set; }

    /// <summary>
    /// 基础属性（动态键值对，如 attack、defense 等）
    /// </summary>
    [JsonProperty("baseAttr")]
    public Dictionary<string, int> BaseAttr { get; set; }

    /// <summary>
    /// 可洗练属性列表
    /// </summary>
    [JsonProperty("washableAttr")]
    public List<WashableAttrData> WashableAttr { get; set; }

    /// <summary>
    /// 附加技能列表
    /// </summary>
    [JsonProperty("skills")]
    public List<SkillData> Skills { get; set; }

    [JsonProperty("socketCount")]
    public int SocketCount { get; set; }

    /// <summary>
    /// 每个槽位允许镶嵌的宝石类型，长度等于 SocketCount
    /// </summary>
    [JsonProperty("socketSlotTypes")]
    public List<Gem> SocketSlotTypes { get; set; }
}

public class EquipData
{
    public float attack;
    public float speed;
    public float defense;
    public float hp;
    public float mp;
}
/// <summary>
/// 可洗练属性配置
/// </summary>
public class WashableAttrData
{
    [JsonProperty("attrType")]
    public string AttrType { get; set; }

    [JsonProperty("min")]
    public int Min { get; set; }

    [JsonProperty("max")]
    public int Max { get; set; }

    /// <summary>
    /// 可洗出的品质范围 [min, max]，例如 [1,3] 表示可洗出灰~蓝品质
    /// </summary>
    [JsonProperty("qualityRange")]
    public int[] QualityRange { get; set; }
}
/// <summary>
/// 附加技能配置
/// </summary>
public class SkillData
{
    [JsonProperty("skillId")]
    public int SkillId { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("desc")]
    public string Desc { get; set; }
}
public class Gem
{
    public int id;
    public string name;
    public int level;
    public string icon;
}
//用户
public class UserData
{
    public BagData MainBag { get; set; }
    public EquippedData Equipped { get; set; }
    public List<QuickDrugSlot> QuickDrugs { get; set; }
    public int Gold { get; set; }  
}