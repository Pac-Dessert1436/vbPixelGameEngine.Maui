Imports Tizen.Multimedia

Namespace Platforms.Tizen
  ' All the code in this file is only included on Tizen.
  Public Class PlatformAudioAgent
    Implements IAudioAgent

    Private _player As Player
    Private _isInitialized As Boolean = False
    Private _isLooping As Boolean = False
    Private _introDuration As Single
    Private _hasPlayedIntro As Boolean

    Public Sub PlayAudioFile(filename As String, shouldLoop As Boolean) Implements IAudioAgent.PlayAudioFile
      Try
        ' Clean up any existing player
        If _player IsNot Nothing Then
          _player.Unprepare()
          _player.Dispose()
        End If

        ' Create a new player
        _player = New Player()
        _isLooping = shouldLoop
        
        ' Set up the data source
        If filename.StartsWith("/") Then
          ' Absolute file path
          _player.Source = New MediaSource(filename)
        ElseIf filename.StartsWith("http") Then
          ' Remote URL
          _player.Source = New MediaSource(New Uri(filename))
        Else
          ' Resource file
          _player.Source = New MediaSource(filename)
        End If
        
        ' Configure looping
        _player.SetLooping(shouldLoop)
        
        ' Start playback
        _player.PrepareAsync()
        _player.Start()
        _isInitialized = True
        
        ' Set up completion handler
        AddHandler _player.PlaybackCompleted, AddressOf OnPlaybackCompleted
      Catch ex As Exception
        System.Diagnostics.Debug.WriteLine($"Tizen audio playback error: {ex.Message}")
      End Try
    End Sub

    Private Sub OnPlaybackCompleted(sender As Object, e As PlaybackCompletedEventArgs)
      ' Handle completion event if needed
      If _isLooping AndAlso _player IsNot Nothing Then
        _player.Start()
      End If
    End Sub

    Public Sub SetAudioVolume(volume As Single) Implements IAudioAgent.SetAudioVolume
      Try
        If _player IsNot Nothing AndAlso _isInitialized Then
          ' Volume range is 0.0 to 1.0
          Dim clampedVolume = Math.Max(0.0F, Math.Min(1.0F, volume))
          _player.Volume = clampedVolume
        End If
      Catch ex As Exception
        System.Diagnostics.Debug.WriteLine($"Tizen audio volume error: {ex.Message}")
      End Try
    End Sub

    Public Sub StopAudioPlayback() Implements IAudioAgent.StopAudioPlayback
      Try
        If _player IsNot Nothing AndAlso _isInitialized Then
          _player.Stop()
          _isInitialized = False
        End If
      Catch ex As Exception
        System.Diagnostics.Debug.WriteLine($"Tizen audio stop error: {ex.Message}")
      End Try
    End Sub

    Public Sub PauseAudioPlayback() Implements IAudioAgent.PauseAudioPlayback
      Try
        If _player IsNot Nothing AndAlso _isInitialized Then
          _player.Pause()
        End If
      Catch ex As Exception
        System.Diagnostics.Debug.WriteLine($"Tizen audio pause error: {ex.Message}")
      End Try
    End Sub

    Public Sub ResumeAudioPlayback() Implements IAudioAgent.ResumeAudioPlayback
      Try
        If _player IsNot Nothing AndAlso _isInitialized Then
          _player.Start()
        End If
      Catch ex As Exception
        System.Diagnostics.Debug.WriteLine($"Tizen audio resume error: {ex.Message}")
      End Try
    End Sub

    Public Sub SyncStatus(isLooping As Boolean, introDuration As Single, hasPlayedIntro As Boolean) Implements IAudioAgent.SyncStatus
      _isLooping = isLooping
      _introDuration = introDuration
      _hasPlayedIntro = hasPlayedIntro
      
      ' Update the player looping state if needed
      If _player IsNot Nothing AndAlso _isInitialized Then
        _player.SetLooping(isLooping)
      End If
    End Sub
  End Class
End Namespace