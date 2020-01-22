﻿using System;
using System.Collections;
using System.Diagnostics;

namespace AoC18
{
    class Program
    {
        static void Main(string[] args)
        {
            // var superSolver = new SuperMazeSolver();
            // var loader = new Stopwatch();
            // loader.Start();
            // superSolver.LoadMaze(SuperMazeSolver.testOne);
            // loader.Stop();
            // Console.WriteLine($"{loader.Elapsed.TotalMilliseconds} ms");
            
            //superSolver.LoadMaze(Data.rawDataPartOne);
            //superSolver.ExploreMaze();

            var solver = new MazeSolver();

            var timerOne = new Stopwatch();
            timerOne.Start();
            //solver.LoadMaze(Data.testDataOne);
            solver.LoadMaze(Data.rawDataPartOne);
            timerOne.Stop();
            Console.WriteLine("Load: " + timerOne.Elapsed.TotalMilliseconds + " ms");
            
            timerOne.Reset();
            timerOne.Start();
            var distancePartOne = solver.WalkMap();
            timerOne.Stop();
            
            Console.WriteLine("PART I - Shortest Distance : " + distancePartOne);
            Console.WriteLine("Time: " + timerOne.Elapsed.TotalMilliseconds + " ms");
            Console.WriteLine();
            //
            // var advSolver = new AdvancedMazeSolver();
            // advSolver.LoadMaze(Data.rawDataPartTwo);
            //
            // var timerTwo = new Stopwatch();
            // Console.WriteLine("Please wait..");
            // timerTwo.Start();
            // var distancePartTwo = advSolver.WalkMap();
            // timerTwo.Stop();
            //
            // Console.WriteLine("PART II - Shortest Distance : " + distancePartTwo);
            // Console.WriteLine("Time: " + timerTwo.Elapsed);
        }
    }

    public static class Data
    {
        // 8
        public static string testDataOne = @"
        #########
        #b.A.@.a#
        #########";
        
        public static string testDataOneA = @"
        ########################
        #@.....................#
        ######################.#
        #a.....................#
        ########################";

        // 86
        public static string testDataTwo = @"
        ########################
        #f.D.E.e.C.b.A.@.a.B.c.#
        ######################.#
        #d.....................#
        ########################";

        // 132
        public static string testDataThree = @"
        ########################
        #...............b.C.D.f#
        #.######################
        #.....@.a.B.c.d.A.e.F.g#
        ########################";

        // 136
        public static string testDataFour = @"
        #################
        #i.G..c...e..H.p#
        ########.########
        #j.A..b...f..D.o#
        ########@########
        #k.E..a...g..B.n#
        ########.########
        #l.F..d...h..C.m#
        #################";
        
        // 81
        public static string testDataFive = @"
        ########################
        #@..............ac.GI.b#
        ###d#e#f################
        ###A#B#C################
        ###g#h#i################
        ########################";

        public static string testDataSix = @"
        #######
        #a.#Cd#
        ##@#@##
        #######
        ##@#@##
        #cB#Ab#
        #######";

        public static string rawDataPartOne = @"
#################################################################################
#.#.....#.........#...S.#...............#...............#.......#.....#.........#
#.#.#.###.#.#####.#.###.#.###########.#.#.###.###########.###.#E#.###.#.#.#######
#...#.....#.#...#.#.#.#.#.#.........#.#.#...#.#...#.....#...#.#.#...#...#.#.....#
#.#########.#.#.###.#.#.###.#######I###.#.#.#.#.#.#.###.###.#.#.#.#.#####.#.###.#
#.#.......#...#...#s#.#.#...#.#.....#...#.#.#.#.#.#.#.#...#.#.#.#.#...#.U.#...#.#
#.#R#####.#######.#.#.#.#.###.#.#####.#.#.#.###.#.#.#B#.#.#.#.#.#####.#.#####.#.#
#.#.#...#.#.....#...#.#..i....#n#.....#.#.#.....#.....#.#.#.#.#.......#.#.....#.#
#.#.###.#.#.#.#.#####.#######.#.#.#.###.#.#############.###.#.###.#####.#.#####.#
#.#..l..#.#.#.#.#.......#...#.#.#.#.#.#.#...#.....#.#...#...#.#...#...#.#.....#w#
#.#.#####.#.#.###.#.###.#N#.###.###.#.#.###.#.###.#.#.###.###.#####.#.#######G#.#
#.#.#...#...#.....#.#...#.#.....#...#.#.#...#...#...#c......#.....#.#r........#.#
#.###.#.#####.#####.#.###.#######.###.#.#.#####.###########.#####.#.###########.#
#....g#.....#.....#.#...#...#.....#...#.#.#...#.#.....#...#...#...#...#...#.....#
###########W#######.#######.#####.###.#.#.#.#.#.#.###.#.#.#####F#####.#.###.#####
#...#.....#.........#......o#...#...#...#.#.#...#.#.#.#.#...#...#.....#.#...#...#
#.#.#.#.#######.#####.#######.#.###.#.###.#.#####.#.#.#.###.#.#.#.#####.#.###.#.#
#v#.#.#.........#j..#.#.......#.....#.#.#.#...#.....#.#.#...#.#.#.#...#...#..q#.#
###.#.###########.#.#.###.###########.#.#.###.#.#####.#.#.###.#.#.#.###.###.#.#.#
#...#.#.......#...#.O.#...#.#.......#...#...#...#...#.#.#.....#.#.#.#..k#...#.#.#
#.#.#K#.#P###.#.#######.###.#.#.#######.###.#####.#.#.#.###.#####.#.#.###.###.#.#
#.#.#.#.#...#.#.#.....#.#...#.#.........#.#.......#.#.#.#...#.L...#...#.#.#.#.#.#
#.#.#.#####.#.#.#.#####.#.#.#.###########.#########.#.#.###########.###.#.#.#H#.#
#.#.#.......#.#.J.#.....#.#.#...........#...#...#...#.#...#...V...#...#...#.#.#.#
#.###########.#####.###.#.#############.#.#.#.#.#.###.###.#.###.#.###.#.###.#.###
#.........#...#.......#.#.........#.#...#.#...#.#.....#.#...#.#.#..p#.#.....#...#
#.#####.###.###########.#.#.#####.#.#.###.#####.#####.#.#####.#.#####.#####.###.#
#.#...#...#...#.....#...#.#.....#...#...#.....#...#.#.#...#...#.#.....#.#...#.#.#
#.#.#.###.###.#.###.#.###.#####.#######.#####.###.#.#.###.#.#.#.#.#####.#.###.#.#
#.#.#...#...#.#.#...#.#.#...#.#.........#.....#...#.#...#.#.#.#...#.....#.....#.#
#.#.###.###.#.#.#.###.#.###.#.###########.#######.#.###.#.#.#.#####.#.#.#####.#.#
#.#.#.....#.#.#.#.....#.....#.......#...#.#.....#.....#.#...#.......#.#...#...#.#
#.#.#####.#.#.#.#######.#####.#.###.###.#.#.###.#####.#.#############.#####.###.#
#.#...#.#.#...#.#.....#.#...#.#...#.#...#.#.#.#.....#.#...#...#.....#...#...#...#
#.#.#.#.#.#####.#.#####.#.#Y#.###.#.#.###.#.#.#####.#####.#.#.#.###.###.#.###D#.#
#.#.#.#.#.#.....#.....#.#.#.....#.#.#...#.#...#.....#...#...#...#.#...#.....#.#.#
#.###.#.#.#####.#.###.#.#########.#.#.#.#.###.#.###.#.#.#########.###.#####.#.#.#
#.....#.#.....#.#...#.#...#.......#.#.#.#.....#...#.#.#.....#.......#.....#.#.#.#
#######.#####.#.###.#.###.#.#######.###.#.#######.###.#####.#.#####.#####.###.#.#
#...............#m..#.......#...................#.........#.......#...........#.#
#######################################.@.#######################################
#...........#.....#...............#.......#......y....#.......#...A...#.........#
#.###.#####.###.#.#.#############.#.#.###.#.###.#####.#.#####.#.#####.###.#####.#
#.#...#t..#x....#.#.#...........#.#.#...#.....#.#...#.#.#...#...#...#...#.....#.#
#.#.###.#.#######.#.#.#########.#.#.###.#.#####.#.###.#.#.#.#####.#.###.#####.###
#.#.....#.#.....#...#d#...#.....#.#.#.#.#.#.....#.#.....#.#.#.....#.#...#...#...#
#.#######.#.#######.#.###.#.#####.#.#.#.#.#.#####.#.#####.#.#.#######.###.#.###.#
#.#.......#.........#.....#.#.....#...#.#.#.#...#...#.....#.#.......#...M.#.....#
#.#.#########.###########.#.#####.###.#.#.#.#.#.#####.#####.#.#####.###########.#
#.#.........#.......#...#.#.....#.#...#.#.#...#.......#.#...#.....#.....#.......#
#.#########.#######.###.#.#####.#.#.###.#.###########.#.#.#############.#.#######
#.....#...#.#.......#...#.#a....#.#.#.#.#.#...#.....#...#.........#.....#.....#.#
#####.#.#.#.#.#######.###.#.#######.#.#.#.###.#.###.###.#########.#.#########.#.#
#.......#z#.#.........#...#...#.......#.#.....#.#...#.........#...#.........#.#.#
###########.#######.###.#####.#.#######.#####.#.###.#########.#.#########.###.#X#
#.........#.......#.#...#...#.....#...#.#.....#...#...#.......#.#.........#...#.#
#.#######.#######.###.###.#.#.#####.#.#.#.#####.#.###.#.#####.#.###.#.#####.###.#
#.#.....#.......#.......#.#.#.#.....#.#.#.#...#.#.#.#.#.#...#.#.#...#.#.....#...#
#.#.###.#######.#########.#.###.#####.#.#.#.#.###.#.#.###.#.###.#.###.#.#######.#
#...#.#.......#...#...#...#.#...#...#.#.#.#.#.#...#.#.....#.....#...#.#.#.......#
#####.#######.###.#.#.#.###.#.###.#.#.#.###.#.#.###.#######.#######.###.#.#####.#
#...........#.#.#...#.#.#.#...#.#.#...#.#...#.#.#.......#...#.....#.....#.....#.#
#.#######.#.#.#.#######.#.#####.#.#####.#.###.#.#.#######.#######.#.#########T#.#
#...#...#.#.#.#.......#.#......e#.#.....#.#.....#.........#.....#.#.......#...#.#
#####.#.###.#.#####.#.#.#.#####.#.#.#.###.#############.###.#.#.#.#######.#.###.#
#.....#...#.#.....#.#...#.....#.#.#.#...#.#...........#.#...#.#.#...#.....#.#.#.#
#.#######.#.#####.#.#######.#.#.#.#####.#.#.#########.###.###.#####.#.#####.#.#.#
#.......#.....#.#...#.....#.#.#.#.#.....#...#.....#.#.....#.#.....#.#.....#.#...#
#.#####.#####.#.#####.###.###.#.#.#.#########.###.#.#######.#####.#.#####.#.#.###
#...#.#.....#.........#.......#.#.#.....#...#...#.#.......#.....#...#...#...#...#
###.#.#####.###########.#########.#.###.#.###.#.#.#####.#.#.#.#######.#########.#
#.#.....#...#...#.......#.........#...#.#...#.#.#.....#.#.#.#.................#.#
#.#####.#.###.#C#.#####.#.###########.#.#.#.#.#.#####.###.#.#############.###.#.#
#...#...#...#.#...#...#.#.#...#...#...#.#.#...#.#...#.#...#.#.......#...#.#.#.#.#
#.###.#####.###.###.#.###.#.#.#.#.#####.#.#####.#.###.#.#.#.#.#####.#.#.#.#.#.#.#
#...#.....#...#...#.#...#.#.#...#...#...#.#...#.#.......#.#.#.#...#.#.#...#...#.#
#.#.###.#####.#####.###.#.#.#######.#.#.#.###.#.#######.###.#.#.#.#.#.#####.###.#
#.#...#.#.....#...#...#.#.#...#...#.#.#.#.....#...#f..#.#...#u#.#...#....b#.#...#
#.###.#.#.#####Q#.###.#.#.###.#.###.#.#.#####.###.#.#.###.#####.###.#####.###Z###
#...#...#.......#.....#....h..#.......#.#.......#...#...........#.......#.......#
#################################################################################";
        
                public static string rawDataPartTwo = @"
#################################################################################
#.#.....#.........#...S.#...............#...............#.......#.....#.........#
#.#.#.###.#.#####.#.###.#.###########.#.#.###.###########.###.#E#.###.#.#.#######
#...#.....#.#...#.#.#.#.#.#.........#.#.#...#.#...#.....#...#.#.#...#...#.#.....#
#.#########.#.#.###.#.#.###.#######I###.#.#.#.#.#.#.###.###.#.#.#.#.#####.#.###.#
#.#.......#...#...#s#.#.#...#.#.....#...#.#.#.#.#.#.#.#...#.#.#.#.#...#.U.#...#.#
#.#R#####.#######.#.#.#.#.###.#.#####.#.#.#.###.#.#.#B#.#.#.#.#.#####.#.#####.#.#
#.#.#...#.#.....#...#.#..i....#n#.....#.#.#.....#.....#.#.#.#.#.......#.#.....#.#
#.#.###.#.#.#.#.#####.#######.#.#.#.###.#.#############.###.#.###.#####.#.#####.#
#.#..l..#.#.#.#.#.......#...#.#.#.#.#.#.#...#.....#.#...#...#.#...#...#.#.....#w#
#.#.#####.#.#.###.#.###.#N#.###.###.#.#.###.#.###.#.#.###.###.#####.#.#######G#.#
#.#.#...#...#.....#.#...#.#.....#...#.#.#...#...#...#c......#.....#.#r........#.#
#.###.#.#####.#####.#.###.#######.###.#.#.#####.###########.#####.#.###########.#
#....g#.....#.....#.#...#...#.....#...#.#.#...#.#.....#...#...#...#...#...#.....#
###########W#######.#######.#####.###.#.#.#.#.#.#.###.#.#.#####F#####.#.###.#####
#...#.....#.........#......o#...#...#...#.#.#...#.#.#.#.#...#...#.....#.#...#...#
#.#.#.#.#######.#####.#######.#.###.#.###.#.#####.#.#.#.###.#.#.#.#####.#.###.#.#
#v#.#.#.........#j..#.#.......#.....#.#.#.#...#.....#.#.#...#.#.#.#...#...#..q#.#
###.#.###########.#.#.###.###########.#.#.###.#.#####.#.#.###.#.#.#.###.###.#.#.#
#...#.#.......#...#.O.#...#.#.......#...#...#...#...#.#.#.....#.#.#.#..k#...#.#.#
#.#.#K#.#P###.#.#######.###.#.#.#######.###.#####.#.#.#.###.#####.#.#.###.###.#.#
#.#.#.#.#...#.#.#.....#.#...#.#.........#.#.......#.#.#.#...#.L...#...#.#.#.#.#.#
#.#.#.#####.#.#.#.#####.#.#.#.###########.#########.#.#.###########.###.#.#.#H#.#
#.#.#.......#.#.J.#.....#.#.#...........#...#...#...#.#...#...V...#...#...#.#.#.#
#.###########.#####.###.#.#############.#.#.#.#.#.###.###.#.###.#.###.#.###.#.###
#.........#...#.......#.#.........#.#...#.#...#.#.....#.#...#.#.#..p#.#.....#...#
#.#####.###.###########.#.#.#####.#.#.###.#####.#####.#.#####.#.#####.#####.###.#
#.#...#...#...#.....#...#.#.....#...#...#.....#...#.#.#...#...#.#.....#.#...#.#.#
#.#.#.###.###.#.###.#.###.#####.#######.#####.###.#.#.###.#.#.#.#.#####.#.###.#.#
#.#.#...#...#.#.#...#.#.#...#.#.........#.....#...#.#...#.#.#.#...#.....#.....#.#
#.#.###.###.#.#.#.###.#.###.#.###########.#######.#.###.#.#.#.#####.#.#.#####.#.#
#.#.#.....#.#.#.#.....#.....#.......#...#.#.....#.....#.#...#.......#.#...#...#.#
#.#.#####.#.#.#.#######.#####.#.###.###.#.#.###.#####.#.#############.#####.###.#
#.#...#.#.#...#.#.....#.#...#.#...#.#...#.#.#.#.....#.#...#...#.....#...#...#...#
#.#.#.#.#.#####.#.#####.#.#Y#.###.#.#.###.#.#.#####.#####.#.#.#.###.###.#.###D#.#
#.#.#.#.#.#.....#.....#.#.#.....#.#.#...#.#...#.....#...#...#...#.#...#.....#.#.#
#.###.#.#.#####.#.###.#.#########.#.#.#.#.###.#.###.#.#.#########.###.#####.#.#.#
#.....#.#.....#.#...#.#...#.......#.#.#.#.....#...#.#.#.....#.......#.....#.#.#.#
#######.#####.#.###.#.###.#.#######.###.#.#######.###.#####.#.#####.#####.###.#.#
#...............#m..#.......#..........@#@......#.........#.......#...........#.#
#################################################################################
#...........#.....#...............#....@#@#......y....#.......#...A...#.........#
#.###.#####.###.#.#.#############.#.#.###.#.###.#####.#.#####.#.#####.###.#####.#
#.#...#t..#x....#.#.#...........#.#.#...#.....#.#...#.#.#...#...#...#...#.....#.#
#.#.###.#.#######.#.#.#########.#.#.###.#.#####.#.###.#.#.#.#####.#.###.#####.###
#.#.....#.#.....#...#d#...#.....#.#.#.#.#.#.....#.#.....#.#.#.....#.#...#...#...#
#.#######.#.#######.#.###.#.#####.#.#.#.#.#.#####.#.#####.#.#.#######.###.#.###.#
#.#.......#.........#.....#.#.....#...#.#.#.#...#...#.....#.#.......#...M.#.....#
#.#.#########.###########.#.#####.###.#.#.#.#.#.#####.#####.#.#####.###########.#
#.#.........#.......#...#.#.....#.#...#.#.#...#.......#.#...#.....#.....#.......#
#.#########.#######.###.#.#####.#.#.###.#.###########.#.#.#############.#.#######
#.....#...#.#.......#...#.#a....#.#.#.#.#.#...#.....#...#.........#.....#.....#.#
#####.#.#.#.#.#######.###.#.#######.#.#.#.###.#.###.###.#########.#.#########.#.#
#.......#z#.#.........#...#...#.......#.#.....#.#...#.........#...#.........#.#.#
###########.#######.###.#####.#.#######.#####.#.###.#########.#.#########.###.#X#
#.........#.......#.#...#...#.....#...#.#.....#...#...#.......#.#.........#...#.#
#.#######.#######.###.###.#.#.#####.#.#.#.#####.#.###.#.#####.#.###.#.#####.###.#
#.#.....#.......#.......#.#.#.#.....#.#.#.#...#.#.#.#.#.#...#.#.#...#.#.....#...#
#.#.###.#######.#########.#.###.#####.#.#.#.#.###.#.#.###.#.###.#.###.#.#######.#
#...#.#.......#...#...#...#.#...#...#.#.#.#.#.#...#.#.....#.....#...#.#.#.......#
#####.#######.###.#.#.#.###.#.###.#.#.#.###.#.#.###.#######.#######.###.#.#####.#
#...........#.#.#...#.#.#.#...#.#.#...#.#...#.#.#.......#...#.....#.....#.....#.#
#.#######.#.#.#.#######.#.#####.#.#####.#.###.#.#.#######.#######.#.#########T#.#
#...#...#.#.#.#.......#.#......e#.#.....#.#.....#.........#.....#.#.......#...#.#
#####.#.###.#.#####.#.#.#.#####.#.#.#.###.#############.###.#.#.#.#######.#.###.#
#.....#...#.#.....#.#...#.....#.#.#.#...#.#...........#.#...#.#.#...#.....#.#.#.#
#.#######.#.#####.#.#######.#.#.#.#####.#.#.#########.###.###.#####.#.#####.#.#.#
#.......#.....#.#...#.....#.#.#.#.#.....#...#.....#.#.....#.#.....#.#.....#.#...#
#.#####.#####.#.#####.###.###.#.#.#.#########.###.#.#######.#####.#.#####.#.#.###
#...#.#.....#.........#.......#.#.#.....#...#...#.#.......#.....#...#...#...#...#
###.#.#####.###########.#########.#.###.#.###.#.#.#####.#.#.#.#######.#########.#
#.#.....#...#...#.......#.........#...#.#...#.#.#.....#.#.#.#.................#.#
#.#####.#.###.#C#.#####.#.###########.#.#.#.#.#.#####.###.#.#############.###.#.#
#...#...#...#.#...#...#.#.#...#...#...#.#.#...#.#...#.#...#.#.......#...#.#.#.#.#
#.###.#####.###.###.#.###.#.#.#.#.#####.#.#####.#.###.#.#.#.#.#####.#.#.#.#.#.#.#
#...#.....#...#...#.#...#.#.#...#...#...#.#...#.#.......#.#.#.#...#.#.#...#...#.#
#.#.###.#####.#####.###.#.#.#######.#.#.#.###.#.#######.###.#.#.#.#.#.#####.###.#
#.#...#.#.....#...#...#.#.#...#...#.#.#.#.....#...#f..#.#...#u#.#...#....b#.#...#
#.###.#.#.#####Q#.###.#.#.###.#.###.#.#.#####.###.#.#.###.#####.###.#####.###Z###
#...#...#.......#.....#....h..#.......#.#.......#...#...........#.......#.......#
#################################################################################";
    }
}