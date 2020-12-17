import fileinput

values = list(map(int, fileinput.input()))

#1
for i in range(len(values)):
    c = values[i]
    for x in range(i + 1, len(values)):
        o = values[x]
        if c + o == 2020:            
            print(f"\n{c * o}")
            break

#2
for i in range(len(values)):
    c = values[i]
    for j in range(len(values)):
        cc = values[j]
        for x in range(i + 1, len(values)):
            o = values[x]
            if c + cc + o == 2020:            
                print(f"\n{c * cc * o}")
                break
        else:
            continue        
        break
    else:
        continue
    break