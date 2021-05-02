using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace titi.Areas.Admin.Models
{
    public class TreeViewModule
    {
        public long Id { get; set; }
        public long Parent { get; set; }
        public string Name { get; set; }
        public IList<TreeViewModule> Children = null;
        public TreeViewModule()
        {
            Children = new List<TreeViewModule>();
        }
    }
    public class TreeViewNode
    {
        public long Id { get; set; }
        public long Parent { get; set; }
        public string Name { get; set; }
        public IList<TreeViewNode> Children = null;
        public TreeViewNode()
        {
            Children = new List<TreeViewNode>();
        }
    }
    public class RecursiveObject
    {
        public string data { get; set; }
        public Int64 id { get; set; }
        public FlatTreeAttribute attr { get; set; }
        public List<RecursiveObject> children { get; set; }
    }
    public class FlatTreeAttribute
    {
        public Int64 id;
        public bool selected;
        public bool opened;
    }
    public class FlatObject
    {
        public Int64 Id { get; set; }
        public Int64 ParentId { get; set; }
        public string data { get; set; }
        public FlatObject(string name, Int64 id, Int64 parentId)
        {
            data = name;
            Id = id;
            ParentId = parentId;
        }
    }
}