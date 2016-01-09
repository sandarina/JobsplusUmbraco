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
using Umbraco.Web.Mvc;
using Umbraco.Core;
using Jobsplus.Backoffice.Controllers;
using umbraco;

namespace Jobsplus.Backoffice.Trees
{
    [Tree("JobsplusGrants", "JobsplusGrantsTree", "Dotace")]
    [PluginController("JobsplusGrants")]
    public class GrantsTreeController: TreeController
    {
        protected override MenuItemCollection GetMenuForNode(string id, System.Net.Http.Formatting.FormDataCollection queryStrings)
        {
            var menu = new MenuItemCollection();
            if (id == Constants.System.Root.ToInvariantString())
            {
                // root actions              
                menu.Items.Add<CreateChildEntity, ActionNew>(ui.Text("actions", ActionNew.Instance.Alias));
                menu.Items.Add<RefreshNode, ActionRefresh>(ui.Text("actions", ActionRefresh.Instance.Alias), true);
                return menu;
            }
            else
            {
                //menu.DefaultMenuAlias = ActionDelete.Instance.Alias;
                menu.Items.Add< ActionDelete>(ui.Text("actions", ActionDelete.Instance.Alias));
                
            }
            return menu;
        }

        protected override TreeNodeCollection GetTreeNodes(string id, System.Net.Http.Formatting.FormDataCollection queryStrings)
        {
            //check if we're rendering the root node's children
            if (id == Constants.System.Root.ToInvariantString())
            {
                var grants = new GrantsApiController();
                var nodes = new TreeNodeCollection();

                foreach (var grant in grants.GetAll())
                {
                    var node = CreateTreeNode(
                        grant.Id.ToString(),
                        "-1",
                        queryStrings,
                        grant.ToString(),
                        "icon-coins-euro-alt",
                        false);

                    nodes.Add(node);

                }
                return nodes;
            }

            //this tree doesn't suport rendering more than 1 level
            throw new NotSupportedException();
        }
    }
}
