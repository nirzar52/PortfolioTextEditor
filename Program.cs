using System;

namespace PortfolioTextEditor
{
    // Supporting methods
    public interface IContainer<T>
    {
        // Resets the stack to empty
        void MakeEmpty();
        // Returns true if the stack is empty; false otherwise
        bool Empty();
        // Returns the number of items in the stack
        int Size();
    }

    // Primary methods
    public interface IStack<T> : IContainer<T>
    {
        void Push(T item);
        void Pop();
        T Top();
    }

    // Stack class
    public class Stack<T> : IStack<T>
    {
        private int capacity; // Maximum capacity of the stack
        private int top; // Index of the top item in the stack
        private T[] A; // Linear array of items (Generic)

        public Stack()
        {
            capacity = 8;
            A = new T[capacity];
            top = -1;
        }

        // Push an item onto the top of the Stack
        public void Push(T item)
        {
            if (top + 1 == capacity)
            {
                DoubleCapacity();
            }
            A[++top] = item;
        }

        // Pop an item off the Stack
        public void Pop()
        {
            if (Empty())
                throw new InvalidOperationException("Stack is empty");
            top--;
        }
        // Retrieve the top item of a Stack
        public T Top()
        {
            if (Empty())
                throw new InvalidOperationException("Stack is empty");
            return A[top];
        }

        public void MakeEmpty()
        {
            top = -1;
        }

        public bool Empty()
        {
            return top == -1;
        }

        public int Size()
        {
            return top + 1;
        }
        
        // Doubles the capacity of the current Stack
        private void DoubleCapacity()
        {
            T[] oldA = A;
            capacity = 2 * capacity;
            A = new T[capacity];
            for (int i = 0; i <= top; i++)
            {
                A[i] = oldA[i];
            }
        }
    }

    // --- Core Text Editor Application ---
    class Program
    {
        static void Main(string[] nameof)
        {
            // Two stacks to manage state history
            Stack<string> undoStack = new Stack<string>();
            Stack<string> redoStack = new Stack<string>();
            
            string currentText = "";

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=================================================");
                Console.WriteLine("          PORTFOLIO LIFO TEXT EDITOR             ");
                Console.WriteLine("=================================================");
                Console.WriteLine($"Current Text: \"{currentText}\"");
                Console.WriteLine("=================================================");
                Console.WriteLine("Commands:  :u (Undo)  |  :r (Redo)  |  :q (Quit)");
                Console.WriteLine("Or just type text and hit Enter to append.");
                Console.WriteLine("=================================================");
                Console.Write("Input: ");

                string input = Console.ReadLine();

                if (string.IsNullOrEmpty(input)) continue;

                // Handle commands
                if (input.Trim() == ":q")
                {
                    break;
                }
                else if (input.Trim() == ":u")
                {
                    if (!undoStack.Empty())
                    {
                        // Save current state to redo stack before moving back
                        redoStack.Push(currentText);
                        currentText = undoStack.Top();
                        undoStack.Pop();
                    }
                    else
                    {
                        Console.WriteLine("\nNothing to undo! Press any key...");
                        Console.ReadKey();
                    }
                }
                else if (input.Trim() == ":r")
                {
                    if (!redoStack.Empty())
                    {
                        // Save current state to undo stack before moving forward
                        undoStack.Push(currentText);
                        currentText = redoStack.Top();
                        redoStack.Pop();
                    }
                    else
                    {
                        Console.WriteLine("\nNothing to redo! Press any key...");
                        Console.ReadKey();
                    }
                }
                else
                {
                    // Regular text entry: Push current state to undo stack
                    undoStack.Push(currentText);
                    
                    // Clear the redo history because a new action breaks the redo chain
                    redoStack.MakeEmpty();

                    // Append text (adds a space if text already exists)
                    if (string.IsNullOrEmpty(currentText))
                        currentText = input;
                    else
                        currentText += " " + input;
                }
            }
        }
    }
}