
def KnapsackTable(capacity, items):
    W = capacity + 1;
    n = len(items) + 1
    table = [0] * n
    for i in range(n):
        table[i] = [0] * W
    
    for i in range(1, n):
        ii = i -1
        item = items[ii]
        for w in range(1, W):
            if item[1] > w:
                table[i][w] = table[ii][w]
            else:
                drop = table[ii][w]
                take = item[0] + table[ii][w - item[1]]

                table[i][w] = max(drop, take)

    return extractIndexes(table, items, W-1, n-1)

def extractIndexes(table, items, W, n):
    sol = []
    
    while n > 0:
        here = table[n][W]
        above = table[n -1][W]
        # This item was used, extract
        if(here != above):
            item =  items[n - 1]
            sol.append(n - 1) # only need the item index
            W = W - item[1]
        n = n -1
    return sol

while True:
    try:
        line = input()
        knapsack = line.split(" ")

        capacity = int(knapsack[0])
        noItems = int(knapsack[1])
        items = [None] * noItems

        for i in range(noItems):
            item = input().split(" ")
            item = list(map(int, item))
            items[i] = item
        # items.reverse()

        solution = KnapsackTable(capacity, items)
        print(len(solution))
        print(' '.join(list(map(str, solution))))

    except EOFError:
        break