using System;
using System.Collections.Generic;
using System.Linq;

namespace DashboardSite.Core.Utilities
{
    public static class TreesHelper
    {
        /// <summary>
        /// Returns first occurrence of node satisfies certain predicate
        /// </summary>
        /// <param name="list">Wood of trees</param>
        /// <param name="predicate">Predicate</param>
        /// <returns></returns>
        public static T Seek<T>(this List<T> list, Func<T, bool> predicate) where T : class, ITreeNode<T>
        {
            foreach (var node in list)
            {
                var childNode = node.Seek(predicate);
                if (childNode != null)
                    return childNode;
            }

            return null;
        }

        /// <summary>
        /// Returns first occurrence of parent node satisfies certain predicate
        /// </summary>
        /// <param name="node">Tree's node</param>
        /// <param name="predicate">Predicate</param>
        /// <returns></returns>
        public static T SeekParent<T>(this T node, Func<T, bool> predicate) where T : class, ITreeNode<T>
        {
            if (predicate(node))
                return node;

            if (node.IsRoot())
                return null;

            if (predicate(node.Parent))
                return node.Parent;

            node.Parent.SeekParent(predicate);

            return null;
        }

        public static void ForEach<T>(this T node, Action<T> action) where T : class, ITreeNode<T>
        {
            foreach (var treeNode in node)
            {
                action(treeNode);
            }
        }

        /// <summary>
        /// Returns first occurrence of node satisfies certain predicate (Full scan of tree regardless of node level.)
        /// </summary>
        /// <param name="node">Tree's node</param>
        /// <param name="predicate">Predicate</param>
        /// <returns></returns>
        public static T Seek<T>(this T node, Func<T, bool> predicate) where T : class, ITreeNode<T>
        {
            return node.IsRoot() ? node.FirstOrDefault(x => predicate(x)) : node.GetRoot().FirstOrDefault(x => predicate(x));
        }

        public static List<T> GetLeafs<T>(this T root) where T : class, ITreeNode<T>
        {
            var result = new List<T>();
            foreach (T node in root)
            {
                if (node.Children.Count == 0)
                    result.Add(node);
            }
            return result;
        }

        public static T GetRoot<T>(this T node) where T : class, ITreeNode<T>
        {
            return node.Parent != null ? node.Parent.GetRoot() : node;
        }

        public static void InitParents<T>(this List<T> trees) where T : class, ITreeNode<T>
        {
            trees.ForEach(x => x.InitParents());
        }

        public static void InitParents<T>(this T root) where T : class, ITreeNode<T>
        {
            foreach (var child in root.Children)
            {
                child.SetParent(root);
                child.InitParents();
            }
        }

        public static bool IsRoot<T>(this T node) where T : class, ITreeNode<T>
        {
            return node.Parent == null;
        }

        public static bool IsLeaf<T>(this T node) where T : class, ITreeNode<T>
        {
            return (node.Children == null || node.Children.Count == 0);
        }

        /// <summary>
        /// TODO: Test
        /// </summary>
        /// <param name="list1">List of tree nodes</param>
        /// <param name="list2">List of tree nodes</param>
        /// <returns>Result of intersect</returns>
        public static List<T> Intersect<T>(this List<T> list1, List<T> list2) where T : class, ITreeNode<T>, new()
        {
            var result = new List<T>();
            result.AddRange(Enumerable.Intersect(list1, list2).Select(x => x.ShallowCopy()));
            foreach (T node in result)
            {
                var node1 = list1.FirstOrDefault(x => x.Equals(node));
                var node2 = list2.FirstOrDefault(x => x.Equals(node));
                if (node1 != null && node2 != null)
                {
                    node.AddChildren(Intersect(node1.Children.ToList(), node2.Children.ToList()));
                } 
            }
            return result;
        }
    }
}
