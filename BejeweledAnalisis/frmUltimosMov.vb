Public Class frmUltimosMov

    Private Sub frmUltimosMov_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim i As Long
        Dim origen As String
        Dim destino As String
        ListBox1.Items.Clear()
        For i = 0 To frmMain.listaMovimientos.Count - 1
            origen = " ( " + frmMain.listaMovimientos.Item(i).posicionOrigen.x.ToString + " , " + frmMain.listaMovimientos.Item(i).posicionOrigen.y.ToString + " )"
            destino = " ( " + frmMain.listaMovimientos.Item(i).posicionDestino.x.ToString + " , " + frmMain.listaMovimientos.Item(i).posicionDestino.y.ToString + " )"
            ListBox1.Items.Add(i.ToString + " -> Cambia  " + origen + " con " + destino)

        Next
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged

    End Sub
End Class