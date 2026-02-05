Namespace PGEX.QuickGui
  Public Class ModalDialog
    Public Sub New()
      MyBase.New() 'True)

      ' Create File Open Dialog
      Dim vScreenSize = Pge.GetScreenSize()

      m_directoryListBox = New ListBox(m_fileSelectManager, m_directories, New Vf2d(20, 20), New Vf2d(300, 500))
      m_fileListBox = New ListBox(m_fileSelectManager, m_files, New Vf2d(330, 20), New Vf2d(300, 500))

      m_path = "/"
      For Each dir_entry In IO.Directory.EnumerateFileSystemEntries(m_path)
        If IO.Directory.Exists(dir_entry) Then
          m_directories.Add(IO.Path.GetFileName(dir_entry))
        Else
          m_files.Add(IO.Path.GetFileName(dir_entry))
        End If
      Next
    End Sub

    Public Sub ShowFileOpen(path As String)
      m_showDialog = True
    End Sub

    'Public Overrides Function OnBeforeUserUpdate(ByRef elapsedTime As Single) As Boolean

    '  If Not m_showDialog Then Return False

    '  m_fileSelectManager.Update(Pge)

    '  If Pge.GetKey(PixelGameEngine.Key.BACK).Pressed Then
    '    m_path = Directory.GetParent(m_path).FullName + "/"
    '    'm_directoryListBox.SelectionChanged = True
    '    'm_directoryListBox.SelectedItem = 0
    '  End If

    '  If m_directoryListBox.SelectionChanged Then
    '    Dim directory = m_directories(CInt(m_directoryListBox.SelectedItem))
    '    'If directory = ".." Then
    '    '    m_path = Directory.GetParent(m_path).FullName + "/"
    '    'Else
    '    '    m_path += directory + "/"
    '    'End If

    '    m_path += directory + "/"
    '    ' Reconstruct Lists
    '    m_directories.Clear()
    '    m_files.Clear()

    '    For Each dir_entry As String In m_directories.EnumerateFileSystemEntries(m_path)
    '      If IO.Directory.Exists(dir_entry) Then
    '        m_directories.Add(Path.GetFileName(dir_entry))
    '      Else
    '        m_files.Add(Path.GetFileName(dir_entry))
    '      End If
    '    Next

    '    'm_vDirectory.Add("..")
    '    'm_listFiles.nSelectedItem = 0
    '    'm_listDirectory.nSelectedItem = 0
    '  End If

    '  Pge.DrawStringDecal(New Vi2d(0, 0), m_path)

    '  m_fileSelectManager.DrawDecal(Pge)

    '  If Pge.GetKey(PixelGameEngine.Key.ESCAPE).Pressed Then
    '    m_showDialog = False
    '    Return False
    '  End If

    '  Return True

    'End Function

    Private m_showDialog As Boolean = False

    Private ReadOnly m_fileSelectManager As Manager
    Private ReadOnly m_volumesListBox As ListBox = Nothing
    Private ReadOnly m_directoryListBox As ListBox = Nothing
    Private ReadOnly m_fileListBox As ListBox = Nothing

    Private ReadOnly m_volumes As List(Of String)
    Private ReadOnly m_directories As List(Of String)
    Private ReadOnly m_files As List(Of String)
    Private ReadOnly m_path As String

  End Class
End Namespace