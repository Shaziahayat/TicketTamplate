namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<int> inputList = new List<int> { 1, 2, 3, 4, 5, 6 };
            List<int> result = GetReversedMiddleElements(inputList);

            Console.WriteLine("Reversed middle elements:");
            foreach (int num in result)
            {
                Console.Write(num + " ");
            }
        }

        static List<int> GetReversedMiddleElements(List<int> lst)
        {
            List<int> result = new List<int>();

            // Check if the list has at least four elements
            if (lst.Count < 4)
            {
                Console.WriteLine("Input list must have at least four elements");
                return result;
            }

            // Calculate the index of the middle elements
            int middleIndex = lst.Count / 2;
            if (lst.Count % 2 == 0)
            {
                // If the list has an even number of elements, take the middle two elements
                result.Add(lst[middleIndex]);
                result.Add(lst[middleIndex - 1]);
            }
            else
            {
                // If the list has an odd number of elements, take the middle three elements
                result.Add(lst[middleIndex + 1]);
                result.Add(lst[middleIndex]);
                result.Add(lst[middleIndex - 1]);
            }

            return result;
        }
    }
}