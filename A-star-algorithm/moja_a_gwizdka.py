import math

def sprawdz_sasiadow(wezel, siatka):
    lista_sasiadow = []
    wspolrzedne_d_g_l_p = [(1,0),(-1,0),(0,-1),(0,1)] # dol, gora, lewo, prawo

    for i, sasiad in enumerate(wspolrzedne_d_g_l_p):
        if 0 <= sasiad[0] + wezel[0] <= 19 and 0 <= sasiad[1] + wezel[1] <= 19:
            #print("i_sasiad:", i)
            print("sasiad:", sasiad)
        else:
            continue
        if siatka[sasiad[0] + wezel[0] ][sasiad[1] + wezel[1]  ] != 5:
            #print("sasiad[0] + wezel[0] :",sasiad[0] + wezel[0] )
            #print("sasiad[1] + wezel[1] ", sasiad[1] + wezel[1] )

            lista_sasiadow.append(
                (sasiad[0] + wezel[0],
                 sasiad[1] + wezel[1])
            )
        else:
            continue
    return lista_sasiadow

def sciezka(wezel, zamknieta_lista, siatka):
    sciezka_koncowa = []
    siatka[::-1]
    while wezel in zamknieta_lista:
        siatka[wezel[0]][wezel[1]] = 3
        sciezka_koncowa.append(wezel)
        wezel =zamknieta_lista[wezel]

    return sciezka_koncowa[::-1], siatka[::-1]


def znajdz_najmniejsze_f(otwarta_lista):
    najm_f = None
    najm_klucz = None

    for klucz, wartosci in otwarta_lista.items():
        if najm_f is None or wartosci[1]<najm_f:
            najm_f = wartosci[1]
            najm_klucz = klucz

    return najm_klucz

def gwiazdka(start, koniec, siatka):
    otwarta_lista = {start: (0, 0, 0)}  # klucz: (rodzic, f, g) np. (19,1):(19,27,1)
    zamknieta_lista = {}

    while otwarta_lista:
        wezel=znajdz_najmniejsze_f(otwarta_lista)
        rodzic=otwarta_lista[wezel][0]
        f=otwarta_lista[wezel][1]
        g=otwarta_lista[wezel][2]
        otwarta_lista.pop(wezel)

        if wezel==koniec:
            zamknieta_lista[wezel]=rodzic
            return sciezka(wezel,zamknieta_lista,siatka)



        zamknieta_lista[wezel] = rodzic
        sasiedzi = sprawdz_sasiadow(wezel, siatka)
        for sasiad in sasiedzi:
            if sasiad in zamknieta_lista:
                continue
            g_sasiad=g + 1
            h_sasiad=math.sqrt((koniec[0] - sasiad[0])**2 + (koniec[1] - sasiad[1])**2)
            f_sasiad=g_sasiad + h_sasiad
            if sasiad not in otwarta_lista or f_sasiad < otwarta_lista[sasiad][1]:
                otwarta_lista[sasiad] = (wezel, f_sasiad, g_sasiad)
    return [], siatka

if __name__ == "__main__":
    link_siatka ="C:/Users/1/Desktop/AstarPB/grid.txt"
    siatka = []
    with open(link_siatka, 'r') as file:
        for line in file:
            linia = []
            for znak in line:
                if znak.isdigit():
                    linia.append(int(znak))
            siatka.append(linia)

    for linia in siatka:
        print(linia)

    start = (0, 0)
    koniec = (19, 19)

    siatka[start[0]][start[1]] = 1
    siatka[koniec[0]][koniec[1]] = 2

    sciezka_koncowa, siatka = gwiazdka(start, koniec, siatka)

    print("Sciezka:", sciezka_koncowa)
    for wiersz in siatka:
        print(wiersz)