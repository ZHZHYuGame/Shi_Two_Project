using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games;
using Google.Protobuf;

public class Manager : Singleton<Manager>
{
    public ChatType chatType = ChatType.World;
    public List<string> userIDs = new List<string>();
}
