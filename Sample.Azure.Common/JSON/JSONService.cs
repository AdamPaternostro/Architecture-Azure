using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Sample.Azure.Common.JSON
{
    public class JSONService
    {
        /// <summary>
        /// Serializes a model to JSON string
        /// This works for when you have a polymorphic list of objects (e.g. List<IThing>() )
        /// </summary>
        public string SerializeKnownTypes<T>(T model)
        {
            return JsonConvert.SerializeObject(model, Formatting.None, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Binder = GetKnownTypesBinder
            });
        } // SerializeKnownTypes


        /// <summary>
        /// Serializes a model to JSON string
        /// </summary>
        public string Serialize<T>(T model)
        {
            return JsonConvert.SerializeObject(model);
        } // Serialize


        /// <summary>
        /// Deserilaizes a model.  Works with a polymorphic list of objects
        /// </summary>
        public T DeserializeKnownTypes<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Binder = Common.JSON.JSONService.GetKnownTypesBinder
            });
        } // Deserialize


        /// <summary>
        /// Deserilaizes a model.  Ignores missing fields
        /// </summary>
        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        } // Deserialize


        /// <summary>
        /// Backing variable
        /// </summary>
        private static KnownTypesBinder knownTypesBinder = null;

        /// <summary>
        /// This must be maintained in order to have polymorphic lists (of Interfaces) so JSON.NET knows how to deserialize each 
        /// generic type.  JSON.NET does not really know the type since each type can be different in a generic list.
        /// </summary>
        public static KnownTypesBinder GetKnownTypesBinder
        {
            get
            {
                if (knownTypesBinder == null)
                {
                    knownTypesBinder = new KnownTypesBinder
                    {
                        KnownTypes = new List<Type> {
                        //typeof(Model.Atom.AtomModel),
                        //typeof(Model.Atom.AtomFieldTypeNameValuePairModel),
                        //typeof(Model.Atom.AtomFieldTypeQuizTextModel),
                        //typeof(Model.Atom.AtomFieldTypeValueModel),
                        //typeof(Model.Atom.AtomFieldTypeQuizChoice),
                        //typeof(Model.Atom.AtomFieldTypeXmlModel),
                        //typeof(Model.Molecule.MoleculeModel),
                        //typeof(Model.Molecule.MoleculeArtifactDocument),
                        //typeof(Model.Molecule.MoleculeArtifactTag),
                        //typeof(Model.Molecule.MoleculeArtifactTerm),
                        //typeof(Model.Term.TermModel)
                    }
                    };
                }

                return knownTypesBinder;
            }
        } // GetKnownTypesBinder

    } // class

} // namepsace

