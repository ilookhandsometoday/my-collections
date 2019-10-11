﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepCloning;

namespace MyCollections
{
    [Serializable]
    public class IdealBinaryTree<T> : ICloneable, IEnumerable<T>
    {
        internal TreeNode<T> _root;
        private int _count;
        private int _capacity;

        public int Count
        {
            get {return this._count; }
            private set { this._count = value;}
        }

        public int Capacity
        {
            get { return this._capacity; }
            set { this._capacity = value; }
        }

        public IdealBinaryTree()
        {
            this.Capacity = 0;
            this.Count = 0;
            this._root = null;
        }

        public IdealBinaryTree(int capacity)
        {
            if (capacity >= 0)
            {
                this.Capacity = capacity;
            }
            else
            {
                Console.WriteLine("Error: Capacity cannot be < 0. Capacity will be set to 0");
                this.Capacity = 0;
            }

            this.Count = 0;
            this._root = TreeNode<T>.ConstructIdealTree(this.Capacity);
        }

        public IdealBinaryTree(IdealBinaryTree<T> c)
        {
            this.Capacity = c.Capacity;
            this.Count = c.Count;
            this._root = SerializationCloning.Clone(c._root);
        }

        public object Clone()
        {
            return new IdealBinaryTree<T>(this);

        }

        public IEnumerator<T> GetEnumerator()
        {
            return new InOrderEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
