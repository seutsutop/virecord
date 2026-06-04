namespace VIrecord.Core;

public enum RecordingState
{
    Stopped,
    Recording,
    Paused
}

public enum EncoderType
{
    Software,    // x264
    NVENC,       // NVIDIA GPU
    QSV,         // Intel Quick Sync
    AMF          // AMD GPU
}

public enum OutputFormat
{
    MP4,
    AVI,
    MKV,
    GIF
}

public class RecordingSettings
{
    public int FrameRate { get; set; } = 30;
    public int BitRate { get; set; } = 8_000_000; // 8 Mbps
    public EncoderType Encoder { get; set; } = EncoderType.Software;
    public OutputFormat Format { get; set; } = OutputFormat.MP4;
    public bool RecordAudio { get; set; } = true;
    public bool RecordCursor { get; set; } = true;
    public string OutputFolder { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
    public string FileNameFormat { get; set; } = "VIrecord_{yyyy-MM-dd_HH-mm-ss}";
}

public class ScreenInfo
{
    public int Index { get; set; }
    public string Name { get; set; } = "";
    public int Width { get; set; }
    public int Height { get; set; }
    public int Left { get; set; }
    public int Top { get; set; }
}