using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepCloning;

namespace MyCollections
{
    public class InOrderEnumerator<T>: IEnumerator<T>
    {
        private Queue<TreeNode<T>> treeAsQueue;
        public InOrderEnumerator(IdealBinaryTree<T> tree)
        {
            treeAsQueue = new Queue<TreeNode<T>>();
            if (tree == null)
            {
                throw new NullReferenceException(); 
            }
            treeAsQueue.Enqueue(tree._root);
        }

        public T Current
        {
            get { return SerializationCloning.Clone(treeAsQueue.Peek()).Data; }
        }
    }
}
