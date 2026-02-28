import math

def sprawdz_sasiada(wezel, siatka):
    lista_sasiadow=[]
    wspolrzedne_d_g_l_p = [(1, 0), (-1, 0), (0, -1), (0, 1)] # dol gora lewo prawo
    wartosci_siatki = []

    for i, sasiad in enumerate(wspolrzedne_d_g_l_p):
        if 0 <= sasiad[0] + wezel[0] <= 19 and 0 <= sasiad[1] + wezel[1] <= 19:
            print("i_sasiad:", i)
            print("sasiad:", sasiad)
        else:
            continue
        if siatka[sasiad[0] + wezel[0] ][sasiad[1] + wezel[1]  ] != 5:
            print("sasiad[0] + wezel[0] :",sasiad[0] + wezel[0] )
            print("sasiad[1] + wezel[1] ", sasiad[1] + wezel[1] )

            lista_sasiadow.append(
                (sasiad[0] + wezel[0],
                 sasiad[1] + wezel[1])
            )
        else:
            continue

    return lista_sasiadow


def sciezka(lista_wezel, otwarta_lista, siatka):
    wezel = otwarta_lista.popitem()
    scieszka_koncowa = []
    scieszka_koncowa.append(lista_wezel[0])

    while otwarta_lista:
        siatka[wezel[0][0]][wezel[0][1]] = 3
        scieszka_koncowa.append(wezel[0])
        wezel = otwarta_lista.popitem()[1]





    return scieszka_koncowa, siatka

def znajdz_najmniejsze_f(otwarta_lista_L):
    min  = None
    for otwarta_lista in otwarta_lista_L:
        #print("otwarta lista_first w while", otwarta_lista)
        #print(min)

        #print("wspolrzedne", wspolrzedne)

        #print("otwarta_lista_L.pop_przed()", otwarta_lista_L)
        if min <= otwarta_lista[1][1] or min is None:
            #otwarta_lista_L.pop(0)
            #print("otwarta_lista_L.pop_po()", otwarta_lista_L)

            continue

        wspolrzedne = otwarta_lista[0]
        min = otwarta_lista[1][1]
        #otwarta_lista_L.pop(0)
        #print("otwarta_lista_L.pop_po()", otwarta_lista_L)
    wspol_i_f = [wspolrzedne, min]
    #print("xd")
    #print("najmniejsze_wspol_i_f",wspol_i_f[0])
    return wspol_i_f

def wybierz_z_otwartej_listy(otwarta_lista):
    wezel = otwarta_lista
    #print("wezel = otwarta_lista:",wezel)
    znajdz_najmniejsze_f_z_listy_otwartej = znajdz_najmniejsze_f(otwarta_lista)
    #print("znajdz_najmniejsze_f_z_listy_otwartej = znajdz_najmniejsze_f(otwarta_lista):", znajdz_najmniejsze_f_z_listy_otwartej) #zwraca: (19,0): 0
    #wezel = list(wezel)
    wezel = {znajdz_najmniejsze_f_z_listy_otwartej[0]: znajdz_najmniejsze_f_z_listy_otwartej[1]}
    #rint("wezel = list(wezel):",wezel)
    #print("wezel = wezel[znajdz_najmniejsze_f_z_listy_otwartej]:", wezel)
    return wezel

def gwiazdka(start, koniec, siatka):
    otwarta_lista = {} #{(koordynaty_wezla): (rodzic[0], rodzic[1]), f} #ten słownik jest po to aby w otwartej liscie nie powtarzały się węzły!
    zamknieta_lista = []
    sciezka_lista_koncowa = {}

    otwarta_lista[(19,0)] = [0,0,1] #(rodzic,f,g)
    while otwarta_lista:

        wezel = wybierz_z_otwartej_listy(otwarta_lista)# wezel: {(19, 0): 0}

        list_wezel = list(wezel)
        if len(otwarta_lista) != 0:
            otwarta_lista.pop(list_wezel[0][0], list_wezel[0][1])
        print("list wezel", wezel)
        otwarta_lista.pop(list_wezel[0]) # wyrzuca pierwszy element dictonary
        print("list_wezel", list_wezel)
        #print("wezel:",wezel[0])
        #print("wezel[1][1]", wezel[1][1])
        if list_wezel[0] == (0, 19):
            zwroc_cala_sciezke = sciezka(list_wezel, otwarta_lista, siatka) #tu jeszcze nie wiem
            return zwroc_cala_sciezke

        if list_wezel[0] in zamknieta_lista:
            continue

        #print(wezel)
        zamknieta_lista.append(list_wezel[0]) # tu trzymane sa wezly, ktorych juz nie uzywamy i kolejne w open liscie nie beda brane pod uwage

        sasiedzi = sprawdz_sasiada(list_wezel[0], siatka) #tutaj wrzuca do listy sasiedzi koordynaty dol, gora, lewo, prawo
        print("sąsiedzi_lista:", sasiedzi)
        print(" ")




        print("_______LITEROWANIE SASIADÓW________")
        for wspol_sasiad in sasiedzi: #itreuje przez sąsiadów wyciągając np. (19,1)

            if wspol_sasiad in zamknieta_lista:
                continue


            h_sasiad = math.sqrt((list_wezel[0][0] - 0)**2 + (list_wezel[0][1]-19)**2)
            if list_wezel[0] != (19,0):
                print("g_sasiad = otwarta_lista.get(list_wezel[0])[2] + 1:", otwarta_lista.get(list_wezel[0]))
                g_sasiad = otwarta_lista.get(list_wezel[0])[2] + 1
                exit()

            else:
                g_sasiad = 1
                print("g_sasiad = 1: ",g_sasiad)
            f_sasiad = g_sasiad + h_sasiad

            for klucz_i_wartosci in otwarta_lista.items():

                print("-")
                print("klucz_i_wartosci", klucz_i_wartosci)
                if len(otwarta_lista.items()) == 2:
                    exit()

                print("f_sasiad", f_sasiad)
                print("wspol_sasiad", wspol_sasiad)

                wspol_z_otwartej = klucz_i_wartosci[0]
                f_z_otwartej = klucz_i_wartosci[1][1]


                print("klucz_i_wartosc[0] z otwartej listy:", wspol_z_otwartej)   #(19, 0)
                print("klucz_i_wartosc[1][1] z otwartej listy:", f_z_otwartej)       #27.870057685088806
                print("-")
                if f_z_otwartej <= f_sasiad and wspol_sasiad == wspol_z_otwartej:
                    print("-")
                    print("f_sasiad w warunku:", f_sasiad)
                    print("wspol_sasiad w warunku:", wspol_sasiad)
                    continue
            print("wezel[0]", list_wezel[0])
            #print("otwarta",otwarta_lista.get((list_wezel[0][0],list_wezel[0][1])))
            #exit()
            otwarta_lista[wspol_sasiad] = [list_wezel[0], #przypisujemy koordynat rodzica
                                          f_sasiad + g_sasiad,
                                           g_sasiad]# obliczamy f dla sąsiada (i)

            print("otwarta lista:", otwarta_lista)

            wezel.pop(list_wezel[0][0], list_wezel[0][1])

    return print("0")




if __name__ == '__main__':
    link_siatka = "C:/Users/1/Desktop/AstarPB/grid.txt"
    siatka = []
    with open(link_siatka, 'r') as file:
        siatka = [list(map(int, line.strip().split())) for line in file]




        #for i in krotka:
         #   print(i)
            #siatka.append(int(i))
            #for j, a in enumerate(i):
                #print("i:",i)
                #print("j:",j)
                #print("a:",a)
                #if a != ' ' and a != '\n':
                    #chwilowa.append(int(a))
                    #print(chwilowa)
                #if j == 39:
                    #chwilowa.append(int(i[39]))
                    #siatka.append(chwilowa)
                   # print(chwilowa)
                   # chwilowa = []


    start = (0,0)
    koniec = (19, 19)

    siatka = siatka[::-1]
    siatka[start[0]][start[1]] = 1
    siatka[koniec[1]][koniec[1]] = 2
    siatka = siatka[::-1]

    for i in siatka:
        print(i)


    scieszka_koncowa = gwiazdka(start, koniec, siatka)



    print(scieszka_koncowa[0])

    for i in scieszka_koncowa[1]:
        print(i)

