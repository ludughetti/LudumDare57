using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Dialgogue", fileName = "Dialogue")]
public class DialogueData : ScriptableObject
{
    [SerializeField] private List<DialogueStruct> _dialogue;

    public List<DialogueStruct> Dialogue {  get { return _dialogue; } }
}
