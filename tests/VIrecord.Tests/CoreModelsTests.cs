using Xunit;
using VIrecord.Core;

namespace VIrecord.Tests;

public class RecordingSettingsTests
{
    [Fact]
    public void DefaultFrameRate_Is30()
    {
        var settings = new RecordingSettings();
        Assert.Equal(30, settings.FrameRate);
    }

    [Fact]
    public void DefaultBitRate_Is8Mbps()
    {
        var settings = new RecordingSettings();
        Assert.Equal(8_000_000, settings.BitRate);
    }

    [Fact]
    public void DefaultEncoder_IsSoftware()
    {
        var settings = new RecordingSettings();
        Assert.Equal(EncoderType.Software, settings.Encoder);
    }

    [Fact]
    public void DefaultFormat_IsMP4()
    {
        var settings = new RecordingSettings();
        Assert.Equal(OutputFormat.MP4, settings.Format);
    }

    [Fact]
    public void DefaultRecordAudio_IsTrue()
    {
        var settings = new RecordingSettings();
        Assert.True(settings.RecordAudio);
    }

    [Fact]
    public void DefaultRecordCursor_IsTrue()
    {
        var settings = new RecordingSettings();
        Assert.True(settings.RecordCursor);
    }

    [Fact]
    public void DefaultOutputFolder_IsNotEmpty()
    {
        var settings = new RecordingSettings();
        Assert.False(string.IsNullOrEmpty(settings.OutputFolder));
    }

    [Fact]
    public void DefaultFileNameFormat_ContainsDatePlaceholder()
    {
        var settings = new RecordingSettings();
        Assert.Contains("{yyyy-MM-dd_HH-mm-ss}", settings.FileNameFormat);
    }

    [Fact]
    public void CanSetCustomFrameRate()
    {
        var settings = new RecordingSettings { FrameRate = 60 };
        Assert.Equal(60, settings.FrameRate);
    }

    [Fact]
    public void CanSetCustomBitRate()
    {
        var settings = new RecordingSettings { BitRate = 16_000_000 };
        Assert.Equal(16_000_000, settings.BitRate);
    }

    [Fact]
    public void CanSetEncoder_NVENC()
    {
        var settings = new RecordingSettings { Encoder = EncoderType.NVENC };
        Assert.Equal(EncoderType.NVENC, settings.Encoder);
    }

    [Fact]
    public void CanSetEncoder_QSV()
    {
        var settings = new RecordingSettings { Encoder = EncoderType.QSV };
        Assert.Equal(EncoderType.QSV, settings.Encoder);
    }

    [Fact]
    public void CanSetEncoder_AMF()
    {
        var settings = new RecordingSettings { Encoder = EncoderType.AMF };
        Assert.Equal(EncoderType.AMF, settings.Encoder);
    }

    [Fact]
    public void CanSetFormat_AVI()
    {
        var settings = new RecordingSettings { Format = OutputFormat.AVI };
        Assert.Equal(OutputFormat.AVI, settings.Format);
    }

    [Fact]
    public void CanSetFormat_MKV()
    {
        var settings = new RecordingSettings { Format = OutputFormat.MKV };
        Assert.Equal(OutputFormat.MKV, settings.Format);
    }

    [Fact]
    public void CanSetFormat_GIF()
    {
        var settings = new RecordingSettings { Format = OutputFormat.GIF };
        Assert.Equal(OutputFormat.GIF, settings.Format);
    }

    [Fact]
    public void CanDisableAudioRecording()
    {
        var settings = new RecordingSettings { RecordAudio = false };
        Assert.False(settings.RecordAudio);
    }

    [Fact]
    public void CanDisableCursorRecording()
    {
        var settings = new RecordingSettings { RecordCursor = false };
        Assert.False(settings.RecordCursor);
    }
}

public class RecordingStateTests
{
    [Fact]
    public void RecordingState_HasExpectedValues()
    {
        Assert.Equal(0, (int)RecordingState.Stopped);
        Assert.Equal(1, (int)RecordingState.Recording);
        Assert.Equal(2, (int)RecordingState.Paused);
    }

    [Fact]
    public void RecordingState_DefaultIsStopped()
    {
        RecordingState state = default;
        Assert.Equal(RecordingState.Stopped, state);
    }
}

public class EncoderTypeTests
{
    [Fact]
    public void EncoderType_HasFourValues()
    {
        var values = Enum.GetValues<EncoderType>();
        Assert.Equal(4, values.Length);
    }

    [Theory]
    [InlineData(EncoderType.Software, 0)]
    [InlineData(EncoderType.NVENC, 1)]
    [InlineData(EncoderType.QSV, 2)]
    [InlineData(EncoderType.AMF, 3)]
    public void EncoderType_HasCorrectOrdinalValues(EncoderType encoder, int expected)
    {
        Assert.Equal(expected, (int)encoder);
    }
}

public class OutputFormatTests
{
    [Fact]
    public void OutputFormat_HasFourValues()
    {
        var values = Enum.GetValues<OutputFormat>();
        Assert.Equal(4, values.Length);
    }

    [Theory]
    [InlineData(OutputFormat.MP4, 0)]
    [InlineData(OutputFormat.AVI, 1)]
    [InlineData(OutputFormat.MKV, 2)]
    [InlineData(OutputFormat.GIF, 3)]
    public void OutputFormat_HasCorrectOrdinalValues(OutputFormat format, int expected)
    {
        Assert.Equal(expected, (int)format);
    }
}

public class ScreenInfoTests
{
    [Fact]
    public void DefaultName_IsEmptyString()
    {
        var info = new ScreenInfo();
        Assert.Equal("", info.Name);
    }

    [Fact]
    public void DefaultDimensions_AreZero()
    {
        var info = new ScreenInfo();
        Assert.Equal(0, info.Width);
        Assert.Equal(0, info.Height);
        Assert.Equal(0, info.Left);
        Assert.Equal(0, info.Top);
    }

    [Fact]
    public void CanSetAllProperties()
    {
        var info = new ScreenInfo
        {
            Index = 1,
            Name = "Main Monitor",
            Width = 1920,
            Height = 1080,
            Left = 0,
            Top = 0
        };

        Assert.Equal(1, info.Index);
        Assert.Equal("Main Monitor", info.Name);
        Assert.Equal(1920, info.Width);
        Assert.Equal(1080, info.Height);
    }

    [Fact]
    public void CanRepresentMultiMonitorSetup()
    {
        var secondary = new ScreenInfo
        {
            Index = 1,
            Name = "Secondary",
            Width = 2560,
            Height = 1440,
            Left = 1920,
            Top = 0
        };

        Assert.Equal(1920, secondary.Left);
        Assert.Equal(2560, secondary.Width);
    }
}
