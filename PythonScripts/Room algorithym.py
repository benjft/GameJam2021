import tkinter as tk, math
centres = []
grid = [[0 for i in range(50)] for j in range(50)]
def connect_circles(circle1, circle2, grid,centres): #connects the 2 given circles
    x1 = centres[circle1][0]
    y1 = centres[circle1][1]
    x2 = centres[circle2][0]
    y2 = centres[circle2][1]
    while x1 != x2 or y1 != y2:
        if math.dist([x1,y1],centres[circle1]) > 3 and math.dist([x1,y1],centres[circle2]) > 3:
            grid[x1][y1] = 2
        if x1 < x2:
            x1+=1
        elif x1 > x2:
            x1-=1
        if y1 < y2:
            y1+=1
        elif y1 > y2:
            y1-=1
    #grid[x1][y1] = 2
    print(x1,y1,x2,y2)
    return grid


    
def add_circle(x,y,grid, centres): #add room to map with centre x,y
    centres.append([x,y])
    #print(grid)
    grid[x+4][y] = 1
    #print(grid[x+4][y])
    grid[x-4][y] = 1
    grid[x][y+4] = 1
    grid[x][y-4] = 1
    
    grid[x-2][y+3] = 1
    grid[x-1][y+3] = 1
    grid[x+1][y+3] = 1
    grid[x+2][y+3] = 1
    
    grid[x-2][y-3] = 1
    grid[x-1][y-3] = 1
    grid[x+1][y-3] = 1
    grid[x+2][y-3] = 1

    grid[x-3][y+1] = 1
    grid[x-3][y-1] = 1
    grid[x+3][y+1] = 1
    grid[x+3][y-1] = 1

    grid[x-2][y+2] = 1
    grid[x-2][y-2] = 1
    grid[x+2][y+2] = 1
    grid[x+2][y-2] = 1

    #print(grid)
    return grid

def draw_map(rooms, grid, centres):
    import random

    while len(centres) < rooms:
        #overlap = True
        #while overlap == True:
        #   overlap = False
        x = random.randint(4,len(grid) - 5)
        y = random.randint(4,len(grid)-5)
        for centre in centres:
            print(math.dist([x,y],centre)) # makes sure rooms do not overlap
            if math.dist([x,y],centre) < 8:
                print("True")
                overlap = True

        add_circle(x,y,grid,centres)

    for i in range(rooms):
        n = -1
        while n == i or n < 0:
            n = random.randint(1,rooms-1)
        connect_circles(i,n,grid, centres)

draw_map(5,grid,centres)
        


#grid1 = [[0,0,0,0,1,0,0,0,0,0],
#        [0,0,1,1,0,1,1,0,0,0],
#        [0,1,0,0,0,0,0,1,0,0],
#        [1,0,0,0,0,0,2,0,1,0],
#        [1,0,0,2,0,2,0,0,1,0],
#        [1,0,0,0,0,0,0,0,1,0],
#        [0,1,0,0,2,0,0,1,0,0],
#        [0,0,1,1,0,1,1,0,0,0],
#        [0,0,0,0,1,0,0,0,0,0],
#        [0,0,0,0,0,0,0,0,0,0]]


#add_circle(7,12,grid, centres)
#add_circle(15,19,grid, centres)
#print(centres)
#connect_circles(0,1,grid,centres)

window = tk.Tk()

for i in range(len(grid)): #displays the map
    for j in range(len(grid)):
        frame = tk.Frame(
            master = window,
            

        )
        if (grid[i][j] == 0):
            color = "grey"
        elif (grid[i][j] == 1):
            color="black"
        else:
            color = "blue"
        frame.grid(row=i, column=j)
        label = tk.Label(master=frame, bg=color, width=1 , height=1)
        label.pack()
window.mainloop()
