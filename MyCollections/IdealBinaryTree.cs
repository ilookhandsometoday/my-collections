using System;
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
            get { return this._count; }
            private set { this._count = value; }
        }

        public int Capacity
        {
            get { return this._capacity; }
            set
            {
                if (value < 0)
                {
                    Console.WriteLine("Error: Capacity cannot be < 0");
                    return;
                }

                this._capacity = value;
                List<T> elements = this.LevelOrderTraversal().ToList();
                if (value < this._capacity && value < this.Count)
                {
                    elements.RemoveRange(value, this.Count - value);
                }

                Queue<T> elementsQueue = new Queue<T>(elements);
                this.Count = elementsQueue.Count;
                this._root = TreeNode<T>.ConstructIdealTree(this._capacity, elementsQueue);
            }
        }

        public IdealBinaryTree()
        {
            this._capacity = 0;
            this.Count = 0;
            this._root = null;
        }

        public IdealBinaryTree(int capacity)
        {
            this.Count = 0;
            if (capacity >= 0)
            {
                this._capacity = capacity;
            }
            else
            {
                Console.WriteLine("Error: Capacity cannot be < 0. Capacity will be set to 0");
            }

            this._root = TreeNode<T>.ConstructIdealTree(this.Capacity);
        }

        public IdealBinaryTree(IdealBinaryTree<T> c)
        {
            this._capacity = c.Capacity;
            this.Count = c.Count;
            this._root = SerializationCloning.Clone(c._root);
        }

        public object Clone()
        {
            return new IdealBinaryTree<T>(this);
        }

        public IEnumerable<T> LevelOrderTraversal()
        {
            if (this._root == null)
            {
                yield break;
            }

            TreeNode<T> currentNode = new TreeNode<T>();
            Queue<TreeNode<T>> queue = new Queue<TreeNode<T>>();
            if (this._root.WasDataModified)
            {
                queue.Enqueue(this._root);
            }

            while (queue.Count > 0)
            {
                currentNode = queue.Dequeue();
                yield return currentNode.Data;

                if (currentNode.Left != null && currentNode.Left.WasDataModified)
                {
                    queue.Enqueue(currentNode.Left);
                }

                if (currentNode.Right != null && currentNode.Right.WasDataModified)
                {
                    queue.Enqueue(currentNode.Right);
                }
            }
        }

        //public IEnumerable<T> InOrderTraversal()
        //{

        //}

        public IEnumerator<T> GetEnumerator() //TODO rewrite as inorder
        {
            Stack<TreeNode<T>> stack = new Stack<TreeNode<T>>();
            if (this._root != null && this._root.WasDataModified)
            {
                stack.Push(this._root);
            }

            while (stack.Count > 0)
            {
                for (TreeNode<T> currentElement = stack.Pop(); currentElement != null && currentElement.WasDataModified; currentElement = currentElement.Left)
                {
                    yield return currentElement.Data;
                    if (currentElement.Right != null && currentElement.Right.WasDataModified)
                    {
                        stack.Push(currentElement.Right);
                    }
                }
            }

            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Add(T element)
        {
            int capacityBefore = this.Capacity;
            if (this.Count < this.Capacity)
            {
                this.Capacity = this.Count;
            }

            this.Capacity += 1; // in case Capacity is 0;
            if (this.Count == 0)
            {
                this._root.Data = element;
                this._root.WasDataModified = true;
                this.Count++;
                return;
            }

            TreeNode<T> currentNode = new TreeNode<T>();
            Queue<TreeNode<T>> queue = new Queue<TreeNode<T>>();
            queue.Enqueue(this._root);

            while (queue.Count > 0) // looking for a place to insert the element
            {
                currentNode = queue.Dequeue();
                if (currentNode.Left != null)
                {
                    if (!currentNode.Left.WasDataModified)
                    {
                        currentNode.Left.Data = element;
                        currentNode.Left.WasDataModified = true;
                        this.Count++;
                        return;
                    }
                    else
                    {
                        queue.Enqueue(currentNode.Left);
                    }
                }


                if (currentNode.Right != null)
                {
                    if (!currentNode.Right.WasDataModified)
                    {
                        currentNode.Right.Data = element;
                        currentNode.Right.WasDataModified = true;
                        this.Count++;
                        return;
                    }
                    else
                    {
                        queue.Enqueue(currentNode.Right);
                    }
                }
            }

            if (this.Capacity > capacityBefore)
            {
                this.Capacity *= 2;
            }
            else
            {
                this.Capacity = capacityBefore;
            }
        }


        public void AddRange(ICollection<T> args)
        {
            foreach (T element in args)
            {
                this.Add(element);
            }
        }
    }
}