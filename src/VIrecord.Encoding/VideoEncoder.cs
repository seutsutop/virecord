using System.Diagnostics;
using System.Drawing;
using System.IO;
using VIrecord.Core;

namespace VIrecord.Encoding;

public class VideoEncoder : IDisposable
{
    private Process? _ffmpegProcess;
    private readonly RecordingSettings _settings;
    private bool _isRecording;
    private readonly object _lock = new();

    public event EventHandler<EncoderErrorEventArgs>? ErrorOccurred;

    public VideoEncoder(RecordingSettings settings)
    {
        _settings = settings;
    }

    public string StartRecording(string outputPath)
    {
        lock (_lock)
        {
            if (_isRecording) return "";

            string fileName = Path.Combine(
                _settings.OutputFolder,
                _settings.FileNameFormat.Replace("{yyyy-MM-dd_HH-mm-ss}", DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")) +
                GetExtension());

            Directory.CreateDirectory(_settings.OutputFolder);

            var args = BuildFFmpegArgs(fileName);

            _ffmpegProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "ffmpeg",
                    Arguments = args,
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };

            try
            {
                _ffmpegProcess.Start();
            }
            catch (Exception ex)
            {
                _ffmpegProcess.Dispose();
                _ffmpegProcess = null;
                ErrorOccurred?.Invoke(this, new EncoderErrorEventArgs(
                    "Failed to start FFmpeg. Ensure FFmpeg is installed and available in PATH.", ex));
                throw new InvalidOperationException(
                    "Failed to start FFmpeg. Ensure FFmpeg is installed and available in PATH.", ex);
            }

            _isRecording = true;

            return fileName;
        }
    }

    public void WriteFrame(Bitmap frame)
    {
        lock (_lock)
        {
            if (!_isRecording || _ffmpegProcess == null) return;

            using var ms = new MemoryStream();
            frame.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            var pngData = ms.ToArray();

            try
            {
                _ffmpegProcess.StandardInput.BaseStream.Write(pngData, 0, pngData.Length);
                _ffmpegProcess.StandardInput.BaseStream.Flush();
            }
            catch (IOException ex)
            {
                _isRecording = false;
                ErrorOccurred?.Invoke(this, new EncoderErrorEventArgs(
                    "FFmpeg pipe broken — recording stopped. The encoder process may have crashed.", ex));
            }
            catch (ObjectDisposedException)
            {
                _isRecording = false;
            }
        }
    }

    public void StopRecording()
    {
        lock (_lock)
        {
            if (!_isRecording) return;

            try
            {
                _ffmpegProcess?.StandardInput.Close();
                _ffmpegProcess?.WaitForExit(10000);

                if (_ffmpegProcess is { HasExited: true, ExitCode: not 0 })
                {
                    string stderr = "";
                    try { stderr = _ffmpegProcess.StandardError.ReadToEnd(); } catch { }
                    ErrorOccurred?.Invoke(this, new EncoderErrorEventArgs(
                        $"FFmpeg exited with code {_ffmpegProcess.ExitCode}.{(string.IsNullOrEmpty(stderr) ? "" : " " + stderr)}", null));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during FFmpeg shutdown: {ex.Message}");
            }
            finally
            {
                _ffmpegProcess?.Dispose();
                _ffmpegProcess = null;
                _isRecording = false;
            }
        }
    }

    private string BuildFFmpegArgs(string outputPath)
    {
        var args = new List<string>
        {
            "-f", "rawvideo",
            "-pix_fmt", "bgra",
            "-s", $"{GetSystemMetrics(78)}x{GetSystemMetrics(79)}",
            "-r", _settings.FrameRate.ToString(),
            "-i", "pipe:0"
        };

        // Video codec
        switch (_settings.Encoder)
        {
            case EncoderType.NVENC:
                args.AddRange(new[] { "-c:v", "h264_nvenc", "-preset", "fast", "-b:v", $"{_settings.BitRate / 1000}k" });
                break;
            case EncoderType.QSV:
                args.AddRange(new[] { "-c:v", "h264_qsv", "-b:v", $"{_settings.BitRate / 1000}k" });
                break;
            case EncoderType.AMF:
                args.AddRange(new[] { "-c:v", "h264_amf", "-b:v", $"{_settings.BitRate / 1000}k" });
                break;
            default:
                args.AddRange(new[] { "-c:v", "libx264", "-preset", "fast", "-b:v", $"{_settings.BitRate / 1000}k" });
                break;
        }

        // Audio
        if (_settings.RecordAudio)
        {
            args.AddRange(new[] { "-c:a", "aac", "-b:a", "128k" });
        }

        args.Add("-y");
        args.Add(outputPath);

        return string.Join(" ", args);
    }

    private string GetExtension() => _settings.Format switch
    {
        OutputFormat.AVI => ".avi",
        OutputFormat.MKV => ".mkv",
        OutputFormat.GIF => ".gif",
        _ => ".mp4"
    };

    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern int GetSystemMetrics(int nIndex);
    private const int SM_CXVIRTUALSCREEN = 78;
    private const int SM_CYVIRTUALSCREEN = 79;

    public void Dispose()
    {
        StopRecording();
        GC.SuppressFinalize(this);
    }
}

public class EncoderErrorEventArgs : EventArgs
{
    public string Message { get; }
    public Exception? InnerException { get; }

    public EncoderErrorEventArgs(string message, Exception? innerException)
    {
        Message = message;
        InnerException = innerException;
    }
}