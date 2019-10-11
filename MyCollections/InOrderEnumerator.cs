using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepCloning;

namespace MyCollections
{
    public class InOrderEnumerator<T> : IEnumerator<T>
    {
        private int currentIndex;
        readonly List<T> treeAsList;
        public InOrderEnumerator(IdealBinaryTree<T> tree)
        {
            this.currentIndex = -1;
            this.treeAsList = new List<T>();
            int treeCount = tree.Count;
            TreeNode<T>.ToListInOrder(tree._root, ref this.treeAsList, ref treeCount);
        }

        public object Current
        {
            get
            {
                T currentElement = this.treeAsList[currentIndex];
                return SerializationCloning.Clone(currentElement);
            }
        }

        T IEnumerator<T>.Current
        {
            get
            {
                T currentElement = this.treeAsList[currentIndex];
                return SerializationCloning.Clone(currentElement);
            }
        }

        public void Dispose() {}

        public bool MoveNext()
        {
            return this.currentIndex++ < this.treeAsList.Count - 1;
        }

        public void Reset()
        {
            this.currentIndex = -1;
        }
    }
}
