import matplotlib.pyplot as plt

def main():
    # turn_off_win = [2, 4, 6, 8, 10, 12, 14, 16, 18] # кол-во выключенных окон
    # turn_off_win2 = [2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 32, 36]
    # tazh = [1, 2, 3, 4, 5, 6, 7, 8, 9]  # этажность
    # ray = list(); # количество лучей
    # for i in turn_off_win:
    #     ray.append(i * 800)
    #
    # time_on = [1128634, 1130400, 1141666, 1163469, 1173836, 1194965, 1225972, 1260547, 1320117]  # время, когда включены окна
    # time_off = [1181080, 1205706, 1213581, 1241637, 1306955, 1355527, 1659598, 1731519, 1937254]  # время, когда выключены окна
    # time_off2 = [1181080, 1205706, 1213581, 1241637, 1306955, 1355527, 1659598, 1731519, 1937254, 2172851, 2272851, 2416185]
    # fig1 = plt.figure(figsize=(9, 6))
    # plot = fig1.add_subplot()
    # # plot.plot(turn_off_win, time_on, label = "Свет включен")
    # plot.plot(turn_off_win2, time_off2, label="Свет выключен")
    # plt.legend()
    # plt.grid()
    # plt.title("Временные характеристики")
    # plt.ylabel("Затраченное время (такты)")
    # plt.xlabel("Кол-во лучей")
    #
    # plt.show()

    # count = [2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 32, 36]
    #
    # paralltrace = [1181080, 1205706, 1213581, 1241637, 1306955, 1355527, 1659598, 1731519, 1937254, 2172851, 2272851, 2416185]
    #
    # ray = [1660, 3243, 4863, 6472, 8085, 9705, 11364, 12984, 14664, 16317, 25977, 29225]
    # fig, ax = plt.subplots()
    #
    # # Что бы шаг по оси x был не стандартным,
    # # А массивом count.
    # plt.xticks(count)
    #
    #
    # ax.plot(count, ray, label="")
    #
    # ax.scatter(count, ray, c='deeppink')
    #
    #
    # ax.legend()
    # ax.grid()
    # ax.set_xlabel('Кол-во окон, в которых не горит свет')
    # ax.set_ylabel('Количество лучей')
    #
    # plt.show()

# Замеры времни 2
    count = [1, 2, 3, 4, 5, 6, 7, 8, 9] # этажы
    ray = [719848, 719848, 719848, 719848, 719848, 719848, 719848, 719848, 719848] # без улучщений - подсчет
    ray_opt = [3300, 6500, 9700, 12900, 16100, 19300, 22500, 25700, 28900] # с улучщенийми - подсчет
    ray_opt_prog = [3361, 6723, 10042, 13283, 16643, 19983, 23343, 26483, 29583] # с улучщениями - программа
    plt.xticks(count)

    fig, ax = plt.subplots()
    # ax.plot(count, ray, label="")
    ax.plot(count, ray_opt, label="Аналитический способ")
    ax.plot(count, ray_opt_prog, label="Программный способ")

    # ax.scatter(count, ray, c='deeppink')
    ax.scatter(count, ray_opt, c='blue')
    ax.scatter(count, ray_opt_prog, c='green')

    ax.legend()
    ax.grid()
    ax.set_xlabel('Кол-во этажей')
    ax.set_ylabel('Количество лучей')

    plt.show()

if __name__ == '__main__':
    main()


