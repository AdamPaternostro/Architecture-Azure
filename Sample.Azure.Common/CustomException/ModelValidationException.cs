using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Azure.Common.CustomException
{
    public class ModelValidationException : Exception
    {
        private List<Sample.Azure.Model.BrokenRule.BrokenRuleModel> brokenRuleList = new List<Sample.Azure.Model.BrokenRule.BrokenRuleModel>();

        public ModelValidationException(List<Sample.Azure.Model.BrokenRule.BrokenRuleModel> brokenRuleList)
        {
            this.brokenRuleList = brokenRuleList;
        }

        public List<Sample.Azure.Model.BrokenRule.BrokenRuleModel> BrokenRuleList
        {
            get { return this.brokenRuleList; }
        }

        public string BrokenRuleListJSON
        {
            get 
            {
                JSON.JSONService jsonService = new JSON.JSONService();
                string result = jsonService.Serialize<List<Sample.Azure.Model.BrokenRule.BrokenRuleModel>>(this.brokenRuleList);
                return result;
            }
        }

    } // class
} // namespace
