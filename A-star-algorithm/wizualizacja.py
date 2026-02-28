import matplotlib.pyplot as plt
import matplotlib.animation as animation
import numpy as np
from matplotlib.colors import ListedColormap, BoundaryNorm
import matplotlib
matplotlib.use('TkAgg')


def visualize(siatka, anim_frames):
    siatka = np.array(siatka)

    fig, ax = plt.subplots()
    cmap = ListedColormap([(1, 1, 1),   # 0 - puste
                            (0, 1, 1),   # 1 - start (turkusowy)
                            (1, 1, 0),   # 2 - meta (żółty)
                            (0, 1, 0),   # 3 - ścieżka (zielony)
                            (1, 0, 0),   # 4 - przeszkoda (czerwony)
                            (0, 0, 0),   # 5 - inny kolor dla przeszkód (czarny)
                            (0, 0, 0.7), # 6 - aktualny węzeł (niebieski)
                            (1, 0.5, 0)  # 7 - sąsiad (pomarańczowy)
                           ])
    norm = BoundaryNorm([0, 1, 2, 3, 4, 5, 6, 7], cmap.N)  # Przypisanie wartości do kolorów
    #ax.grid(which="both", color="gray", linestyle="-", linewidth=0.5)
    ax.set_xticks(np.arange(-0.5, siatka.shape[1], 1))
    ax.set_yticks(np.arange(-0.5, siatka.shape[0], 1))
    ax.set_xticklabels(np.arange(0, siatka.shape[1] + 1))
    ax.set_yticklabels(np.arange(0, siatka.shape[0] + 1))
    im = ax.imshow(siatka, cmap=cmap, norm=norm)

    def update(frame_idx):
        im.set_data(anim_frames[frame_idx])
        return [im]

    ani = animation.FuncAnimation(fig, update, frames=len(anim_frames), interval=10, repeat=False, blit=True)
    plt.show()
