using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DashboardSite.Core.Utilities
{
    public interface ITreeNode<T> : IEnumerable<T>, ICloneable, IEquatable<T> where T : class, ITreeNode<T>
    {
        T Parent { get; }
        ReadOnlyCollection<T> Children { get; }
        Guid NodeId { get; }
        int Level { get; }
        void SetParent(T parent);
        void AddChild(T child);
        void AddChildren(IEnumerable<T> child);
        T ShallowCopy();
        bool FullEquals(T node);
    }
}
