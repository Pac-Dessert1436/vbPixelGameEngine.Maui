Imports Android.Media
Imports Android.App
Imports Android.Net

Namespace Platforms.Android
  ' All the code in this file is only included on Android.
  Public Class PlatformAudioAgent
    Implements IAudioAgent

    Private _mediaPlayer As MediaPlayer
    Private _isInitialized As Boolean = False
    Private _isLooping As Boolean
    Private _introDuration As Single
    Private _hasPlayedIntro As Boolean

    Public Sub PlayAudioFile(filename As String, shouldLoop As Boolean) Implements IAudioAgent.PlayAudioFile
      Try
        ' Clean up any existing player
        _mediaPlayer?.Release()

        ' Create a new media player
        _mediaPlayer = New MediaPlayer()
        _isLooping = shouldLoop

        ' Set up the data source
        If filename.StartsWith("/"c) Then
          ' Absolute file path
          _mediaPlayer.SetDataSource(filename)
        ElseIf filename.StartsWith("http") Then
          ' Remote URL
          _mediaPlayer.SetDataSource(Application.Context, Uri.Parse(filename))
        Else
          ' Asset file
          Dim afd = Application.Context.Assets.OpenFd(filename)
          _mediaPlayer.SetDataSource(afd.FileDescriptor, afd.StartOffset, afd.Length)
          afd.Close()
        End If
        
        ' Prepare and start playback
        _mediaPlayer.Prepare()
        _mediaPlayer.Looping = shouldLoop
        _mediaPlayer.Start()
        _isInitialized = True
        
        ' Set up completion handler for looping if needed
        AddHandler _mediaPlayer.Completion, AddressOf OnPlaybackCompleted
      Catch ex As Exception
        System.Diagnostics.Debug.WriteLine($"Android audio playback error: {ex.Message}")
      End Try
    End Sub

    Private Sub OnPlaybackCompleted(sender As Object, e As EventArgs)
      ' Handle completion event if needed
      If _isLooping AndAlso _mediaPlayer IsNot Nothing Then
        If _hasPlayedIntro Then _mediaPlayer.SeekTo(CInt(_introDuration * 1000))
        _mediaPlayer.Start()
      End If
    End Sub

    Public Sub SetAudioVolume(volume As Single) Implements IAudioAgent.SetAudioVolume
      Try
        If _mediaPlayer IsNot Nothing AndAlso _isInitialized Then
          ' Volume range is 0.0 to 1.0
          Dim clampedVolume = Math.Max(0.0F, Math.Min(1.0F, volume))
          _mediaPlayer.SetVolume(clampedVolume, clampedVolume)
        End If
      Catch ex As Exception
        System.Diagnostics.Debug.WriteLine($"Android audio volume error: {ex.Message}")
      End Try
    End Sub

    Public Sub StopAudioPlayback() Implements IAudioAgent.StopAudioPlayback
      Try
        If _mediaPlayer IsNot Nothing AndAlso _isInitialized Then
          _mediaPlayer.Stop()
          _isInitialized = False
        End If
      Catch ex As Exception
        System.Diagnostics.Debug.WriteLine($"Android audio stop error: {ex.Message}")
      End Try
    End Sub

    Public Sub PauseAudioPlayback() Implements IAudioAgent.PauseAudioPlayback
      Try
        If _mediaPlayer IsNot Nothing AndAlso _isInitialized Then
          _mediaPlayer.Pause()
        End If
      Catch ex As Exception
        System.Diagnostics.Debug.WriteLine($"Android audio pause error: {ex.Message}")
      End Try
    End Sub

    Public Sub ResumeAudioPlayback() Implements IAudioAgent.ResumeAudioPlayback
      Try
        If _mediaPlayer IsNot Nothing AndAlso _isInitialized Then
          _mediaPlayer.Start()
        End If
      Catch ex As Exception
        System.Diagnostics.Debug.WriteLine($"Android audio resume error: {ex.Message}")
      End Try
    End Sub

    Public Sub SyncStatus(isLooping As Boolean, introDuration As Single, hasPlayedIntro As Boolean) Implements IAudioAgent.SyncStatus
      _isLooping = isLooping
      _introDuration = introDuration
      _hasPlayedIntro = hasPlayedIntro
      
      ' Update the media player looping state if needed
      If _mediaPlayer IsNot Nothing AndAlso _isInitialized Then
        _mediaPlayer.Looping = isLooping
      End If
    End Sub
  End Class
End Namespace