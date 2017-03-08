using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Azure.Service.LargeObject
{
    public class LargeObjectService : BaseService, Interface.Service.ILargeObjectService
    {
        Interface.Repository.ILargeObjectRepository largeObjectRepository = null;

        public LargeObjectService(Interface.Repository.ILargeObjectRepository largeObjectRepository)
        {
            this.largeObjectRepository = largeObjectRepository;
        }


        /// <summary>
        /// Gets a largeObject based upon their largeObject key 
        /// </summary>
        /// <param name="largeObjectKey"></param>
        /// <returns></returns>
        public Model.LargeObject.LargeObjectModel Get(int largeObjectId)
        {
            if (largeObjectId == 0)
            {
                List<Model.BrokenRule.BrokenRuleModel> brokenRuleList = new List<Model.BrokenRule.BrokenRuleModel>();
                brokenRuleList.Add(new Model.BrokenRule.BrokenRuleModel() { Message = "Please provide a largeObject id.", PropertyName = "LargeObjectId" });
                throw new Common.CustomException.ModelValidationException(brokenRuleList);
            }
            return largeObjectRepository.Select(largeObjectId);
        }


        /// <summary>
        /// Validates and saves a largeObject
        /// </summary>
        /// <param name="largeObjectModel"></param>
        public void Save(Model.LargeObject.LargeObjectModel largeObjectModel)
        {
            Common.Validation.ValidationService validationService = new Common.Validation.ValidationService();
            validationService.ValidateObject(largeObjectModel);

            largeObjectRepository.InsertOrReplace(largeObjectModel);
        }

    }
}
