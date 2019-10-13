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

        public TreeNode()
        {
            this.WasDataModified = false;
            this.data = default(T);
            this.Left = null;
            this.Right = null;
        }

        public TreeNode(T d)
        {
            this.WasDataModified = true;
            this.data = d;
            this.Left = null;
            this.Right = null;
        }

        public T Data
        {
            get
            {
                return this.data;
            }

            set
            {
                this.data = value;
                this.WasDataModified = true;
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

        internal bool WasDataModified // helper property to make Add() in IdealBinaryTree class possible;
        {                             // as Capacity has to be implemented, a way to judge if a node exists
            get;                      // or not is required (from the user's perspective a node could be a leaf)
            set;                      // while on the inside it could be not a leaf for a certain combination
        }                             // of Count and Capacity

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

        public void Fill(int numberOfElementsToFill, Queue<T> elements) // helper method for resizing the IdealBinaryTree
        {
            if (numberOfElementsToFill > 0 && elements.Count > 0)
            {
                int numberOfElementsToFillLeft = numberOfElementsToFill / 2;
                int numberOfElementsToFillRight = numberOfElementsToFill - numberOfElementsToFillLeft - 1;
                this.Data = elements.Dequeue();
                if (this.Left != null)
                {
                    this.Left.Fill(numberOfElementsToFillLeft, elements);
                }

                if (this.Right != null)
                { 
                    this.Right.Fill(numberOfElementsToFillRight, elements);
                }
            }
        }

        public void Show(string indent, bool last, string location)
        {
            if (this != null && this.WasDataModified)
            {
                Console.WriteLine($"{indent}+----{location}:({this.Data})");
                indent += last ? "     " : "|    ";
                if ((this.Left != null && this.Left.WasDataModified) && (this.Right != null && this.Right.WasDataModified))
                {
                    this.Right.Show(indent, false, "R");
                    this.Left.Show(indent, true, "L");
                }
                else if ((this.Left != null && this.Left.WasDataModified) && (this.Right == null || !this.Right.WasDataModified))
                {
                    this.Left.Show(indent, true, "L");
                }
                else if ((this.Left == null || !this.Left.WasDataModified) && (this.Right != null && this.Right.WasDataModified))
                {
                    this.Right.Show(indent, true, "R");
                }
            }
            else
            {
                Console.WriteLine("The tree you are trying to print out is empty");
            }
        }
    }
}
