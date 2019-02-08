Imports System.Threading
Public Class frmMain
    'hola marco flores
    Structure posicion
        Dim x As Integer
        Dim y As Integer
    End Structure
    Structure movimiento
        Dim posicionOrigen As posicion
        Dim posicionDestino As posicion
    End Structure
    Dim _Profundidad As Integer
    Dim _puntaje As Integer = 100
    Public movimientos() As movimiento
    Public listaMovimientos As New List(Of movimiento)
    Dim _colores As Integer = 5 ' entre 5 y 7
    Dim mySleep As Integer = 25
    Public superficieDibujo As Bitmap
    Public disp As Graphics
    Public bmp As Bitmap
    Dim _filas As Integer = 25
    Dim _columnas As Integer = 4

    Dim _filasV As Integer = 7
    Dim _columnasV As Integer = _columnas

    Dim ruta1(10000) As movimiento
    Dim _movimientosCorrectos As Long
    Dim _pasos As Long
    Dim _puntos As Long
    Dim _p1 As posicion
    Dim _p2 As posicion
    Dim _p3 As posicion

    Dim Lastx As Integer = 34
    Dim Lasty As Integer = 4
    
    'Public tablero(_columnas, _filas) As tablero.figura
    Public tablero(_columnas, _filas) As Integer
    Public tableroWork(_columnas, _filas) As Integer
    Public tableroBack(_columnas, _filas) As Integer
    Public tableroPruebas(_columnas, _filas) As Integer
    Private Sub tbToT()
        Dim f As Integer
        Dim c As Integer
        For f = 0 To _filas
            For c = 0 To _columnas
                tablero(c, f) = tableroBack(c, f)
            Next
        Next
    End Sub
    Private Sub tToTB()
        Dim f As Integer
        Dim c As Integer
        For f = 0 To _filas
            For c = 0 To _columnas
                tableroBack(c, f) = tablero(c, f)
            Next
        Next
    End Sub
    Private Sub tbToTw()
        Dim f As Integer
        Dim c As Integer
        For f = 0 To _filas
            For c = 0 To _columnas
                tableroWork(c, f) = tableroBack(c, f)
            Next
        Next
    End Sub
    Private Sub tToTw()
        Dim f As Integer
        Dim c As Integer
        For f = 0 To _filas
            For c = 0 To _columnas
                tableroWork(c, f) = tablero(c, f)
            Next
        Next
    End Sub
    Private Sub twToTp()
        Dim f As Integer
        Dim c As Integer
        For f = 0 To _filas
            For c = 0 To _columnas
                tableroPruebas(c, f) = tableroWork(c, f)
            Next
        Next
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If _filasV > _filas Then
            _filasV = _filas
        End If
        Label1.Text = ""
        Label2.Text = ""
        superficieDibujo = New Bitmap(Me.Size.Width, Me.Size.Height)
        PictureBox1.Image = superficieDibujo
        disp = Graphics.FromImage(superficieDibujo)
        ' MsgBox(app_path)

        Try
            bmp = New Bitmap(app_path() & "/1.png")
        Catch ex As Exception
            MsgBox("Error al cargar el archivo: " + ex.Message)
        End Try
        'bmp.RotateFlip(RotateFlipType.Rotate90FlipNone)
        disp.DrawImage(bmp, 25, 25, 36, 36)

    End Sub

    'Misfunciones Especiales
    Private Function app_path() As String
        ' Return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)
        Return System.AppDomain.CurrentDomain.BaseDirectory()
    End Function
    Private Function generaAleatoreo(min As Integer, max As Integer)
        Randomize()  ' Next Generate random value between min and max.
        Dim value As Integer = CInt(Int((max * Rnd()) + min))
        Return value
    End Function
    Private Sub llenaTablero(ByRef t As Integer(,)) 'As Integer(,)
        Dim pathApp As String
        pathApp = app_path()
        Dim x As Integer
        Dim y As Integer
        Dim figura As Integer
        For x = 0 To _columnas
            For y = 0 To _filas
                If y = 0 Then
                    figura = 1
                Else
                    figura = generaAleatoreo(1, _colores)
                    'MsgBox("Genero : " + figura.ToString)
                End If
                t(x, y) = figura
                'tablero(x, y).filename = pathApp + "/" + figura.ToString + ".png"
            Next
        Next
    End Sub
    Public Function recuperaImagen(cual As Integer) As Bitmap
        bmp = Nothing
        Try
            bmp = New Bitmap(app_path() & "/" + cual.ToString + ".png")
            Return bmp
        Catch ex As Exception
            MsgBox("Error al cargar el archivo: " + ex.Message)
            Return bmp
        End Try
    End Function

    Private Sub swapArreglo(ByRef t As Integer(,), p1 As posicion, p2 As posicion)
        Dim temp As Integer
        temp = t(p1.x, p1.y)
        t(p1.x, p1.y) = t(p2.x, p2.y)
        t(p2.x, p2.y) = temp
    End Sub
    Public Sub bajaColumnas(ByRef tt As Integer(,), c As Integer)
        Dim p1 As posicion
        Dim p2 As posicion
        If c > _columnas Then
            Exit Sub
        End If
        Dim y As Integer
        y = 0
        Dim hayGemas As Boolean = True
        p1.x = c
        p2.x = c
        While hayGemas
            If (tablero(c, y) = 0 And tablero(c, y + 1) > 0) Then
                't(c, y) = t(c, y + 1)
                p1.y = y
                p2.y = y + 1

                swapArreglo(tablero, p1, p2)
            End If

            y = y + 1
            If y >= _filas Then   ' And Not hayCeros(t, c) Then
                hayGemas = False
            End If
        End While
    End Sub
    Private Function hayCeros(ByVal t As Integer(,), c As Integer) As Boolean
        Dim i As Integer

        Dim v As Boolean = False
        Dim r As Boolean = False
        For i = 1 To _filas
            If t(c, i) = 0 Then
                v = True

            End If
            If t(c, i) > 1 And v = True Then
                r = True

            End If
        Next
        If v And r Then
            'hay ceros abajo
            Return True
        Else
            Return False
        End If
    End Function
    Public Sub borraLineaTablero3(ByRef t As Integer(,), p1 As posicion, p2 As posicion, p3 As posicion)
        t(p1.x, p1.y) = 0
        t(p2.x, p2.y) = 0
        t(p3.x, p3.y) = 0

    End Sub
    Public Sub tachaLineaTablero3(ByRef t As Integer(,), p1 As posicion, p2 As posicion, p3 As posicion)
        t(p1.x, p1.y) = t(p1.x, p1.y) + 10
        t(p2.x, p2.y) = t(p2.x, p2.y) + 10
        t(p3.x, p3.y) = t(p3.x, p3.y) + 10

    End Sub
    Private Function existeLineaH(ByRef t As Integer(,)) As posicion
        Dim po As posicion
        Dim x As Integer
        Dim y As Integer
        po.x = -1
        po.y = -1
        For x = 1 To _columnasV - 1
            For y = 0 To _filasV
                If (t(x - 1, y) = t(x, y)) And (t(x + 1, y) = t(x, y)) And t(x, y) > 0 Then
                    po.x = x
                    po.y = y
                    'MsgBox("Existe linea H en X:" + x.ToString + " y:" + y.ToString)
                    _p1.x = x - 1
                    _p1.y = y
                    _p2.x = x
                    _p2.y = y
                    _p3.x = x + 1
                    _p3.y = y
                    Return po
                End If
            Next
        Next
        Return po
    End Function
    Private Function existeLineaV(ByRef t As Integer(,)) As posicion
        Dim po As posicion
        Dim x As Integer
        Dim y As Integer
        po.x = -1
        po.y = -1
        For x = 0 To _columnasV
            For y = 1 To _filasV - 1
                If (t(x, y - 1) = t(x, y)) And (t(x, y) = t(x, y + 1)) And t(x, y) > 0 Then
                    po.x = x
                    po.y = y
                    ' MsgBox("Existe linea V en X:" + x.ToString + " y:" + y.ToString)
                    _p1.x = x
                    _p1.y = y - 1
                    _p2.x = x
                    _p2.y = y
                    _p3.x = x
                    _p3.y = y + 1
                    Return po
                End If
            Next
        Next
        Return po
    End Function
    Private Function buscaSiExisteLinea(ByRef t As Integer(,)) As Boolean
        If existeLineaH(t).x >= 0 Or existeLineaV(t).x >= 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Private Sub QuitaSeguidasV(ByRef t As Integer(,))
        Dim y As Integer
        Dim x As Integer
        For x = 0 To _columnas
            For y = 1 To _filas - 1
                If (t(x, y - 1) = t(x, y)) And (t(x, y + 1) = t(x, y)) And t(x, y) > 0 Then
                    mete5(t, x, y)
                End If
            Next
        Next
        If existeLineaV(t).x > 0 Then
            QuitaSeguidasV(t)
        End If
    End Sub
    Private Sub QuitaSeguidasH(ByRef t As Integer(,))
        Dim v As Boolean = False
        Dim x As Integer
        Dim y As Integer
        For x = 1 To _columnas - 1
            For y = 0 To _filas
                If (t(x - 1, y) = t(x, y)) And (t(x + 1, y) = t(x, y)) And t(x, y) > 0 Then
                    mete5(t, x, y)
                End If
            Next
        Next
        If existeLineaH(t).x > 0 Then
            QuitaSeguidasH(t)
        End If
    End Sub
    Public Sub mete5(ByRef t As Integer(,), x As Integer, y As Integer)
        Dim c1 As Integer = 1
        Dim c2 As Integer = 2
        Dim c3 As Integer = 1
        Dim c4 As Integer = 2
        Dim c5 As Integer = 1
        If _colores <= 3 Then
            c1 = 2
            c2 = 3
            c3 = 1
            c4 = 2
        End If
        If _colores = 4 Then
            c1 = 2
            c2 = 3
            c3 = 4
            c4 = 1
        End If
        If _colores >= 5 Then
            c1 = 2
            c2 = 6
            c3 = 3
            c4 = 4
            c5 = 5
        End If
        t(x, y) = 1
        If x < _columnas Then t(x + 1, y) = c2
        If x > 1 Then t(x - 1, y) = c3
        If y < _filas Then t(x, y + 1) = c4
        If y > 1 Then t(x, y - 1) = c5
    End Sub

    Public Sub dibujaTablero(ByRef t As Integer(,))

        superficieDibujo = New Bitmap(Me.Size.Width, Me.Size.Height)
        PictureBox1.Image = superficieDibujo
        disp = Graphics.FromImage(superficieDibujo)
        Dim x As Integer
        Dim y As Integer
        Dim ScreenX As Integer
        Dim ScreenY As Integer
        Dim resta As Integer = 135
        ScreenX = 0
        ScreenY = Me.Height - resta
        For x = 0 To _columnasV
            For y = 0 To _filasV
                bmp = recuperaImagen(t(x, y))
                disp.DrawImage(bmp, ScreenX, ScreenY, 36, 36)
                ScreenY = ScreenY - 36
            Next
            ScreenY = Me.Height - resta
            ScreenX = ScreenX + 36
        Next
    End Sub


    Private Sub nuevoJuego()
        Dim siHayLinea As Boolean = False
        Randomize()
        llenaTablero(tableroBack)
        tbToT()

        QuitaSeguidasH(tablero)
        QuitaSeguidasV(tablero)
        Do While siHayLinea
            If buscaSiExisteLinea(tablero) Then
                QuitaSeguidasH(tablero)
                QuitaSeguidasV(tablero)
            Else
                siHayLinea = False
            End If
        Loop
        _pasos = 0
        _puntos = 0
        _movimientosCorrectos = 0
        tToTB()
        dibujaTablero(tablero)
    End Sub
    Private Sub NuevoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NuevoToolStripMenuItem.Click
        nuevoJuego()
    End Sub

    Private Sub swapV(ByRef t As Integer(,), p1 As posicion, p2 As posicion)
        Dim pu As posicion
        Dim pd As posicion
        If p1.y > p2.y Then
            pu.x = p1.x
            pu.y = p1.y
            pd.x = p2.x
            pd.y = p2.y
        Else
            pu.x = p2.x
            pu.y = p2.y
            pd.x = p1.x
            pd.y = p1.y
        End If
        If chkAnimacion.Checked = True Then
            dibujaTableroSwapV(t, pd, pu, 1)
            Application.DoEvents()
            Threading.Thread.Sleep(mySleep)
            dibujaTableroSwapV(t, pd, pu, 2)
            Application.DoEvents()
            Threading.Thread.Sleep(mySleep)
            dibujaTableroSwapV(t, pd, pu, 3)
            Application.DoEvents()
            Threading.Thread.Sleep(mySleep)
            dibujaTableroSwapV(t, pd, pu, 4)
            Application.DoEvents()
        End If
        swapArreglo(t, pd, pu)
    End Sub
    Private Sub swapH(ByRef t As Integer(,), p1 As posicion, p2 As posicion)
        Dim pl As posicion
        Dim pr As posicion
        If p1.x > p2.x Then
            pl.x = p2.x
            pl.y = p2.y
            pr.x = p1.x
            pr.y = p1.y
        Else
            pl.x = p1.x
            pl.y = p1.y
            pr.x = p2.x
            pr.y = p2.y
        End If

        If chkAnimacion.Checked = True Then

            dibujaTableroSwapH(t, pl, pr, 1)
            Application.DoEvents()
            Threading.Thread.Sleep(mySleep)
            dibujaTableroSwapH(t, pl, pr, 2)
            Application.DoEvents()
            Threading.Thread.Sleep(mySleep)
            dibujaTableroSwapH(t, pl, pr, 3)
            Application.DoEvents()
            Threading.Thread.Sleep(mySleep)
            dibujaTableroSwapH(t, pl, pr, 4)
            Application.DoEvents()
        End If
        swapArreglo(t, pl, pr)
    End Sub
    Public Sub swap(ByRef t As Integer(,), p1 As posicion, p2 As posicion)

        If p1.x = p2.x Then
            swapV(t, p1, p2)
            Exit Sub
        End If
        If p1.y = p2.y Then
            swapH(t, p1, p2)
            Exit Sub
        End If
    End Sub
    Public Sub dibujaTableroSwapV(ByRef t As Integer(,), pa1 As posicion, pa2 As posicion, cambio As Integer)
        Dim pp1 As posicion
        Dim pp2 As posicion
        superficieDibujo = New Bitmap(Me.Size.Width, Me.Size.Height)
        PictureBox1.Image = superficieDibujo
        disp = Graphics.FromImage(superficieDibujo)
        Dim x As Integer
        Dim y As Integer
        Dim ScreenX As Integer
        Dim ScreenY As Integer
        Dim resta As Integer = 135
        ScreenX = 0
        ScreenY = Me.Height - resta
        For x = 0 To _columnasV
            For y = 0 To _filasV
                If (pa1.x = x And pa1.y = y) Or (pa2.x = x And pa2.y = y) Then
                    If pa1.x = x And pa1.y = y Then
                        pp1.x = ScreenX
                        pp1.y = ScreenY
                    End If
                    If pa2.x = x And pa2.y = y Then
                        pp2.x = ScreenX
                        pp2.y = ScreenY
                    End If
                    bmp = recuperaImagen(0)
                Else
                    bmp = recuperaImagen(t(x, y))
                End If
                disp.DrawImage(bmp, ScreenX, ScreenY, 36, 36)
                ScreenY = ScreenY - 36
            Next
            ScreenY = Me.Height - resta
            ScreenX = ScreenX + 36
        Next

        If cambio = 1 Then
            bmp = recuperaImagen(t(pa1.x, pa1.y))
            disp.DrawImage(bmp, pp1.x + 5, pp1.y - 9, 36, 36)
            bmp = recuperaImagen(t(pa2.x, pa2.y))
            disp.DrawImage(bmp, pp2.x - 5, pp2.y + 9, 36, 36)
        End If
        If cambio = 2 Then
            bmp = recuperaImagen(t(pa1.x, pa1.y))
            disp.DrawImage(bmp, pp1.x + 10, pp1.y - 18, 36, 36)
            bmp = recuperaImagen(t(pa2.x, pa2.y))
            disp.DrawImage(bmp, pp2.x - 10, pp2.y + 18, 36, 36)
        End If
        If cambio = 3 Then
            bmp = recuperaImagen(t(pa1.x, pa1.y))
            disp.DrawImage(bmp, pp1.x + 5, pp1.y - 27, 36, 36)
            bmp = recuperaImagen(t(pa2.x, pa2.y))
            disp.DrawImage(bmp, pp2.x - 5, pp2.y + 27, 36, 36)
        End If
        If cambio = 4 Then
            bmp = recuperaImagen(t(pa1.x, pa1.y))
            disp.DrawImage(bmp, pp1.x, pp1.y - 36, 36, 36)
            bmp = recuperaImagen(t(pa2.x, pa2.y))
            disp.DrawImage(bmp, pp2.x, pp2.y + 36, 36, 36)
        End If
    End Sub
    Public Sub dibujaTableroSwapH(ByRef t As Integer(,), pa1 As posicion, pa2 As posicion, cambio As Integer)
        Dim pp1 As posicion
        Dim pp2 As posicion
        superficieDibujo = New Bitmap(Me.Size.Width, Me.Size.Height)
        PictureBox1.Image = superficieDibujo
        disp = Graphics.FromImage(superficieDibujo)
        Dim x As Integer
        Dim y As Integer
        Dim ScreenX As Integer
        Dim ScreenY As Integer
        Dim resta As Integer = 135
        ScreenX = 0
        ScreenY = Me.Height - resta
        For x = 0 To _columnasV
            For y = 0 To _filasV
                If (pa1.x = x And pa1.y = y) Or (pa2.x = x And pa2.y = y) Then
                    If pa1.x = x And pa1.y = y Then
                        pp1.x = ScreenX
                        pp1.y = ScreenY
                    End If
                    If pa2.x = x And pa2.y = y Then
                        pp2.x = ScreenX
                        pp2.y = ScreenY
                    End If
                    bmp = recuperaImagen(0)
                Else
                    bmp = recuperaImagen(t(x, y))
                End If
                disp.DrawImage(bmp, ScreenX, ScreenY, 36, 36)
                ScreenY = ScreenY - 36
            Next
            ScreenY = Me.Height - resta
            ScreenX = ScreenX + 36
        Next

        If cambio = 1 Then
            bmp = recuperaImagen(t(pa1.x, pa1.y))
            disp.DrawImage(bmp, pp1.x + 9, pp1.y + 5, 36, 36)
            bmp = recuperaImagen(t(pa2.x, pa2.y))
            disp.DrawImage(bmp, pp2.x - 9, pp2.y - 5, 36, 36)
        End If
        If cambio = 2 Then
            bmp = recuperaImagen(t(pa1.x, pa1.y))
            disp.DrawImage(bmp, pp1.x + 18, pp1.y + 10, 36, 36)
            bmp = recuperaImagen(t(pa2.x, pa2.y))
            disp.DrawImage(bmp, pp2.x - 18, pp2.y - 10, 36, 36)
        End If
        If cambio = 3 Then
            bmp = recuperaImagen(t(pa1.x, pa1.y))
            disp.DrawImage(bmp, pp1.x + 27, pp1.y + 5, 36, 36)
            bmp = recuperaImagen(t(pa2.x, pa2.y))
            disp.DrawImage(bmp, pp2.x - 27, pp2.y - 5, 36, 36)
        End If
        If cambio = 4 Then
            bmp = recuperaImagen(t(pa1.x, pa1.y))
            disp.DrawImage(bmp, pp1.x + 36, pp1.y, 36, 36)
            bmp = recuperaImagen(t(pa2.x, pa2.y))
            disp.DrawImage(bmp, pp2.x - 36, pp2.y, 36, 36)
        End If
    End Sub


    Private Function GeneraPosicionPar(P As posicion) As posicion
        Dim pos As posicion
        Dim ram As Integer = generaAleatoreo(1, 4)
        If ram <= 2 Then
            If P.x + 1 < _columnasV Then
                pos.x = P.x + 1
                pos.y = P.y
                Return pos
            End If
            If P.y + 1 < _filasV Then
                pos.x = P.x
                pos.y = P.y + 1
                Return pos
            End If
            If P.x - 1 > 0 Then
                pos.x = P.x - 1
                pos.y = P.y
                Return pos
            End If
            If P.y - 1 > 0 Then
                pos.x = P.x
                pos.y = P.y - 1
                Return pos
            End If
        Else
            If P.y - 1 > 0 Then
                pos.x = P.x
                pos.y = P.y - 1
                Return pos
            End If
            If P.x + 1 < _columnasV Then
                pos.x = P.x + 1
                pos.y = P.y
                Return pos
            End If

            If P.x - 1 > 0 Then
                pos.x = P.x - 1
                pos.y = P.y
                Return pos
            End If
            If P.y + 1 < _filasV Then
                pos.x = P.x
                pos.y = P.y + 1
                Return pos
            End If
        End If
    End Function
    Private Function GeneraPosicionRNd() As posicion
        Dim pos As posicion
        Randomize()  ' Next Generate random value between min and max.
        Dim xc As Integer = CInt(Int((_columnasV * Rnd()) + 0))
        Randomize()
        Dim yf As Integer = CInt(Int((_filasV * Rnd()) + 0))
        pos.x = xc
        pos.y = yf
        Return pos
    End Function
    Private Function posiblePositivoH(ByVal t As Integer(,), p1 As posicion, p2 As posicion) As movimiento
        Dim le As posicion
        Dim ri As posicion
        Dim mov As movimiento
        mov.posicionOrigen.x = -1
        If p1.x < p2.x Then
            le.x = p1.x
            le.y = p1.y
            ri.x = p2.x
            ri.y = p2.y
        Else
            le.x = p2.x
            le.y = p2.y
            ri.x = p1.x
            ri.y = p1.y
        End If

        If t(le.x, le.y) = t(ri.x, ri.y) Then
            If (le.x >= 2) Then
                If t(le.x, le.y) = t(le.x - 2, le.y) And t(le.x, le.y) = t(ri.x, ri.y) Then
                    mov.posicionOrigen.x = le.x - 2
                    mov.posicionOrigen.y = le.y
                    mov.posicionDestino.x = le.x - 1
                    mov.posicionDestino.y = le.y
                    Return mov ' 1 y 2
                End If
                If t(ri.x, ri.y) = t(le.x - 2, le.y) And t(le.x - 2, le.y) = t(le.x - 1, le.y) Then
                    mov.posicionOrigen.x = le.x
                    mov.posicionOrigen.y = le.y
                    mov.posicionDestino.x = ri.x
                    mov.posicionDestino.y = ri.y
                    Return mov ' p1 y p2
                End If
            End If
            If (le.x >= 2) Then
                If (ri.x < _columnasV - 1) And t(le.x - 1, le.y) = t(ri.x, ri.y) And t(ri.x, ri.y) = t(ri.x + 1, ri.y) Then
                    mov.posicionOrigen.x = le.x - 1
                    mov.posicionOrigen.y = le.y
                    mov.posicionDestino.x = le.x
                    mov.posicionDestino.y = le.y
                    Return mov ' 2 y p1
                End If
            End If
            If (ri.x < _columnasV - 2) And t(le.x, le.y) = t(ri.x, ri.y) And t(ri.x, ri.y) = t(ri.x + 2, ri.y) Then
                mov.posicionOrigen.x = ri.x + 2
                mov.posicionOrigen.y = ri.y
                mov.posicionDestino.x = ri.x + 1
                mov.posicionDestino.y = ri.y
                Return mov '  3, y 4
            End If
        End If

    End Function
    Private Function posiblePositivoV(ByVal t As Integer(,), p1 As posicion, p2 As posicion) As movimiento
        Dim dn As posicion
        Dim up As posicion
        Dim mov As movimiento
        mov.posicionOrigen.x = -1
        If p1.y < p2.y Then
            dn.x = p1.x
            dn.y = p1.y
            up.x = p2.x
            up.y = p2.y
        Else
            dn.x = p2.x
            dn.y = p2.y
            up.x = p1.x
            up.y = p1.y
        End If

        If t(dn.x, dn.y) = t(up.x, up.y) Then
            If (dn.y >= 2) Then
                If t(dn.x, dn.y) = t(dn.x, dn.y - 2) And t(dn.x, dn.y) = t(up.x, up.y) Then
                    mov.posicionOrigen.x = dn.x
                    mov.posicionOrigen.y = dn.y - 2
                    mov.posicionDestino.x = dn.x
                    mov.posicionDestino.y = dn.y - 1
                    Return mov ' 3 y 4
                End If
                If t(up.x, up.y) = t(dn.x, dn.y - 2) And t(dn.x, dn.y - 2) = t(dn.x, dn.y - 1) Then
                    mov.posicionOrigen.x = dn.x
                    mov.posicionOrigen.y = dn.y
                    mov.posicionDestino.x = up.x
                    mov.posicionDestino.y = up.y
                    Return mov '  up y dn
                End If
            End If
            If (dn.y >= 1) Then
                If (up.y < _filasV - 1) And t(dn.x, dn.y - 1) = t(up.x, up.y) And t(up.x, up.y) = t(up.x, up.y + 1) Then
                    mov.posicionOrigen.x = dn.x
                    mov.posicionOrigen.y = dn.y - 1
                    mov.posicionDestino.x = dn.x
                    mov.posicionDestino.y = dn.y
                    Return mov ' up ,dn
                End If
            End If
            If (up.y < _filasV - 2) And t(dn.x, dn.y) = t(up.x, up.y) And t(up.x, up.y) = t(up.x, up.y + 2) Then
                mov.posicionOrigen.x = up.x
                mov.posicionOrigen.y = up.y + 2
                mov.posicionDestino.x = up.x
                mov.posicionDestino.y = up.y + 1
                Return mov '  1 y 2
            End If
        End If

    End Function
    Private Function buscaPar(ByVal t As Integer(,)) As movimiento
        Dim hayLineas As Boolean = False
        Dim x As Integer
        Dim y As Integer
        Dim xx As Integer
        Dim yy As Integer
        Dim mov As movimiento
        Dim rand As New Random
        Dim azar As Integer
        Randomize()
        Do
            yy = rand.Next(0, _filasV - 3)
            ' y = generaAleatoreo(0, _filasV)
            xx = generaAleatoreo(0, _columnasV - 3)
        Loop While xx = Lastx And Lasty = yy
        Lastx = xx
        Lasty = yy
        mov.posicionOrigen.x = 2
        mov.posicionOrigen.y = 2

        mov.posicionDestino.x = 2
        mov.posicionDestino.y = 1
        'MsgBox("x:" + x.ToString + " y: " + y.ToString)
        Do
            For x = xx To _columnasV - 2
                For y = yy To _filasV - 2

                    '''''''''''''''''''''''''''''''''''''''''''1
                    If (y >= 1 And x > 1) AndAlso t(x, y) = t(x, y - 1) Then
                        azar = generaAleatoreo(1, 3)
                        mov.posicionOrigen.x = x
                        mov.posicionOrigen.y = y + 1
                        If azar = 1 Then
                            mov.posicionDestino.x = x
                            mov.posicionDestino.y = y + 2
                        End If
                        If azar = 2 Then
                            mov.posicionDestino.x = x - 1
                            mov.posicionDestino.y = y + 1
                        End If
                        If azar = 3 Then
                            mov.posicionDestino.x = x + 1
                            mov.posicionDestino.y = y + 1
                        End If
                        Return mov
                    End If
                    If (x > 1) AndAlso t(x, y) = t(x, y + 2) Then ''2
                        azar = generaAleatoreo(1, 3)
                        mov.posicionOrigen.x = x
                        mov.posicionOrigen.y = y + 1
                        If azar = 1 Then
                            mov.posicionDestino.x = x
                            mov.posicionDestino.y = y
                        End If
                        If azar = 2 Then
                            mov.posicionDestino.x = x - 1
                            mov.posicionDestino.y = y + 1
                        End If
                        If azar = 3 Then
                            mov.posicionDestino.x = x + 1
                            mov.posicionDestino.y = y + 1
                        End If
                        Return mov

                    End If
                    ''''''''''''''''''''''''''''''''3

                    If (x > 1) AndAlso t(x, y) = t(x, y + 1) Then
                        mov.posicionOrigen.x = x
                        mov.posicionOrigen.y = y + 2
                        azar = generaAleatoreo(1, 3)

                        If azar = 1 Then
                            mov.posicionDestino.x = x
                            mov.posicionDestino.y = y + 3
                        End If
                        If azar = 2 Then
                            mov.posicionDestino.x = x - 1
                            mov.posicionDestino.y = y + 2
                        End If
                        If azar = 3 Then
                            mov.posicionDestino.x = x + 1
                            mov.posicionDestino.y = y + 2
                        End If
                        Return mov

                        Return mov
                    End If
                    ''''''''''''''''''''''''''''''''''''''''''''4
                    If (x <= _columnasV - 2 And y > 1) AndAlso t(x, y) = t(x + 1, y) Then
                        azar = generaAleatoreo(1, 3)
                        mov.posicionOrigen.x = x + 2
                        mov.posicionOrigen.y = y
                        If azar = 1 Then
                            mov.posicionDestino.x = x + 3
                            mov.posicionDestino.y = y
                        End If
                        If azar = 2 Then
                            mov.posicionDestino.x = x + 2
                            mov.posicionDestino.y = y + 1
                        End If
                        If azar = 3 Then
                            mov.posicionDestino.x = x + 2
                            mov.posicionDestino.y = y - 1
                        End If
                        Return mov
                    End If
                    '''''''''''''''''''''''''''''''''''''5
                    If (x <= _columnasV - 2 And y > 1) AndAlso t(x + 2, y) = t(x, y) Then
                        azar = generaAleatoreo(1, 3)
                        mov.posicionOrigen.x = x + 1
                        mov.posicionOrigen.y = y
                        If azar = 1 Then
                            mov.posicionDestino.x = x + 2
                            mov.posicionDestino.y = y
                        End If
                        If azar = 2 Then
                            mov.posicionDestino.x = x + 1
                            mov.posicionDestino.y = y + 1
                        End If
                        If azar = 3 Then
                            mov.posicionDestino.x = x + 1
                            mov.posicionDestino.y = y - 1
                        End If
                        Return mov
                    End If

                    If (x > 1 And y > 1) AndAlso t(x - 1, y) = t(x, y) Then ''6
                        azar = generaAleatoreo(1, 3)
                        mov.posicionOrigen.x = x + 1
                        mov.posicionOrigen.y = y
                        If azar = 1 Then
                            mov.posicionDestino.x = x + 2
                            mov.posicionDestino.y = y
                        End If
                        If azar = 2 Then
                            mov.posicionDestino.x = x + 1
                            mov.posicionDestino.y = y + 1
                        End If
                        If azar = 3 Then
                            mov.posicionDestino.x = x + 1
                            mov.posicionDestino.y = y - 1
                        End If
                        Return mov
                        Return mov
                    End If
                Next
            Next
        Loop While Not hayLineas
    End Function

    Private Function posiblePositivo(ByVal t As Integer(,), p1 As posicion, p2 As posicion) As movimiento
        Dim m As movimiento

        m = posiblePositivoH(t, p1, p2)
        If m.posicionOrigen.x > 0 Then
            Return m
        End If
        m = posiblePositivoV(t, p1, p2)
        If m.posicionOrigen.x > 0 Then
            Return m
        End If
    End Function


    Private Function buscaMejordelTablero2(ByRef tt As Integer(,)) As movimiento
        Dim mejorMovimiento As movimiento
        Dim puntaje As Integer = 0
        Dim mejorPuntaje As Integer = 0
        Dim x As Integer
        Dim y As Integer
        Dim mov As movimiento
        Dim xx As Integer
        Dim yy As Integer
 
        '   mov = buscaPar(tt)
        '  On Error Resume Next
        Randomize()
        'Do
        '    yy = generaAleatoreo(0, _filasV - 1)
        '    xx = generaAleatoreo(0, _columnasV - 1)
        'Loop While xx = Lastx And Lasty = yy
        Lastx = xx
        Lasty = yy
        For x = 0 To _columnasV - 1
            For y = 0 To _filasV - 1

                mov.posicionOrigen.x = x
                mov.posicionOrigen.y = y
                mov.posicionDestino.x = x
                mov.posicionDestino.y = y + 1
                twToTp()
                puntaje = calculaPuntosMovimiento(mov)
                If puntaje > mejorPuntaje Then
                    mejorPuntaje = puntaje
                    mejorMovimiento = mov
                End If

                mov.posicionOrigen.x = x
                mov.posicionOrigen.y = y
                mov.posicionDestino.x = x + 1
                mov.posicionDestino.y = y
                twToTp()
                puntaje = calculaPuntosMovimiento(mov)
                If puntaje > mejorPuntaje Then
                    mejorPuntaje = puntaje
                    mejorMovimiento = mov
                End If

            Next
        Next
        'MsgBox("El mejor movimiento generara : " + mejorPuntaje.ToString + " puntos. En (" + mejorMovimiento.posicionOrigen.x.ToString + "," + mejorMovimiento.posicionOrigen.y.ToString + ") (" + mejorMovimiento.posicionDestino.x.ToString + "," + mejorMovimiento.posicionDestino.y.ToString + ")")
        Return mejorMovimiento
    End Function
    'Private Function buscaMejordelTablero(ByVal t As Integer(,)) As movimiento
    '    Dim mejorMovimiento As movimiento
    '    Dim puntaje As Integer = 0
    '    Dim mejorPuntaje As Integer = 0
    '    Dim x As Integer
    '    Dim y As Integer
    '    Dim mov As movimiento
    '    mov = buscaPar(t)

    '    For x = 0 To _columnasV - 1
    '        For y = 0 To _filasV - 1
    '            If t(x, y) = t(x, y + 1) Then
    '                mov.posicionOrigen.x = x
    '                mov.posicionOrigen.y = y
    '                mov.posicionDestino.x = x
    '                mov.posicionDestino.y = y + 1
    '                puntaje = calculaPuntosMovimiento(t, mov)
    '                If puntaje > mejorPuntaje Then
    '                    mejorPuntaje = puntaje
    '                    mejorMovimiento = mov
    '                End If
    '            End If
    '            If (y >= 1) AndAlso t(x, y) = t(x, y - 1) Then
    '                mov.posicionOrigen.x = x
    '                mov.posicionOrigen.y = y
    '                mov.posicionDestino.x = x
    '                mov.posicionDestino.y = y - 1
    '                puntaje = calculaPuntosMovimiento(t, mov)
    '                If puntaje > mejorPuntaje Then
    '                    mejorPuntaje = puntaje
    '                    mejorMovimiento = mov
    '                End If
    '            End If
    '            If t(x, y) = t(x, y + 2) Then
    '                mov.posicionOrigen.x = x
    '                mov.posicionOrigen.y = y
    '                mov.posicionDestino.x = x
    '                mov.posicionDestino.y = y + 1
    '                puntaje = calculaPuntosMovimiento(t, mov)
    '                If puntaje > mejorPuntaje Then
    '                    mejorPuntaje = puntaje
    '                    mejorMovimiento = mov
    '                End If
    '            End If
    '            If t(x, y) = t(x, y + 2) Then
    '                mov.posicionOrigen.x = x
    '                mov.posicionOrigen.y = y + 1
    '                mov.posicionDestino.x = x
    '                mov.posicionDestino.y = y + 2
    '                puntaje = calculaPuntosMovimiento(t, mov)
    '                If puntaje > mejorPuntaje Then
    '                    mejorPuntaje = puntaje
    '                    mejorMovimiento = mov
    '                End If
    '            End If
    '            If t(x, y) = t(x + 1, y) Then
    '                mov.posicionOrigen.x = x
    '                mov.posicionOrigen.y = y
    '                mov.posicionDestino.x = x + 1
    '                mov.posicionDestino.y = y
    '                puntaje = calculaPuntosMovimiento(t, mov)
    '                If puntaje > mejorPuntaje Then
    '                    mejorPuntaje = puntaje
    '                    mejorMovimiento = mov
    '                End If
    '            End If

    '            If (x >= 1) AndAlso t(x, y) = t(x - 1, y) Then
    '                mov.posicionOrigen.x = x
    '                mov.posicionOrigen.y = y
    '                mov.posicionDestino.x = x - 1
    '                mov.posicionDestino.y = y
    '                puntaje = calculaPuntosMovimiento(t, mov)
    '                If puntaje > mejorPuntaje Then
    '                    mejorPuntaje = puntaje
    '                    mejorMovimiento = mov
    '                End If
    '            End If
    '        Next
    '    Next
    '    ' MsgBox("El mejor movimiento generara : " + mejorPuntaje.ToString + " puntos")
    '    Return mejorMovimiento
    'End Function
    Private Function calculaPuntosMovimiento(m1 As movimiento) As Integer

        Dim puntosCambio = 0
        Dim jugando As Boolean = True
        Dim hayLineas As Boolean = False
        Dim p1, p2 As posicion
        Dim mov As New movimiento
        Dim _movimientosCorrecto As Integer = 0
        p1.x = m1.posicionOrigen.x
        p1.y = m1.posicionOrigen.y
        p2.x = m1.posicionDestino.x
        p2.y = m1.posicionDestino.y
        swapArreglo(tableroPruebas, p1, p2)
        Do While jugando

            If buscaSiExisteLinea(tableroPruebas) Then

                hayLineas = True
                'dibujaTablero(tableroPruebas)
                borraLineaTablero3(tableroPruebas, _p1, _p2, _p3)
                bajaColumnas(tableroPruebas, _p1.x)
                bajaColumnas(tableroPruebas, _p2.x)
                bajaColumnas(tableroPruebas, _p3.x)
                puntosCambio = puntosCambio + 5

                jugando = True
                'MsgBox("Movimiento Eliminara linea")
            Else
                hayLineas = False
                jugando = False
            End If

        Loop
        ' swapArreglo(tableroPruebas, p1, p2)
        '  _puntos = _puntos + puntosCambio
        ' Label2.Text = "Puntos: " + _puntos.ToString
        ' MsgBox("Este cambio genera : " + puntosCambio.ToString + " puntos")
        Return puntosCambio
    End Function
    Private Sub cmdAnalisis_Click(sender As Object, e As EventArgs) Handles cmdAnalisis.Click
        'tablero = tableroBack
        listaMovimientos.Clear()
        tbToT()
        dibujaTablero(tablero)
        creaRuta2(tablero)
    End Sub
    Private Sub cmdAnalisis3_Click(sender As Object, e As EventArgs) Handles cmdAnalisis3.Click
        'tablero = tableroBack

        listaMovimientos.Clear()
        tbToT()
        creaRuta33(tablero)
    End Sub
    Sub creaRutaX(ByVal trx As Integer(,))
        Dim i As Integer = 0
        Dim _lpuntos As Integer = 0

        Dim movio As Boolean = False
        Dim _moves As New List(Of movimiento)
        Dim jugando As Boolean = True
        Dim mov As movimiento
        '     Dim par As movimiento
        Dim hayLineas As Boolean = False
        Do While jugando
            'par = buscaPar(t)
            '  mov = posiblePositivo(t, par.posicionOrigen, par.posicionDestino)
            ' If Not hayLineas Then
            mov = buscaMejordelTablero2(tablero) ' cagadal
            swapArreglo(tablero, mov.posicionOrigen, mov.posicionDestino)
            ' End If
            dibujaTablero(tablero)
            If buscaSiExisteLinea(tablero) Then
                hayLineas = True
                i = 0
                borraLineaTablero3(tablero, _p1, _p2, _p3)
                bajaColumnas(tablero, _p1.x)
                bajaColumnas(tablero, _p2.x)
                bajaColumnas(tablero, _p3.x)
                Application.DoEvents()
                Threading.Thread.Sleep(mySleep)
                If chkAnimacion.Checked = True Then
                    dibujaTablero(tablero)
                End If
                movio = True
                _movimientosCorrectos = _movimientosCorrectos + 1
                _lpuntos = _lpuntos + 5
                '  listaMovimientos.Add(mov)
            Else
                hayLineas = True = False
                swap(tablero, mov.posicionOrigen, mov.posicionDestino)
            End If
            i = i + 1
            listaMovimientos.Add(mov)
            _pasos = _pasos + 1
            If _pasos > 300 Then
                jugando = False
            End If
            If _puntos >= 100 Then
                jugando = False
                dibujaTablero(tablero)
                MsgBox("Logro el objetivo en :" + _pasos.ToString + " Movimientos")
            End If

            Label1.Text = "Pasos :" + _pasos.ToString
            hayLineas = True
        Loop


    End Sub
    Sub creaRuta2(ByRef t As Integer(,))
        _pasos = 0
        _puntos = 0
        Label1.Text = "Pasos :" + _pasos.ToString
        Label2.Text = "Puntos: " + _puntos.ToString
        Dim jugando As Boolean = True
        Dim hayLineas As Boolean = False
        Dim p1, p2 As posicion
        Dim mov As New movimiento
        Randomize()
        Do While jugando
            Application.DoEvents()
            'busca posicion
            Do
                mov = buscaPar(tablero)
                p1.x = mov.posicionOrigen.x
                p1.y = mov.posicionOrigen.y

                p2.x = mov.posicionDestino.x
                p2.y = mov.posicionDestino.y
            Loop While tablero(p1.x, p1.y) = 0 Or tablero(p2.x, p2.y) = 0
            'ciclo
            swap(tablero, p1, p2)
            hayLineas = buscaSiExisteLinea(tablero)
            mov.posicionOrigen = p1
            mov.posicionDestino = p2
            listaMovimientos.Add(mov)
            Do While hayLineas
                tachaLineaTablero3(tablero, _p1, _p2, _p3)

                If chkAnimacion.Checked = True Then
                    dibujaTablero(tablero)
                End If
                Application.DoEvents()
                Threading.Thread.Sleep(700)
                borraLineaTablero3(tablero, _p1, _p2, _p3)
                bajaColumnas(tablero, _p1.x)
                bajaColumnas(tablero, _p2.x)
                bajaColumnas(tablero, _p3.x)
                Application.DoEvents()
                Threading.Thread.Sleep(mySleep)
                If chkAnimacion.Checked = True Then
                    dibujaTablero(tablero)
                End If
                _movimientosCorrectos = _movimientosCorrectos + 1
                _puntos = _puntos + 5
                hayLineas = buscaSiExisteLinea(tablero)

            Loop
            ' quita lineas
            'fin ciclo

            _pasos = _pasos + 1
            If _pasos > 1500 Then
                jugando = False
            End If
            If _puntos >= _puntaje Then
                jugando = False
                dibujaTablero(tablero)
                MsgBox("Logro el objetivo en :" + _pasos.ToString + " Movimientos")
            End If

            Label1.Text = "Pasos :" + _pasos.ToString
            Label2.Text = "Puntos: " + _puntos.ToString
        Loop
    



    End Sub
    Sub creaRuta3(ByRef tz As Integer(,))
        _pasos = 0
        _puntos = 0
        Label1.Text = "Pasos :" + _pasos.ToString
        Label2.Text = "Puntos: " + _puntos.ToString
        Dim jugando As Boolean = True
        Dim hayLineas As Boolean = False
        Dim p1, p2 As posicion
        Dim mov As New movimiento
        Randomize()
        Do While jugando
            Application.DoEvents()
            'Threading.Thread.Sleep(generaAleatoreo(10, 150))

            Do
                tToTw()
                mov = buscaMejordelTablero2(tableroWork)
                p1.x = mov.posicionOrigen.x
                p1.y = mov.posicionOrigen.y

                p2.x = mov.posicionDestino.x
                p2.y = mov.posicionDestino.y
            Loop While tableroWork(p1.x, p1.y) = 0 Or tableroWork(p2.x, p2.y) = 0

            If Not hayLineas Then
                swap(tablero, p1, p2)
                If buscaSiExisteLinea(tablero) Then
                    hayLineas = True
                    borraLineaTablero3(tablero, _p1, _p2, _p3)
                    bajaColumnas(tablero, _p1.x)
                    bajaColumnas(tablero, _p2.x)
                    bajaColumnas(tablero, _p3.x)
                    Application.DoEvents()
                    Threading.Thread.Sleep(mySleep)
                    If chkAnimacion.Checked = True Then
                        dibujaTablero(tablero)
                    End If
                    _movimientosCorrectos = _movimientosCorrectos + 1
                    _puntos = _puntos + 5
                    mov.posicionOrigen = p1
                    mov.posicionDestino = p2
                    listaMovimientos.Add(mov)
                Else
                    swap(tablero, p1, p2)
                End If
                mov.posicionOrigen = p1
                mov.posicionDestino = p2
                listaMovimientos.Add(mov)
                '  MsgBox("Normal")
            Else
                ' MsgBox("Aun queda otra")
                If buscaSiExisteLinea(tablero) Then
                    hayLineas = True
                Else
                    hayLineas = False
                End If
                borraLineaTablero3(tablero, _p1, _p2, _p3)
                bajaColumnas(tablero, _p1.x)
                bajaColumnas(tablero, _p2.x)
                bajaColumnas(tablero, _p3.x)
                _puntos = _puntos + 5
                Application.DoEvents()
                Threading.Thread.Sleep(mySleep)
                If chkAnimacion.Checked = True Then
                    dibujaTablero(tablero)
                End If
            End If
            _pasos = _pasos + 1
            If _pasos > 1500 Then
                jugando = False
            End If
            If _puntos >= _puntaje Then
                jugando = False
                dibujaTablero(tablero)
                MsgBox("Logro el objetivo en :" + _pasos.ToString + " Movimientos")
            End If

            Label1.Text = "Pasos :" + _pasos.ToString
            Label2.Text = "Puntos: " + _puntos.ToString
        Loop


    End Sub
    Sub creaRuta33(ByRef tz As Integer(,))
        _pasos = 0
        _puntos = 0
        Label1.Text = "Pasos :" + _pasos.ToString
        Label2.Text = "Puntos: " + _puntos.ToString
        Dim jugando As Boolean = True
        Dim hayLineas As Boolean = False
        Dim p1, p2 As posicion
        Dim mov As New movimiento
        Randomize()
        Do While jugando
            Application.DoEvents()
            'busca posicion
            Do
                tToTw()
                mov = buscaMejordelTablero2(tableroWork)
                p1.x = mov.posicionOrigen.x
                p1.y = mov.posicionOrigen.y

                p2.x = mov.posicionDestino.x
                p2.y = mov.posicionDestino.y
            Loop While tableroWork(p1.x, p1.y) = 0 Or tableroWork(p2.x, p2.y) = 0
            'ciclo
            swap(tablero, p1, p2)
            hayLineas = buscaSiExisteLinea(tablero)
            mov.posicionOrigen = p1
            mov.posicionDestino = p2
            listaMovimientos.Add(mov)
            Do
                tachaLineaTablero3(tablero, _p1, _p2, _p3)

                If chkAnimacion.Checked = True Then
                    dibujaTablero(tablero)
                End If
                Application.DoEvents()
                Threading.Thread.Sleep(700)
                borraLineaTablero3(tablero, _p1, _p2, _p3)
                bajaColumnas(tablero, _p1.x)
                bajaColumnas(tablero, _p2.x)
                bajaColumnas(tablero, _p3.x)
                Application.DoEvents()
                Threading.Thread.Sleep(mySleep)
                If chkAnimacion.Checked = True Then
                    dibujaTablero(tablero)
                End If
                    _movimientosCorrectos = _movimientosCorrectos + 1
                    _puntos = _puntos + 5



                hayLineas = buscaSiExisteLinea(tablero)

            Loop While hayLineas
            ' quita lineas
            'fin ciclo
  
            _pasos = _pasos + 1
            If _pasos > 1500 Then
                jugando = False
            End If
            If _puntos >= _puntaje Then
                jugando = False
                dibujaTablero(tablero)
                MsgBox("Logro el objetivo en :" + _pasos.ToString + " Movimientos")
            End If

            Label1.Text = "Pasos :" + _pasos.ToString
            Label2.Text = "Puntos: " + _puntos.ToString
        Loop


    End Sub
    Sub creaRutaRandom2(ByRef tM As Integer(,))
        _pasos = 0
        _puntos = 0
        Label1.Text = "Pasos :" + _pasos.ToString
        Label2.Text = "Puntos: " + _puntos.ToString
        Dim jugando As Boolean = True
        Dim hayLineas As Boolean = False
        Dim p1, p2 As posicion
        Dim mov As New movimiento
        Randomize()
        tbToT()

        Do While jugando
            Application.DoEvents()
            'busca posicion
            Do
                p1 = GeneraPosicionRNd()
                p2 = GeneraPosicionPar(p1)
            Loop While tablero(p1.x, p1.y) = 0 Or tablero(p2.x, p2.y) = 0

            'ciclo
            swap(tablero, p1, p2)
            hayLineas = buscaSiExisteLinea(tablero)
            mov.posicionOrigen = p1
            mov.posicionDestino = p2
            listaMovimientos.Add(mov)
            If Not hayLineas Then
                swap(tablero, p1, p2)
            End If
            Do While hayLineas
                tachaLineaTablero3(tablero, _p1, _p2, _p3)

                If chkAnimacion.Checked = True Then
                    dibujaTablero(tablero)
                End If
                Application.DoEvents()
                Threading.Thread.Sleep(700)
                borraLineaTablero3(tablero, _p1, _p2, _p3)
                bajaColumnas(tablero, _p1.x)
                bajaColumnas(tablero, _p2.x)
                bajaColumnas(tablero, _p3.x)
                Application.DoEvents()
                Threading.Thread.Sleep(mySleep)
                If chkAnimacion.Checked = True Then
                    dibujaTablero(tablero)
                End If
                _movimientosCorrectos = _movimientosCorrectos + 1
                _puntos = _puntos + 5
                hayLineas = buscaSiExisteLinea(tablero)

            Loop
            ' quita lineas
            'fin ciclo

            _pasos = _pasos + 1
            If _pasos > 1500 Then
                jugando = False
            End If
            If _puntos >= _puntaje Then
                jugando = False
                dibujaTablero(tablero)
                MsgBox("Logro el objetivo en :" + _pasos.ToString + " Movimientos")
            End If

            Label1.Text = "Pasos :" + _pasos.ToString
            Label2.Text = "Puntos: " + _puntos.ToString
        Loop


    End Sub
    Sub creaRutaRandom(ByRef t As Integer(,))
        Randomize()
        _pasos = 0
        _puntos = 0
        Label1.Text = "Pasos :" + _pasos.ToString
        Label2.Text = "Puntos: " + _puntos.ToString
        Dim jugando As Boolean = True
        Dim hayLineas As Boolean = False
        Dim p1, p2 As posicion
        Dim mov As New movimiento
        Do While jugando

            Do
                p1 = GeneraPosicionRNd()
                p2 = GeneraPosicionPar(p1)
            Loop While t(p1.x, p1.y) = 0 Or t(p2.x, p2.y) = 0
            If Not hayLineas Then
                swap(t, p1, p2)
                If buscaSiExisteLinea(t) Then
                    hayLineas = True
                    borraLineaTablero3(t, _p1, _p2, _p3)
                    bajaColumnas(t, _p1.x)
                    bajaColumnas(t, _p2.x)
                    bajaColumnas(t, _p3.x)
                    Application.DoEvents()
                    Threading.Thread.Sleep(mySleep)
                    If chkAnimacion.Checked = True Then
                        dibujaTablero(t)
                    End If
                    _movimientosCorrectos = _movimientosCorrectos + 1
                    _puntos = _puntos + 5
                    mov.posicionOrigen = p1
                    mov.posicionDestino = p2
                    listaMovimientos.Add(mov)
                Else
                    swap(t, p1, p2)
                End If
                mov.posicionOrigen = p1
                mov.posicionDestino = p2
                listaMovimientos.Add(mov)
                '  MsgBox("Normal")
            Else
                ' MsgBox("Aun queda otra")
                If buscaSiExisteLinea(t) Then
                    hayLineas = True
                Else
                    hayLineas = False
                End If
                borraLineaTablero3(t, _p1, _p2, _p3)
                bajaColumnas(t, _p1.x)
                bajaColumnas(t, _p2.x)
                bajaColumnas(t, _p3.x)
                _puntos = _puntos + 5
                Application.DoEvents()
                Threading.Thread.Sleep(mySleep)
                If chkAnimacion.Checked = True Then
                    dibujaTablero(t)
                End If
            End If
            _pasos = _pasos + 1
            If _pasos > 1500 Then
                jugando = False
            End If
            If _puntos >= _puntaje Then
                jugando = False
                dibujaTablero(t)
                MsgBox("Logro el objetivo en :" + _pasos.ToString + " Movimientos")
            End If

            Label1.Text = "Pasos :" + _pasos.ToString
            Label2.Text = "Puntos: " + _puntos.ToString
        Loop


    End Sub
    Sub verUltimosMovimientos()
        frmUltimosMov.Show()
    End Sub
    Private Sub cmdIntellRandom_Click(sender As Object, e As EventArgs) Handles cmdIntellRandom.Click
        'tablero = tableroBack
        listaMovimientos.Clear()
        tbToT()
        creaRutaRandom2(tablero)

    End Sub



    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim m As movimiento
        m.posicionOrigen.x = Integer.Parse(TextBox1.Text)
        m.posicionOrigen.y = Integer.Parse(TextBox2.Text)
        m.posicionDestino.x = Integer.Parse(TextBox3.Text)
        m.posicionDestino.y = Integer.Parse(TextBox4.Text)

        calculaPuntosMovimiento(m)
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click

    End Sub

    Private Sub VerMovimientosToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VerMovimientosToolStripMenuItem.Click
        verUltimosMovimientos()
    End Sub




End Class
