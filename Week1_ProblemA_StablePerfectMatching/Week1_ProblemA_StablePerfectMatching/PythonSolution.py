# c# doesn't work for some fucking reasion

T = int(input())

for i in range(T):
    n = int(input())
    vector1 = list(map(int, input().split(' ')))
    vector2 = list(map(int, input().split(' ')))

    vector1.sort(reverse=True)
    vector2.sort()

    dotProduct = 0

    for j in range(n):
        smallest = vector1[j]
        biggest = vector2[j]
        dotProduct = dotProduct + (smallest * biggest)

    print(f"Case #{str(i + 1)}: {str(dotProduct)}")



