Option Strict On
Option Infer On

''' <summary>
''' Interface for audio playback agent. Users can implement this interface for
''' custom audio playback agent.
''' </summary>
Public Interface IAudioAgent
  Sub PlayAudioFile(filename As String, shouldLoop As Boolean)
  Sub SetAudioVolume(volume As Single)
  Sub StopAudioPlayback()
  Sub PauseAudioPlayback()
  Sub ResumeAudioPlayback()
  Sub SyncStatus(isLooping As Boolean, introDuration As Single, hasPlayedIntro As Boolean)
End Interface

''' <summary>
''' Audio player that uses platform-specific audio agents to play audio files.
''' </summary>
Public Class AudioPlayer
  Implements IDisposable

  Private ReadOnly _audioAgent As IAudioAgent
  Private ReadOnly _audioSource As String
  Private isLooping As Boolean = False
  Private ReadOnly introDuration As Single
  Private hasPlayedIntro As Boolean = False
  Private _isPlaying As Boolean = False
  Private _isDisposed As Boolean

  Public Sub New(filename As String, Optional introDuration As Single = 0,
                 Optional audioAgent As IAudioAgent = Nothing)
    Me.introDuration = introDuration
    _audioSource = filename

    ' Initialize the user-defined audio agent using a real dependency injection.
    ' Fall back to dependency lookup for platform-specific audio agent when the custom
    ' audio agent is not provided.
    _audioAgent = If(audioAgent, PlatformAudioAgent)
  End Sub

  ' Factory method to get the platform-specific audio agent (more like a dependency lookup
  ' than a dependency injection)
  Private Shared ReadOnly Property PlatformAudioAgent As IAudioAgent
    Get
#If WINDOWS10_0_19041_0_OR_GREATER Then
      Return New Platforms.Windows.PlatformAudioAgent
#ElseIf ANDROID29_0_OR_GREATER Then
      Return New Platforms.Android.PlatformAudioAgent
#ElseIf IOS14_0_OR_GREATER Then
      Return New Platforms.iOS.PlatformAudioAgent
#ElseIf MACCATALYST14_0_OR_GREATER Then
      Return New Platforms.MacCatalyst.PlatformAudioAgent
#ElseIf TIZEN6_5_OR_GREATER Then
      Return New Platforms.Tizen.PlatformAudioAgent
#Else
      Throw New NotSupportedException("Audio playback is not supported on the current platform")
#End If
    End Get
  End Property

  Public Sub PlayOnce()
    If Not String.IsNullOrEmpty(_audioSource) Then
      isLooping = False
      hasPlayedIntro = False
      _isPlaying = True

      ' Use the platform-specific implementation
      _audioAgent.PlayAudioFile(_audioSource, False)
      _audioAgent.SyncStatus(isLooping, introDuration, hasPlayedIntro)
    End If
  End Sub

  Public Sub PlayLooping()
    If Not String.IsNullOrEmpty(_audioSource) Then
      isLooping = True
      hasPlayedIntro = False
      _isPlaying = True

      ' Use the platform-specific implementation
      _audioAgent.PlayAudioFile(_audioSource, True)
      _audioAgent.SyncStatus(isLooping, introDuration, hasPlayedIntro)
    End If
  End Sub

  Public Sub [Stop]()
    _isPlaying = False
    isLooping = False
    hasPlayedIntro = False

    ' Use the platform-specific implementation
    _audioAgent.StopAudioPlayback()
    _audioAgent.SyncStatus(isLooping, introDuration, hasPlayedIntro)
  End Sub

  Public Sub Pause()
    ' Use the platform-specific implementation
    If _isPlaying Then _audioAgent.PauseAudioPlayback()
  End Sub

  Public Sub ResumePlayback()
    If Not _isPlaying Then
      _isPlaying = True
      ' Use the platform-specific implementation
      _audioAgent.ResumeAudioPlayback()
    End If
  End Sub

  Public Sub SetVolume(volume As Single)
    ' Volume range is 0.0 to 1.0
    Dim clampedVolume = Math.Max(0.0F, Math.Min(1.0F, volume))

    ' Use the platform-specific implementation
    _audioAgent.SetAudioVolume(clampedVolume)
  End Sub

  Protected Overridable Sub Dispose(disposing As Boolean)
    If Not _isDisposed Then
      If disposing Then [Stop]()
      _isDisposed = True
    End If
  End Sub

  Public Sub Dispose() Implements IDisposable.Dispose
    Dispose(disposing:=True)
    GC.SuppressFinalize(Me)
  End Sub
End Class
