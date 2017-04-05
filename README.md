# Azure-Sample-Web-REST-Architecture
Sample Azure Architecture that I use on all my personal projects for creating a flexible way to change Azure services without rewriting my business code.  To me, it is clean, easy to debug and easy to know where to put various portions of code.

##Why this Architecture:
- It treats each Azure service as a Repository.  I never know if I will be using SQL Server, Azure Table, DocumentDB, etc... and like to adjust my code depending on how things change in the project.  I might start off logging Exceptions to my database and then realized it is cheaper and faster to store them in an Azure table (which takes the load off my database).  Also, Azure is constantly changing and I want to use new features by re-arranging my code (not rewriting)
- My business tier is pure .NET (or Java) code.  This same architecture can be used in Java.  The architecture is very much centered around organization and not fancy coding techniques.  I don't think much code needs to be complex and breaking complex designs into simple pieces is what I like to do. 
- I used dependency injection as clean as I can so my repositories can be swapped out with just a one line of code.  I personally think dependency injection can make code hard to read, debug and just painful.  I keep it isolated in this architecture.


##Layers of this Architecture

**Frontend Tier(s)** - Web project, Windows service, Console application (some type of "user" interface).  This project should not contain alot of code (except HTML).  You should be able to put "1 line of code" in your console app and the same "1 line of code" in your Windows service.  The line of code should call to your services tier to "start itself".  

**RESTful tier(s)** - APIs that I will put in front of my Services Tier.  This is a pass-through tier that calls my Services tier (again it should not contain a lot of code).  I do use the REST tier to do some security.  I will strip fields from my outbound JSON during its serialization.  I will also not map fields that are posted by a user to fields they do not have permission to update.  This is important if you "expose" a large model like "customer" in this tier.  People can post fields they do not have access to in the hope they can update.  I like to load the object from my "database", map only the fields the user has permission to into the loaded object, then save the object.  If they "overpost" those fields are not mapped.

**Services tier(s)** - This is my "pure .NET (or Java)" code that contains my business logic.  It should not have any 3rd party references.  This mean I am only dealing with POCO (or POJO) objects which do not tie the architecture to any 3rd party (or Azure) dependencies.

**Repository Tier(s)** - This interacts with all 3rd parties (Azure, PDF writers, Databases, etc...)


##How this project is setup

I have put a lot of through on how my dependencies are ordered.  This is designed to keep things clean and not circular references.  Please review the references on the projects.


**Sample.Azure.Common** - Contains common elements like custom exceptions and helper methods.

**Sample.Azure.Config** - This is where ALL your configuration values will be populated.  You should NOT scatter your configuration values throughout your code. NOTE: This places the configuration in a "static" object which it NOT good for Azure VIP swaps.  You should read the values from Azure if you are going to VIP swap.

**Sample.Azure.DI** - This is my CLEAN dependency injection.  You can use any dependency injection you like, just keep it hidden.  I like to have this in code and not alot of configs that override each other.  I find that too hard to debug.

**Sample.Azure.Interface** - This is ALL your interfaces reside.  It references just your POCO Models so you are not tightly coupled.

**Sample.Azure.Model** - This is ALL your POCO (or POJO) reside.  These models will "flow" from your web tier, REST tier, Services tier and finally to your Repository tier (where the Repository tier can copy to its native 3rd party representation)

**Sample.Azure.Repository.Cach**e - This is my caching tier. I use what I call a "Near" (local) cache.  This means for items like "State Listings" I keep this local on my "web server" since we almost never add a State and it saves me a call to something like Redis.

**Sample.Azure.Repository.DocumentDB** - Interacts with DocumentDB.

**Sample.Azure.Repository.File** - This abstracts the File system.  If you are moving from on-prem to Azure, you should start using this and save to a SAN/NAS. You should remove all System.IO.File from your code and abstract saving files.  Then when you move to Azure, you save to Blob versus some local storage.

**Sample.Azure.Repository.Queue** - Interacts with Azure Queues.  Queues should be used heavily in the cloud since failures are to be expected and Queues all you to restart processes and spread a work across many servers (without the need for complex threading code).

**Sample.Azure.Repository.Search** - Interacts with Azure Search.  You should use this for Typeahead to keep the load off your database or caching tier.

**Sample.Azure.Repository.SqlServer** - Interacts with SQL Azure (has retry logic in it).

**Sample.Azure.Service** - This is where ALL your Business Logic code should reside.  This is what "makes your business money" or your "secret sauce".  This should not have any dependencies on 3rd party components since your business logic will probably change at a different rate then your technology stack. You should be able to change your backend database from SQL Server to DocumentDB without any changes to your business logic.  Also, you should be able to mix and match Azure services without changing your business logic.  Azure provides developers with a "Bigger Toolbox" and you should use the tool best fit for the job.  Before the cloud, developers would typically get a web server, possibly a middle tier and a database.  You then put everything on these 3 servers.  Asking for a caching server would take weeks to months.  Now, you no longer have those limits and you use the best technology for the job, but keep your business logic out of technology.

**Sample.Azure.UnitTest** - Yes, you should have automated tests.  Plus this provides developers with good samples.  Notice I just call 1 line of code to initialize my system "Sample.Azure.Config.Configuration.Configure();".  This one line of code is my idea of being able to quickly add a new console app, client app, etc.., and be up and running very quick!

##Architecture in Azure
This is how my architecture looks in Azure.  You can see how the Model span all tiers, the Configuration/Common and Interfaces span the various tiers.

I also now use Application Insights for my exception/logging as well as to see the performance of my entire application stack.  You can export your application insights to blob storage for further analysis or for historical purposes.

![alt tag](https://samplebase.blob.core.windows.net/architecture/AdamPaternostroArchitecture.png?st=2017-03-07T14%3A17%3A00Z&se=2025-03-08T14%3A17%3A00Z&sp=rl&sv=2015-12-11&sr=b&sig=1di7%2Bt0nAgIAbkdCN4%2BQssyf1ktos8K6RDMnYSScb88%3D)



