using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace DashboardSite.Core.Utilities
{
    [Serializable]
    public abstract class TreeNode<T> : ITreeNode<T> where T : class, ITreeNode<T>, new()
    {
        private List<T> _allChildrenInnerList = new List<T>();
        private List<T> _childrenInnerList = new List<T>();

        protected TreeNode()
        {
            Children = new ReadOnlyCollection<T>(_childrenInnerList);
            NodeId = Guid.NewGuid();
        }

        public Guid NodeId { get; private set; }
        public T Parent { get; private set; }
        public ReadOnlyCollection<T> Children { get; private set; }
        public int Level
        {
            get { return this.Parent != null ? Parent.Level + 1 : 0; }
        }

        public object Clone()
        {
            return DeepCopy();
        }

        public abstract T ShallowCopy();

        public bool FullEquals(T node)
        {
            return this.Except(node).Count() == 0;
        }

        /// <summary>
        /// Be carefull!
        /// Do not try to invoke this method for each node of the tree!
        /// You'll get the set of separated trees!
        /// Just do it once for root node of the tree!
        /// </summary>
        /// <returns>Deep copy of the tree</returns>
        public virtual T DeepCopy()
        {
            using (Stream stream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
                stream.Position = 0;
                var result = (T)formatter.Deserialize(stream);
                result.InitParents();
                return result;
            }
        }

        public override bool Equals(object obj)
        {
            return Equals((T)obj);
        }

        public virtual bool Equals(T other)
        {
            return NodeId == other.NodeId;
        }

        public override int GetHashCode()
        {
            return NodeId.GetHashCode();
        }

        public void SetParent(T parent)
        {
            Parent = parent;
            InternalStateChanged();
        }

        public virtual void AddChild(T child)
        {
            child.SetParent(this as T);
            _childrenInnerList.Add(child);
            InternalStateChanged();
        }

        public virtual void AddChildren(params T[] children)
        {
            AddChildren(children.AsEnumerable());
        }

        public virtual void AddChildren(IEnumerable<T> children)
        {
            foreach (T child in children)
            {
                child.SetParent(this as T);
                _childrenInnerList.Add(child);
            }
            InternalStateChanged();
        }

        #region IEnumerable<T> implementation & supporting methods
        private void InternalStateChanged()
        {
            _allChildrenInnerList.Clear();
        }

        private void InitInnerEnumerableList(T node)
        {
            _allChildrenInnerList.Add(node);
            foreach (var treeNode in node.Children)
            {
                InitInnerEnumerableList(treeNode);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            _allChildrenInnerList.Clear();
            InitInnerEnumerableList(this as T);
            return _allChildrenInnerList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}