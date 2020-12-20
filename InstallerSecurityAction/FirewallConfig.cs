using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFwTypeLib;
using Microsoft.Win32;

namespace InstalerSecurityAction
{
    public class FirewallConfig
    {
        protected int[] DiscoPorts = { 6112 };
        protected INetFwProfile FwProfile;
        string ApplicationPath;
        NET_FW_RULE_DIRECTION_[] directions = { NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT, NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN };

        public FirewallConfig (string applicationPath)
        {
            ApplicationPath = applicationPath;
        }

        public void OpenFirewall()
        {
            

            foreach (NET_FW_RULE_DIRECTION_ direction in directions)
            {
                Type tNetFwPolicy2 = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
                INetFwPolicy2 fwPolicy2 = (INetFwPolicy2)Activator.CreateInstance(tNetFwPolicy2);

                INetFwRule2 firewallRule = (INetFwRule2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
                firewallRule.Enabled = true;
                firewallRule.Action = NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
                firewallRule.Protocol = 6; // TCP
                firewallRule.LocalPorts = "6112";
                firewallRule.Name = "Avocados Wars";
                firewallRule.ApplicationName = ApplicationPath;
                firewallRule.Direction = direction;
                firewallRule.Profiles = (int)NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_ALL;

                INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                firewallPolicy.Rules.Add(firewallRule);
            }
        }

        public void CloseFirewall()
        {
            INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));

            foreach (NET_FW_RULE_DIRECTION_ direction in directions)
            {
                firewallPolicy.Rules.Remove("Avocados Wars");
            }
        }

    }
}
