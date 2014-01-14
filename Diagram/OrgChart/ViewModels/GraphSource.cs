using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Diagrams.Extensions.ViewModels;
using Telerik.Windows.Diagrams.Core;

namespace OrgChart.ViewModels
{
	public class GraphSource : ViewModelBase, IObservableGraphSource, INotifyPropertyChanged
	{
		public void PopulateGraphSource(Node node)
		{
			this.AddNode(node);
			foreach (Node subNode in node.Children)
			{
				Link link = new Link(node, subNode);
				this.AddLink(link);
				this.PopulateGraphSource(subNode);
			}
		}

        // --- implement a changing items collection
        public void ChangeItemsCollection()
        {
            this.InternalItems = new ObservableCollection<Node>();
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("Items"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        // ------------ Nicked from ObservableGraphSourceBase ----------

        /// <summary>
        /// Adds a link in the Links collection.
        /// </summary>
        /// <param name="link">The link.</param>
        void IObservableGraphSource.AddLink(ILink link)
        {
            this.AddLink((Link)link);
        }

        /// <summary>
        /// Adds a node in the Items collection.
        /// </summary>
        /// <param name="node">The node.</param>
        void IObservableGraphSource.AddNode(object node)
        {
            this.AddNode((Node)node);
        }

        /// <summary>
        /// Creates a link based on the associated source and target nodes.
        /// </summary>
        /// <param name="source">The source node.</param>
        /// <param name="target">The target node.</param>
        /// <returns>
        /// Returns the created link.
        /// </returns>
        ILink IObservableGraphSource.CreateLink(object source, object target)
        {
            return this.CreateLink(source, target);
        }

        /// <summary>
        /// Creates a node based on an associated shape.
        /// </summary>
        /// <param name="shape">The associated shape.</param>
        /// <returns>
        /// Returns the created node.
        /// </returns>
        object IObservableGraphSource.CreateNode(IShape shape)
        {
            return this.CreateNode(shape);
        }

        /// <summary>
        /// Removes a link from the Links collection.
        /// </summary>
        /// <param name="link">The link.</param>
        /// <returns></returns>
        bool IObservableGraphSource.RemoveLink(ILink link)
        {
            return this.RemoveLink((Link)link);
        }

        /// <summary>
        /// Removes a node from the Items collection.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        bool IObservableGraphSource.RemoveNode(object node)
        {
            return this.RemoveItem((Node)node);
        }

        /// <summary>
        /// Creates a link based on the associated source and target nodes.
        /// </summary>
        /// <param name="source">The source node.</param>
        /// <param name="target">The target node.</param>
        /// <returns>
        /// Returns the created link.
        /// </returns>
        public virtual ILink CreateLink(object source, object target)
        {
            ILink link = System.Activator.CreateInstance<Link>();
            link.Source = source;
            link.Target = target;
            return link;
        }

        /// <summary>
        /// Creates a node based on an associated shape.
        /// </summary>
        /// <param name="shape">The associated shape.</param>
        /// <returns>
        /// Returns the created node.
        /// </returns>
        public virtual object CreateNode(IShape shape)
        {
            return System.Activator.CreateInstance<Node>();
        }

        // ------------ Nicked from GraphSourceBase ----------

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphSourceBase{Node,Link}"/> class.
        /// </summary>
        public GraphSource()
        {
            this.InternalItems = new ObservableCollection<Node>();
            this.InternalLinks = new ObservableCollection<Link>();
        }

        /// <summary>
        /// Gets the internal items collection.
        /// </summary>
        public ObservableCollection<Node> InternalItems { get; private set; }

        /// <summary>
        /// Gets the internal links collection.
        /// </summary>
        public ObservableCollection<Link> InternalLinks { get; private set; }

        /// <summary>
        /// Gets the node items.
        /// </summary>
        public IEnumerable Items
        {
            get
            {
                return this.InternalItems;
            }
        }

        /// <summary>
        /// Gets the links or connections of this diagram source.
        /// </summary>
        public IEnumerable<ILink> Links
        {
            get
            {
                return this.InternalLinks as IEnumerable<ILink>;
            }
        }

        /// <summary>
        /// Adds a node (shape) to this diagram source.
        /// </summary>
        /// <param name="node">
        /// The node to add.
        /// </param>
        public virtual void AddNode(Node node)
        {
            if (!this.InternalItems.Contains(node))
                this.InternalItems.Add(node);
        }

        /// <summary>
        /// Adds the given link to this diagram source.
        /// </summary>
        /// <param name="link">The link to add.</param>
        public virtual void AddLink(Link link)
        {
            this.InternalLinks.Add(link);
        }

        /// <summary>
        /// Removes the link from this source.
        /// </summary>
        /// <param name="link">The link.</param>
        public virtual bool RemoveLink(Link link)
        {
            return this.InternalLinks.Remove(link);
        }

        /// <summary>
        /// Removes the item from the source.
        /// </summary>
        /// <param name="node">The node.</param>
        public virtual bool RemoveItem(Node node)
        {
            return this.InternalItems.Remove(node);
        }

        /// <summary>
        /// Removes all items and links from this source.
        /// </summary>
        public virtual void Clear()
        {
            this.InternalLinks.Clear();
            this.InternalItems.Clear();
        }

        
    }
}
