n = int(input())
intervals = []
noOfIntervals = 0
currentHour = 0

for i in range(n):
    (s, f) = map(int, input().split(" "))
    intervals.append((s,f))

intervals.sort(key=lambda x: x[1])

for i in range(len(intervals)):
    interval = intervals[i]
    if interval[0] >= currentHour:
        currentHour = interval[1]
        noOfIntervals = noOfIntervals + 1

# for (s, f) in intervals:
#     print(f"Start: {str(s)}, Finish: {str(f)}")

print(str(noOfIntervals))