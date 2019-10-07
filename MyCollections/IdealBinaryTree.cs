using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepCloning;

namespace MyCollections
{
    [Serializable]
    public class IdealBinaryTree<T> : ICloneable
    {
        private TreeNode<T> root;

        public int Count
        {
            get;
            set;
        }

        public int Capacity
        {
            get;
            set;
        }

        public IdealBinaryTree()
        {
            this.Capacity = 0;
            this.Count = 0;
            this.root = null;
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
            this.root = TreeNode<T>.ConstructIdealTree(this.Capacity);
        }

        public IdealBinaryTree(IdealBinaryTree<T> c)
        {
            this.Capacity = c.Capacity;
            this.Count = c.Count;
            this.root = SerializationCloning.Clone(c.root);
        }

        public object Clone()
        {
            return new IdealBinaryTree<T>(this);
        }
    }
}
