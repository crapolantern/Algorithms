using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoArrayLookup
{
    class Program
    {
        static void Main(string[] args)
        {
            /*** Test 1 ***/
            bool pass = true;
            int[] A = { 0, 2, 4, 6, 8, 10 };
            int[] B = { 1, 3, 5, 7, 9 };

            //  Do a chain of tests!
            for (int i = 0; i < 11; i++)    //  A.len + B.len = 11
            {
                //  Select should always return the number given for this test
                if (select(A, B, i) != i)   
                {
                    pass = false;
                    break;
                }
            }
            if (pass)
                Console.WriteLine("Passed Test 1");
            else
                Console.WriteLine("Failed Test 1");

            /*** Test 2 ***/
            pass = true;
            A = new int[] { 1, 2, 3, 4, 5 };
            B = new int[] { 1, 2, 3, 4, 5 };

            //  A new chain of tests!
            int index_value = 1;
            for (int i = 0; i < 9; i++)    //  A.len + B.len = 9
            {
                //  Select should return each number twice
                if (select(A, B, i) != index_value)
                {
                    pass = false;
                    break;
                }

                //  Increment index_value every other time
                if (i % 2 == 1)
                    index_value++;
            }
            if (pass)
                Console.WriteLine("Passed Test 2");
            else
                Console.WriteLine("Failed Test 2");

            /*** Test 3 ***/
            pass = true;
            //  Set A to empty
            A = new int[0];
            B = new int[] { 0, 1, 2, 3, 4 };
            //  Test should always return the value for i.
            for (int i = 0; i < 5; i++)
            {
                if (select(A, B, i) != i)
                {
                    pass = false;
                    break;
                }
            }
            //  Set B to empty
            A = new int[] { 0, 1, 2, 3, 4 };
            B = new int[0];
            //  Test should always return the value for i.
            for (int i = 0; i < 5; i++)
            {
                if (pass == false || select(A, B, i) != i)
                {
                    pass = false;
                    break;
                }
            }
            if (pass)
                Console.WriteLine("Passed Test 3");
            else
                Console.WriteLine("Failed Test 3");

            /*** Test 4 ***/
            pass = true;
            const int test_cases = 10000;
            //  Load the arrays with x unique values
            A = new int[test_cases/2];
            B = new int[test_cases/2];
            for (int i = 0; i < test_cases / 2; i++)
            {
                A[i] = (i * 2);
                B[i] = (i * 2) + 1;
            }

            // Test 1000 cases
            for (int i = 0; i < test_cases; i++)
                if (select(A, B, i) != i)
                {
                    pass = false;
                    break;
                }

            if (pass)
                Console.WriteLine("Passed Test 4");
            else
                Console.WriteLine("Failed Test 4");
            Console.Read();
        }

        // A and B are each sorted into ascending order, and 0 <= k < |A|+|B| 
        // Returns the element that would be stored at index k if A and B were
        // combined into a single array that was sorted into ascending order.
        static int select(int [] A, int [] B, int k)
        {
            return select(A, 0, A.Length -1, B, 0, B.Length -1, k);
        }
    

        static int select(int[] A, int loA, int hiA, int[] B, int loB, int hiB, int k)
        {
            // A[loA..hiA] is empty
            if (hiA < loA)
                return B[k - loA];
            // B[loB..hiB] is empty
            if (hiB < loB)
                return A[k - loB];
            // Get the midpoints of A[loA..hiA] and B[loB..hiB]
            int i = (loA + hiA) / 2;
            int j = (loB + hiB) / 2;
            // Figure out which one of four cases apply
            if (A[i] > B[j])
                if (k <= i + j)
                    return select(A, loA, i-1, B, loB, j, k);
                else
                    return select(A, i, hiA, B, j+1, hiB, k);
            else
                if (k <= i + j)
                return select(A, loA, i, B, loB, j-1, k);
            else
                return select(A, i+1, hiA, B, j, hiB, k);
        }
    }
}
