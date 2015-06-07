using System.Data.Entity;

namespace SimpleBet.Data
{

    public class Initializer : DropCreateDatabaseIfModelChanges<SimpleBetContext>
    {
    }

}