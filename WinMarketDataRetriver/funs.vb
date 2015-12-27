Imports System.Xml
Imports System.Xml.Linq
Imports System.Text
Imports System.Text.Encoding
Imports System.Net.Mime.MediaTypeNames
Imports System.Net
Imports System.Data.OleDb
Imports System.Data.Sql
Imports System.Data.SqlClient

#Region "Universal Data Structures"
Public Structure DataSaved
    Public QueryTime As String
    Public itemID As Integer
    Public itemISK As String
    Public itemName As String
    Public itemPriceType As Boolean
    Public QueryRegion As RegionInfo

    ''' <summary>
    ''' save data structor
    ''' </summary>
    ''' <param name="TimeStr">Time to Execute Query</param>
    ''' <param name="id">Type ID</param>
    ''' <param name="isk">ISK Get</param>
    ''' <param name="name">Item Name</param>
    ''' <param name="PriceType">Price Type, False = Buy, True = Sell</param>
    ''' <param name="RGInfo">Region of the Query</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal TimeStr As String, ByVal id As Integer, isk As String, name As String, PriceType As Boolean, RGInfo As RegionInfo)
        QueryTime = TimeStr
        itemID = id
        itemISK = isk
        itemName = name
        itemPriceType = PriceType
        QueryRegion = RGInfo
    End Sub

End Structure

Public Structure RegionInfo
    Public SystemID As String
    Public SystemName As String
    Public RegionID As String
    Public RegionName As String

    ''' <summary>
    ''' Init new Region info structure
    ''' </summary>
    ''' <param name="a">SystemID</param>
    ''' <param name="b">SystemName</param>
    ''' <param name="c">RegionID</param>
    ''' <param name="d">RegionName</param>
    ''' <remarks></remarks>
    Public Sub New(a As String, b As String, c As String, d As String)
        SystemID = a
        SystemName = b
        RegionID = c
        RegionName = d
    End Sub
End Structure

#End Region
Module funs

#Region "Universal Variables and declearations"
    Public sz, xz, ss, syst, systname, spst, szname, xzname, ssname, spstname As String
    Public desc As String
    Public FZRetstr As String = ""
    Public conn As OleDbConnection = New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=eve_db.mdb")
    Public SystemList As New Dictionary(Of String, RegionInfo)
    Public SelectedRegion As RegionInfo = New RegionInfo("", "", "", "")
#End Region

#Region "Universal Functions"
    ''' <summary>
    ''' Get the exact value of item.
    ''' </summary>
    ''' <param name="valtype">the Type ID</param>
    ''' <param name="selltype">Buy Or Sell</param>
    ''' <param name="RegionID">Region ID</param>
    ''' <param name="SystemID">System ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetValue(ByVal valtype As String, _
                             ByVal selltype As Integer, _
                             Optional ByVal RegionID As String = "-1", _
                             Optional ByVal SystemID As String = "-1") As String
        Dim doc As New XmlDocument
        Dim re As XmlNodeReader
        Dim nowcount As Integer = 0
        Dim addr As String
        addr = GenerateAddr(valtype, RegionID, SystemID)
        Try
            doc.Load(addr)
            re = New XmlNodeReader(doc)
        Catch ex As Exception
            MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
        Try
            Select Case selltype
                Case 1
                    Return doc.SelectSingleNode("/evec_api/marketstat/type/sell/min").InnerText
                Case 2
                    Return doc.SelectSingleNode("/evec_api/marketstat/type/buy/max").InnerText
                Case Else
                    Throw New Exception()
            End Select
        Catch ex As Exception
            Return "-1"
            MsgBox(ex.Message)
        End Try
    End Function

    Private Function GenerateAddr(ByVal valtype As String, Optional ByVal RegionID As String = "-1", Optional ByVal SystemID As String = "-1") As String
        Dim netaddr As String = "http://www.ceve-market.org/api/marketstat?typeid="
        netaddr = netaddr & valtype
        If RegionID <> "-1" And RegionID <> "" Then
            netaddr = netaddr & "&regionlimit=" & RegionID
        End If
        If SystemID <> "-1" And SystemID <> "" Then
            netaddr = netaddr & "&usesystem=" & SystemID
        End If
        Return netaddr
    End Function

    Public Function checkvalue(ByVal name As String, ByVal type As Integer, Optional ByVal szname As String = "noszname") As String
        If type = 5 Then Throw New Exception()

        Try
            Dim cmdEx As OleDbCommand
            Dim line1 As String = ""
            Dim line2 As String = ""
            name = name.Replace("'", "''")
            Dim cmdA As String = "select typeID from invItems where itemName='" & name & "'"
            'Dim cmdA As String = "select typeID from invItems where itemName='@itName'"
            'Dim cmdAParam As OleDbParameter = New OleDbParameter("@itName", OleDbType.BSTR)
            'cmdAParam.Value = name
            Dim cmdB As String = "select RegionID from mapRegionID where RegionName='" & name & "'"
            Dim cmdC As String = "select constellationID from mapConstellationID where ConstellationName='" & name & "'"
            Dim cmdD As String = "select RegionName from mapSystemID where SystemName='" & name & "'"
            Dim cmdE As String = "select SystemID from mapSystemID where SystemName='" & name & "'"
            Dim cmdF As String = "select itemName from invItems where typeID='" & name & "'"
            conn.Open()
            Select Case type
                Case 1
                    cmdEx = New OleDbCommand(cmdA, conn)
                    'cmdEx.Parameters.Add(cmdAParam)
                    'cmdEx.Prepare()
                    line1 = cmdEx.ExecuteScalar()
                Case 2
                    cmdEx = New OleDbCommand(cmdB, conn)
                    line1 = cmdEx.ExecuteScalar()
                Case 3
                    cmdEx = New OleDbCommand(cmdD, conn)
                    line1 = cmdEx.ExecuteScalar
                Case 5
                    cmdEx = New OleDbCommand(cmdE, conn)
                    line1 = cmdEx.ExecuteScalar()
                    cmdEx = New OleDbCommand(cmdD, conn)
                    line2 = cmdEx.ExecuteScalar()
                    If line2 <> szname And szname <> "noszname" Then
                        line1 = -1
                    End If
                Case 6
                    cmdEx = New OleDbCommand(cmdF, conn)
                    line1 = cmdEx.ExecuteScalar()
            End Select
            If line1 = "" Then
                line1 = "-1"
            End If
            conn.Close()
            Return line1
        Catch ex As Exception
            conn.Close()
            MessageBox.Show("SQL查询错误！请重试！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return "-1"
        End Try
        conn.Close()
    End Function
#End Region

End Module

