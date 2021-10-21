using backend.Models;
using GraphQL.Types;

namespace backend.Graphs.GraphTypes
{
    public class BeverageType : ObjectGraphType<Beverage>
    {
        public BeverageType(){
            Field(x => x.Id).Description("The Id of the Beverage");
            Field(x => x.Name).Description("The Name of the Beverage");
            Field(x => x.Description).Description("The Description of the Beverage");
            Field(x => x.Image).Description("The Image url of the Beverage");
            Field(x => x.Native).Description("Is the Beverage Native");
        }
    }
}