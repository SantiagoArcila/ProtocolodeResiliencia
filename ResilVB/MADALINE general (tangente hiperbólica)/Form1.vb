Public Class Form1

    'los elementos de la conexión 
    Public OXL As Object
    Public OWB As Object
    Public Ruta As String

    'declaramos las propiedades

    Dim NT As Integer, NC As Integer
    Dim T As Integer
    Dim emisor As Integer, receptor As Integer, balor As Integer, tasa As Integer, tipot As String
    Dim distribuidor As Integer
    Dim distribution_flag As Boolean

    Public M(,) As Single

    'matriz de enlaces, matriz de tasa de enlaces
    Public L(,,) As Single, tL(,,) As Single
    Public PR(,) As Single, B(,) As Single, Baux(,) As Single, V(,) As Single, Acc(,) As Integer, inL(,) As Integer
    Public slo As Single, sli As Single, sumaLT As Single, sumaL As Single
    Public Ld() As Integer, sPR As Single, Daux As Single


    'CONEXIÓN 
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Ruta = Application.StartupPath
        OXL = CreateObject("Excel.Application")
        OWB = OXL.Workbooks.Open(Ruta & "\Resil1")
        OXL.Visible = True
        Me.Activate()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        NC = Val(InputBox("Ingresar el número de cuentas:", , "5"))
        NT = Val(InputBox("Ingresar el número de transacciones:", , "10"))

        'ReDim M(NC * (NT + 1), 2 * NC + 6)
        ReDim L(NC, NC, NT), tL(NC, NC, NT), PR(NC, NT), B(NC, NT), Baux(NC, NT), V(NC, NT), Acc(NC, NT), inL(NC, NT)

        'Seed aleatoria
        Randomize()

        'T=0 
        For i = 1 To NC
            B(i, 0) = 100 + Math.Round(200 * Rnd())
            V(i, 0) = B(i, 0)
            PR(i, 0) = 5
            inL(i, 0) = 1
        Next i

        ProgressBar2.Maximum = NT
        For T = 0 To NT

            If T > 0 Then

                'Traspaso de datos
                For i = 1 To NC
                    B(i, T) = B(i, T - 1)
                    Baux(i, T) = Baux(i, T - 1)
                    V(i, T) = V(i, T - 1)
                    PR(i, T) = PR(i, T - 1)
                    Acc(i, T) = Acc(i, T - 1)
                    inL(i, T) = inL(i, T - 1)
                    For j = 1 To NC
                        L(i, j, T) = L(i, j, T - 1)
                        tL(i, j, T) = tL(i, j, T - 1)
                    Next j
                Next i

                If distribution_flag = False Then

                    'Transacciones de Valor

                    emisor = Math.Round((NC - 1) * Rnd()) + 1
                    receptor = Math.Round((NC - 1) * Rnd()) + 1
                    'el receptor no puede ser igual al emisor
                    While emisor = receptor
                        receptor = Math.Round((NC - 1) * Rnd()) + 1
                    End While
                    balor = Math.Round(((B(emisor, T) * 0.3) - 1) * Rnd()) + 1
                    tasa = Math.Round(4 * Rnd()) + 1

                    If V(emisor, T) < V(receptor, T) Then
                        tipot = "BVT"
                        'Actualización del enlace
                        If L(receptor, emisor, T) = 0 Then 'Refuerzo del enlace
                            'actualización de la tasa del enlace
                            tL(emisor, receptor, T) = ((balor + 2 * balor * tasa / 100) * tasa + L(emisor, receptor, T) * tL(emisor, receptor, T)) / ((balor + 2 * balor * tasa / 100) + L(emisor, receptor, T))
                            L(emisor, receptor, T) = (balor + 2 * balor * tasa / 100) + L(emisor, receptor, T)
                        Else
                            If balor + 2 * balor * tasa / 100 > L(receptor, emisor, T) Then 'Cambio de la dirección del enlace
                                L(emisor, receptor, T) = (balor + 2 * balor * tasa / 100) - L(receptor, emisor, T)
                                tL(emisor, receptor, T) = tasa
                                L(receptor, emisor, T) = 0
                                tL(receptor, emisor, T) = 0
                            Else 'Debilitamiento del enlace
                                L(receptor, emisor, T) = L(receptor, emisor, T) - (balor + 2 * balor * tasa / 100)
                            End If
                        End If
                    Else
                        tipot = "FVT"
                        'Actualización del enlace
                        If L(receptor, emisor, T) = 0 Then 'Refuerzo del enlace
                            'Actualización de la tasa del enlace
                            tL(emisor, receptor, T) = (balor * tasa + L(emisor, receptor, T) * tL(emisor, receptor, T)) / (balor + L(emisor, receptor, T))
                            L(emisor, receptor, T) = balor + L(emisor, receptor, T)
                        Else
                            If balor > L(receptor, emisor, T) Then 'Cambio de la dirección del enlace
                                L(emisor, receptor, T) = balor - L(receptor, emisor, T)
                                tL(emisor, receptor, T) = tasa
                                L(receptor, emisor, T) = 0
                                tL(receptor, emisor, T) = 0
                            Else 'Debilitamiento del enlace
                                L(receptor, emisor, T) = L(receptor, emisor, T) - balor
                            End If
                        End If
                    End If
                    B(emisor, T) = B(emisor, T) - (balor + balor * tasa / 100)
                    B(receptor, T) = B(receptor, T) + balor
                    Baux(receptor, T) = Baux(receptor, T) + balor * tasa / 100
                    Acc(receptor, T) = Acc(receptor, T) + 1
                    'Declaración de Transacción
                    OWB.Worksheets(1).cells(T * (NC + 1) + 1, 1).value = balor & "E" & emisor & "R" & receptor & "-" & tasa & "%-" & tipot
                Else

                    'Transacciones de Distribución

                    distribution_flag = False
                    'Detección del distribuidor
                    While distribution_flag = False
                        For i = 1 To NC
                            If inL(i, T) <= Acc(i, T) Then
                                distribuidor = i
                                distribution_flag = True
                            End If
                        Next i
                    End While
                    distribution_flag = False

                    'lista de distribución
                    ReDim Ld(inL(distribuidor, T))
                    Dim d As Integer
                    d = 2
                    For i = 1 To NC
                        If L(i, distribuidor, T) > 0 Then
                            Ld(d) = i
                            d = d + 1
                        End If
                    Next i
                    If B(distribuidor, T) < V(distribuidor, T) Then
                        Ld(1) = distribuidor
                    Else
                        Ld(1) = 0
                    End If

                    'Distribuciones
                    sPR = 0
                    Daux = 0
                    For i = 1 To inL(distribuidor, T)
                        sPR = sPR + PR(Ld(i), T)
                    Next i
                    For i = 1 To inL(distribuidor, T)
                        If Ld(i) = distribuidor Then
                            B(distribuidor, T) = B(distribuidor, T) + Baux(distribuidor, T) * PR(distribuidor, T) / sPR
                        Else
                            'transferencia entre auxiliares
                            If L(Ld(i), distribuidor, T) > Baux(distribuidor, T) * PR(Ld(i), T) / sPR Then
                                Baux(Ld(i), T) = Baux(Ld(i), T) + Baux(distribuidor, T) * PR(Ld(i), T) / sPR
                                Acc(Ld(i), T) = Acc(Ld(i), T) + 1
                                'Debilitamiento del enlace
                                L(Ld(i), distribuidor, T) = L(Ld(i), distribuidor, T) - Baux(distribuidor, T) * PR(Ld(i), T) / sPR
                            Else
                                Baux(Ld(i), T) = Baux(Ld(i), T) + L(Ld(i), distribuidor, T)
                                Daux = Daux + Baux(distribuidor, T) * PR(Ld(i), T) / sPR - L(Ld(i), distribuidor, T)
                                L(Ld(i), distribuidor, T) = 0
                            End If
                        End If
                    Next i
                    Baux(distribuidor, T) = Daux
                    Acc(distribuidor, T) = 0

                    OWB.Worksheets(1).cells(T * (NC + 1) + 1 + distribuidor, 1).value = "D"
                    For i = 1 To inL(distribuidor, T)
                        OWB.Worksheets(1).cells(T * (NC + 1) + 1, i + 1).value = Ld(i)
                    Next i

                End If

                'Condición de distribución
                For i = 1 To NC
                    inL(i, T) = 1
                    For j = 1 To NC
                        If L(j, i, T) > 0 Then
                            inL(i, T) = inL(i, T) + 1
                        End If
                    Next j
                    If inL(i, T) <= Acc(i, T) Then
                        distribution_flag = True
                    End If
                Next i

                'Cálculo del PR
                For i = 1 To NC
                    sumaLT = 0
                    sumaL = 0
                    For j = 1 To NC
                        sumaLT = sumaLT + L(i, j, T) * tL(i, j, T)
                        sumaL = sumaL + L(i, j, T)
                    Next j
                    If sumaL > 0 Then
                        PR(i, T) = sumaLT / sumaL
                    End If
                Next i

                'cálculo de V
                For i = 1 To NC
                        slo = 0
                        sli = 0
                        For j = 1 To NC
                            slo = slo + L(i, j, T)
                            sli = sli + L(j, i, T)
                        Next j
                        V(i, T) = B(i, T) + Baux(i, T) + slo - sli
                    Next i

                End If

                'Escritura de datos de transacción
                For i = 1 To NC
                OWB.Worksheets(1).cells(T * (NC + 1) + i + 1, 2).value = B(i, T)
                OWB.Worksheets(1).cells(T * (NC + 1) + i + 1, 3).value = Baux(i, T)
                OWB.Worksheets(1).cells(T * (NC + 1) + i + 1, 4).value = V(i, T)
                OWB.Worksheets(1).cells(T * (NC + 1) + i + 1, 5).value = Acc(i, T)
                OWB.Worksheets(1).cells(T * (NC + 1) + i + 1, 6).value = inL(i, T)
                OWB.Worksheets(1).cells(T * (NC + 1) + i + 1, 7).value = PR(i, T)
                For j = 1 To NC
                    OWB.Worksheets(1).cells(T * (NC + 1) + i + 1, j + 7).value = L(i, j, T)
                    OWB.Worksheets(1).cells(T * (NC + 1) + i + 1, j + 7 + NC).value = tL(i, j, T)
                Next
            Next

            ProgressBar2.Value() = T
            Label4.Text = Math.Round((T / NT) * 100) & "%"
        Next T







        'For i = 1 To NT


        'Next i

        'r = Val(InputBox("Resolución:", , "5"))
        'p = Val(InputBox("Porcentaje de datos de validación:", , "30"))
        'NE = Val(InputBox("Ingresar el número de entradas:", , "14"))
        'NS = Val(InputBox("Ingresar el número de salidas:", , "5"))

        'NIT = Val(InputBox("Número de Iteraciones:", , "1000"))
        'alfa = Val(InputBox("Factor de Aprendizaje:", , "0.03"))

        'Se redimensiona el tamaño de las matrices
        'ReDim X(ND, NE), W(NO, NE), S(NO), yr(ND, NS), yrd(ND, NS), yra(ND, NS), W1(NO, NE), Sa(NO), Sd(NO)
        'ReDim C(NS, NO), yd(ND, NS), er(ND, NS), C1(NS, NO)

        'ENTRENAMIENTO

        ''Lectura de los datos
        'ProgressBar1.Maximum = ND
        'For d = 1 To Math.Round(ND * (1 - (p / 100))) Step r
        '    X(d, 0) = 1
        'For i = 1 To NE
        '    X(d, i) = Val(OWB.Worksheets(1).cells(d, i).value)
        'Next i
        '    For i = 1 To NS
        '        yd(d, i) = Val(OWB.Worksheets(2).cells(d, i).value)
        '    Next i
        '    ProgressBar1.Value() = d
        '    Label3.Text = Math.Round((d / (ND * 0.7)) * 100) & "%"
        'Next d

        ''Se generan los pesos de las conexiones
        'Randomize()
        'For j = 1 To NO
        '    For i = 0 To NE
        '        W(j, i) = (-1 + 2 * Rnd())
        '    Next i
        'Next j
        'For k = 1 To NS
        '    For j = 0 To NO
        '        C(k, j) = (-1 + 2 * Rnd())
        '    Next j
        'Next k

        'ProgressBar2.Maximum = NIT
        'For it = 1 To NIT
        '    ERS = 0
        '    For d = 1 To Math.Round(ND * (1 - (p / 100))) Step r

        '        'Feedforward
        '        S(0) = 1
        '        For j = 1 To NO
        '            Sa(j) = 0
        '            For i = 0 To NE
        '                Sa(j) = Sa(j) + X(d, i) * W(j, i)
        '            Next i
        '            S(j) = (Math.Tanh(2 * Sa(j)) + 1) / 2
        '            Sd(j) = 1 - Math.Pow(Math.Tanh(2 * Sa(j)), 2)
        '        Next j
        '        For k = 1 To NS
        '            yra(d, k) = 0
        '            For j = 0 To NO
        '                yra(d, k) = yra(d, k) + S(j) * C(k, j)
        '            Next j
        '            yr(d, k) = (Math.Tanh(2 * yra(d, k)) + 1) / 2
        '            yrd(d, k) = 1 - Math.Pow(Math.Tanh(2 * yra(d, k)), 2)
        '            er(d, k) = yd(d, k) - yr(d, k)
        '            ERS = ERS + (er(d, k) ^ 2)
        '        Next k

        '        'Backpropagation
        '        'Actualización de C
        '        For k = 1 To NS
        '            For j = 0 To NO
        '                C1(k, j) = C(k, j)
        '                C(k, j) = C(k, j) + alfa * er(d, k) * S(j) * yrd(d, k)
        '            Next j
        '        Next k
        '        'Actualización de W
        '        For j = 1 To NO
        '            For i = 0 To NE
        '                'W1(j, i) = W(j, i)
        '                sigma = 0
        '                For k = 1 To NS
        '                    sigma = sigma + er(d, k) * C1(k, j) * X(d, i) * yrd(d, k) * Sd(j)
        '                Next k
        '                W(j, i) = W(j, i) + alfa * sigma
        '            Next i
        '        Next j
        '    Next d
        '    ProgressBar2.Value() = it
        '    Label4.Text = Math.Round((it / NIT) * 100) & "%"
        '    'Escritura del Error Cuadrático
        '    OWB.Worksheets(3).cells(it, 2 * NS + 1).value = ERS
        'Next it

        ''Escritura de Resultados
        'For d = 1 To Math.Round(ND * (1 - (p / 100))) Step r
        '    For k = 1 To NS
        '        OWB.Worksheets(3).cells(d, k).value = yr(d, k)
        '        OWB.WORKSHEETS(3).CELLS(d, k + NS).value = er(d, k)
        '    Next k
        'Next d
        ''OWB.Application.MaxChange = 0.001

        ''VALIDACIÓN

        ''Lectura de datos de Validación
        'ProgressBar1.Maximum = ND
        'For d = Math.Round(ND * (1 - (p / 100))) + 1 To ND Step r
        '    X(d, 0) = 1
        '    For i = 1 To NE
        '        X(d, i) = Val(OWB.Worksheets(1).cells(d, i).value)
        '    Next i
        '    For i = 1 To NS
        '        yd(d, i) = Val(OWB.Worksheets(2).cells(d, i).value)
        '    Next i
        '    ProgressBar1.Value() = d
        '    Label3.Text = Math.Round((d / ND) * 100) & "%"
        'Next d

        'ERS = 0
        'For d = Math.Round(ND * (1 - (p / 100))) + 1 To ND Step r
        '    'Feedforward
        '    S(0) = 1
        '    For j = 1 To NO
        '        Sa(j) = 0
        '        For i = 0 To NE
        '            Sa(j) = Sa(j) + X(d, i) * W(j, i)
        '        Next i
        '        S(j) = Math.Tanh(Sa(j))
        '    Next j
        '    For k = 1 To NS
        '        yra(d, k) = 0
        '        For j = 0 To NO
        '            yra(d, k) = yra(d, k) + S(j) * C(k, j)
        '        Next j
        '        yr(d, k) = Math.Tanh(yra(d, k))
        '        er(d, k) = yd(d, k) - yr(d, k)
        '        ERS = ERS + (er(d, k) ^ 2)
        '    Next k
        'Next d

        ''Escritura de Resultados de validación
        'For d = Math.Round(ND * (1 - (p / 100))) + 1 To ND Step r
        '    For k = 1 To NS
        '        OWB.Worksheets(3).cells(d, k).value = yr(d, k)
        '        OWB.WORKSHEETS(3).CELLS(d, k + NS).value = er(d, k)
        '    Next k
        'Next d
        'OWB.WORKSHEETS(3).CELLS(ND, 2 * NS + 1).value = ERS

    End Sub
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Try
            'OWB.Save()
            OWB.CLose(False)
            OWB = Nothing
            OXL.Quit()
            OXL = Nothing
        Catch ex As Exception
        End Try
        Application.Exit()
    End Sub
End Class
