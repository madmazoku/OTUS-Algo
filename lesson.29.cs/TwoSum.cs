using System;
using System.Collections.Generic;
using System.Text;

namespace lesson._29.cs
{
    class TwoSum
    {
        public int[] Do(int[] nums, int target)
        {
            Dictionary<int, int> dict = new Dictionary<int, int>();
            for (int i = 0; i < nums.Length; ++i)
            {
                int j = -1;
                if(dict.TryGetValue(target - nums[i], out j))
                    return new int[] { j, i };
                dict[nums[i]] = i;
            }
            return null;
        }
    }
}
