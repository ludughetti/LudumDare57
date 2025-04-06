using Audio;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioManagerSource", menuName = "DataSource/AudioManager" +
    "Source")]
public class AudioDataSource : ScriptableObject
{
    public AudioManager DataInstance { get; set; }
}
