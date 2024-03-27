using NAudio.CoreAudioApi;

public class MicrophoneService
{
    private MMDeviceEnumerator deviceEnumerator;
    public MicrophoneEntity CurrentMicrophone { get; private set; }
    private List<MicrophoneEntity> microphones;

    public MicrophoneService()
    {
        deviceEnumerator = new MMDeviceEnumerator();

        // Get all microphones and create a MicrophoneEntity for each
        microphones = deviceEnumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active)
            .Select(device => new MicrophoneEntity(device))
            .ToList();

        // Set the system default microphone as the current microphone
        var defaultMicrophone = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Console);
        CurrentMicrophone = microphones.FirstOrDefault(mic => mic.Microphone.ID == defaultMicrophone.ID);
    }

    public IEnumerable<MicrophoneEntity> GetMicrophones()
    {
        return microphones;
    }

    public void SetMicrophone(MicrophoneEntity newMicrophone)
    {
        CurrentMicrophone = newMicrophone;
    }
    public void RefreshMicrophones()
    {
        // Get all microphones and create a MicrophoneEntity for each
        microphones = deviceEnumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active)
            .Select(device => new MicrophoneEntity(device))
            .ToList();
    }

    public bool IsCurrentMicrophone(MicrophoneEntity microphone)
    {
        return CurrentMicrophone.Microphone.ID == microphone.Microphone.ID;
    }

    public MicrophoneEntity GetDefaultMicrophone()
    {
        var defaultMicrophone = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Communications);
        return microphones.FirstOrDefault(mic => mic.Microphone.ID == defaultMicrophone.ID);
    }
}
