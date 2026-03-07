Imports Windows.Media

Namespace Platforms.Windows
  ' All the code in this file is only included on Windows.
  Public Class PlatformAudioAgent
    Implements IAudioAgent

    Private _mediaPlayer As Playback.MediaPlayer
    Private _isInitialized As Boolean = False
    Private _isLooping As Boolean
    Private _introDuration As Single
    Private _hasPlayedIntro As Boolean

    Public Sub PlayAudioFile(filename As String, shouldLoop As Boolean) Implements IAudioAgent.PlayAudioFile
      Try
        ' Clean up any existing player
        _mediaPlayer?.Dispose()

        ' Create a new media player
        _mediaPlayer = New Playback.MediaPlayer()
        
        ' Set up the media source
        Dim source = Core.MediaSource.CreateFromUri(New Uri(filename, UriKind.RelativeOrAbsolute))
        _mediaPlayer.Source = source
        
        ' Configure looping
        _mediaPlayer.IsLoopingEnabled = shouldLoop
        _isLooping = shouldLoop
        
        ' Start playback
        _mediaPlayer.Play()
        _isInitialized = True
      Catch ex As Exception
        System.Diagnostics.Debug.WriteLine($"Windows audio playback error: {ex.Message}")
      End Try
    End Sub

    Public Sub SetAudioVolume(volume As Single) Implements IAudioAgent.SetAudioVolume
      Try
        If _mediaPlayer IsNot Nothing AndAlso _isInitialized Then
          ' Volume range is 0.0 to 1.0
          _mediaPlayer.Volume = Math.Max(0.0, Math.Min(1.0, volume))
        End If
      Catch ex As Exception
        System.Diagnostics.Debug.WriteLine($"Windows audio volume error: {ex.Message}")
      End Try
    End Sub

    Private Sub OnPlaybackCompleted(sender As Object, e As EventArgs)
      ' Handle completion event if needed
      If _isLooping AndAlso _mediaPlayer IsNot Nothing Then
        If _hasPlayedIntro Then _mediaPlayer.Position = TimeSpan.FromSeconds(_introDuration)
        _mediaPlayer.Play()
      End If
    End Sub

    Public Sub StopAudioPlayback() Implements IAudioAgent.StopAudioPlayback
      Try
        If _mediaPlayer IsNot Nothing AndAlso _isInitialized Then
          _mediaPlayer.Pause()
          _mediaPlayer.Source = Nothing
          _isInitialized = False
        End If
      Catch ex As Exception
        System.Diagnostics.Debug.WriteLine($"Windows audio stop error: {ex.Message}")
      End Try
    End Sub

    Public Sub PauseAudioPlayback() Implements IAudioAgent.PauseAudioPlayback
      Try
        If _mediaPlayer IsNot Nothing AndAlso _isInitialized Then
          _mediaPlayer.Pause()
        End If
      Catch ex As Exception
        System.Diagnostics.Debug.WriteLine($"Windows audio pause error: {ex.Message}")
      End Try
    End Sub

    Public Sub ResumeAudioPlayback() Implements IAudioAgent.ResumeAudioPlayback
      Try
        If _mediaPlayer IsNot Nothing AndAlso _isInitialized Then
          _mediaPlayer.Play()
        End If
      Catch ex As Exception
        Debug.WriteLine($"Windows audio resume error: {ex.Message}")
      End Try
    End Sub

    Public Sub SyncStatus(isLooping As Boolean, introDuration As Single, hasPlayedIntro As Boolean) Implements IAudioAgent.SyncStatus
      _isLooping = isLooping
      _introDuration = introDuration
      _hasPlayedIntro = hasPlayedIntro
      
      ' Update the media player looping state if needed
      If _mediaPlayer IsNot Nothing AndAlso _isInitialized Then
        _mediaPlayer.IsLoopingEnabled = isLooping
      End If
    End Sub
  End Class
End Namespace