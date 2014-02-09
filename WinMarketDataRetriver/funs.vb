Imports System.Xml
Imports System.Xml.Linq
Imports System.Text
Imports System.Text.Encoding
Imports System.Net.Mime.MediaTypeNames
Imports System.Net
Imports System.Data.OleDb
Imports System.Data.Sql
Imports System.Data.SqlClient

Public Structure DataSaved
    Public itemID As Integer
    Public itemISK As String
    Public itemName As String
    Public itemPriceType As Boolean
    Public itemArea As String
End Structure

Module funs
    Public sz, xz, ss, syst, systname, spst, szname, xzname, ssname, spstname As String
    Public desc As String
    Private Function sdlz()
        Return 1
    End Function
    Private Function sdlz2()
        Return 1
    End Function
    Private Function sdl3z()
        Return 1
    End Function
    Public Function GetValue(ByVal valtype As String, ByVal selltype As Integer, Optional ByVal szid As String = "-1", Optional ByVal spstid As String = "-1") As String
        Dim doc As New XmlDocument
        Dim re As XmlNodeReader
        Dim namenode As String
        Dim nowcount As Integer = 0
        Dim addr, ntp As String
        addr = GenerateAddr(valtype, sz)
        Try
            doc.Load(addr)
            re = New XmlNodeReader(doc)
        Catch ex As Exception
            MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try

        ntp = ""
        namenode = ""
        Select Case selltype
            Case 1
                ntp = "min"
            Case 2
                ntp = "max"
        End Select
        Try
            While re.Read()
                Select Case re.NodeType
                    Case XmlNodeType.Element
                        namenode = re.Name
                        If namenode = "buy" Then nowcount = 2
                        If namenode = "sell" Then nowcount = 1
                        If namenode = "all" Then nowcount = 3
                        If namenode = "type" Then addr = re.NodeType
                    Case XmlNodeType.Text
                        If namenode = ntp And nowcount = selltype Then
                            Return re.Value
                            Exit Try
                        Else
                            If namenode = ntp And nowcount <> selltype Then
                                nowcount = 0
                            End If
                        End If
                End Select

            End While
        Catch ex As Exception
            Return "-1"
            MsgBox(ex.Message)
        End Try

    End Function

    Private Function GenerateAddr(ByVal valtype As String, Optional ByVal starzone As String = "-1", Optional ByVal spstid As String = "-1") As String
        Dim netaddr As String = "http://www.ceve-market.org/api/marketstat?typeid="
        netaddr = netaddr & valtype
        If sz <> "" Then
            netaddr = netaddr & "&regionlimit=" & starzone
        End If
        If syst <> "" Then
            netaddr = netaddr & "&usesystem=" & syst
        End If
        Return netaddr
    End Function

    Public Function checkvalue(ByVal name As String, ByVal type As Integer, Optional ByVal szname As String = "noszname") As String

        Dim conn As OleDbConnection
        Dim cmdEx As OleDbCommand
        Dim line1 As String = ""
        Dim line2 As String = ""

        conn = New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=eve_db.mdb")
        Dim cmdA As String = "select typeID from 物品列表 where 物品名称='" & name & "'"
        Dim cmdB As String = "select 星域ID from 星域列表 where 星域名称='" & name & "'"
        Dim cmdC As String = "select 星座ID from 星座列表 where 星座名称='" & name & "'"
        Dim cmdD As String = "select 星域名称 from 星系列表 where 星系名称='" & name & "'"
        Dim cmdE As String = "select 星系ID from 星系列表 where 星系名称='" & name & "'"
        Dim cmdF As String = "select 物品名称 from 物品列表 where typeID='" & name & "'"
        conn.Open()
        Select Case type
            Case 1
                cmdEx = New OleDbCommand(cmdA, conn)
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
        Return line1

    End Function





End Module

