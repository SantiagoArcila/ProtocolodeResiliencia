Public Class Form1

    'los elementos de la conexión 
    Public OXL As Object
    Public OWB As Object
    Public Ruta As String

    'declaramos las propiedades

    Dim NT As Integer, NC As Integer
    Dim T As Integer
    Dim emisor As Integer, receptor As Integer, balor As Single, tasa As Integer, tipot As String
    Dim distribuidor As Integer
    Dim distribution_flag As Boolean, emisor1_flag As Boolean

    'matriz de enlaces, matriz de tasa de enlaces
    Public L(,,) As Single, tL(,,) As Single
    Public PR(,) As Single, B(,) As Single, Baux(,) As Single, V(,) As Single, Acc(,) As Integer, inL(,) As Integer
    Public slo As Single, sli As Single, sumaLT As Single, sumaL As Single
    Public Ld() As Integer, sPR As Single, Daux As Single

    'Variables para la ruta
    Public RT(,) As Integer, RTd(,) As Integer, ruta_flag As Boolean, newruta_flag As Boolean, k As Integer, a As Integer, d As Integer, ret_flag As Boolean, kchange As Boolean

    Dim backwardslink_flag As Boolean, wl(2) As Integer, wlv As Single



    'CONEXIÓN 
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Ruta = Application.StartupPath
        OXL = CreateObject("Excel.Application")
        OWB = OXL.Workbooks.Open(Ruta & "\Resil3op")
        'cambio para ver si hace más rápido la escritura
        OXL.Visible = False
        Me.Activate()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        NC = Val(InputBox("Ingresar el número de cuentas:", , "10"))
        NT = Val(InputBox("Ingresar el número de transacciones:", , "100"))

        'ReDim M(NC * (NT + 1), 2 * NC + 6)
        ReDim L(NC, NC, NT), tL(NC, NC, NT), PR(NC, NT), B(NC, NT), Baux(NC, NT), V(NC, NT), Acc(NC, NT), inL(NC, NT), RT(NC, NT), RTd(NC, NT)

        'Seed aleatoria
        Randomize()

        'T=0 
        For i = 1 To NC
            B(i, 0) = 100 + Math.Round(200 * Rnd())
            V(i, 0) = B(i, 0)
            PR(i, 0) = 5
            inL(i, 0) = 1
        Next i
        emisor1_flag = False

        ProgressBar1.Maximum = NT
        ProgressBar2.Maximum = NT

        For T = 1 To NT

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

                    'TRANSACCIONES DE VALOR

                    '--Selección de Emisor--
                    emisor = Math.Round((NC - 1) * Rnd()) + 1
                    'Sólo una transacción de 1 (PT1)
                    While emisor = 1 And emisor1_flag = True
                        emisor = Math.Round((NC - 1) * Rnd()) + 1
                    End While

                    '--Selección de Receptor--

                    'Cambio: 1 no puede recibir:
                    receptor = Math.Round((NC - 2) * Rnd()) + 2
                    'Original: receptor = Math.Round((NC - 1) * Rnd()) + 1
                    'el receptor no puede ser igual al emisor
                    While emisor = receptor
                        'Cambio: 1 no puede recibir:
                        receptor = Math.Round((NC - 2) * Rnd()) + 2
                        'Original: receptor = Math.Round((NC - 1) * Rnd()) + 1
                    End While


                    '+++Ruta de Transacción++ // Debería ser función. Entradas: emisor, receptor; Salidas RT, RTd
                    '+++++Ruta de Transacción++++
                    newruta_flag = False
                    ruta_flag = False
                    ret_flag = False
                    RT(1, T) = emisor
                    'Búsqueda de la ruta:
                    While ruta_flag = False
                        For k = 2 To NC
                            kchange = False
                            If k = 1 Then
                                GoTo Here
                            End If
                            For i = 1 To NC
                                If ret_flag = True Then
                                    ret_flag = False
                                    i = a
                                End If
                                If L(RT(k - 1, T), i, T) > 0 Or L(i, RT(k - 1, T), T) > 0 Then
                                    If RT(k - 2, T) <> i Then
                                        RT(k, T) = i
                                        'Condición de finalización
                                        If receptor = i Then
                                            For j = k + 1 To NC
                                                RT(j, T) = 0
                                                RTd(j, T) = 0
                                            Next j
                                            ruta_flag = True
                                            k = NC
                                        End If
                                        i = NC
                                        kchange = True
                                    End If
                                End If
                            Next i
                            If kchange = False Then
                                If k = 2 Then
Here:
                                    RT(2, T) = receptor
                                    For i = 3 To NC
                                        RT(i, T) = 0
                                        RTd(i, T) = 0
                                    Next i
                                    newruta_flag = True
                                    ruta_flag = True
                                    k = NC
                                Else
                                    a = RT(k - 1, T) + 1
                                    If a = NC + 1 Then
                                        a = RT(k - 2, T) + 1
                                        k = k - 3
                                        ret_flag = True
                                    Else
                                        k = k - 2
                                        ret_flag = True
                                    End If
                                End If
                            End If
                        Next k
                    End While
                    'Dirección de la ruta:
                    For k = 2 To NC
                        If L(RT(k - 1, T), RT(k, T), T) > 0 Then
                            RTd(k, T) = 0
                        Else
                            If newruta_flag = True Then
                                RTd(k, T) = 0
                            Else
                                RTd(k, T) = 1
                            End If
                        End If
                        If RT(k, T) = 0 Then
                            RTd(k, T) = 0
                        End If
                    Next k

                    'escritura de la ruta
                    For k = 1 To NC
                        OWB.Worksheets(1).cells(T * (NC + 1) + 1, k + 7).value = RT(k, T)
                    Next k

                    'Enlace inverso más débil de la ruta //También función auxiliar
                    'enlace inverso más débil de la ruta
                    wlv = 0
                    wl(1) = 0
                    wl(2) = 0
                    backwardslink_flag = False
                    For i = 2 To NC
                        If RTd(i, T) = 1 Then
                            If backwardslink_flag = False Then
                                backwardslink_flag = True
                                wl(1) = RT(i - 1, T)
                                wl(2) = RT(i, T)
                                wlv = L(wl(2), wl(1), T)
                            Else
                                If L(RT(i, T), RT(i - 1, T), T) < wlv Then
                                    wl(1) = RT(i - 1, T)
                                    wl(2) = RT(i, T)
                                    wlv = L(wl(2), wl(1), T)
                                End If
                            End If
                        End If
                    Next i

                    'Selección de Valor y Tasa
                    balor = Math.Round(((B(emisor, T) * 0.3) - 1) * Rnd()) + 1
                    tasa = Math.Round(4 * Rnd()) + 1

                    B(emisor, T) = B(emisor, T) - (balor + balor * tasa / 100)
                    B(receptor, T) = B(receptor, T) + balor
                    Baux(receptor, T) = Baux(receptor, T) + balor * tasa / 100
                    Acc(receptor, T) = Acc(receptor, T) + 1

                    If V(emisor, T) < V(receptor, T) Then
                        'BVT
                        'Declaración de Transacción
                        OWB.Worksheets(1).cells(T * (NC + 1) + 1, 1).value = "T" & T & "-" & balor & "E" & emisor & "R" & receptor & "-" & tasa & "%-" & "BVT"
                        balor = balor + (2 * balor * tasa) / 100
                    Else
                        'FVT
                        'Declaración de Transacción
                        OWB.Worksheets(1).cells(T * (NC + 1) + 1, 1).value = "T" & T & "-" & balor & "E" & emisor & "R" & receptor & "-" & tasa & "%-" & "FVT"
                    End If
                    'Modificación del los enlaces:
                    If backwardslink_flag = False Then
                        'se actualiza el tl del primer enlace
                        tL(emisor, RT(2, T), T) = (balor * tasa + L(emisor, RT(2, T), T)) / (balor + L(emisor, RT(2, T), T))
                        'se refuerzan todos los enlaces de la ruta
                        For i = 2 To NC
                            If RT(i, T) = 0 Then
                                i = NC
                            Else
                                L(RT(i - 1, T), RT(i, T), T) = L(RT(i - 1, T), RT(i, T), T) + balor
                            End If
                        Next i
                    Else
                        If balor < wlv Then
                            'se actualiza el tL del primer enlace
                            If tL(emisor, RT(2, T), T) > 0 Then
                                tL(emisor, RT(2, T), T) = (balor * tasa + L(emisor, RT(2, T), T)) / (balor + L(emisor, RT(2, T), T))
                            Else
                                tL(RT(2, T), emisor, T) = (balor * tasa + L(RT(2, T), emisor, T)) / (balor + L(RT(2, T), emisor, T))
                            End If
                            'se refuerzan los enlaces forward de la ruta, 
                            'se debilitan los enlaces de la ruta 
                            For i = 2 To NC
                                If RT(i, T) = 0 Then
                                    i = NC
                                Else
                                    If RTd(i, T) = 0 Then 'Refuerzo
                                        L(RT(i - 1, T), RT(i, T), T) = L(RT(i - 1, T), RT(i, T), T) + balor
                                    Else 'Debilitamiento
                                        L(RT(i, T), RT(i - 1, T), T) = L(RT(i, T), RT(i - 1, T), T) - balor
                                    End If
                                End If
                            Next i
                        Else
                            If RT(2, T) <> wl(2) Then
                                If tL(emisor, RT(2, T), T) > 0 Then
                                    tL(emisor, RT(2, T), T) = (balor * tasa + L(emisor, RT(2, T), T)) / (balor + L(emisor, RT(2, T), T))
                                Else
                                    tL(RT(2, T), emisor, T) = (balor * tasa + L(RT(2, T), emisor, T)) / (balor + L(RT(2, T), emisor, T))
                                End If
                            End If
                            'se refuerzan los enlaces forward de la ruta por el valor del wl, 
                            'se debilitan los enlaces backwards por el valor del wl, 
                            'se elimina el wl *automático durante el ciclo*, 
                            'se crea el enlace directo emisor receptor por el restante valor-wl
                            For i = 2 To NC
                                If RT(i, T) = 0 Then
                                    i = NC
                                Else
                                    If RTd(i, T) = 0 Then 'Refuerzo
                                        L(RT(i - 1, T), RT(i, T), T) = L(RT(i - 1, T), RT(i, T), T) + wlv
                                    Else 'Debilitamiento
                                        L(RT(i, T), RT(i - 1, T), T) = L(RT(i, T), RT(i - 1, T), T) - wlv
                                    End If
                                End If
                            Next i
                            L(emisor, receptor, T) = balor - wlv
                            tL(emisor, receptor, T) = PR(emisor, T)
                            'rematada del wl
                            tL(wl(2), wl(1), T) = 0
                        End If
                    End If

                    '****Sólo una transacción de 1 (PT2)
                    If emisor = 1 Then
                        emisor1_flag = True
                    End If
                Else

                    'TRANSACCIONES DE DISTRIBUCIÓN
                    'Falta cambiar a la versión del paper


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
                    If sPR = 0 Then
                        sPR = PR(distribuidor, T)
                    End If
                    For i = 1 To inL(distribuidor, T)
                        If Ld(i) <> 0 Then
                            If Ld(i) = distribuidor Then
                                B(distribuidor, T) = B(distribuidor, T) + Baux(distribuidor, T) * PR(distribuidor, T) / sPR
                            Else
                                'transferencia entre auxiliares
                                Baux(Ld(i), T) = Baux(Ld(i), T) + Baux(distribuidor, T) * PR(Ld(i), T) / sPR
                                Acc(Ld(i), T) = Acc(Ld(i), T) + 1
                                If L(Ld(i), distribuidor, T) > Baux(distribuidor, T) * PR(Ld(i), T) / sPR Then
                                    'Debilitamiento del enlace
                                    L(Ld(i), distribuidor, T) = L(Ld(i), distribuidor, T) - Baux(distribuidor, T) * PR(Ld(i), T) / sPR
                                Else
                                    'Cambio de dirección del enlace
                                    L(distribuidor, Ld(i), T) = (Baux(distribuidor, T) * PR(Ld(i), T) / sPR) - L(Ld(i), distribuidor, T)
                                    L(Ld(i), distribuidor, T) = 0
                                    tL(Ld(i), distribuidor, T) = 0
                                    tL(distribuidor, Ld(i), T) = PR(distribuidor, T)
                                End If
                            End If
                        End If
                    Next i
                    Baux(distribuidor, T) = 0
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

            ProgressBar1.Value() = T
            Label3.Text = Math.Round((T / NT) * 100) & "%"
        Next T

        'Escritura de datos de transacción
        For T = 0 To NT
            For i = 1 To NC
                OWB.Worksheets(1).cells(T * (NC + 1) + i + 1, 2).value = B(i, T)
                OWB.Worksheets(1).cells(T * (NC + 1) + i + 1, 3).value = Baux(i, T)
                OWB.Worksheets(1).cells(T * (NC + 1) + i + 1, 4).value = V(i, T)
                OWB.Worksheets(1).cells(T * (NC + 1) + i + 1, 5).value = Acc(i, T)
                OWB.Worksheets(1).cells(T * (NC + 1) + i + 1, 6).value = inL(i, T)
                OWB.Worksheets(1).cells(T * (NC + 1) + i + 1, 7).value = PR(i, T)
                OWB.Worksheets(2).cells(T + 1, 1).value = B(1, T)
                OWB.Worksheets(2).cells(T + 1, 2).value = V(1, T)
                OWB.Worksheets(2).cells(T + 1, 4).value = B(7, T)
                OWB.Worksheets(2).cells(T + 1, 5).value = V(7, T)
                For j = 1 To NC
                    OWB.Worksheets(1).cells(T * (NC + 1) + i + 1, j + 7).value = L(i, j, T)
                    OWB.Worksheets(1).cells(T * (NC + 1) + i + 1, j + 7 + NC).value = tL(i, j, T)
                Next j
            Next i
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
            OWB.Save()
            OWB.CLose(False)
            OWB = Nothing
            OXL.Quit()
            OXL = Nothing
        Catch ex As Exception
        End Try
        Application.Exit()
    End Sub
End Class
