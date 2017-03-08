using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace Sample.Azure.DI
{
    /// <summary>
    /// To swap out Unity for another DI framework, you would need to replace the below code
    /// You would also have to remove Unity from the NuGet references and then add the new DI framework
    /// </summary>
    public class Container
    {
        /// <summary>
        /// Single instance of our DI container
        /// </summary>
        private static readonly UnityContainer unityContainer = new UnityContainer();


        /// <summary>
        /// Would like to hide this at all costs!
        /// Returns the container
        /// </summary>
        /// <returns></returns>
        //public static UnityContainer GetContainer()
        //{
        //    return unityContainer;
        //}

        /// <summary>
        /// Registers a type for resolving to a class during injection
        /// </summary>
        /// <typeparam name="I"></typeparam>
        /// <typeparam name="T"></typeparam>
        public static void RegisterType<I, T>() where T : I
        {
            unityContainer.RegisterType<I, T>();
        }


        /// <summary>
        /// Registers a type for resolving to a class instance during injection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="I"></typeparam>
        public static void RegisterInstance<I>(I instance)
        {
            unityContainer.RegisterInstance<I>(instance);
        }


        /// <summary>
        /// Returns an instance of the requested class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Resolve<T>()
        {
            return unityContainer.Resolve<T>();
        }


        /// <summary>
        /// Helps with debugging what is regrister
        /// </summary>
        /// <param name="unityContainer"></param>
        /// <returns></returns>
        public static List<string> ShowRegistrations()
        {
            List<string> resultList = new List<string>();
            foreach (ContainerRegistration item in unityContainer.Registrations)
            {
                resultList.Add(item.GetMappingAsString());
            }
            return resultList;
        } // ShowRegistrations

    } // class
} // namespace