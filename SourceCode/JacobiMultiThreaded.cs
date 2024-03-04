//Author: Muhammad Farrukh Naveed
//StartDate: 28/02/2024
//StartTime: 1:24 PM

//Class: BS(CS)-7A
//Enrollment: 01-134211-056
//Group: None


//*******************************************  PDC ASSIGNMENT NO.1 *******************************************


//Problem: Solving system of linear equationn using sequential,multithreading and threadpool techniques

//                                        --- Solving Multithreaded ---

// ...

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

class Program
{
    static void Main()
    {
        //creating a timer varibale to keep track of the execution time
        var timer = new Stopwatch();
        timer.Start();

        //creatin a variable to store the main thread's info
        Thread mainSeq = Thread.CurrentThread;

        //Naming the main thread
        mainSeq.Name = "Parent Main Thread";

        if (mainSeq.ThreadState == System.Threading.ThreadState.Running)
        {
            Console.WriteLine("Main thread started...");
        }


        // since or only change occur at the number value of IN test case
        string commonPath = "C:\\Users\\farru\\source\\repos\\assignmentPDC\\testcases\\IN";

        // Number of files to be fetched and read
        int numFiles = 10;

        // creating an array of threads such that each thread corresponds to solving one testcase
        Thread[] threads = new Thread[numFiles + 1];
        for (int i = 0; i <= numFiles; i++)
        {
            string filePath = $"{commonPath}{i}.txt";
            threads[i] = new Thread(() => performJacobi(filePath));
            threads[i].Start();
        }

        // we will wait so that all threads have finished job
        // This is done to prevent the termination of main thread before hand
        foreach (Thread thread in threads)
        {
            thread.Join();
        }


        //we prit this once the main thread has finished execution using the sequential method
        Console.WriteLine(mainSeq.Name + " has completed execution");
        timer.Stop();
        Console.WriteLine("Execution time: " + timer.ElapsedMilliseconds + "ms");
    }

    static void performJacobi(string filePath)
    {
        //No of iterations for performing jacobi are set here
        //more iterations means more accurate resultd
        int iter = 100;

        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            float[] varval = new float[lines.Length];

            Console.WriteLine(varval.Length);

            //In jacobi initially we assume all the variables to be 0
            for (int i = 0; i < varval.Length; i++)
            {
                varval[i] = 0;
            }

            // Convert the tokens to integers and store them in a array
            float[,] eqtk = new float[lines.Length, lines[0].Split(' ').Length];

            for (int i = 0; i < lines.Length; i++)
            {
                // Spliting each line into individual numbers
                string[] numbers = lines[i].Split(' ');

                for (int j = 0; j < numbers.Length; j++)
                {
                    // typecasting tofloat and storing in the 2d array
                    eqtk[i, j] = float.Parse(numbers[j]);
                }
            }


            /*considering the last element of the equation is that present on the other side of the "=" sign,
             *hence when we want to perform operations on it we want it on the other side of the equation. so I
             *change the sign for that element*/
            for (int i = 0; i < eqtk.GetLength(0); i++)
            {
                eqtk[i, eqtk.GetLength(1) - 1] *= -1;
            }


            /*we will divide each respective row with the diagonal element of the
             matrix assuming that is the largest absolute value amongst the variables*/

            //This will only be performed once to calculate the euation for each variable
            HandleDivision(eqtk);

            //Printing the value of variables at each iteration
            for (int i = 0; i <= iter; i++)
            {
                Console.WriteLine("\nITERATION: " + i);
                for (int j = 0; j < varval.Length; j++)
                {
                    Console.WriteLine("x" + (j + 1) + ": " + varval[j]);
                }
                HandleMultiplication(eqtk, varval);
            }
        }
        else
        {
            Console.WriteLine("File not found: " + filePath);
        }
    }

    //Function used for debugging purposes
    static void PrintArray(float[,] array)
    {
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                Console.Write(array[i, j] + " ");
            }
            Console.WriteLine();
        }
    }

    /*In this function what we do is we divide the row by the diagonal element of that matrix
     present in that particular row and we also see if that element is positive then we have to change
    sign for the rest of the equation because when that element moves across the = sign it becomes negative
    which is then negated by multiplying both sides by -1*/
    static void HandleDivision(float[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            // finding the diagonal element
            float diagonalElement = matrix[i, i];

            //since this element moves on the other side of the equation we do not need it in the final sum
            matrix[i, i] = 0;

            // if the element is positive then that means that we have to change the sign for the rest of the equation
            for (int j = 0; j < cols; j++)
            {
                if (diagonalElement >= 0)
                {
                    matrix[i, j] *= -1;
                }

                //Console.WriteLine("\n\n" + matrix[i, j] + "/" + diagonalElement + "\n\n");
                matrix[i, j] /= diagonalElement;
            }
        }
    }

    static void HandleMultiplication(float[,] eq, float[] varval)
    {
        //creating a duplicate 2d array to store the manipulations and keep the original one intact
        float[,] temp = new float[eq.GetLength(0), eq.GetLength(1)];
        for (int i = 0; i < eq.GetLength(0); i++)
        {
            for (int j = 0; j < eq.GetLength(1); j++)
            {
                temp[i, j] = eq[i, j];
            }
        }

        //First we need to replace the values of variables in each equation
        for (int i = 0; i < varval.Length; i++)
        {
            for (int j = 0; j < temp.GetLength(0); j++)
            {
                //Console.WriteLine(temp[j, i] + " * " + varval[i]);
                temp[j, i] *= varval[i];
            }
        }


        //we nned to clear the previous values stored for variables
        for (int i = 0; i < varval.Length; i++)
        {
            varval[i] = 0;
        }

        //Sum of each line of the matrix is stored in the varval array as variable values
        for (int i = 0; i < temp.GetLength(0); i++)
        {
            for (int j = 0; j < temp.GetLength(1); j++)
            {

                varval[i] += temp[i, j];
                //Console.WriteLine(temp[i, j] + " + " + varval[i]);

            }
        }
    }
}

// ...

// Time to completion: 6.3 hrs
// Actual Lines of code: 121
// This code has been pushed to origin on 02/02/2024 at 10:42pm