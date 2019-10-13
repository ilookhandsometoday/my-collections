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
    public class IdealBinaryTree<T> : ICloneable, ICollection<T>
    {
        internal TreeNode<T> Root;
        private int _count;
        private int _capacity;

        public IdealBinaryTree()
        {
            this._capacity = 0;
            this.Count = 0;
            this.Root = null;
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

            this.Root = TreeNode<T>.ConstructIdealTree(this.Capacity);
        }

        public IdealBinaryTree(IdealBinaryTree<T> c)
        {
            this._capacity = c.Capacity;
            this.Count = c.Count;
            this.Root = SerializationCloning.Clone(c.Root);
        }

        public int Count
        {
            get { return this._count; }
            private set { this._count = value; }
        }

        public int Capacity
        {
            get
            {
                return this._capacity;
            }

            set
            {
                if (value < 0)
                {
                    Console.WriteLine("Error: Capacity cannot be < 0");
                    return;
                }

                // setting capacity to less than count won't work
                if (value >= this.Count) 
                {
                    this._capacity = value;
                    List<T> elements = this.PreOrderTraversal().ToList();
                    Queue<T> elementsQueue = new Queue<T>(elements);
                    this.Count = elementsQueue.Count;
                    this.Root = TreeNode<T>.ConstructIdealTree(this.Capacity);
                    if (this.Root != null)
                    {
                        this.Root.Fill(elementsQueue.Count, elementsQueue);
                    }
                }
                else
                {
                    Console.WriteLine("Error: Cannot set Capacity to be less than Count");
                }
            }
        }

        public bool IsReadOnly => false;

        public static void WeakDispose(ref IdealBinaryTree<T> toBeDisposed)
        {
            toBeDisposed.Root = null;
            toBeDisposed = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public object Clone()
        {
            return new IdealBinaryTree<T>(this);
        }

        public IEnumerable<T> LevelOrderTraversal()
        {
            if (this.Root == null)
            {
                yield break;
            }

            TreeNode<T> currentNode = new TreeNode<T>();
            Queue<TreeNode<T>> queue = new Queue<TreeNode<T>>();
            if (this.Root.WasDataModified)
            {
                queue.Enqueue(this.Root);
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

            yield break;
        }
        
        public IEnumerable<T> PreOrderTraversal()
        {
            if (this.Root == null)
            {
                yield break;
            }

            Stack<TreeNode<T>> stack = new Stack<TreeNode<T>>();
            stack.Push(this.Root);

            while (stack.Count > 0)
            {
                for (TreeNode<T> currentNode = stack.Pop(); currentNode != null && currentNode.WasDataModified; currentNode = currentNode.Left)
                {
                    yield return currentNode.Data;
                    if (currentNode.Right != null && currentNode.Right.WasDataModified)
                    {
                        stack.Push(currentNode.Right);
                    }
                }
            }

            yield break;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.LevelOrderTraversal().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Add(T element)
        {
            bool needsResizing = this.Capacity - this.Count > 1 || this.Capacity - this.Count == 0;
            int capacityBefore = this.Capacity;
            if (needsResizing)
            {
                this.Capacity = this.Count;
            }

            if (needsResizing)
            {
                this.Capacity += 1;
            }

            if (this.Count == 0)
            {
                this.Root.Data = element;
                this.Count++;
                if (needsResizing)
                {
                    this.Resize(capacityBefore);
                }

                return;
            }

            TreeNode<T> currentNode = new TreeNode<T>();
            Queue<TreeNode<T>> queue = new Queue<TreeNode<T>>();
            queue.Enqueue(this.Root);

            // looking for a place to insert the element
            while (queue.Count > 0) 
            {
                currentNode = queue.Dequeue();
                if (currentNode.Left != null)
                {
                    if (!currentNode.Left.WasDataModified)
                    {
                        currentNode.Left.Data = element;
                        this.Count++;
                        if (needsResizing)
                        {
                            this.Resize(capacityBefore);
                        }

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
                        this.Count++;
                        if (needsResizing)
                        {
                            this.Resize(capacityBefore);
                        }

                        return;
                    }
                    else
                    {
                        queue.Enqueue(currentNode.Right);
                    }
                }
            }
        }

        public void AddRange(ICollection<T> args)
        {
            foreach (T element in args)
            {
                this.Add(element);
            }
        }

        public bool Contains(T element)
        {
            if (element == null)
            {
                foreach (T entry in this)
                {
                    if (entry == null)
                    {
                        return true;
                    }
                }
            }
            else
            {
                foreach (T entry in this)
                {
                    if (entry.Equals(element))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void Clear()
        {
            this.Root = null;
            this.Count = 0;
        }

        public IdealBinaryTree<T> ShallowCopy()
        {
            IdealBinaryTree<T> clone = new IdealBinaryTree<T>(this.Count);
            clone.AddRange(this.PreOrderTraversal().ToArray());
            return clone;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        private void Resize(int capacityBefore) // helper function for Add()
        {
            if (this.Capacity > capacityBefore)
            {
                this.Capacity *= 2;
            }
            else
            {
                this.Capacity = capacityBefore;
            }
        }
    }
}