using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Sample.Azure.Common.Validation
{
    public class ValidationService
    {
        public ValidationService() { }

        /// <summary>
        /// Return a list of broken business rules (data annotations)
        /// </summary>
        /// <returns></returns>
        public ICollection<ValidationResult> BrokenRules<TModel>(TModel model)
        {
            // Need to add data annotations of the associated metadataclass
            // Check to see if we have a meta data tag for this class (This should be cached?)
            MetadataTypeAttribute metadataTypeAttribute =
                        (MetadataTypeAttribute)Attribute.GetCustomAttribute(typeof(TModel), typeof(MetadataTypeAttribute));
            if (metadataTypeAttribute != null)
            {
                // Add the meta data class so we get the additional data annotations
                System.ComponentModel.TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(TModel),
                    metadataTypeAttribute.MetadataClassType.UnderlyingSystemType), typeof(TModel));
            }

            ValidationContext validationContext = new ValidationContext(model, null, null);
            List<ValidationResult> validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            return validationResults;
        } // BrokenRules


        /// <summary>
        /// Validates the data annotations on a model (POCO) and throws an exception 
        /// </summary>
        /// <param name="o"></param>
        public void ValidateObject<TModel>(TModel model)
        {
            List<Sample.Azure.Model.BrokenRule.BrokenRuleModel> brokenRuleList = new List< Sample.Azure.Model.BrokenRule.BrokenRuleModel>();

            if (model == null)
            {
                Sample.Azure.Model.BrokenRule.BrokenRuleModel brokenRule = new Sample.Azure.Model.BrokenRule.BrokenRuleModel();
                brokenRule.PropertyName = string.Empty; // since it applies to the whole object
                brokenRule.Message = "Object is null and cannot be validated.";
                brokenRuleList.Add(brokenRule);
                throw new Sample.Azure.Common.CustomException.ModelValidationException(brokenRuleList);
            }
            else
            {
                ICollection<ValidationResult> validationResultList = this.BrokenRules(model);

                if (validationResultList.Count == 0) return;

                foreach (ValidationResult validationResult in validationResultList)
                {
                    Sample.Azure.Model.BrokenRule.BrokenRuleModel brokenRule = new Sample.Azure.Model.BrokenRule.BrokenRuleModel();

                    if (validationResult.MemberNames != null && validationResult.MemberNames.Count() > 0)
                    {
                        brokenRule.PropertyName = validationResult.MemberNames.First();
                    }

                    brokenRule.Message = validationResult.ErrorMessage;
                    brokenRuleList.Add(brokenRule);
                } // foreach

                throw new Sample.Azure.Common.CustomException.ModelValidationException(brokenRuleList);
            }
        } // ValidateObject  

    } // class
} // namespace