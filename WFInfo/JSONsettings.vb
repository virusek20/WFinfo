﻿Imports System.IO
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class JSONsettings
    Private job As JObject
    Private ReadOnly settings_path As String = Path.Combine(appData, "WFInfo\settings.json")
    Public delay As Integer = 1000

    Public Sub New()
        If Directory.Exists(appData) AndAlso File.Exists(settings_path) Then
            job = JsonConvert.DeserializeObject(Of JObject)(File.ReadAllText(settings_path))
            delay = job("delay").ToObject(Of Integer)
        Else
            job = New JObject()
            job("delay") = delay
            SaveFile()
        End If
    End Sub

    Public Sub SaveFile()
        If Not Directory.Exists(Path.Combine(appData, "WFInfo")) Then
            Directory.CreateDirectory(Path.Combine(appData, "WFInfo"))
        End If
        File.Create(Path.Combine(appData, "WFInfo\settings.json"))
        File.WriteAllText(settings_path, JsonConvert.SerializeObject(job, Formatting.Indented))
    End Sub
End Class