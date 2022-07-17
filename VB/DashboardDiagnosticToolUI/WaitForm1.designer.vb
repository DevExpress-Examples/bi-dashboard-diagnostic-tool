Imports DevExpress.XtraWaitForm

Namespace DashboardDiagnosticToolUI

    Partial Class WaitForm1

        ''' <summary>
        ''' Required designer variable.
        ''' </summary>
        Private components As System.ComponentModel.IContainer = Nothing

        ''' <summary>
        ''' Clean up any resources being used.
        ''' </summary>
        ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing AndAlso (Me.components IsNot Nothing) Then
                Me.components.Dispose()
            End If

            MyBase.Dispose(disposing)
        End Sub

#Region "Windows Form Designer generated code"
        ''' <summary>
        ''' Required method for Designer support - do not modify
        ''' the contents of this method with the code editor.
        ''' </summary>
        Private Sub InitializeComponent()
            Me.panel1 = New DevExpress.XtraWaitForm.ProgressPanel()
            Me.SuspendLayout()
            ' 
            ' panel1
            ' 
            Me.panel1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.panel1.ImageHorzOffset = 40
            Me.panel1.Location = New System.Drawing.Point(0, 0)
            Me.panel1.Name = "panel1"
            Me.panel1.Size = New System.Drawing.Size(240, 80)
            Me.panel1.TabIndex = 0
            ' 
            ' WaitForm1
            ' 
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(240, 80)
            Me.Controls.Add(Me.panel1)
            Me.Name = "WaitForm1"
            Me.Text = "WaitForm1"
            Me.ResumeLayout(False)
        End Sub

#End Region
        Private panel1 As DevExpress.XtraWaitForm.ProgressPanel
    End Class
End Namespace
