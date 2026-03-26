Public Module RCodeExtensions

  <Runtime.CompilerServices.Extension>
  Public Function IsRCodeOk(RCode As PixelGameEngine.RCode) As Boolean
    Return RCode = PixelGameEngine.RCode.Ok
  End Function

  <Runtime.CompilerServices.Extension>
  Public Function IsRCodeFail(RCode As PixelGameEngine.RCode) As Boolean
    Return RCode = PixelGameEngine.RCode.Fail
  End Function

  <Runtime.CompilerServices.Extension>
  Public Function IsRCodeNoFile(RCode As PixelGameEngine.RCode) As Boolean
    Return RCode = PixelGameEngine.RCode.NoFile
  End Function

End Module

<Serializable>
Public NotInheritable Class RCodeException
  Inherits Exception

  Public Sub New(objStr As String, RCode As PixelGameEngine.RCode)
    MyBase.New($"RCodeException raised by '{objStr}' with RCode: {RCode}")
  End Sub
End Class

''' <summary>
''' A generic object that represents a result, either a success or a failure.
''' </summary>
''' <typeparam name="TOk">The type of the success value.</typeparam>
''' <typeparam name="TErr">The type of the error value.</typeparam>
Public NotInheritable Class RObject(Of TOk, TErr)

  Public ReadOnly Property OkValue As TOk
  Public ReadOnly Property ErrValue As TErr
  Public ReadOnly Property RCode As PixelGameEngine.RCode

  Public Sub New(okValue As TOk)
    Me.OkValue = okValue
    RCode = PixelGameEngine.RCode.Ok
  End Sub

  Public Sub New(errValue As TErr, Optional isNoFile As Boolean = False)
    Me.ErrValue = errValue
    RCode = If(isNoFile, PixelGameEngine.RCode.NoFile, PixelGameEngine.RCode.Fail)
  End Sub

  Public Function Unwrap() As TOk
    If RCode.IsRCodeOk() Then Return OkValue
    Throw New RCodeException(ErrValue.ToString(), RCode)
  End Function

  Public ReadOnly Property IsOk As Boolean
    Get
      Return RCode.IsRCodeOk()
    End Get
  End Property

  Public ReadOnly Property IsFail As Boolean
    Get
      Return RCode.IsRCodeFail() OrElse RCode.IsRCodeNoFile()
    End Get
  End Property

  Public Function MapOkValue(Of T)(mapper As Func(Of TOk, T)) As RObject(Of T, TErr)
    If IsOk Then Return New RObject(Of T, TErr)(mapper(OkValue))
    Return New RObject(Of T, TErr)(ErrValue, RCode.IsRCodeNoFile())
  End Function

  Public Function MapErrValue(Of T)(mapper As Func(Of TErr, T)) As RObject(Of TOk, T)
    If IsOk Then Return New RObject(Of TOk, T)(OkValue)
    Return New RObject(Of TOk, T)(mapper(ErrValue), RCode.IsRCodeNoFile())
  End Function

End Class