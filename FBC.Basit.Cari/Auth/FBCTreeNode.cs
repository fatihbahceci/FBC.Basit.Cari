namespace FBC.Basit.Cari.Auth
{
    public class FBCTreeNode<T>
    {
        private FBCTreeNode<T>? parent;
        private List<FBCTreeNode<T>> childs;
        private T _data;

        public FBCTreeNode(T data)
        {
            this._data = data;
            childs = new List<FBCTreeNode<T>>();
        }


        public void AddChild(FBCTreeNode<T> node)
        {
            this.childs.Add(node);
            node.parent = this;
        }

        public void RemoveChild(FBCTreeNode<T> node)
        {
            if (this.childs.Remove(node))
            {
                node.parent = null;
            }
        }

        public void RemoveParent()
        {
            if (this.parent != null)
            {
                this.parent.RemoveChild(this);
            }
            this.parent = null;
        }

        public FBCTreeNode<T> CreateChild(T data)
        {
            var node = new FBCTreeNode<T>(data);
            this.AddChild(node);
            return node;
        }

        public FBCTreeNode<T>? Parent => parent;
        public T Data => _data;
        public IReadOnlyList<FBCTreeNode<T>> Childs => childs.AsReadOnly();

        internal void AddChilds(List<FBCTreeNode<T>> items)
        {
            foreach (var item in items) AddChild(item);
        }
    }
}