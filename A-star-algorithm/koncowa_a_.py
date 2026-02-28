import math
import copy
from wizualizacja import visualize
def sprawdz_sasiadow(wezel, siatka):
    lista_sasiadow = []
    wspolrzedne_d_g_l_p = [(1, 0), (-1, 0), (0, -1), (0, 1)]  # dol, gora, lewo, prawo

    for sasiad in wspolrzedne_d_g_l_p:
        dol_gora = sasiad[0] + wezel[0]
        lewo_prawo = sasiad[1] + wezel[1]
        if 0 <= dol_gora <= 19 and 0 <= lewo_prawo <= 19 and siatka[dol_gora][lewo_prawo] != 5:
            lista_sasiadow.append((dol_gora, lewo_prawo))
    return lista_sasiadow

def sciezka(wezel, zamknieta_lista, siatka, anim_frames):
    sciezka_koncowa = []
    while wezel in zamknieta_lista:
        siatka[wezel[0]][wezel[1]] = 3  # Ustawienie koloru drogi
        sciezka_koncowa.append(wezel)
        anim_frames.append(copy.deepcopy(siatka))
        wezel = zamknieta_lista[wezel]

    return sciezka_koncowa[::-1], siatka

def znajdz_najmniejsze_f(otwarta_lista):
    najm_f = None
    najm_klucz = None

    for klucz, wartosci in otwarta_lista.items():
        if najm_f is None or wartosci[1] < najm_f:
            najm_f = wartosci[1]
            najm_klucz = klucz

    return najm_klucz

def gwiazdka(start, koniec, siatka, anim_frames):
    otwarta_lista = {start: (0, 0, 0)}  # klucz: (rodzic, f, g)
    zamknieta_lista = {}

    while otwarta_lista:
        wezel = znajdz_najmniejsze_f(otwarta_lista)
        rodzic = otwarta_lista[wezel][0]
        f = otwarta_lista[wezel][1]
        g = otwarta_lista[wezel][2]
        otwarta_lista.pop(wezel)

        if wezel == koniec:
            zamknieta_lista[wezel] = rodzic
            return sciezka(wezel, zamknieta_lista, siatka, anim_frames)

        zamknieta_lista[wezel] = rodzic
        siatka[wezel[0]][wezel[1]] = 6  # Ustawienie koloru obecnego węzła
        anim_frames.append(copy.deepcopy(siatka))  # Dodanie stanu siatki do animacji

        sasiedzi = sprawdz_sasiadow(wezel, siatka)
        for sasiad in sasiedzi:
            if sasiad in zamknieta_lista:
                continue
            g_sasiad = g + 1
            h_sasiad = math.sqrt((koniec[0] - sasiad[0]) ** 2 + (koniec[1] - sasiad[1]) ** 2)
            f_sasiad = g_sasiad + h_sasiad
            if sasiad not in otwarta_lista or f_sasiad < otwarta_lista[sasiad][1]:
                otwarta_lista[sasiad] = (wezel, f_sasiad, g_sasiad)

    return [], siatka


if __name__ == "__main__":
    link_siatka = "C:/Users/1/Desktop/AstarPB/grid.txt"
    siatka = []
    with open(link_siatka, 'r') as file:
        for line in file:
            linia = [int(znak) for znak in line if znak.isdigit()]
            siatka.append(linia)

    for linia in siatka:
        print(linia)

    start = (0, 0)
    koniec = (19, 19)
    siatka = siatka[::-1]
    siatka[start[0]][start[1]] = 1  # Start
    siatka[koniec[0]][koniec[1]] = 2  # Meta
    siatka = siatka[::-1]
    anim_frames = [copy.deepcopy(siatka)]  # Inicjalizacja klatek animacji

    sciezka_koncowa, siatka = gwiazdka((19, 0), (0, 19), siatka, anim_frames)
    siatka = siatka[::-1]
    print("Sciezka:")
    for i, wiersz in enumerate(siatka):
        for j, liczba in enumerate(wiersz):
            if liczba == 6:
                siatka[i][j] = 0

    for i in siatka:
        print(i)

    visualize(siatka, anim_frames)