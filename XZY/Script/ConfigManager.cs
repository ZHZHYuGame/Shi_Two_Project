
     using System;
     using System.Collections.Generic;
     using Newtonsoft.Json;
     using Palmmedia.ReportGenerator.Core.Parser.Analysis;
     using UnityEngine;

     public class Item
     {
         public int id;
         public string name;
         public string des;
         public int type;
         public string path;
     }
     public class YJ
     {
         public int emailID;
         public string title;
         public string content;
         public bool isAward;
         public List<Item> awardList;
         public string StartTime;
         public float Time;
     }
     public class YJBase
     {
         public YJ YJ;
         public int YJID;
         public bool Is;
     }

     public class ConfigManager
     {
         public List<YJ> YJList;
         public List<Item> Itemlist;
         private static ConfigManager instance;
         public static ConfigManager Instance()
         {
             if (instance == null)
             {
                 instance = new ConfigManager();
             }
             return instance;
         }

         public ConfigManager()
         {
             YJList = JsonConvert.DeserializeObject<List<YJ>>(Resources.Load<TextAsset>("MailMsg").text);
             Itemlist = JsonConvert.DeserializeObject<List<Item>>(Resources.Load<TextAsset>("Item").text);
         }

     }
    

    
