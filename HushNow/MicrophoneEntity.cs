using NAudio.CoreAudioApi;

public class MicrophoneEntity
{
    public MMDevice Microphone { get; private set; }

    public MicrophoneEntity(MMDevice microphone)
    {
        Microphone = microphone;
    }

    public bool IsMuted()
    {
        return Microphone.AudioEndpointVolume.Mute;
    }

    public void Mute()
    {
        Microphone.AudioEndpointVolume.Mute = true;
    }

    public void Unmute()
    {
        Microphone.AudioEndpointVolume.Mute = false;
    }
}
