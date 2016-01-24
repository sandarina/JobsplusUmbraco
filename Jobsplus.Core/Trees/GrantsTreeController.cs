using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Umbraco.Web.Trees;
using Umbraco.Web.Models.Trees;
using umbraco.BusinessLogic.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;
using Umbraco.Web;
using umbraco.BusinessLogic.Actions;
using Jobsplus.Backoffice.Controllers;
using umbraco;

namespace Jobsplus.Backoffice.Trees
{
    /*
    public class ActionCustom : IAction
    {
        public string Alias { get; set; }

        public bool CanBePermissionAssigned { get; set; }

        public string Icon { get; set; }

        public string JsFunctionName { get; set; }

        public string JsSource { get; set; }

        public char Letter { get; set; }

        public bool ShowInNotifier { get; set; }
    }*/

    [Tree("JobsplusGrants", "JobsplusGrantsTree", "Dotace")]
    [PluginController("JobsplusGrants")]
    public class GrantsTreeController : TreeController
    {

        #region Prefixes
        private const string GrantPrefix = "grant-";
        private const string GrantDefinitionPrefix = "grantdef-";
        #endregion


        protected override MenuItemCollection GetMenuForNode(string id, System.Net.Http.Formatting.FormDataCollection queryStrings)
        {
            var menu = new MenuItemCollection();
            if (id == Constants.System.Root.ToInvariantString())
            {
                // root && grant actions
                menu.Items.Add<CreateChildEntity, ActionNew>(ui.Text("actions", ActionNew.Instance.Alias));
                menu.Items.Add<RefreshNode, ActionRefresh>(ui.Text("actions", ActionRefresh.Instance.Alias), true);
                return menu;
            }
            else if (id.StartsWith(GrantPrefix))
            {
                //menu.Items.Add<CreateChildEntity, ActionNew>(ui.Text("actions", ActionNew.Instance.Alias)).AdditionalData.Add("actionView", "/");
                //var i = menu.Items.Add<CreateChildEntity, ActionNew>(ui.Text("actions", ActionNew.Instance.Alias) );
                //menu.Items.Add<ActionDelete>(ui.Text("actions", ActionDelete.Instance.Alias), "id", id);
                //menu.Items.Add<CreateChildEntity, ActionNew>(ui.Text("actions", ActionNew.Instance.Alias));
                //menu.Items.Add<CreateChildEntity, ActionNew>(ui.Text("actions", ActionNew.Instance.Alias));
                 
                //menu.DefaultMenuAlias = "createGrantDef";
                menu.Items.Add(new MenuItem("createGrantDef", ui.Text("actions", ActionNew.Instance.Alias)) { Icon = "add" });

                menu.Items.Add<ActionDelete>(ui.Text("actions", ActionDelete.Instance.Alias));
                menu.Items.Add<RefreshNode, ActionRefresh>(ui.Text("actions", ActionRefresh.Instance.Alias), true);
            }
            else if (id.StartsWith(GrantDefinitionPrefix))
            {
                menu.Items.Add<ActionDelete>(ui.Text("actions", ActionDelete.Instance.Alias));
                menu.Items.Add<RefreshNode, ActionRefresh>(ui.Text("actions", ActionRefresh.Instance.Alias), true);
            }
            return menu;
        }
    
        protected override TreeNodeCollection GetTreeNodes(string id, System.Net.Http.Formatting.FormDataCollection queryStrings)
        {
            var grants = new GrantsApiController();
            var grantDefs = new GrantDefinitionsApiController();

            //check if we're rendering the root node's children
            if (id == Constants.System.Root.ToInvariantString())
            {
                var nodes = new TreeNodeCollection();

                foreach (var grant in grants.GetAll())
                {
                    string grantId = GrantPrefix + grant.Id.ToString();
                    var node = CreateTreeNode(
                        grantId,
                        "-1",
                        queryStrings,
                        grant.ToString(),
                        "icon-coins-euro-alt",
                        true); 
                    // FormDataCollectionExtensions.GetValue<string>(queryStrings, "application") + StringExtensions.EnsureStartsWith(this.TreeAlias, '/') + "/overviewCalendar/all");

                    // definice dotací
                    /*
                    foreach (var grantDef in grantDefs.GetAll())
                    {
                        var chNode = CreateTreeNode(
                            GrantDefinitionPrefix + grantDef.Id.ToString(),
                            grantId,
                            queryStrings,
                            grantDef.ToString(),
                            "icon-car",
                            false);

                        nodes.Add(chNode);
                    }*/

                    nodes.Add(node);

                }
                return nodes;
            }
            else if (id.StartsWith(GrantPrefix))
            {

                var tree = new TreeNodeCollection();

                foreach (var grantDef in grantDefs.GetAll(int.Parse(id.Replace(GrantPrefix, ""))))
                {
                    var treeNodeId = GrantDefinitionPrefix + grantDef.Id.ToString();
                    tree.Add(CreateTreeNode(
                        treeNodeId,
                        id,
                        queryStrings,
                        grantDef.ToString(), 
                        "icon-calendar", 
                        false,
                        FormDataCollectionExtensions.GetValue<string>(queryStrings, "application") + StringExtensions.EnsureStartsWith(this.TreeAlias, '/') + "/editGrantDef/" + treeNodeId
                        )
                    );
                    //, FormDataCollectionExtensions.GetValue<string>(queryStrings, "application") + StringExtensions.EnsureStartsWith(this.TreeAlias, '/') + "/editCalendar/" + cal.Id));
                }
                return tree;
            }


            //this tree doesn't suport rendering more than 1 level
            throw new NotSupportedException();
        }
    }
}
