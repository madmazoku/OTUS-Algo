namespace lesson._29.cs
{
    public class TreeNode
    {
        public int val;
        public TreeNode left;
        public TreeNode right;
        public TreeNode(int val = 0, TreeNode left = null, TreeNode right = null)
        {
            this.val = val;
            this.left = left;
            this.right = right;
        }
    }

    class SumOfLeftLeaves
    {
        public int Do(TreeNode root)
        {
            int sum = 0;
            down(root, false, ref sum);
            return sum;
        }

        void down(TreeNode root, bool left, ref int sum)
        {
            if (root == null) return;
            if (left && root.left == null && root.right == null) sum += root.val;
            down(root.left, true, ref sum);
            down(root.right, false, ref sum);
        }
    }
}
