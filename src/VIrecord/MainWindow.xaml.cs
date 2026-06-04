using System.Windows;
using System.Windows.Threading;
using VIrecord.Core;
using VIrecord.Capture;
using VIrecord.Encoding;

namespace VIrecord;

public partial class MainWindow : Window
{
    private readonly RecordingSettings _settings = new();
    private VideoEncoder? _encoder;
    private DispatcherTimer? _captureTimer;
    private RecordingState _state = RecordingState.Stopped;
    private DateTime _recordingStart;

    public MainWindow()
    {
        InitializeComponent();
        UpdateStatus();
    }

    private void BtnRecord_Click(object sender, RoutedEventArgs e)
    {
        if (_state == RecordingState.Stopped)
            StartRecording();
        else
            StopRecording();
    }

    private void StartRecording()
    {
        _encoder = new VideoEncoder(_settings);
        string outputPath = _encoder.StartRecording("");

        _recordingStart = DateTime.Now;
        _state = RecordingState.Recording;

        _captureTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(1000.0 / _settings.FrameRate)
        };
        _captureTimer.Tick += CaptureFrame;
        _captureTimer.Start();

        UpdateStatus();
    }

    private void StopRecording()
    {
        _captureTimer?.Stop();
        _encoder?.StopRecording();
        _encoder?.Dispose();
        _state = RecordingState.Stopped;
        UpdateStatus();
    }

    private void CaptureFrame(object? sender, EventArgs e)
    {
        if (_state != RecordingState.Recording) return;

        try
        {
            using var bitmap = ScreenCapturer.CaptureScreen();
            _encoder?.WriteFrame(bitmap);
        }
        catch { }
    }

    private void UpdateStatus()
    {
        var duration = _state == RecordingState.Recording
            ? DateTime.Now - _recordingStart
            : TimeSpan.Zero;

        TxtStatus.Text = _state switch
        {
            RecordingState.Recording => $"● REC {duration:hh\\:mm\\:ss}",
            RecordingState.Paused => "⏸ PAUSED",
            _ => "⏹ STOPPED"
        };

        BtnRecord.Content = _state == RecordingState.Stopped ? "● REC" : "■ STOP";
    }

    protected override void OnClosed(EventArgs e)
    {
        _captureTimer?.Stop();
        _encoder?.Dispose();
        base.OnClosed(e);
    }
}