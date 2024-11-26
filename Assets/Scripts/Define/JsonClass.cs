using System.Collections.Generic;

public class LevelData
{
    /// <summary>
    /// 
    /// </summary>
    public string Title { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string Question { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int SelectionNum { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string SelectionTxt_1 { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string SelectionTxt_2 { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string SelectionTxt_3 { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string SelectionTxt_4 { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string SelectionImg_1 { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string SelectionImg_2 { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string SelectionImg_3 { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string SelectionImg_4 { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<int> SelectionTag_1 { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<int> SelectionTag_2 { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<int> SelectionTag_3 { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<int> SelectionTag_4 { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public float SelectPercent_1 { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public float SelectPercent_2 { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public float SelectPercent_3 { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public float SelectPercent_4 { get; set; }
}

public class LevelListData
{
    /// <summary>
    /// 
    /// </summary>
    public List<LevelData> Level { get; set; }
}

public class TagData
{
    /// <summary>
    /// 
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string TypeName { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string DetailDesc { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string FunDesc { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string Tag { get; set; }
}

public class TagListData
{
    /// <summary>
    /// 
    /// </summary>
    public List<TagData> TagList { get; set; }
}