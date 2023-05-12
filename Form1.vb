Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Threading
Imports System.Reflection.PortableExecutable
Imports System.Configuration
''Calculate strain energy subroutine contains the actual code for calculating strain energy
Public Class Form1
    Private _progressBar As ProgressBar
    Private _worker As BackgroundWorker

    Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        Initialize()
        BindComponent()
    End Sub

    Private Sub Initialize()
        _progressBar = ProgressBar1
        '_progressBar.Dock = DockStyle.Fill

        _worker = New BackgroundWorker()
        _worker.WorkerReportsProgress = True
        _worker.WorkerSupportsCancellation = True

        'Me.Controls.Add(_progressBar)
    End Sub

    Private Sub BindComponent()
        AddHandler _worker.ProgressChanged, AddressOf _worker_ProgressChanged
        AddHandler _worker.RunWorkerCompleted, AddressOf _worker_RunWorkerCompleted
        AddHandler _worker.DoWork, AddressOf _worker_DoWork
        AddHandler Me.Load, AddressOf Form1_Load
    End Sub

    Private Sub Form1_Load()

    End Sub

    Private Sub _worker_ProgressChanged(ByVal o As Object, ByVal e As ProgressChangedEventArgs)

        'Update progress bar in GUI form
        _progressBar.Increment(e.ProgressPercentage)

    End Sub

    Private Sub _worker_RunWorkerCompleted(ByVal o As Object, ByVal e As RunWorkerCompletedEventArgs)

        If (e.Cancelled) Then
            'If user cancels the process then display a message
            MsgBox("Process was interrupted by the user.")
        ElseIf e.Error IsNot Nothing Then
            'Display an error message if there is an error
            MsgBox(e.Error.Message)
        Else
            'Display an final output for maximum strain energy and the corresponding load case in GUI.
            Label_max_str_value.Text = e.Result(0)
            Label_max_str_LC.Text = e.Result(1)
            MsgBox("Done.")
        End If
    End Sub

    Private Sub _worker_DoWork(ByVal o As Object, ByVal e As DoWorkEventArgs)
        Dim worker = DirectCast(o, BackgroundWorker)
        'Run code to calculate strain energy
        calculate_strain_energy(worker, e)

    End Sub

    Private Sub SetProgressMaximum(ByVal max As Integer)
        If _progressBar.InvokeRequired Then
            _progressBar.Invoke(Sub() SetProgressMaximum(max))
        Else
            _progressBar.Maximum = max
        End If
    End Sub

    Public Sub calculate_strain_energy(worker, e)

        Dim objOpenSTAAD As Object
        'Create COM object to access STAADpro software API commands
        objOpenSTAAD = GetObject(, "StaadPro.OpenSTAAD")

        'Check if the COM object was created
        If objOpenSTAAD Is Nothing Then
            MsgBox("Please ensure STAAD file is open.")
            End
        End If

        'Check if the FEM analysis results are available 
        If objOpenSTAAD.Output.AreResultsAvailable() = False Then
            MsgBox("Please ensure results are available in STAAD.")
            End
        End If

        Dim BaseUnit As Double
        'Get Base Unit
        '1=English
        '2=Metric

        'Get the base unit used in the FEM analysis
        BaseUnit = objOpenSTAAD.GetBaseUnit

        Dim strain_energy_unit As String

        'Decide the output units to be displayed based on the base unit
        If BaseUnit = 1 Then
            strain_energy_unit = "kip-in"
        Else
            strain_energy_unit = "kN-m"
        End If


        Dim SelbeamsNo As Long

        Dim Selbeams() As Long

        'Get no. of beams selected by the user (in STAADpro software GUI)
        SelbeamsNo = objOpenSTAAD.Geometry.GetNoOfSelectedBeams

        'Prompt user to select beam
        If SelbeamsNo = 0 Then
            MsgBox("Please select members in STAAD.")
            End
        End If

        'Reallocate array for storing selected beam numbers
        ReDim Selbeams(SelbeamsNo - 1)
        'Populate Selbeams array using API command
        objOpenSTAAD.Geometry.GetSelectedBeams(Selbeams, 1)

        'Declare beam property variables
        Dim Width As Double
        Dim Depth As Double
        Dim Ax As Double
        Dim Ay As Double
        Dim Az As Double
        Dim Ix As Double
        Dim Iy As Double
        Dim Iz As Double

        Dim BeamLen As Double

        'Declare array for storing forces in beam
        Dim ForceArray(0 To 5) As Double

        'Store start and end load cases entered by the user in the form
        Dim start_load_case = CLng(e.argument(0))
        Dim end_load_case = CLng(e.argument(1))

        'Prompt user to enter valid load case values
        If start_load_case = 0 Or end_load_case = 0 Then
            MsgBox("Enter a valid integer value for start/end load cases.")
        End If

        'Declare variables to store strain energy contribution from different forces (i.e. axial, bending, torsion)
        Dim strain_energy(end_load_case - start_load_case + 1) As Double
        Dim strain_energy_mem As Double
        Dim strain_energy_axial As Double
        Dim strain_energy_my As Double
        Dim strain_energy_mz As Double
        Dim strain_energy_mx As Double

        'Calculate total calculations to be done. This will be used to update the progress bar
        'and to estimate the time remaining
        Dim max_progress = SelbeamsNo * (end_load_case - start_load_case + 1)
        SetProgressMaximum(max_progress)
        Dim progress_val = 0

        'Declare variable for showing progress to the user
        Dim pctCompl As Double
        Dim time_remaining As Double
        Static start_time As DateTime
        Static stop_time As DateTime
        Dim elapsed_time As TimeSpan

        'Declare variables to store beam material property
        Dim dElasticity As Double, dPoisson As Double, dDensity As Double, dAlpha As Double, dDamp As Double

        'Start timer (This will be used to estimate remaining time based on linear proportion)
        'For example, if time taken to complete x percent work is y seconds then time required to do
        'the remaining 100-x percent work is y / x * (100 - x) seconds.
        start_time = Now

        'Set maximum number of threads for parallel for loop
        Dim par_opt As New ParallelOptions
        par_opt.MaxDegreeOfParallelism = -1
        'Use parallel for loop for outer loop that iterates over the user-specified load cases
        Parallel.For(start_load_case, end_load_case, par_opt,
                     Sub(j)
                         'Inner loop iterates over all the beams selected by the user in STAADpro GUI
                         For i = 0 To SelbeamsNo - 1

                             'Get beam properties using API command
                             objOpenSTAAD.Property.GetBeamProperty(Selbeams(i), Width, Depth, Ax, Ay, Az, Ix, Iy, Iz)
                             'Get beam length using API command
                             BeamLen = objOpenSTAAD.Geometry.GetBeamLength(Selbeams(i))

                             'Get Elasticity using API command
                             objOpenSTAAD.Property.GetBeamConstants(Selbeams(i), dElasticity, dPoisson, dDensity, dAlpha, dDamp)

                             'Inner loops calculates stain energy at every 1/10th location of the beam length
                             For k = 0 To BeamLen Step Math.Round(BeamLen / 10, 3)
                                 'Get forces at various sections along the beam length
                                 objOpenSTAAD.Output.GetIntermediateMemberForcesAtDistance(CLng(i), k, CLng(j), ForceArray)

                                 'Calculate strain energy due to axial load in the beam
                                 strain_energy_axial = ForceArray(0) ^ 2 * (BeamLen / 10) / (2 * Ax * dElasticity)

                                 'Calculate strain energy due to minor axis bending in the beam
                                 strain_energy_my = ForceArray(4) ^ 2 * (BeamLen / 10) / (2 * Iy * dElasticity)

                                 'Calculate strain energy due to major axis bending in the beam
                                 strain_energy_mz = ForceArray(5) ^ 2 * (BeamLen / 10) / (2 * Iz * dElasticity)

                                 'Calcuate strain energy due to torsion in the beam
                                 strain_energy_mx = ForceArray(3) ^ 2 * (BeamLen / 10) / (2 * Ix * dElasticity)

                                 'Sum up the total strain energy due to each of the four components calculated above
                                 strain_energy_mem = strain_energy_axial + strain_energy_my + strain_energy_mz + strain_energy_mx

                                 'Store the strain energy calculated in an array for each load case
                                 strain_energy(j) += strain_energy_mem
                             Next k
                             'Update progress
                             progress_val = (i) * (end_load_case - start_load_case + 1) + (j - start_load_case + 1)
                             'Calcuate percentage completion
                             pctCompl = progress_val / max_progress * 100
                             'Report progress to GUI
                             worker.ReportProgress(pctCompl)

                             'Stop timer
                             stop_time = Now
                             'Calculate elapsed time
                             elapsed_time = stop_time.Subtract(start_time)
                             'Start timer again
                             start_time = Now
                             'Estimate time remaining
                             time_remaining = Math.Round(elapsed_time.TotalSeconds / pctCompl * (100 - pctCompl), 0)

                             'Update label in GUI to indicate time remaining to the user 
                             Label_time_remaining.Text = time_remaining.ToString & " sec"

                             'If user has clicked the cancel button, abort the proess
                             If _worker.CancellationPending Then
                                 e.Cancel = True
                                 _worker.ReportProgress(0)
                             End If


                         Next i
                     End Sub)

        'Declare variables to store the final output
        Dim max_value As Double = 0
        Dim max_LC As Int16 = 0

        'Find maximum strain energy and the corresponding load case
        For j = 0 To UBound(strain_energy)
            If strain_energy(j) > max_value Or j = 0 Then
                max_value = strain_energy(j)
                max_LC = start_load_case + j
            End If
        Next

        'Store results in an array and assign array to e.result
        Dim result_arr(1) As String
        result_arr(0) = max_value.ToString
        result_arr(1) = max_LC.ToString
        e.result = result_arr

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'Check if background worker is already busy running the asynchronous operation
        'Will avoid error if user clicks the button "calculate strain energy" multiple times while the program is still running
        If Not _worker.IsBusy Then
            Dim LC_arr(1) As String
            'Store user-input in LC_arr variable
            LC_arr(0) = StartLC.Text
            LC_arr(1) = EndLC.Text
            'Start execution asynchronously in the background
            'Also pass LC_arr as argument
            _worker.RunWorkerAsync(LC_arr)
        End If

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'Check if background worker is running
        'Will cancel the process if the user clicks the cancel button
        If _worker.IsBusy Then
            'Cancel the asynchronous operation if still in progress
            _worker.CancelAsync()
        End If
        End
    End Sub

End Class
