using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepCloning;

namespace MyCollections
{
    [Serializable]
    internal class TreeNode<T>
    {
        
        private T data;
        private TreeNode<T> left;
        private TreeNode<T> right;

        internal bool WasDataModified // helper property to make Add() in IdealBinaryTree class possible;
        {                             // as Capacity has to be implemented, a way to judge if a node exists
            get;                      // or not is required (from the user's perspective a node could be a leaf)
            set;                      // while on the inside it could be not a leaf for a certain combination
        }                             // of Count and Capacity

        public TreeNode()
        {
            this.WasDataModified = false;
            this.Data = default(T);
            this.Left = null;
            this.Right = null;
        }

        public TreeNode(T d)
        {
            this.WasDataModified = false;
            this.Data = d;
            this.Left = null;
            this.Right = null;
        }

        public T Data
        {
            get { return this.data; }
            set
            {
                this.data = value;
            }
        }

        public TreeNode<T> Left
        {
            get { return this.left; }
            set { this.left = value; }
        }

        public TreeNode<T> Right
        {
            get { return this.right; }
            set { this.right = value; }
        }

        public static TreeNode<T> ConstructIdealTree(int size)
        {
            TreeNode<T> root = null;
            if (size > 0)
            {
                int sizeLeft = size / 2;
                int sizeRight = size - sizeLeft - 1;
                root = new TreeNode<T>();
                root.Left = TreeNode<T>.ConstructIdealTree(sizeLeft);
                root.Right = TreeNode<T>.ConstructIdealTree(sizeRight);
            }
            else if (size < 0)
            {
                Console.WriteLine("Error: Size of the tree cannot be < 0");
            }
            return root;
        }

        public int Height
        {
            get
            {
                int heightLeft = 0;
                int heightRight = 0;
                if (this.Left != null)
                {
                    heightLeft = this.Left.Height;
                }

                if (this.Right != null)
                {
                    heightRight = this.Right.Height;
                }

                return Math.Max(heightLeft, heightRight) + 1;
            }
        }
    }
}
