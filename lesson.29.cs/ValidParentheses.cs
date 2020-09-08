using System;
using System.Collections.Generic;
using System.Text;

namespace lesson._29.cs
{
    class ValidParentheses
    {
        bool IsOpen(char c)
        {
            return c == '(' || c == '{' || c == '[';
        }

        bool IsCorrect(char a, char b)
        {
            return (a == '(' && b == ')') || (a == '{' && b == '}') || (a == '[' && b == ']');
        }

        public bool Do(string s)
        {
            Stack<char> stack = new Stack<char>();
            foreach(char c in s)
            {
                if (IsOpen(c))
                    stack.Push(c);
                else
                {
                    if (stack.Count == 0)
                        return false;
                    if (!IsCorrect(stack.Pop(), c))
                        return false;
                }
            }
            return stack.Count == 0;
        }
    }
}
