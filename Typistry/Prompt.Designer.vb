<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Prompt
    Inherits System.Windows.Forms.Form

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Command = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'Command
        '
        Me.Command.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Command.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.Command.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Command.Font = New System.Drawing.Font("Segoe UI Light", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Command.ForeColor = System.Drawing.Color.White
        Me.Command.Location = New System.Drawing.Point(0, 0)
        Me.Command.Name = "Command"
        Me.Command.Size = New System.Drawing.Size(779, 36)
        Me.Command.TabIndex = 0
        '
        'Prompt
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(779, 47)
        Me.Controls.Add(Me.Command)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "Prompt"
        Me.ShowInTaskbar = False
        Me.Text = "Typistry"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents Command As System.Windows.Forms.TextBox

End Class
