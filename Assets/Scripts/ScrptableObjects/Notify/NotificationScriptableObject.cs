using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "NotificationData", menuName = "Data/Notification")]
public class NotificationScriptableObject : ScriptableObject
{
    [TextArea(minLines: 3, maxLines: 110)] public string agreeNotify;
    [TextArea(minLines: 3, maxLines: 110)] public string discardNotify;
    public Sprite iconPerson;
}
