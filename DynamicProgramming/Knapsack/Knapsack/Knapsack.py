while True:
    line = input()
    if not line:
        break;

    C, n = line.strip().split(' ')

    print(str(C))
    print(str(n))

    profits = []
    weights = []
