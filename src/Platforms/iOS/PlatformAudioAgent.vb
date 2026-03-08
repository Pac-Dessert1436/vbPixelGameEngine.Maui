Imports Foundation
Imports AVFoundation

Namespace Platforms.iOS
  ' All the code in this file is only included on iOS.
  Public Class PlatformAudioAgent
    Implements IAudioAgent

    Private _audioPlayer As AVAudioPlayer
    Private _isInitialized As Boolean = False
    Private _isLooping As Boolean
    Private _introDuration As Single
    Private _hasPlayedIntro As Boolean

    Public Sub PlayAudioFile(filename As String, shouldLoop As Boolean) Implements IAudioAgent.PlayAudioFile
      Try
        ' Clean up any existing player
        If _audioPlayer IsNot Nothing Then
          _audioPlayer.Stop()
          _audioPlayer.Dispose()
        End If

        ' Get the audio file URL
        Dim url As NSUrl
        If filename.StartsWith("/") Then
          ' Absolute file path
          url = NSUrl.FromFilename(filename)
        ElseIf filename.StartsWith("http") Then
          ' Remote URL
          url = NSUrl.FromString(filename)
        Else
          ' Bundle resource
          url = NSBundle.MainBundle.GetUrlForResource(filename, Nothing)
        End If

        ' Create a new audio player
        _audioPlayer = AVAudioPlayer.FromUrl(url)
        _isLooping = shouldLoop
        
        If _audioPlayer IsNot Nothing Then
          _audioPlayer.NumberOfLoops = CType(If(shouldLoop, -1, 0), IntPtr) ' -1 for infinite loop
          _audioPlayer.Volume = 1.0F
          _audioPlayer.Play()
          _isInitialized = True
          
          ' Set up completion handler
          AddHandler _audioPlayer.FinishedPlaying, AddressOf OnFinishedPlaying
        End If
      Catch ex As Exception
        System.Diagnostics.Debug.WriteLine($"iOS audio playback error: {ex.Message}")
      End Try
    End Sub

    Private Sub OnFinishedPlaying(sender As Object, e As AVStatusEventArgs)
      ' Handle completion event if needed
      If _isLooping AndAlso _audioPlayer IsNot Nothing Then
        If _hasPlayedIntro Then _audioPlayer.CurrentTime = _introDuration
        _audioPlayer.Play()
      End If
    End Sub

    Public Sub SetAudioVolume(volume As Single) Implements IAudioAgent.SetAudioVolume
      Try
        If _audioPlayer IsNot Nothing AndAlso _isInitialized Then
          ' Volume range is 0.0 to 1.0
          _audioPlayer.Volume = Math.Max(0.0F, Math.Min(1.0F, volume))
        End If
      Catch ex As Exception
        System.Diagnostics.Debug.WriteLine($"iOS audio volume error: {ex.Message}")
      End Try
    End Sub

    Public Sub StopAudioPlayback() Implements IAudioAgent.StopAudioPlayback
      Try
        If _audioPlayer IsNot Nothing AndAlso _isInitialized Then
          _audioPlayer.Stop()
          _audioPlayer.CurrentTime = 0.0
          _isInitialized = False
        End If
      Catch ex As Exception
        System.Diagnostics.Debug.WriteLine($"iOS audio stop error: {ex.Message}")
      End Try
    End Sub

    Public Sub PauseAudioPlayback() Implements IAudioAgent.PauseAudioPlayback
      Try
        If _audioPlayer IsNot Nothing AndAlso _isInitialized Then
          _audioPlayer.Pause()
        End If
      Catch ex As Exception
        System.Diagnostics.Debug.WriteLine($"iOS audio pause error: {ex.Message}")
      End Try
    End Sub

    Public Sub ResumeAudioPlayback() Implements IAudioAgent.ResumeAudioPlayback
      Try
        If _audioPlayer IsNot Nothing AndAlso _isInitialized Then
          _audioPlayer.Play()
        End If
      Catch ex As Exception
        System.Diagnostics.Debug.WriteLine($"iOS audio resume error: {ex.Message}")
      End Try
    End Sub

    Public Sub SyncStatus(isLooping As Boolean, introDuration As Single, hasPlayedIntro As Boolean) Implements IAudioAgent.SyncStatus
      _isLooping = isLooping
      _introDuration = introDuration
      _hasPlayedIntro = hasPlayedIntro
      
      ' Update the audio player looping state if needed
      If _audioPlayer IsNot Nothing AndAlso _isInitialized Then
        _audioPlayer.NumberOfLoops = CType(If(isLooping, -1, 0), IntPtr) ' -1 for infinite loop
      End If
    End Sub
  End Class
End Namespace