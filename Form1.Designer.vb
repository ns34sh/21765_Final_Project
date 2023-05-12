<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Button1 = New Button()
        Button2 = New Button()
        Label_time = New Label()
        Label2 = New Label()
        ProgressBar1 = New ProgressBar()
        Label3 = New Label()
        Label4 = New Label()
        StartLC = New TextBox()
        EndLC = New TextBox()
        Label_max_str = New Label()
        LC_label = New Label()
        Label_max_str_value = New Label()
        Label_max_str_LC = New Label()
        Label_time_remaining = New Label()
        SuspendLayout()
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(116, 106)
        Button1.Name = "Button1"
        Button1.Size = New Size(248, 56)
        Button1.TabIndex = 0
        Button1.Text = "Calculate Strain Energy"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' Button2
        ' 
        Button2.Location = New Point(191, 349)
        Button2.Name = "Button2"
        Button2.Size = New Size(93, 39)
        Button2.TabIndex = 2
        Button2.Text = "Cancel"
        Button2.UseVisualStyleBackColor = True
        ' 
        ' Label_time
        ' 
        Label_time.AutoSize = True
        Label_time.Location = New Point(79, 185)
        Label_time.Name = "Label_time"
        Label_time.Size = New Size(164, 25)
        Label_time.TabIndex = 5
        Label_time.Text = "Estimated time left:"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(243, 122)
        Label2.Name = "Label2"
        Label2.Size = New Size(0, 25)
        Label2.TabIndex = 6
        ' 
        ' ProgressBar1
        ' 
        ProgressBar1.Location = New Point(161, 226)
        ProgressBar1.Name = "ProgressBar1"
        ProgressBar1.Size = New Size(150, 34)
        ProgressBar1.TabIndex = 7
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(66, 23)
        Label3.Name = "Label3"
        Label3.Size = New Size(200, 25)
        Label3.TabIndex = 8
        Label3.Text = "Start Load Combination"
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(66, 65)
        Label4.Name = "Label4"
        Label4.Size = New Size(194, 25)
        Label4.TabIndex = 9
        Label4.Text = "End Load Combination"
        ' 
        ' StartLC
        ' 
        StartLC.Location = New Point(272, 20)
        StartLC.Name = "StartLC"
        StartLC.Size = New Size(150, 31)
        StartLC.TabIndex = 10
        ' 
        ' EndLC
        ' 
        EndLC.Location = New Point(272, 65)
        EndLC.Name = "EndLC"
        EndLC.Size = New Size(150, 31)
        EndLC.TabIndex = 11
        ' 
        ' Label_max_str
        ' 
        Label_max_str.AutoSize = True
        Label_max_str.Location = New Point(79, 278)
        Label_max_str.Name = "Label_max_str"
        Label_max_str.Size = New Size(202, 25)
        Label_max_str.TabIndex = 12
        Label_max_str.Text = "Maximum strain energy:"
        ' 
        ' LC_label
        ' 
        LC_label.AutoSize = True
        LC_label.Location = New Point(79, 309)
        LC_label.Name = "LC_label"
        LC_label.Size = New Size(214, 25)
        LC_label.TabIndex = 13
        LC_label.Text = "Corresponding load case:"
        ' 
        ' Label_max_str_value
        ' 
        Label_max_str_value.AutoSize = True
        Label_max_str_value.Location = New Point(272, 278)
        Label_max_str_value.Name = "Label_max_str_value"
        Label_max_str_value.Size = New Size(22, 25)
        Label_max_str_value.TabIndex = 14
        Label_max_str_value.Text = "0"
        ' 
        ' Label_max_str_LC
        ' 
        Label_max_str_LC.AutoSize = True
        Label_max_str_LC.Location = New Point(289, 309)
        Label_max_str_LC.Name = "Label_max_str_LC"
        Label_max_str_LC.Size = New Size(22, 25)
        Label_max_str_LC.TabIndex = 15
        Label_max_str_LC.Text = "0"
        ' 
        ' Label_time_remaining
        ' 
        Label_time_remaining.AutoSize = True
        Label_time_remaining.Location = New Point(244, 185)
        Label_time_remaining.Name = "Label_time_remaining"
        Label_time_remaining.Size = New Size(22, 25)
        Label_time_remaining.TabIndex = 16
        Label_time_remaining.Text = "0"
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(10F, 25F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(506, 415)
        Controls.Add(Label_time_remaining)
        Controls.Add(Label_max_str_LC)
        Controls.Add(Label_max_str_value)
        Controls.Add(LC_label)
        Controls.Add(Label_max_str)
        Controls.Add(EndLC)
        Controls.Add(StartLC)
        Controls.Add(Label4)
        Controls.Add(Label3)
        Controls.Add(ProgressBar1)
        Controls.Add(Label2)
        Controls.Add(Label_time)
        Controls.Add(Button2)
        Controls.Add(Button1)
        Name = "Form1"
        Text = "Calculate Strain Energy"
        ResumeLayout(False)
        PerformLayout()
    End Sub
    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents Label_time As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents ProgressBar1 As ProgressBar
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents StartLC As TextBox
    Friend WithEvents EndLC As TextBox
    Friend WithEvents Label_max_str As Label
    Friend WithEvents LC_label As Label
    Friend WithEvents Label_max_str_value As Label
    Friend WithEvents Label_max_str_LC As Label
    Friend WithEvents Label_time_remaining As Label
End Class
