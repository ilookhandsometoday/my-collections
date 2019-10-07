﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            this.Data = default(T);
            this.Left = null;
            this.Right = null;
        }

        public TreeNode(T d)
        {
            this.Data = d;
            this.Left = null;
            this.Right = null;
        }

        public T Data
        {
            get { return this.data; }
            set { this.data = value; }
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
    }
}
